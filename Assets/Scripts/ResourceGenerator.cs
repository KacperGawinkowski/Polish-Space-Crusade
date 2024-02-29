using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceGenerator : MonoBehaviour
{
    [SerializeField] private int resourceIncome;
    [SerializeField] private float incomeTime;
    [SerializeField] private bool active;
    
    private ResourcesController controller;

    private void Awake()
    {
        controller = FindObjectOfType<ResourcesController>();
    }

    private void Update()
    {
        controller.UpdateButtons();
    }

    public void AddToList()
    {
        controller.onionGenerators.Add(this);
    }

    public void Activate()
    {
        active = true;
        controller.StartCoroutine(GenerateIncome());
    }

    private IEnumerator GenerateIncome()
    {
        while (active)
        {
            yield return new WaitForSeconds(incomeTime);
            controller.Onion += resourceIncome;
            controller.onionCounter.text = controller.Onion.ToString();
            //controller.UpdateButtons();
        }
    }
}
