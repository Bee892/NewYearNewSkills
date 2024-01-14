using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public abstract class Node : MonoBehaviour
{
    protected LandSeaDesignation landSeaDesignation;
    public List<Node> NeighborNodes = new List<Node>();
    protected bool passable;
    protected GameObject visual;
    public NodeSettingsSO Settings;
    public NodeType Type;

    public bool Passable
    {
        get { return passable; }
    }
    
    public LandSeaDesignation LandSeaDesignation
    {
        get
        {
            return landSeaDesignation;
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

    public virtual void Setup()
    {
        landSeaDesignation = GetComponent<Tile>().type;
	}

    public abstract void Selected();
}
