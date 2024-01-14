using UnityEngine;
using static Constants;

public class Tile : MonoBehaviour
{
    Color newColor;
    public LandSeaDesignation type;
    public Renderer myRenderer;

    public void Start()
    {
        ChangeMaterialColor();
    }
    void ChangeMaterialColor()
    {
        // Get the renderer component of the child object
        //Renderer renderer = GetComponentInChildren<Renderer>();
        if (type == LandSeaDesignation.Land) newColor = Color.green; else newColor= Color.blue;

        if (myRenderer == null)
        {
            myRenderer = GetComponentInChildren<Renderer>();
        }

        if (myRenderer != null)
        {
            // Create a new material to avoid modifying the shared material
            Material newMaterial = new Material(myRenderer.material);

            // Set the new color
            newMaterial.color = newColor;

            // Assign the new material to the renderer
            myRenderer.material = newMaterial;
        }
        else
        {
            Debug.LogError("Renderer component not found on the child object.");
        }
    }
}

