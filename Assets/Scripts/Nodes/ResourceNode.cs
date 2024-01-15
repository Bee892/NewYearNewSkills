using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
#if UNITY_EDITOR
using UnityEditor;
#endif
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
    public GameObject[] visuals;
    public GameObject[] seaVisuals;
    public LandSeaDesignation landOrSea;
    public int eraIndex = -1;
    public GameObject GenericNodePrefabs;
    public bool isOriginal = true;
    public string prefabPathName;


    public override void Setup()
    {
        base.Setup();
        Type = NodeType.Resource;
        passable = true;
        landOrSea = GetComponentInParent<Tile>().type;
        updateVisuals();
        GameObject obj = null;
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        nodeManager = manager.GetComponent<NodeManager>();
        if (isOriginal)
        {
            obj = InstantiatePrefab();
            obj.GetComponent<ResourceNode>().isOriginal = false;
            obj.transform.position = this.gameObject.transform.position;
            obj.transform.rotation = this.gameObject.transform.rotation;
            obj.transform.parent = this.gameObject.transform;
            obj.GetComponent<ResourceNode>().nodeStart();
        }
    }

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
        if (!isOriginal)
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
    }

    public IEnumerator nodeReplenish()
    {
        yield return new WaitForSeconds(timeSpanForNodeToReplenish);
        totalResourceGenerated = 0;
    }
    public void activateNode() //activate node when trade route is created
    {
        if (!isOriginal)
        {
            StartCoroutine(Generation());
        }
    }
    // Start is called before the first frame update
    void Start()
    {
            
            
    }
    public void nodeStart()
    {
        landOrSea = GetComponentInParent<Tile>().type;
        updateVisuals();
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        nodeManager = manager.GetComponent<NodeManager>();
    }
    public void OnMouseDown()
    {
        if (!isOriginal)
        {
            if (nodeManager.isSelectingTarget)
            {
                nodeManager.target = this.gameObject;
                nodeManager.isSelectingTarget = false;

            }
        }
    }

    public void OnEnable()
    {
        CityNode.OnCityUpgrade += updateVisuals;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    GameObject InstantiatePrefab()
    {

       
        return Instantiate(nodeManager.ResourceNodePrefab[(int)resourceType]);

    }

    public void updateVisuals()
    {
        if (!isOriginal)
        {
            if (landOrSea == LandSeaDesignation.Land)
            {
                if (eraIndex >= 0)
                    visuals[eraIndex].SetActive(false);
                eraIndex++;
                visuals[eraIndex].SetActive(true);
            }
            else
            {
                if (eraIndex >= 0)
                    seaVisuals[eraIndex].SetActive(false);
                eraIndex++;
                seaVisuals[eraIndex].SetActive(true);
            }
        }
    }
}
