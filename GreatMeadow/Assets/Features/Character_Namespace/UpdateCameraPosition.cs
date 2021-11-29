using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateCameraPosition : MonoBehaviour
{
    [SerializeField] private float CameraDistance = -2f;
    private Transform mainCameraTransform;
    private void Awake()
    {
        mainCameraTransform = Camera.main.transform;
    }
    void Update()
    {
        mainCameraTransform.position = new Vector3(transform.position.x, transform.position.y, CameraDistance);

    }
}
