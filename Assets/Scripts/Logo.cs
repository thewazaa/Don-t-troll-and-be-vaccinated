using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Logo : MonoBehaviour
{
    private CanvasGroup canvasGroup;

    private void Awake() => canvasGroup = GetComponent<CanvasGroup>();

    private void Start() => StartCoroutine(FadeLogo());

    public IEnumerator FadeLogo()
    {
        yield return new WaitForSecondsRealtime(5f);
        canvasGroup.DOFade(0, 4f);
    }
}
