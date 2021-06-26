using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public Camera[] cameras;
    public Camera initialCamera;
    public static Camera activeCamera;
    public Camera previousCamera;

    public bool fixedToTroll = false;

    private void Awake()
    {
        cameras = GetComponentsInChildren<Camera>();
        activeCamera = initialCamera;
    }

    private void Start()
    {
        foreach (Camera camera in cameras)
            camera.gameObject.SetActive(false);
        activeCamera.gameObject.SetActive(true);
        StartCoroutine(ChangeCamera());
    }

    public IEnumerator ChangeCamera()
    {
        while (1 == 1)
        {
            yield return new WaitForSeconds(60);
            if (!fixedToTroll)
            {
                activeCamera.gameObject.SetActive(false);

                float what = Random.Range(0, 1f);
                if (what <= .5f)
                {
                    PlayerController[] list = FindObjectsOfType<PlayerController>();
                    int i = Random.Range(0, list.Length);
                    activeCamera = list[i].camera;
                }
                else
                {
                    int i = Random.Range(0, cameras.Length);
                    activeCamera = cameras[i];
                }
                activeCamera.gameObject.SetActive(true);
            }
        }
    }

    public void FixCameraToTroll(PlayerController playerController)
    {
        if (fixedToTroll || activeCamera == null)
            return;
        fixedToTroll = true;
        previousCamera = activeCamera;
        Debug.Log(activeCamera.name);
        activeCamera.gameObject.SetActive(false);
        activeCamera = playerController.camera;
        activeCamera.gameObject.SetActive(true);
    }

    public void UnfixCamera()
    {
        if (!fixedToTroll)
            return;
        fixedToTroll = false;
        activeCamera.gameObject.SetActive(false);
        activeCamera = previousCamera;
        if (activeCamera != null)
            activeCamera.gameObject.SetActive(true);
    }
}