using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class Transport : MonoBehaviour
{
    protected List<Node> nodes;
	protected TransportType type;
    protected TransportRoute route;
    protected Node currentNode;
    protected int currentNodeIndex;
	protected float timeSinceLastMove;
	private float timeBetweenMovements;
	CityNode city;
	[SerializeField] protected GameObject[] visual;
    public TransportSO TransportSettings;

    public Node CurrentNode { get { return currentNode; } }

	private void Awake()
	{
		for (int i = 0; i < visual.Length; i++)
		{
			visual[i] = Instantiate(TransportSettings.visual[i], transform);
		}
	}

	// Start is called before the first frame update
	void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		timeSinceLastMove += Time.deltaTime;
		if (timeSinceLastMove >= timeBetweenMovements)
		{
			Move();
		}
	}

    public void Setup(List<Node> nodes, TransportType type)
    {
		this.nodes = nodes;
		this.type = type;
        timeBetweenMovements = TransportSettings.SecondsBetweenMoves;
	}

    public void Initiate()
    {
		timeSinceLastMove = 0;
        currentNodeIndex = route.ReverseRoute ? nodes.Count - 1 : 0;
		currentNode = nodes[currentNodeIndex];
		gameObject.SetActive(true);
	}

	public void Move()
	{
		if (currentNodeIndex == 0 || currentNodeIndex == route.Nodes.Count - 1)
		{
			route.EndVehicleRoute(this, () =>
			{
				gameObject.SetActive(false);
			});
		}
		else
		{
			timeSinceLastMove = 0;
			currentNodeIndex += route.ReverseRoute ? -1 : 1;
			currentNode = nodes[currentNodeIndex];
			transform.parent = currentNode.transform;
			transform.SetAsFirstSibling();
			if (type == TransportType.Plane)
			{
				transform.localPosition = Vector3.zero;
			}
			AngleTowardNextNode();
		}
	}

    public void AngleTowardNextNode()
    {
        Transform nextT = route.Nodes[currentNodeIndex + (route.ReverseRoute ? -1 : 1)].transform;
        transform.LookAt(nextT);
		transform.up = nextT.up;
		if (type == TransportType.Plane)
		{
			transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y + 4, transform.localPosition.z);
		}
	}
}
