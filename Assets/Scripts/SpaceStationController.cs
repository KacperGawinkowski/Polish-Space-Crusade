using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class SpaceStationController : MonoBehaviour
{

    private GameObject palace;
    private bool takeOver;
    private float takeOverTime;
    [SerializeField] private float totalTakeOverTime;
    private bool takingOverSucces;
    [SerializeField] private GameObject domkirs;

    [SerializeField] GameObject canvas;
    [SerializeField] Image image;

    private ResourceGenerator generator;

    public UnityEvent stationCaptured;

    // Start is called before the first frame update
    void Start()
    {
        palace = GameObject.FindWithTag("Player");
        generator = GetComponent<ResourceGenerator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (takeOver && !takingOverSucces)
        {
            takeOverTime += Time.deltaTime;
            image.fillAmount = takeOverTime / totalTakeOverTime;

            if (takeOverTime > totalTakeOverTime)
            {
                takingOverSucces = true;
                domkirs.SetActive(true);
                generator.AddToList();
                generator.Activate();
                canvas.SetActive(false);
                
                if (FindObjectOfType<EnemySpawnerController>())
                {
                    FindObjectOfType<EnemySpawnerController>().spawnRate -= 1;
                }

                stationCaptured.Invoke();
                //TODO: polska flaga
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        print(other.gameObject.tag);
        if (other.gameObject.CompareTag("Player") /*other.gameObject == palace */)
        {
            takeOver = true;
            canvas.SetActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!takingOverSucces)
        {
            if (other.gameObject.CompareTag("Player") /*other.gameObject == palace*/)
            {
                canvas.SetActive(false);
                takeOver = false;
                takeOverTime = 0;
            }
        }
    }
}
