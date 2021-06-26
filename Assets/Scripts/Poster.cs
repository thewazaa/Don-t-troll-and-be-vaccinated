using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Poster : MonoBehaviour
{
    public int hour;
    public int minute;
    public bool human;
    public List<PlayerController> queue = new List<PlayerController>();

    public int Time => hour * 60 + minute;

    private TextMeshProUGUI text;

    private void Awake() => text = GetComponentInChildren<TextMeshProUGUI>();

    private void Start() => UpdateText(Time);

    public void UpdateText(int setTime, List<PlayerController> tmpQueue = null)
    {
        this.hour = setTime / 60;
        this.minute = setTime % 60;

        if (tmpQueue == null)
            tmpQueue = new List<PlayerController>();
        queue = tmpQueue;
        foreach (PlayerController playerController in queue)
            playerController.SetPoster(this);

        text.text = $"{hour:00}:{minute:00}";
    }
}
