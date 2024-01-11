using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Constants;

public class Transport : MonoBehaviour
{
    protected float timeBetweenMovements;
    protected TransportRoute route;
    protected Node currentNode;
    //protected Node destination;
    protected int currentNodeIndex;
    protected bool reverseRoute;
    protected float timeSinceLastMove;
    protected bool arrived;
    protected float timeSinceArrival;
    protected float timeSinceLastArrivalCheck;
    protected System.Random rnd = new System.Random();
    [SerializeField] protected GameObject visual;
    public TransportSO TransportSettings;

	private void Awake()
	{
		visual = Instantiate(TransportSettings.visual, transform);
	}

	// Start is called before the first frame update
	void Start()
    {
        timeSinceArrival = 0;
        timeSinceLastArrivalCheck = 0;
        timeSinceLastMove = 0;
        reverseRoute = false;
        currentNode = route.Nodes[0];
        currentNodeIndex = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (arrived)
        {
            timeSinceArrival += Time.deltaTime;
            timeSinceLastArrivalCheck += Time.deltaTime;
            if (timeSinceLastArrivalCheck >= 1)
            {
                timeSinceLastArrivalCheck -= 1;
                if (rnd.Next(1, 101) <= ChanceOfDeparturePerSecond * 100)
                {
                    Depart();
                }
            }
        }
        else
        {
            timeSinceLastMove += Time.deltaTime;
            if (timeSinceLastMove >= timeBetweenMovements)
            {
                Move();
            }
        }
    }

    protected void Depart()
    {
        arrived = false;
        reverseRoute = !reverseRoute;
        timeSinceLastMove = 0;
        timeSinceArrival = 0;
        timeSinceLastArrivalCheck = 0;
        //destination = reverseRoute ? route.Nodes[0] : route.Nodes[route.Nodes.Count - 1];
        visual.SetActive(true);
        AngleTowardNextNode();
    }

    protected void Arrive()
    {
        arrived = true;
        visual.SetActive(false);
    }

	protected void Move()
	{
		timeSinceLastMove = 0;
        currentNodeIndex += reverseRoute ? -1 : 1;
        currentNode = route.Nodes[currentNodeIndex];
        transform.parent = currentNode.transform;
        transform.SetAsFirstSibling();
        if (currentNodeIndex == 0 || currentNodeIndex == route.Nodes.Count - 1)
        {
            Arrive();
        }
        else
        {
            AngleTowardNextNode();
        }
	}

    protected void AngleTowardNextNode()
    {
        Transform nextT = route.Nodes[currentNodeIndex + (reverseRoute ? -1 : 1)].transform;
        transform.LookAt(nextT);
		transform.up = nextT.up;
	}
}
