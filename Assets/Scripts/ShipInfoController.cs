using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShipInfoController : MonoBehaviour
{
    [SerializeField] private Image shipIcon;
    [SerializeField] private TextMeshProUGUI shipName;
    [SerializeField] private Image shipHealth;
    [SerializeField] private TextMeshProUGUI multipleShipsText;

    [SerializeField] private GameObject sus;
    
    [SerializeField] private Image shipIcon2;
    [SerializeField] private TextMeshProUGUI shipName2;
    [SerializeField] private Image shipHealth2;
    [SerializeField] private TextMeshProUGUI multipleShipsText2;

    [SerializeField] private GameObject sus2;
    

    public void AssignInfo(Sprite icon, Sprite healthbar, string name, float healthPercentage)
    {
        sus.SetActive(true);
        SwitchActiveInAllChildren(true);
        multipleShipsText.gameObject.SetActive(false);
        
        shipIcon.sprite = icon;
        shipName.text = name;
        shipHealth.sprite = healthbar;
        shipHealth.fillAmount = healthPercentage;
    }

    public void NoShipSelected()
    {
        sus.SetActive(false);
    }

    public void MultipleShipsInfo(int counter)
    {
        sus.SetActive(true);
        SwitchActiveInAllChildren(false);
        multipleShipsText.gameObject.SetActive(true);

        multipleShipsText.text = counter + " ships selected.";
    }

    private void SwitchActiveInAllChildren(bool state)
    {
        foreach (Transform child in sus.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
    
    
    
    public void AssignInfo2(Sprite icon, Sprite healthbar, string name, float healthPercentage)
    {
        sus2.SetActive(true);
        SwitchActiveInAllChildren2(true);
        multipleShipsText2.gameObject.SetActive(false);
        
        shipIcon2.sprite = icon;
        shipName2.text = name;
        shipHealth2.sprite = healthbar;
        shipHealth2.fillAmount = healthPercentage;
    }

    public void NoShipSelected2()
    {
        sus2.SetActive(false);
    }

    public void MultipleShipsInfo2(int counter)
    {
        sus2.SetActive(true);
        SwitchActiveInAllChildren(false);
        multipleShipsText2.gameObject.SetActive(true);

        multipleShipsText2.text = counter + " ships selected.";
    }

    private void SwitchActiveInAllChildren2(bool state)
    {
        foreach (Transform child in sus2.transform)
        {
            child.gameObject.SetActive(state);
        }
    }
    
    
}
