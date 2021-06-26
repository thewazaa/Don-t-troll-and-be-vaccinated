using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Speak : MonoBehaviour
{
    public enum EFeeling
    {
        talk,
        troll,
        fear
    }

    public AudioClip audioClipTalk, audioClipTroll, audioFear;
    private CanvasGroup canvasGroup;
    private TextMeshProUGUI text;
    private AudioSource audioSource;

    private Action cb;

    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        text = GetComponentInChildren<TextMeshProUGUI>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start() => canvasGroup.alpha = 0;

    public void ShowText(bool focus, string text, Action cb = null, float time = .5f, EFeeling feeling = EFeeling.talk)
    {
        if (focus)
            TimeControl.NormalSpeed();
        this.cb = cb;
        this.text.text = text;
        switch (feeling)
        {
            case EFeeling.talk: audioSource.PlayOneShot(audioClipTalk); break;
            case EFeeling.troll: audioSource.PlayOneShot(audioClipTroll); break;
            case EFeeling.fear: audioSource.PlayOneShot(audioFear); break;
        }

        if (time >= 1)
        {
            canvasGroup.DOFade(1, time).OnComplete(HideText);
            transform.DOShakePosition(1f);
        }
        else
        {
            canvasGroup.DOFade(1, .5f);
            transform.DOShakePosition(1f).OnComplete(HideText);
        }
    }

    public void HideText()
    {
        canvasGroup.DOFade(0, .5f);
        if (cb != null)
            cb.Invoke();
        TimeControl.RealSpeed();
    }
}