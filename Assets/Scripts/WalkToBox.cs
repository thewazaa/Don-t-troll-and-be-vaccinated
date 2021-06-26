using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkToBox : MonoBehaviour, IMovement
{
    public Box box;

    private PlayerController playerController;
    private TrollController trollController;
    private Speak speak;

    private bool talking = false;
    private bool next = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        speak = GetComponentInChildren<Speak>();
        if (playerController.isTroll)
            trollController = GetComponent<TrollController>();
    }

    public bool CheckNextState(bool isInFear)
    {
        if (isInFear || next)
            box.free = true;
        return isInFear || next;
    }

    public bool CheckNeedToMove()
    {
        if (talking)
            return false;
        if (Vector3.Distance(transform.position, box.waitPoint.position) > .1f)
            return true;
        BeginTalk();
        return false;
    }

    public void InitState()
    {
        box.free = false;
        talking = false;
        next = false;
    }

    public void GetNextPosition() => playerController.SetDestination(box.waitPoint.position, Vector3.zero, 0);

    public PlayerController.EState GetNextState() => PlayerController.EState.walkToChair;

    public void ConfigState() { }

    public void BeginTalk()
    {
        talking = true;
        playerController.Sit(box.waitPoint.forward);
        box.Say(playerController.isTroll, "Now I will inject you the vaccine", Reply, 2f);
    }

    public void Reply() => speak.ShowText(playerController.isTroll, "Ok", Reply2);

    public void Reply2()
    {
        if (playerController.isTroll && trollController.totalTrolled > 0)
            box.Say(playerController.isTroll, "Wait a second", ReplyTroll1);
        else
            box.Say(playerController.isTroll, "...", Reply3, 30);
    }

    public void Reply3() => box.Say(playerController.isTroll, "Done", Reply4);

    public void Reply4()
    {
        playerController.Up();
        box.Say(playerController.isTroll, "Go out and sit in a chair", Reply5, 2);
        GameManager.totalVaccinated++;
    }

    public void Reply5() => box.Say(playerController.isTroll, "If past 15 minutes you feel well you can go back home", Reply6, 4);

    public void Reply6() => box.Say(playerController.isTroll, "If not, come back to a box", Reply7, 2);

    public void Reply7() => speak.ShowText(playerController.isTroll, "Ok, thanks!", Reply8);

    public void Reply8() => next = true;

    public void ReplyTroll1() => box.Say(playerController.isTroll, $"You had trolled {trollController.totalTrolled} people!", ReplyTroll2, 2);

    public void ReplyTroll2() => box.Say(playerController.isTroll, "I will not vaccinate YOU! MONSTER!!!!", ReplyTroll3, 2);

    public void ReplyTroll3()
    {
        playerController.Up();
        box.Say(playerController.isTroll, "But...", ReplyTroll4);
    }

    public void ReplyTroll4() => box.Say(playerController.isTroll, "OUT OF MY BOX!!", ReplyTroll5, 2);

    public void ReplyTroll5()
    {
        playerController.Fear();
        next = true;
    }
}