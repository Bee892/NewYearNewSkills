using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.Events;
using static Constants;

public class TransportRoute : MonoBehaviour
{
    public List<Node> Nodes;
    public Dictionary<Node, Transport> Vehicles;
    private bool persistent;
	private Transport currentVehicle;
	private bool reverseRoute;
	private bool arrived;
	private float timeSinceArrival;
	private float timeSinceLastArrivalCheck;
	private System.Random rnd = new System.Random();
	private Action<Node, Node> arrivalCallback;

	public bool ReverseRoute
	{
		get { return reverseRoute; }
	}

	/// <summary>
	/// Includes start and end points.
	/// </summary>
	public int RouteLength
    {
        get
        {
            return Nodes.Count;
        }
    }

	private void Update()
	{
		if (arrived)
		{
			if (persistent)
			{
				timeSinceArrival += Time.deltaTime;
				timeSinceLastArrivalCheck += Time.deltaTime;
				if (timeSinceLastArrivalCheck >= 1 && timeSinceArrival >= GameManager.Instance.Settings.MinTimeArrivalToDeparture)
				{
					timeSinceLastArrivalCheck -= 1;
					if (timeSinceArrival >= GameManager.Instance.Settings.MaxTimeArrivalToDeparture || rnd.Next(1, 101) <= ChanceOfDeparturePerSecond * 100)
					{
						Depart();
					}
				}
			}
			else
			{
				ArriveFinal();
			}
		}
	}

	/// <summary>
	/// 
	/// </summary>
	/// <param name="persist">Does the route keep going back and forth indefinitely?</param>
	/// <param name="nodes">A list of all nodes that make up the route, in order from start node to end node.</param>
	/// <param name="vehicles">The start nodes where all of the vehicles begin in the route.</param>
	/// <param name="arrivalCallback">The function that's invoked when the final vehicle arrives at the destination.</param>
    public void Setup(bool persist, List<Node> nodes, Dictionary<Node, Transport> vehicles, Action<Node, Node> arrivalCallback = null)
    {
		Nodes = nodes;
		persistent = persist;
		Vehicles = vehicles;
		this.arrivalCallback = arrivalCallback;
	}

	public void EndVehicleRoute(Transport vehicle, Action callback = null)
    {
        Node node = Nodes[Nodes.IndexOf(vehicle.CurrentNode) + (reverseRoute ? -1 : 1)];
        Transport newVehicle = Vehicles[node];
        if (newVehicle != null)
        {
			newVehicle.transform.parent = node.transform;
			newVehicle.transform.SetAsFirstSibling();
			currentVehicle = newVehicle;
            newVehicle.Initiate();
		}
		else
		{
			arrived = true;
		}

		if (callback != null)
		{
			callback.Invoke();
		}
	}

	protected void Depart()
	{
		arrived = false;
		reverseRoute = !reverseRoute;
		timeSinceArrival = 0;
		timeSinceLastArrivalCheck = 0;
		currentVehicle.gameObject.SetActive(true);
		currentVehicle.AngleTowardNextNode();
	}

	private void ArriveFinal()
	{
		if (arrivalCallback != null)
		{
			arrivalCallback.Invoke(Nodes[0], Nodes[Nodes.Count - 1]);
		}
		foreach (KeyValuePair<Node, Transport> pair in Vehicles)
		{
			Destroy(Vehicles[pair.Key].gameObject);
		}
		Destroy(gameObject);
	}
}
