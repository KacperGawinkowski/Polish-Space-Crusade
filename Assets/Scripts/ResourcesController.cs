using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ResourcesController : MonoBehaviour
{
    public int Onion;
    [SerializeField] public List<ResourceGenerator> onionGenerators = new List<ResourceGenerator>();

    public int Coal;
    [SerializeField] private float coalDecreaseTimer;
    [SerializeField] private float hpDecreaseTime;
    [SerializeField] private SpaceshipController pkinSpaceship;
    [SerializeField] private GameObject spaceshipContainer;
    private bool dmgStarted = false;

    [Header("Interface")]
    [SerializeField] public TextMeshProUGUI onionCounter;
    [SerializeField] private TextMeshProUGUI coalCounter;
    //[SerializeField] private GameObject deathScreen;

    [Header("Buttons")]
    [SerializeField] private Button husarButton;
    [SerializeField] private int husarCost;
    [SerializeField] private Button cebulaCorpButton;
    [SerializeField] private int cebulaCorpCost;
    [SerializeField] private Button witcherButton;
    [SerializeField] private int witcherCost;
    [SerializeField] private Button pierougiButton;
    [SerializeField] private int pierougiCost;

    private void Start()
    {
        StartCoroutine(DecreaseCoal());
        foreach (ResourceGenerator generator in onionGenerators)
        {
            if (generator != null)
            {
                generator.Activate();
            }
        }
    }


    public void AddCoal(int amount)
    {
        Coal += amount;
        coalCounter.text = Coal.ToString();
    }

    private IEnumerator DecreaseCoal()
    {
        while (Coal >= 0)
        {
            yield return new WaitForSeconds(coalDecreaseTimer);
            Coal -= spaceshipContainer.transform.childCount + 1;
            coalCounter.text = Coal.ToString();
        }
        if (Coal < 0)
        {
            coalCounter.text = "0";
            if (dmgStarted == false)
            {
                StartCoroutine(DecreaseHPOfTheOldOne());
                dmgStarted = true;
            }
        }
    }

    private IEnumerator DecreaseHPOfTheOldOne()
    {
        while (Coal < 0)
        {
            yield return new WaitForSeconds(hpDecreaseTime);
            if (pkinSpaceship)
            {
                pkinSpaceship.DealDamage((spaceshipContainer.transform.childCount + 1)*2);
            }
        }

        dmgStarted = false;
    }


    public int GetCost(int index)
    {
        if (index == 0)
        {
            return husarCost;
        }
        else if (index == 1)
        {
            return cebulaCorpCost;
        }
        else if (index == 2)
        {
            return witcherCost;
        }
        else
        {
            return pierougiCost;
        }
    }

    public void UpdateButtons()
    {
        if (Onion >= husarCost)
        {
            husarButton.interactable = true;
        }
        else
        {
            husarButton.interactable = false;
        }
        if (Onion >= cebulaCorpCost)
        {
            cebulaCorpButton.interactable = true;
        }
        else
        {
            cebulaCorpButton.interactable = false;
        }
        if (Onion >= witcherCost)
        {
            witcherButton.interactable = true;
        }
        else
        {
            witcherButton.interactable = false;
        }
        if (Onion >= pierougiCost)
        {
            pierougiButton.interactable = true;
        }
        else
        {
            pierougiButton.interactable = false;
        }
    }

    //public void DEATH_MAN()
    //{
    //    deathScreen.SetActive(true);
    //}
}