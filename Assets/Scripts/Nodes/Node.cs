using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public abstract class Node : MonoBehaviour
{
    protected LandSeaDesignation landSeaDesignation;
    
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

    public abstract void Selected();
}
