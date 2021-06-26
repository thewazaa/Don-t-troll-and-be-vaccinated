using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Screen : MonoBehaviour
{
    private TextMeshProUGUI text;

    private void Awake() => text = GetComponentInChildren<TextMeshProUGUI>();

    private void Start() => StartCoroutine(UpdateScreen());

    private IEnumerator UpdateScreen()
    {
        while (1 == 1)
        {
            yield return new WaitForSecondsRealtime(10);
            if (GameManager.totalTrolled > 0)
                text.text = $"Total people vaccinated today: {GameManager.totalVaccinated}\n-\nTotal people vaccinated here: {GameManager.totalVaccinated + 112345}\n-\nTotal people trolled: {GameManager.totalTrolled}";
            else
                text.text = $"Total people vaccinated today: {GameManager.totalVaccinated}\n-\nTotal people vaccinated here: {GameManager.totalVaccinated + 112345}";
        }
    }
}