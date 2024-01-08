using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

public abstract class ResourceNode : Node
{
    [Range(MinYield, MaxYield)] protected float resourceYield;
    protected ResourceType type;

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

    public ResourceYieldCategory ResourceYieldCategory
    {
        get
        {
            ResourceYieldCategory prevCat = 0;
            foreach (KeyValuePair<ResourceYieldCategory, float> cat in ResourceYieldMinValues)
            {
                if (cat.Value > resourceYield)
                {
                    break;
                }

                prevCat = cat.Key;
            }

            return prevCat;
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
}
