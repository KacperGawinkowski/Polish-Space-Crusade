using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputHandler : MonoBehaviour
{

    public List<SpaceshipController> spaceshipController = new List<SpaceshipController>();

    private Camera cam;

    [SerializeField] LayerMask clickableMask;

    public event Action ClearSelectionCircles;

    private ShipInfoController infoController;

    private GameObject player;

    [SerializeField] private Transform camera;

    private EnemyShip mouseOverEnemy;
    // Start is called before the first frame update
    void Start()
    {
        cam = Camera.main;
        infoController = FindObjectOfType<ShipInfoController>();
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, clickableMask))
        {
            mouseOverEnemy = hit.collider.GetComponentInParent<EnemyShip>();
            if (mouseOverEnemy != null)
            {
                infoController.AssignInfo2(mouseOverEnemy.shipIcon, mouseOverEnemy.shipHpIcon, mouseOverEnemy.shipName, mouseOverEnemy.Hp / (float)mouseOverEnemy.MaxHp);
            }
            else
            {
                infoController.NoShipSelected2();
            }
        }
    }

    public void ClearSelectionCirclesInvoke()
    {
        ClearSelectionCircles?.Invoke();
    }

    private void OnClick(InputValue value)
    {
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Ray ray = cam.ScreenPointToRay(mousePos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, clickableMask))
        {
            ClearSelectionCircles?.Invoke();

            if (spaceshipController.Count > 0)
            {
                //print("not spc");
                EnemyShip es = hit.collider.gameObject.GetComponentInParent<EnemyShip>();
                if (es != null)
                {
                    //print("es

                    foreach (var item in spaceshipController)
                    {
                        item.MoveToPosition(es.transform.position);
                        item.agent.stoppingDistance = item.fireRange / 2f;
                        item.target = es;

                        item.target?.ChangeSelection(true);
                    }
                }
                else
                {
                    //print("move");
                    int unitsInLine = (int)Mathf.Sqrt(spaceshipController.Count);
                    int line = 0;
                    int row = 0;
                    const int m = 2;
                    for (int i = 0; i < spaceshipController.Count; i++)
                    {
                        SpaceshipController item = spaceshipController[i];
                        if (item == null)
                        {
                            continue;
                        }

                        Vector3 p = hit.point + new Vector3(line - (unitsInLine / 2), 0, row) * m;
                        print(p);

                        item.MoveToPosition(p);
                        item.agent.stoppingDistance = 0;
                        item.target = null;

                        item.target?.ChangeSelection(true);

                        line++;
                        if (line > unitsInLine)
                        {
                            line = 0;
                            row++;
                        }
                    }
                }
            }
        }
    }

    private void OnCameraToPlayer()
    {
        camera.position = new Vector3(player.transform.position.x, camera.position.y, player.transform.position.z - 3f);
    }
}
