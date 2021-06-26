using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrollController : MonoBehaviour
{
    public string[] sentences;
    public int totalTrolled = 0;
    public AudioClip trollMusic;
    public AudioSource audioSourceMusic;

    private CameraManager cameraManager;
    private PlayerController playerController;
    private Speak speak;

    private PlayerController affecting;

    private static bool firstTroll = false;

    private bool speaking = false;    

    private void Awake()
    {
        cameraManager = FindObjectOfType<CameraManager>();
        playerController = GetComponent<PlayerController>();
        speak = GetComponentInChildren<Speak>();
    }

    private void OnEnable()
    {
        cameraManager.FixCameraToTroll(playerController);
        totalTrolled = 0;
        speaking = false;
    }

    private void OnDisable() => cameraManager.UnfixCamera();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Speak();
    }

    private void Speak()
    {
        int i = Random.Range(0, sentences.Length);
        speak.ShowText(true, sentences[i], EndSpeak, .5f, global::Speak.EFeeling.troll);
        speaking = true;
        affecting = null;
    }

    private void EndSpeak() => speaking = false;

    private void OnTriggerStay(Collider other)
    {
        if (!speaking || other.gameObject.layer != 3)
            return;

        affecting = other.GetComponent<PlayerController>();
        affecting.Fear();
        if (!firstTroll)
        {
            firstTroll = true;
            audioSourceMusic.clip = trollMusic;
            audioSourceMusic.Play();
        }
        totalTrolled++;
        speaking = false;
    }
}
