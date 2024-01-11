using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CityNode : Node
{
    protected CityEra era;
    public float[] resourcesStored; 
    public float[] resourcesConsumption; // positive is production, negative is cosumption]
    public float money;
    public float[] upgradeCost;
    public bool[] cityAspects;

    public CityEra Era
    {
        get
        {
            return era;
        }
    }

    public void Consumpiton ()
    {
        for (int i = 0; i < resourcesStored.Length; i++)
        {
            resourcesStored[i] += resourcesConsumption[i]; 
            if (resourcesStored[i] < 0) resourcesStored[i] = 0; //Limit the resource to 0
            if(i==(int)ResourceType.Minerals || i==(int)ResourceType.Metal)
            {

            }
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
        
    }

	public override void Selected()
	{
		throw new System.NotImplementedException();
	}
    public void UpgradeAspect(int index)
    {
        if (!cityAspects[index]&&(int)era<2&&resourcesStored[0] > upgradeCost[0]&& resourcesStored[1] > upgradeCost[1]&&resourcesStored[2] > upgradeCost[2]&& resourcesStored[3] > upgradeCost[3])
        {
            cityAspects[index] = true;
        }
    }
    public void UpgradeCity()
    {
        if (cityAspects[0] && cityAspects[1] && cityAspects[2] && cityAspects[3]) 
        {
            era++;
            for (int i = 0; i < cityAspects.Length; i++)
            {
                cityAspects[i] = false;
            }
        }
       ResourceNode[] resourceNodes = GetComponentsInChildren<ResourceNode>();
    }
}
