using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GuideDesk : MonoBehaviour
{
    public Desktop[] desktops;
    public List<PlayerController> queue = new List<PlayerController>();

    private Speak speak;

    private void Awake() => speak = GetComponentInChildren<Speak>();

    public void Say(bool focus, string text, Action cb) => speak.ShowText(focus, text, cb);
}
