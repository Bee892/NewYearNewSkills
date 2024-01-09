using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class NodeManager : MonoBehaviour
{
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
}
