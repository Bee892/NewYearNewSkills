using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public abstract class ResourceNode : Node
{
    [Range(MinYield, MaxYield)] protected float resourceYield;
    protected ResourceType resourceType;
    public Era era;
    public float resourceGeneration;
    public float resourceTransmitted;
    public float totalResourceGenerated;
    public float maxResourcesGenerated;
    public float timeSpanForNodeToReplenish;
    public float resourceStored;
    public CityNode City;
    public int[] eraMultipliers = { 1, 5, 12 };
    public bool used;
    public NodeManager nodeManager;

    public float ResourceYield
    {
        get
        {
            return resourceYield;
        }
    }
    public ResourceType ResourceType
    {
        get { return resourceType; }
    }
    public IEnumerator Generation()
    {
        if (totalResourceGenerated < maxResourcesGenerated)
        {
            resourceStored += resourceGeneration * eraMultipliers[(int)City.CityEra];
            totalResourceGenerated += resourceGeneration;
            resourceStored -= resourceTransmitted;
            
        }
        else
        {
            StartCoroutine(nodeReplenish());
        }
        yield return new WaitForSeconds(1f);
        StartCoroutine(Generation());
    }

    public IEnumerator nodeReplenish()
    {
        yield return new WaitForSeconds(timeSpanForNodeToReplenish);
        totalResourceGenerated = 0;
    }
    public void activateNode() //activate node when trade route is created
    {
        StartCoroutine(Generation());
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void OnMouseDown()
    {
        if (nodeManager.isSelectingTarget)
        {
            nodeManager.target = this.gameObject;
            nodeManager.isSelectingTarget = false;

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

	public override void Setup()
	{
		base.Setup();
        Type = NodeType.Resource;
        passable = true;
	}
}
