using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class MineralsMineNode : MineNode
{

    public float[] resourcesOfEachCategory; // vector with resources generated on each category, setted on inspector
    private void Awake()
    {
        type = ResourceType.Minerals;
        StartCoroutine(Generation());
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
}
