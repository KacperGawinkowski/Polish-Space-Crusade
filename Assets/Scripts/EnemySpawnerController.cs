using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnerController : MonoBehaviour
{
    [SerializeField] GameObject lightEnemyShip;
    [SerializeField] private float radius;

    private Camera cam;
    private GameObject pkin;

    Coroutine normalSpawnCoroutine;

    public float spawnRate = 10f;


    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        pkin = GameObject.FindGameObjectWithTag("Player");
        normalSpawnCoroutine = StartCoroutine(NormalSpawn());
    }

    IEnumerator NormalSpawn()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnRate);

            Vector3 spawn;
            while (true)
            {
                Vector3 offset = FindPlace();
                spawn = pkin.transform.position + offset;
                Vector3 vector3 = cam.WorldToViewportPoint(spawn);

                print(vector3);

                yield return null;

                if ((vector3.x < 0 || vector3.x > 1) || (vector3.y < 0 || vector3.y > 1))
                {
                    break;
                }
            }


            GameObject go = Instantiate(lightEnemyShip, spawn, Quaternion.identity);
            EnemyShip enemyShip = go.GetComponent<EnemyShip>();
            

            if (Random.value > 0.1f)
            {
                enemyShip.target = pkin.transform;
                enemyShip.spc = pkin.GetComponent<SpaceshipController>();
            }
            else
            {
                SpaceshipController[] scs = FindObjectsOfType<SpaceshipController>();
                SpaceshipController random = scs[Random.Range(0, scs.Length)];

                enemyShip.target = random.transform;
                enemyShip.spc = random.GetComponent<SpaceshipController>();
            }

            enemyShip.agent.stoppingDistance = enemyShip.fireRange / 2;

        }
    }

    private Vector3 FindPlace()
    {
        return Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.forward * radius);
    }
}
