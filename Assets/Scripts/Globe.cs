using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class Globe : MonoBehaviour
{
    public Settings MainSettings;
    
    private bool rotating;
    private float rotationSensitivity;
    private Vector3 mousePos;
    private Vector3 deltaMousePos;
    private List<Node> nodes;

    public bool Rotating
    {
        get
        {
            return rotating;
        }
        set
        {
            if (value && !rotating)
            {
                mousePos = Input.mousePosition;
                deltaMousePos = Vector3.zero;
            }
            rotating = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rotationSensitivity = MainSettings.RotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Rotating)
        {
            if (mousePos != Input.mousePosition)
            {
                deltaMousePos = Input.mousePosition - mousePos;
                if (Vector3.Dot(transform.up, Vector3.up) >= 0)
                {
					transform.Rotate(transform.up, -Vector3.Dot(deltaMousePos * rotationSensitivity, Camera.main.transform.right), Space.World);
				}
                else
                {
					transform.Rotate(transform.up, Vector3.Dot(deltaMousePos * rotationSensitivity, Camera.main.transform.right), Space.World);
				}
                
                transform.Rotate(Camera.main.transform.right, Vector3.Dot(deltaMousePos * rotationSensitivity, Camera.main.transform.up), Space.World);
            }

            mousePos = Input.mousePosition;
        }
    }

    public void Setup(int numOfCities, int numOfFuel, int numOfFood, int numOfMetal, int numOfMinerals)
    {
        List<GameObject> objs = GameObject.FindGameObjectsWithTag("Node").ToList();
        List<int> takenObjIndices = new List<int>();
        System.Random rnd = new System.Random();

        CreateRandomNodes<CityNode>(numOfCities, ref objs, ref takenObjIndices, true, false);
		CreateRandomNodes<FuelNode>(numOfFuel, ref objs, ref takenObjIndices);
		CreateRandomNodes<FarmNode>(numOfFood, ref objs, ref takenObjIndices);
		CreateRandomNodes<MetalMineNode>(numOfMetal, ref objs, ref takenObjIndices, true, false);
		CreateRandomNodes<MineralsMineNode>(numOfMinerals, ref objs, ref takenObjIndices, true, false);

        List<Node> nodes = new List<Node>();

        for (int i = 0; i < objs.Count; i++)
        {
            if (!takenObjIndices.Contains(i))
            {
				nodes.Add(objs[i].AddComponent<BarrenNode>());
			}
            else
            {
                nodes.Add(objs[i].GetComponent<Node>());
            }
        }

        NodeManager.Instance.Setup(nodes);
	}

    private void CreateRandomNodes<T>(int n, ref List<GameObject> objs, ref List<int> takenObjIndices, bool allowLand = true, bool allowWater = true) where T : Node
    {
        System.Random rnd = new System.Random();
		for (int i = 0; i < n; i++)
		{
			int index = rnd.Next(objs.Count);
			while (takenObjIndices.Contains(index) || (!allowLand && objs[index].GetComponent<Land>() != null) || (!allowWater && objs[index].GetComponent<Water>() != null))
			{
				index = rnd.Next(objs.Count);
			}
			objs[index].AddComponent<T>();
			takenObjIndices.Add(index);
		}
	}
}
