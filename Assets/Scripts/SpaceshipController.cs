using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.SceneManagement;

public class SpaceshipController : MonoBehaviour
{
    public bool isPkin;

    public NavMeshAgent agent;
    public Animator animator;
    private InputHandler inputHandler;
    [SerializeField] private GameObject blast;
    private AudioSource adioSource;

    public float fireRange;
    public float fireDelay;
    public float fireTime = 0.1f;
    public int fireDmg;

    public int MaxHp = 1;
    public int Hp;

    public Laser[] laser;
    public EnemyShip target;

    private Coroutine shootCoroutine;

    [SerializeField] private GameObject selectionCircle;

    [SerializeField] private bool isWiedzmin;
    [SerializeField] private bool isCebulaCorp;

    private ResourcesController resourcesController;

    [Header("Cebula Corp Generator")]
    [SerializeField] private ResourceGenerator generator;

    [Header("Interface")]
    public Sprite shipIcon;
    public Sprite shipHpIcon;
    public string shipName;


    // Start is called before the first frame update
    void Start()
    {
        adioSource = GetComponent<AudioSource>();
        inputHandler = FindObjectOfType<InputHandler>();
        resourcesController = FindObjectOfType<ResourcesController>();
        agent = GetComponent<NavMeshAgent>();

        Hp = MaxHp;

        if (isCebulaCorp)
        {
            generator.AddToList();
            generator.Activate();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            agent.destination = target.transform.position;
            agent.updateRotation = false;
            transform.forward = -(transform.position - target.transform.position);

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
            agent.updateRotation = true;
        }
    }

    public void OnDisable()
    {
        inputHandler.spaceshipController.Remove(this);
    }

    public void MoveToPosition(Vector3 position)
    {
        agent.SetDestination(position);
    }

    public void SpawnLasers()
    {
        if (target != null)
        {
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

            if (isWiedzmin)
            {
                animator.SetTrigger("Attack");
                yield return new WaitForSeconds(fireTime);
                if (target)
                {
                    adioSource.Play();
                    target.DealDamage(fireDmg);
                }
            }
            else
            {
                SpawnLasers();
                if (target)
                {
                    adioSource.Play();
                    target.DealDamage(fireDmg);
                }

                yield return new WaitForSeconds(fireTime);
                RemoveLasers();
            }
        }
    }

    public void DealDamage(int d)
    {
        Hp -= d;

        if (Hp <= 0)
        {
            GameObject go = Instantiate(blast);
            go.transform.position = transform.position;
            if (isPkin)
            {
                SceneManager.LoadScene(4);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    public void ChangeSelection(bool value)
    {
        selectionCircle.SetActive(value);
    }

    private void OnDestroy()
    {
        if (isCebulaCorp)
        {
            resourcesController.onionGenerators.Remove(generator);
        }
    }
}

[System.Serializable]
public class Laser
{
    public Transform startPoint;
    public LineRenderer lineRenderer;
}
