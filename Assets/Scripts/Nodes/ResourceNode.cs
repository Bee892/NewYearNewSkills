using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public abstract class ResourceNode : Node
{
    [Range(MinYield, MaxYield)] protected float resourceYield;
    protected ResourceType type;
    public float resourceGeneration;
    public float totalResourceGenerated;
    public float maxResourcesGenerated;
    public float timeSpanForNodeToReplenish;
    public float resourceStored;
    public CityNode City;
    public int[] eraMultipliers;

    public float ResourceYield
    {
        get
        {
            return resourceYield;
        }
    }
    public ResourceType Type
    {
        get { return type; }
    }
    public IEnumerator Generation()
    {
        if (totalResourceGenerated < maxResourcesGenerated)
        {
            resourceStored += resourceGeneration;
            totalResourceGenerated += resourceGeneration;
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
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
