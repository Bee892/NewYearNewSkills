using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class MineralsMineNode : MineNode
{
	private void Awake()
	{
		type = ResourceType.Minerals;
	}

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
		throw new System.NotImplementedException();
	}
}
