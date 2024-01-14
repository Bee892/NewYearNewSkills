using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarrenNode : Node
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

	public override void Selected()
	{
		throw new System.NotImplementedException();
	}

	public override void Setup()
	{
        base.Setup();
        Type = Constants.NodeType.Barren;

        passable = true;
	}
}
