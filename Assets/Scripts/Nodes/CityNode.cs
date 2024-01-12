using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Constants;

public class CityNode : Node
{

    public float[] resourcesStored;
    public float[] resourcesConsumption; // positive is production, negative is cosumption]
    public float[] initialResourcesConsumption;
    public float money;
    public float[] upgradeCost;
    public bool[] cityAspects;
    public float moneyIncrease;
    protected Era cityEra;
    public int[] eraMultipliers = { 1, 4, 10 };
    public bool isCityAlive = true;
    public float cityNotCrumbleThreshold;
    public float cityRevivalThreshold;

    public Era CityEra
    {
        get
        {
            return cityEra;
        }
    }

    public void Consumpiton()
    {
        for (int i = 0; i < resourcesStored.Length; i++)
        {
            resourcesStored[i] += resourcesConsumption[i] - initialResourcesConsumption[i];
            
            if (i == (int)ResourceType.Minerals || i == (int)ResourceType.Metal)
            {
                if (resourcesStored[i] > 0) money += moneyIncrease * initialResourcesConsumption[i];
            }
            else
            {
                if (resourcesStored[i] == 0&& isCityAlive) StartCoroutine(Crumbling());
            }
            if (resourcesStored[i] < 0) resourcesStored[i] = 0; //Limit the resource to 0
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        InvokeRepeating("Consumpiton", 0f, 1f);

    }

    // Update is called once per frame
    void Update()
    {
        if(!isCityAlive)
        {
            if (resourcesStored[(int)ResourceType.Fuel] > cityRevivalThreshold && resourcesStored[(int)ResourceType.Food] > cityRevivalThreshold)
            isCityAlive = true;
        }
    }

    public IEnumerator Crumbling ()
    {
        yield return new WaitForSeconds(100f);
        //implement warning that the city is going to crumble
        if (resourcesStored[(int)ResourceType.Fuel] < cityNotCrumbleThreshold || resourcesStored[(int)ResourceType.Food] < cityNotCrumbleThreshold) 
        isCityAlive = false;
    }

    public override void Selected()
    {
        throw new System.NotImplementedException();
    }

    public void UpgradeAspect(int index)
    {
        if (!cityAspects[index] && (int)cityEra < 2 && resourcesStored[0] > upgradeCost[0] && resourcesStored[1] > upgradeCost[1] && resourcesStored[2] > upgradeCost[2] && resourcesStored[3] > upgradeCost[3])
        {
            cityAspects[index] = true;
        }
    }
    public void UpgradeCity()
    {
        if (cityAspects[0] && cityAspects[1] && cityAspects[2] && cityAspects[3])
        {
            cityEra++;
            for(int i = 0; i <initialResourcesConsumption.Length;i++)
                initialResourcesConsumption[i] = initialResourcesConsumption[i] * eraMultipliers[(int)cityEra];
            for (int i = 0; i < cityAspects.Length; i++)
            {
                cityAspects[i] = false;
            }
        }
        ResourceNode[] resourceNodes = GetComponentsInChildren<ResourceNode>();
    }

    public void CreateTradeRouteBetweenCities(CityNode destination, float quantity, ResourceType type)
    {
        //implement code that creates a trade route
        resourcesConsumption[(int)type] -= quantity;
        destination.resourcesConsumption[(int)type] += quantity;
    }
    public void DeleteTradeRouteBetweenCities()
    {

    }
    public void CreateTradeRouteBetweenCityAndResourceNode (ResourceNode destination)
    {
        float quantity = 0;
        //implement code that creates a trade route
        if (!destination.used)
        {
            destination.City = this;
            ResourceType type = destination.Type;
            resourcesConsumption[(int)type] += quantity;
            destination.resourceTransmitted += quantity;
            destination.used = true;
        }
    }

    public override void Setup()
    {
        throw new System.NotImplementedException();
    }
    
}