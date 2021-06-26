using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ScapeInFear : MonoBehaviour, IMovement
{
    private PlayerController playerController;
    private Speak speak;

    public Vector3 position = new Vector3(-89, .3f, 0);

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        speak = GetComponentInChildren<Speak>();
    }

    public bool CheckNextState(bool isInFear) => Vector3.Distance(transform.position, position) < 10;

    public void InitState()
    {
        if (playerController.isTroll)
            speak.ShowText(true, "NOOOO, WHY ME?", null, .5f, Speak.EFeeling.fear);
        else
        {
            speak.ShowText(false, "NOOOO, VACCINATION NOOOO", null, .5f, Speak.EFeeling.fear);
            GameManager.totalTrolled++;
        }
    }

    public bool CheckNeedToMove() => Vector3.Distance(transform.position, position) > 10;

    public void GetNextPosition()
    {
        position = new Vector3(Random.Range(0, 1f) < .5f ? -89 : 89, .3f, Random.Range(-10, 10));
        playerController.SetDestination(position, Vector3.zero, 3);
    }

    public PlayerController.EState GetNextState() => PlayerController.EState.exit;

    public void ConfigState() { }
}