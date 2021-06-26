using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimeControl : MonoBehaviour
{
    public float timeScale = 10;
    public float currentTime;
    public float startHour = 8;
    public float startMinute = 45;

    public static float realTimeScale = 10;

    private float startTime;
    private TextMeshProUGUI text;

    private void Awake()
    {
        Time.timeScale = timeScale;
        realTimeScale = timeScale;
        text = GetComponentInChildren<TextMeshProUGUI>();
        startTime = Time.time;        
    }

    private void Update()
    {
        float totalTime = Time.time - startTime + 3600 * startHour + startMinute * 60;
        currentTime = totalTime / 60; // no seconds

        float seconds = Mathf.Floor(totalTime % 60);
        float minutes = Mathf.Floor((totalTime / 60) % 60);
        float hours = Mathf.Floor(totalTime / 3600);

        if (GameManager.totalTrolled > 0)
            text.text = $"{hours:00}:{minutes:00}:{seconds:00} - {GameManager.totalVaccinated} people vaccinated - {GameManager.totalTrolled} people trolled";
        else
            text.text = $"{hours:00}:{minutes:00}:{seconds:00} - {GameManager.totalVaccinated} people vaccinated";
    }

    public static void NormalSpeed() => Time.timeScale = 1;

    public static void RealSpeed() => Time.timeScale = realTimeScale;
}
