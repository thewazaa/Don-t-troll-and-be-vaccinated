using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Desktop : MonoBehaviour
{
    public bool free = true;
    public int number;

    public TextMeshProUGUI text;
    public Transform waitPoint;
    private Speak speak;

    private void Awake() => speak = GetComponentInChildren<Speak>();

    private void Start() => text.text = number.ToString();

    public void Say(bool focus, string text, Action cb, float time = .5f) => speak.ShowText(focus, text, cb, time);
}
