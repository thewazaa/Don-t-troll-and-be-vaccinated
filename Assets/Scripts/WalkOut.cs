using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkOut : MonoBehaviour, IMovement
{
    private PlayerController playerController;

    private Vector3 position = new Vector3(-89, .3f, 0);

    private void Awake() => playerController = GetComponent<PlayerController>();

    public bool CheckNextState(bool isInFear) => Vector3.Distance(transform.position, position) < 3;

    public void InitState() { }

    public bool CheckNeedToMove() => Vector3.Distance(transform.position, position) > 3;

    public void GetNextPosition()
    {
        position = new Vector3(-89, .3f, Random.Range(-10, 10));
        playerController.SetDestination(position);
    }

    public PlayerController.EState GetNextState() => PlayerController.EState.exit;

    public void ConfigState() { }
}
