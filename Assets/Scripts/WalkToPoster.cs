using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class WalkToPoster : MonoBehaviour, IMovement
{
    public int hour = 9;
    public int minute = 15;

    public int Time => hour * 60 + minute;

    public Poster poster;

    private PlayerController playerController;
    private Speak speak;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        speak = GetComponentInChildren<Speak>();
    }

    public void InitState() => speak.ShowText(playerController.isTroll, $"My vaccination hour is {hour:00}:{minute:00}");

    public bool CheckNextState(bool isInFear)
    {
        if (poster == null)
            return true;
        if (!isInFear && poster.Time == Time)
            return false;
        poster.queue.Remove(playerController);
        return isInFear || !FindPosterToMove();
    }

    public bool CheckNeedToMove()
    {
        if (poster == null)
            return false;
        int index = poster.queue.FindIndex(x => x.name == playerController.name);
        if (index < 0)
            return false;
        if (index == 0)
            return Vector3.Distance(transform.position, poster.transform.position + poster.transform.right) > 3;
        else
            return Vector3.Distance(transform.position, poster.queue[index - 1].transform.position - poster.queue[index - 1].transform.forward) > 3;
    }

    public void GetNextPosition()
    {
        int index = poster.queue.FindIndex(x => x.name == this.playerController.name);
        if (index == 0)
            playerController.SetDestination(poster.transform.position, poster.transform.right);
        else
            playerController.SetDestination(poster.queue[index - 1].transform.position, -poster.transform.forward);
    }

    public PlayerController.EState GetNextState() => PlayerController.EState.walkToEntrance;

    public void ConfigState() => FindPosterToMove();

    public bool FindPosterToMove()
    {
        foreach (Poster poster in FindObjectsOfType<Poster>())
            if (MoveToPoster(poster, hour, minute))
                return true;
        return false;
    }

    public bool MoveToPoster(Poster poster, int hour, int minute)
    {
        if (poster.hour != hour || poster.minute != minute)
            return false;
        this.poster = poster;

        poster.queue.Add(playerController);
        return true;
    }
}
