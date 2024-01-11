using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransportRoute
{
    public List<Node> Nodes;
    public Transport Vehicle;

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

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
