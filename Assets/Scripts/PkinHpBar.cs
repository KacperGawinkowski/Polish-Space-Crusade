using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PkinHpBar : MonoBehaviour
{

    public SpaceshipController spaceshipController;
    public Image image;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        image.fillAmount = spaceshipController.Hp / (float)spaceshipController.MaxHp;
    }
}
