using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectPlacer : MonoBehaviour
{
    public InventoryManager inventoryManager;
    public GameObject[] sizePrefabs;
    public int selectedPrefabIndex = 0;
    public LayerMask groundLayer;
    private GameObject selectedObject;
    private bool isDragging = false;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //choose Object size
        if (Input.GetKeyDown(KeyCode.Alpha1)) selectedPrefabIndex = 0;
        if (Input.GetKeyDown(KeyCode.Alpha2)) selectedPrefabIndex = 1;
        if (Input.GetKeyDown(KeyCode.Alpha3)) selectedPrefabIndex = 2;

        if (!EventSystem.current.IsPointerOverGameObject())
        {
            //place object
            if (inventoryManager.HaveSelectedItem() && selectedObject == null && Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
                {
                    GameObject placed = Instantiate(sizePrefabs[selectedPrefabIndex], hit.point, Quaternion.identity);
                }
                inventoryManager.GetSelectedItem(true);
            }

            //select Object
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f))
                {
                    if (hit.collider.CompareTag("Placeable"))
                    {
                        if (selectedObject != null)
                        {
                            PlantGrowth pg = selectedObject.GetComponent<PlantGrowth>();
                            if (pg != null) pg.isSelected = false;
                        }

                        selectedObject = hit.collider.gameObject;

                        PlantGrowth newPG = selectedObject.GetComponent<PlantGrowth>();
                        if (newPG != null) newPG.isSelected = true;
                    }
                }
            }

            // collect object
            // if (selectedObject != null && Input.GetKeyDown(KeyCode.P))
            // {
            //     PlantGrowth pg = selectedObject.GetComponent<PlantGrowth>();

            // }

            //drag Object
            if (selectedObject != null && Input.GetMouseButton(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
                {
                    selectedObject.transform.position = hit.point;
                    isDragging = true;
                }
            }

            //release Object
            if (Input.GetMouseButtonUp(0))
            {
                isDragging = false;
                selectedObject = null;
            }

            //remove Object
            if (selectedObject != null && Input.GetKeyDown(KeyCode.Backspace))
            {
                Destroy(selectedObject);
                selectedObject = null;
            }
        }
    }
}
