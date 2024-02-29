using UnityEngine;
using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine.InputSystem;

public class UnitSelectionComponent : MonoBehaviour
{
    bool isSelecting = false;
    Vector3 mousePosition1;

    public GameObject selectionCirclePrefab;

    InputHandler inputHandler;
    ShipInfoController shipInfoController;

    SpaceshipController sc;

    private void Awake()
    {
        inputHandler = FindObjectOfType<InputHandler>();
        shipInfoController = FindObjectOfType<ShipInfoController>();
    }

    void Update()
    {
        
        // If we press the left mouse button, begin selection and remember the location of the mouse
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            sc = null;
            isSelecting = true;
            mousePosition1 = Mouse.current.position.ReadValue();

            foreach (var selectableObject in FindObjectsOfType<SpaceshipController>())
            {
                selectableObject.ChangeSelection(false);
            }
        }
        // If we let go of the left mouse button, end selection
        if (Mouse.current.leftButton.wasReleasedThisFrame)
        {
            sc = null;
            inputHandler.ClearSelectionCirclesInvoke();
            var selectedObjects = inputHandler.spaceshipController;
            selectedObjects.Clear();
            foreach (var selectableObject in FindObjectsOfType<SpaceshipController>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectedObjects.Add(selectableObject);
                    if (selectableObject.target)
                    {
                        selectableObject.target.ChangeSelection(true);
                    }
                }
            }

            // ustawianie interface
            if (selectedObjects.Count == 1)
            {
                sc = selectedObjects[0];
                shipInfoController.AssignInfo(sc.shipIcon, sc.shipHpIcon, sc.shipName, sc.Hp / (float)sc.MaxHp);
            }
            else if (selectedObjects.Count > 1)
            {
                shipInfoController.NoShipSelected();
                shipInfoController.MultipleShipsInfo(selectedObjects.Count);
            }
            else
            {
                shipInfoController.NoShipSelected();
                //TODO: nie wyświetlać xd
            }

            var sb = new StringBuilder();
            sb.AppendLine(string.Format("Selecting [{0}] Units", selectedObjects.Count));
            foreach (var selectedObject in selectedObjects)
                sb.AppendLine("-> " + selectedObject.gameObject.name);
            Debug.Log(sb.ToString());

            isSelecting = false;
        }

        // Highlight all objects within the selection box
        if (isSelecting)
        {
            foreach (var selectableObject in FindObjectsOfType<SpaceshipController>())
            {
                if (IsWithinSelectionBounds(selectableObject.gameObject))
                {
                    selectableObject.ChangeSelection(true);
                }
                else
                {
                    selectableObject.ChangeSelection(false);
                }
            }
        }

        //giga sus
        if (inputHandler.spaceshipController.Count == 0)
        {
            shipInfoController.NoShipSelected();
        }
        else if (inputHandler.spaceshipController.Count == 1)
        {
            SpaceshipController sc = inputHandler.spaceshipController[0];
            print(sc);
            if (sc != null)
            {
                shipInfoController.AssignInfo(sc.shipIcon, sc.shipHpIcon, sc.shipName, sc.Hp / (float)sc.MaxHp);
            }
        }
        else
        {
            shipInfoController.MultipleShipsInfo(inputHandler.spaceshipController.Count);
        }
    }

    public bool IsWithinSelectionBounds(GameObject gameObject)
    {
        if (!isSelecting)
            return false;

        var camera = Camera.main;
        var viewportBounds = Utils.GetViewportBounds(camera, mousePosition1, Mouse.current.position.ReadValue());
        return viewportBounds.Contains(camera.WorldToViewportPoint(gameObject.transform.position));
    }

    void OnGUI()
    {
        if (isSelecting)
        {
            // Create a rect from both mouse positions
            var rect = Utils.GetScreenRect(mousePosition1, Mouse.current.position.ReadValue());
            Utils.DrawScreenRect(rect, new Color(0.8f, 0.8f, 0.95f, 0.25f));
            Utils.DrawScreenRectBorder(rect, 2, new Color(0.8f, 0.8f, 0.95f));
        }
    }
}