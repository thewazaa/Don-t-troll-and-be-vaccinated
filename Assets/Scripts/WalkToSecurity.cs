using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToSecurity : MonoBehaviour, IMovement
{
    private GuideSecurity guideSecurity;

    private PlayerController playerController;

    private void Awake() => playerController = GetComponent<PlayerController>();

    private void Start() => guideSecurity = FindObjectOfType<GuideSecurity>();

    public bool CheckNextState(bool isInFear)
    {
        int index = guideSecurity.queue.FindIndex(x => x.name == playerController.name);
        if (isInFear || index == 0)
            guideSecurity.queue.Remove(playerController);
        return isInFear || index == 0;
    }

    public void InitState() { }

    public bool CheckNeedToMove()
    {
        int index = guideSecurity.queue.FindIndex(x => x.name == playerController.name);
        if (index == 0)
            return Vector3.Distance(transform.position, guideSecurity.transform.position - guideSecurity.transform.forward) > 3;
        else
            return Vector3.Distance(transform.position, guideSecurity.queue[index - 1].transform.position - guideSecurity.queue[index - 1].transform.forward) > 3;
    }

    public void GetNextPosition()
    {
        int index = guideSecurity.queue.FindIndex(x => x.name == this.playerController.name);
        if (index == 0)
            playerController.SetDestination(guideSecurity.transform.position, guideSecurity.transform.forward);
        else
            playerController.SetDestination(guideSecurity.queue[index - 1].transform.position, -guideSecurity.queue[index - 1].transform.forward);
    }

    public PlayerController.EState GetNextState() => PlayerController.EState.walkToDeskGuide;

    public void ConfigState() => guideSecurity.queue.Add(playerController);
}
