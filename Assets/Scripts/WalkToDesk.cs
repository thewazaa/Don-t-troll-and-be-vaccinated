using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkToDesk : MonoBehaviour, IMovement
{
    public Desktop desktop;

    private PlayerController playerController;
    private Speak speak;

    private bool talking = false;
    private bool next = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        speak = GetComponentInChildren<Speak>();
    }

    public bool CheckNextState(bool isInFear)
    {
        if (isInFear || next)
            desktop.free = true;
        return isInFear || next;
    }

    public bool CheckNeedToMove()
    {
        if (talking)
            return false;
        if (Vector3.Distance(transform.position, desktop.waitPoint.position) > .5f)
            return true;
        BeginTalk();
        return false;
    }

    public void InitState()
    {
        desktop.free = false;
        talking = false;
        next = false;
    }

    public void GetNextPosition() => playerController.SetDestination(desktop.waitPoint.position, Vector3.zero, .5f);

    public PlayerController.EState GetNextState() => PlayerController.EState.walkToBoxGuide1;

    public void ConfigState() { }

    public void BeginTalk()
    {
        talking = true;
        desktop.Say(playerController.isTroll, "Show me your credentials", Reply);
    }

    public void Reply() => speak.ShowText(playerController.isTroll, "My credentials", Reply2);

    public void Reply2() => desktop.Say(playerController.isTroll, "...", Reply3, 30f);

    public void Reply3() => desktop.Say(playerController.isTroll, "You can pass", Reply4);

    public void Reply4() => next = true;
}
