using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;

public class WalkToChair : MonoBehaviour, IMovement
{
    public Chair chair;

    private PlayerController playerController;

    private bool waiting = false;
    private bool next = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
    }

    public bool CheckNextState(bool isInFear)
    {
        if (isInFear || next)
            chair.free = true;
        return isInFear || next;
    }

    public bool CheckNeedToMove()
    {
        if (waiting)
            return false;
        if (Vector3.Distance(transform.position, chair.waitPoint.position) > .1f)
            return true;
        StartCoroutine(WaitSomeTime());
        return false;
    }

    public void InitState()
    {
        chair = FindObjectsOfType<Chair>().First(x => x.free);
        chair.free = false;
        waiting = false;
        next = false;
    }

    public void GetNextPosition() => playerController.SetDestination(chair.waitPoint.position, Vector3.zero, 0);

    public PlayerController.EState GetNextState() => PlayerController.EState.walkOut;

    public void ConfigState() { }

    public IEnumerator WaitSomeTime()
    {
        waiting = true;
        playerController.Sit(chair.waitPoint.forward);
        yield return new WaitForSeconds(60 * 15);
        playerController.Up();
        next = true;
    }
}
