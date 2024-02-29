using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlanetController : MonoBehaviour
{
    [SerializeField] private float rotationSpeed;

    void Update()
    {
        RotateAroundAxis();
    }


    private void RotateAroundAxis()
    {
        transform.RotateAround(transform.position, transform.up, Time.unscaledDeltaTime * rotationSpeed);
        transform.RotateAround(transform.position, transform.right, Time.unscaledDeltaTime * rotationSpeed);
    }
}
