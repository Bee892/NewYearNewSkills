using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class MetalMineNode : MineNode
{
	private void Awake()
	{
		resourceType = ResourceType.Metal;
        StartCoroutine(Generation());
		prefabPathName = "Assets/Nodes Prefabs/Mine Node Prefab.prefab";
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
		base.Setup();
		resourceType = ResourceType.Metal;
	}
}
