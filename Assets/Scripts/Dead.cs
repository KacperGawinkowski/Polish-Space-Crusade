using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Dead : MonoBehaviour
{
    public SpaceshipController spaceshipController;

    private void Start()
    {
        spaceshipController = GetComponent<SpaceshipController>();
    }

    private void Update()
    {
        if (spaceshipController.Hp <= 0)
        {
            SceneManager.LoadScene(4);
        }
    }

}
