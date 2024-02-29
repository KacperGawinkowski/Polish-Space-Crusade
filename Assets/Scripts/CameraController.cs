using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform cameraTransform;

    private Vector3 moveCamera;
    private float rotateCamera;
    
    private void OnMoveCamera(InputValue value)
    {
        Vector2 move = value.Get<Vector2>();
        moveCamera = move/16;
    }

    public void OnRotateCamera(InputValue value)
    {
        // float rotate = value.Get<float>();
        // print(rotate);
        // rotateCamera = rotate/180;
    }
    
    private void OnZoomCamera(InputValue value)
    {
        Vector3 position = cameraTransform.position;
        float zoom = value.Get<float>() / 100;
        
        position = new Vector3(position.x, position.y + -zoom, position.z);
        position.y = Math.Clamp(position.y, 2, 25);
        cameraTransform.position = position;
    }

    private void Update()
    {
        cameraTransform.position += new Vector3(moveCamera.x, 0, moveCamera.y);
    }
}
