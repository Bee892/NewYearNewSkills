using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
    public List<Node> nodes = new List<Node>();
    public List<CityNode> cityNodes = new List<CityNode>();
    public bool isSelectingTarget;
    public GameObject target;
    public GameObject CityNodePrefab;
    public GameObject[] ResourceNodePrefab;
    private class LinkedNode
    {
        public Node n;
        public Node parent;

        public LinkedNode(Node givenNode, Node parentNode)
        {
            n = givenNode;
            parent = parentNode;
        }
    }

    private static NodeManager instance;

    public static NodeManager Instance
    {
        get
        {
            return instance;
        }
    }

	private void Awake()
	{
		if (instance == null)
		{
			instance = this;
		}
		else
		{
			Destroy(this);
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public LinkedList<Node> GetShortestPath(Node from, Node to)
    {
		LinkedList<Node> shortestPath = new LinkedList<Node>();

		if (from == to)
        {
            return shortestPath;
        }

        List<Node> closed = new List<Node>();
        closed.Add(from);
		Queue<LinkedNode> fringe = new Queue<LinkedNode>();

		foreach (Node n in from.NeighborNodes)
        {
            fringe.Enqueue(new LinkedNode(n, from));
        }

		while (fringe.Count > 0)
        {
            LinkedNode currentNode = fringe.Dequeue();
			if (closed.Contains(currentNode.n))
			{
                continue;
			}

			if (currentNode.n == to)
			{
				while (currentNode.n != from)
				{
					shortestPath.AddFirst(currentNode.n);
				}
				return shortestPath;
			}

			closed.Add(currentNode.n);

			if (!currentNode.n.Passable)
            {
                continue;
            }

			foreach (Node n in currentNode.n.NeighborNodes)
            {
                fringe.Enqueue(new LinkedNode(n, currentNode.n));
            }
        }

        return null;
    }

    public void Setup(List<Node> newNodes, List<CityNode> cityNodes)
    {
        nodes = newNodes;
        this.cityNodes = cityNodes;

		foreach (Node n in nodes)
		{
			int neighborCount = 0;
			foreach (Node n2 in nodes)
			{
                if (Math.Sqrt(Math.Pow((n2.transform.position.x - n.transform.position.x), 2) + Math.Pow((n2.transform.position.y - n.transform.position.y), 2) + Math.Pow((n2.transform.position.z - n.transform.position.z), 2)) <= n.GetComponentInChildren<MeshRenderer>().bounds.size.y && n2 != n)
				{
					n.NeighborNodes.Add(n2);
                    neighborCount++;
				}
				
				if (neighborCount == 6)
				{
					break;
				}
			}

			n.Setup();
            n.transform.up = n.GetComponent<Tile>().transform.up;
		}
	}
}
