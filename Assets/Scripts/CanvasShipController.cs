using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class CanvasShipController : MonoBehaviour
{
    [SerializeField] private GameObject spaceshipHolder;
    [SerializeField] private Transform shipSpawnPoint;
    [SerializeField] private List<GameObject> spaceships;

    private ResourcesController controller;

    private void Start()
    {
        controller = FindObjectOfType<ResourcesController>();
    }

    public void SpawnShip(int index)
    {
        Vector3 offset = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.forward * 2);
        Instantiate(spaceships[index], shipSpawnPoint.position + offset, Quaternion.identity, spaceshipHolder.transform);
        controller.Onion -= controller.GetCost(index);
    }
}
