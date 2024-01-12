using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public class CityNode : Node
{
    protected Era era;

    public Era CityEra
    {
        get
        {
            return cityEra;
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
}
