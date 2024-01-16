using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
using UnityEngine;
using static Constants;

public class Globe : MonoBehaviour
{
    private static Globe instance;
    private bool rotating;
    private float rotationSensitivity;
    private Vector3 mousePos;
    private Vector3 deltaMousePos;
    private List<Node> nodes;
    public NodeManager nodeManager;
    public GameObject[][] transportations;
    

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

    public static Globe Instance
    {
        get
        {
            return instance;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
		Settings settings = GameManager.Instance.Settings;
		rotationSensitivity = settings.RotationSpeed;
        instance = this;
        Setup(settings.NumStartCities,settings.NumStartFuel, settings.NumStartFood, settings.NumStartMetal, settings.NumStartMinerals);
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
        List<Tile> objs = FindObjectsByType<Tile>(FindObjectsSortMode.None).ToList();
        List<int> takenObjIndices = new List<int>();
        System.Random rnd = new System.Random();

        List<CityNode> cityNodes = CreateRandomNodes<CityNode>(numOfCities, ref objs, ref takenObjIndices, true, false);
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

        NodeManager.Instance.Setup(nodes, cityNodes);
	}

    private List<T> CreateRandomNodes<T>(int n, ref List<Tile> objs, ref List<int> takenObjIndices, bool allowLand = true, bool allowWater = true) where T : Node
    {
        List<T> nodeList = new List<T>();
        System.Random rnd = new System.Random();
		for (int i = 0; i < n; i++)
		{
			int index = rnd.Next(objs.Count);
			while (takenObjIndices.Contains(index) || (!allowLand && objs[index].type == LandSeaDesignation.Land) || (!allowWater && objs[index].type == LandSeaDesignation.Sea))
			{
				index = rnd.Next(objs.Count);
			}
			nodeList.Add(objs[index].AddComponent<T>());
			takenObjIndices.Add(index);
		}

        return nodeList;
	}

   
}
