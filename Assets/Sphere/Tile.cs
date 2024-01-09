using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Constants;

public class Tile : MonoBehaviour
{
    public Material material;
    public LandSeaDesignation type;

    void Start()
    {
        material = new Material(Shader.Find("Standard"));
        Renderer renderer = GetComponent<Renderer>();

        if (renderer != null)
        {
            renderer.material = material;
        }
    }
        void Update()
        {
            if (type == LandSeaDesignation.Land) material.color = Color.green;
            else material.color = Color.blue;
        }
    }

