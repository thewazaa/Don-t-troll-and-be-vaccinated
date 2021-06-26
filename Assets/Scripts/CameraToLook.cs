using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class CameraToLook : MonoBehaviour
{
    public void Update() => transform.LookAt(CameraManager.activeCamera.transform, Vector3.up);
}
