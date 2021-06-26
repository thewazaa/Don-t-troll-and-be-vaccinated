using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkToDeskGuide : MonoBehaviour, IMovement
{
    private GuideDesk guideDesk;

    private PlayerController playerController;

    private bool talking = false;
    private bool next = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        guideDesk = FindObjectOfType<GuideDesk>();
    }

    public bool CheckNextState(bool isInFear)
    {
        if (isInFear || next)
            guideDesk.queue.Remove(playerController);
        return isInFear || next;
    }

    public bool CheckNeedToMove()
    {
        if (talking)
            return false;
        int index = guideDesk.queue.FindIndex(x => x.name == playerController.name);
        if (index == 0)
        {
            if (Vector3.Distance(transform.position, guideDesk.transform.position + guideDesk.transform.right) > 3)
                return true;
            BeginTalk();
            return false;
        }
        else
            return Vector3.Distance(transform.position, guideDesk.queue[index - 1].transform.position - guideDesk.queue[index - 1].transform.forward) > 3;
    }

    public void InitState()
    {
        talking = false;
        next = false;
    }

    public void GetNextPosition()
    {
        int index = guideDesk.queue.FindIndex(x => x.name == playerController.name);
        if (index == 0)
            playerController.SetDestination(guideDesk.transform.position, guideDesk.transform.right);
        else
            playerController.SetDestination(guideDesk.queue[index - 1].transform.position, -guideDesk.queue[index - 1].transform.forward);
    }

    public PlayerController.EState GetNextState() => PlayerController.EState.walkToDesk;

    public void ConfigState() => guideDesk.queue.Add(playerController);

    public void BeginTalk()
    {
        talking = true;
        Reply();
    }

    public void Reply()
    {
        if (guideDesk.desktops.Where(x => x.free).Count() > 0)
        {
            Desktop d = guideDesk.desktops.First(x => x.free);
            playerController.SetDesktop(d);
            guideDesk.Say(playerController.isTroll, $"Go to desktop {d.number}", Reply2);
        }
        else
            guideDesk.Say(playerController.isTroll, "Wait...", Reply);
    }

    public void Reply2() => next = true;
}