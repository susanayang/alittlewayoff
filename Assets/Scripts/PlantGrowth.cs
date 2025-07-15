using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantGrowth : MonoBehaviour
{
    public Mesh[] growthMeshes;
    public Material[] growthMaterials;
    public Mesh[] produceMeshes; //0 = fruit, 1 = flower
    public Material[] produceMaterials; 
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;

    public float[] stageDurations;
    public float produceCooldown;

    public bool isSelected = false;

    public Item fruitItem;
    public Item flowerItem;
    public static event System.Action<Item> OnItemPicked;

    // Start is called before the first frame update
    void Start()
    {
        meshFilter = GetComponentInChildren<MeshFilter>();
        meshRenderer = GetComponentInChildren<MeshRenderer>();

        //Debug.Log("PlantGrowth Start called on " + gameObject.name);

        StartCoroutine(Grow());
    }

    IEnumerator Grow()
    {
        for (int i = 0; i < growthMaterials.Length; i++)
        {
            // meshFilter.mesh = growthMeshes[i];
            meshRenderer.material = growthMaterials[i]; 
            //Debug.Log($"Switched to growth material {i}");
            yield return new WaitForSeconds(stageDurations[i]);
        }

        StartCoroutine(Produce());
    }

    IEnumerator Produce()
    {
        while (true)
        {
            bool produceFruit = Random.value > 0.5f;

            int meshIndex = produceFruit ? 0 : 1;
            // meshFilter.mesh = produceMeshes[meshIndex];
            meshRenderer.material = produceMaterials[meshIndex];

            //Debug.Log($"Producing {(produceFruit ? "fruit" : "flower")}");

            //placeholder pick
            bool picked = false;
            while (!picked)
            {
                // Wait until this plant is selected and player presses pick key
                if (isSelected && Input.GetKeyDown(KeyCode.P)) // Replace with proper input later
                {
                    picked = true;
                    Item item = produceFruit ? fruitItem : flowerItem;
                    OnItemPicked?.Invoke(item);

                    // meshFilter.mesh = growthMeshes[growthMeshes.Length - 1];
                    meshRenderer.material = growthMaterials[growthMaterials.Length - 1];

                    isSelected = false; // Deselect after picking

                    yield return new WaitForSeconds(produceCooldown);
                }
                yield return null;
            }

        }
    }
}
