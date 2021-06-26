using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkToBoxGuide : MonoBehaviour, IMovement
{
    private GuideBox guideBox1, guideBox2;

    private PlayerController playerController;

    private GuideBox GuideBox => playerController.state == PlayerController.EState.walkToBoxGuide1 ? guideBox1 : guideBox2;

    private bool talking = false;
    private bool next = false;
    private bool nextBox = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        GuideBox[] list = FindObjectsOfType<GuideBox>();
        guideBox1 = list[0].name == "Guide Box 1" ? list[0] : list[1];
        guideBox2 = list[0].name == "Guide Box 1" ? list[1] : list[0];
    }

    public bool CheckNextState(bool isInFear)
    {
        if (isInFear || next)
            GuideBox.queue.Remove(playerController);
        return isInFear || next;
    }

    public bool CheckNeedToMove()
    {
        if (talking)
            return false;
        int index = GuideBox.queue.FindIndex(x => x.name == playerController.name);
        if (index == 0)
        {
            if (Vector3.Distance(transform.position, GuideBox.transform.position + GuideBox.transform.forward) > 1)
                return true;
            BeginTalk();
            return false;
        }
        else
            return Vector3.Distance(transform.position, GuideBox.queue[index - 1].transform.position - GuideBox.queue[index - 1].transform.forward) > 3;
    }

    public void InitState()
    {
        talking = false;
        next = false;
        nextBox = false;
    }

    public void GetNextPosition()
    {
        int index = GuideBox.queue.FindIndex(x => x.name == playerController.name);
        if (index == 0)
            playerController.SetDestination(GuideBox.transform.position, GuideBox.transform.forward);
        else
            playerController.SetDestination(GuideBox.queue[index - 1].transform.position, -GuideBox.queue[index - 1].transform.forward);
    }
    
    public PlayerController.EState GetNextState()=>nextBox ? PlayerController.EState.walkToBox : PlayerController.EState.walkToBoxGuide2;

    public void ConfigState() => GuideBox.queue.Add(playerController);

    public void BeginTalk()
    {
        talking = true;
        Reply();
    }

    public void Reply()
    {
        if (GuideBox.boxs.Where(x => x.free).Count() > 0)
        {
            Box b = GuideBox.boxs.First(x => x.free);
            playerController.SetBox(b);
            GuideBox.Say(playerController.isTroll, "Go to a free box", Reply2);
        }
        else if (playerController.state == PlayerController.EState.walkToBoxGuide1)
            next = true;
        else
            GuideBox.Say(playerController.isTroll, "Wait...", Reply);
    }

    public void Reply2()
    {
        next = true;
        nextBox = true;
    }
}