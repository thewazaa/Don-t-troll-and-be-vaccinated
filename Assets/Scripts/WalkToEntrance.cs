using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToEntrance : MonoBehaviour, IMovement
{
    private GuideEntrance guideEntrance;

    private Speak speak;
    private PlayerController playerController;

    private bool talking = false;
    private bool next = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        speak = GetComponentInChildren<Speak>();
    }

    private void Start() => guideEntrance = FindObjectOfType<GuideEntrance>();

    public void InitState()
    {
        talking = false;
        next = false;
    }

    public bool CheckNextState(bool isInFear)
    {
        if (next || isInFear)
            guideEntrance.queue.Remove(playerController);
        return isInFear || next;
    }

    public bool CheckNeedToMove()
    {
        if (talking)
            return false;
        int index = guideEntrance.queue.FindIndex(x => x.name == playerController.name);
        if (index == 0)
        {
            if (Vector3.Distance(transform.position, guideEntrance.transform.position - guideEntrance.transform.right) > 3)
                return true;
            BeginTalk();
            return false;
        }
        else
            return Vector3.Distance(transform.position, guideEntrance.queue[index - 1].transform.position - guideEntrance.queue[index - 1].transform.forward) > 3;
    }

    public void GetNextPosition()
    {
        int index = guideEntrance.queue.FindIndex(x => x.name == this.playerController.name);
        if (index == 0)
            playerController.SetDestination(guideEntrance.transform.position, -guideEntrance.transform.right);
        else
            playerController.SetDestination(guideEntrance.queue[index - 1].transform.position, -guideEntrance.queue[index - 1].transform.forward);
    }

    public PlayerController.EState GetNextState() => PlayerController.EState.walkToDeskGuide;

    public void ConfigState()
    {
        if (!guideEntrance.queue.Contains(playerController))
            guideEntrance.queue.Add(playerController);
    }

    public void BeginTalk()
    {
        talking = true;
        guideEntrance.Say(playerController.isTroll, "Show me your credentials", Reply);
    }

    public void Reply() => speak.ShowText(playerController.isTroll, "My credentials", Reply2);

    public void Reply2() => guideEntrance.Say(playerController.isTroll, "You can pass", Reply3);

    public void Reply3() => next = true;
}
