using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyShip : MonoBehaviour
{
    public NavMeshAgent agent;
    [SerializeField] private GameObject blast;

    InputHandler inputHandler;

    public int MaxHp = 1;
    public int Hp = 1;

    public int CoalIncrease;

    [SerializeField] private GameObject selectionCircle;

    public enum EnemyType { Standing, Carrier }
    private EnemyType enemyType;

    public float fireRange;
    public float fireDelay;
    public float fireTime = 0.1f;
    public int fireDmg;

    [SerializeField] private LayerMask spaceshipMask;

    private AudioSource audioSource;

    [Header("standing enemy")]
    [SerializeField] private float activationDistance;
    private Vector3 initialPos;
    public Transform target;
    public SpaceshipController spc;
    private Coroutine shootCoroutine;
    public Laser[] laser;

    [Header("Interface")]
    public Sprite shipIcon;
    public Sprite shipHpIcon;
    public string shipName;

    [Header("Carrier")]
    [SerializeField] private bool isCarrier;
    [SerializeField] private GameObject carrierSmallShip;
    [SerializeField] private float carrierSpawnTime;
    [SerializeField] private Coroutine carrierCoroutine;

    private ResourcesController resourcesController;




    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        inputHandler = FindObjectOfType<InputHandler>();
        agent = GetComponent<NavMeshAgent>();
        resourcesController = FindObjectOfType<ResourcesController>();

        Hp = MaxHp;
        initialPos = transform.position;
    }

    private void OnEnable()
    {
        inputHandler.ClearSelectionCircles += SelectionFalse;
    }

    private void OnDisable()
    {
        inputHandler.ClearSelectionCircles -= SelectionFalse;
    }

    private void OnDestroy()
    {
        resourcesController.AddCoal(CoalIncrease);
        inputHandler.ClearSelectionCircles -= SelectionFalse;
    }

    public void SelectionFalse()
    {
        ChangeSelection(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Standing:

                if (target == null)
                {

                    agent.updateRotation = true;
                    agent.destination = initialPos;
                    agent.stoppingDistance = 0;
                    Collider[] colliders = Physics.OverlapSphere(transform.position, activationDistance, spaceshipMask);
                    print(colliders.Length);
                    if (colliders.Length > 0)
                    {
                        target = colliders[Random.Range(0, colliders.Length)].transform;
                        spc = target.gameObject.GetComponentInParent<SpaceshipController>();
                        //print(target.gameObject);
                        agent.stoppingDistance = fireRange / 2;

                        if (isCarrier)
                        {
                            if (carrierCoroutine == null)
                            {
                                carrierCoroutine = StartCoroutine(CarrierEnumerator());
                            }
                        }
                    }
                }
                else
                {
                    agent.updateRotation = false;
                    transform.forward = -(transform.position - target.transform.position);
                    agent.destination = target.position;
                    if (Vector3.Distance(transform.position, target.gameObject.transform.position) < fireRange)
                    {
                        if (shootCoroutine == null)
                        {
                            shootCoroutine = StartCoroutine(ShootEnumerator());
                        }
                    }
                    else
                    {
                        if (shootCoroutine != null)
                        {
                            StopCoroutine(shootCoroutine);
                            shootCoroutine = null;
                            RemoveLasers();
                        }

                        if (carrierCoroutine != null)
                        {
                            StopCoroutine(carrierCoroutine);
                            carrierCoroutine = null;
                        }
                    }
                }

                break;
            default:
                break;
        }
    }

    public IEnumerator CarrierEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(carrierSpawnTime);

            Vector3 offset = Quaternion.AngleAxis(Random.Range(0, 360), Vector3.up) * (Vector3.forward * 1);

            Instantiate(carrierSmallShip, transform.position + offset, Quaternion.identity);
        }
    }

    public void DealDamage(int d)
    {
        Hp -= d;

        if (Hp <= 0)
        {
            GameObject go = Instantiate(blast);
            go.transform.position = transform.position;
            Destroy(gameObject);
        }
    }

    public void ChangeSelection(bool value)
    {
        selectionCircle.SetActive(value);
    }

    public void SpawnLasers()
    {
        if (target != null)
        {
            audioSource.Play();
            foreach (var item in laser)
            {
                item.lineRenderer.gameObject.SetActive(true);
                item.lineRenderer.SetPosition(0, item.startPoint.position);
                item.lineRenderer.SetPosition(1, target.transform.position);
            }
        }
    }

    public void RemoveLasers()
    {
        foreach (var item in laser)
        {
            item.lineRenderer.gameObject.SetActive(false);
        }
    }

    private IEnumerator ShootEnumerator()
    {
        while (true)
        {
            yield return new WaitForSeconds(fireDelay);

            yield return new WaitWhile(() => shootCoroutine == null);

            SpawnLasers();
            if (spc)
            {
                spc.DealDamage(fireDmg);
            }
            
            yield return new WaitForSeconds(fireTime);
            RemoveLasers();
        }
    }
}
