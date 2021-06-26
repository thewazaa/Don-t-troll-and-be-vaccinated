using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Box : MonoBehaviour
{
    public bool free = true;

    public Transform waitPoint;
    private Speak speak;

    private void Awake() => speak = GetComponentInChildren<Speak>();

    public void Say(bool focus, string text, Action cb, float time = .5f) => speak.ShowText(focus, text, cb, time);
}
