using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Constants;
using TMPro;
using UnityEngine.UI;

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
    public float[] cityRevivalThreshold;
    public TextMeshProUGUI moneyText;
    public TextMeshProUGUI foodText;
    public TextMeshProUGUI fuelText;
    public TextMeshProUGUI mineralsText;
    public TextMeshProUGUI metalText;
    public GameObject createTradeRouteUI;
    private bool isSelectingTarget;
    public TextMeshProUGUI message;
    private GameObject target;
    public Canvas canvas;
    public TextMeshProUGUI value;
    private float valueToBeTransfered;
    public InputField inputField;
    private string userInput;
    public GameObject[] icons;
    public GameObject[] icons2;
    private ResourceType resourceType;
    public bool isSelectingResourceType;
    public bool IsSelectingVehicle;
    public GameObject[] vehiclesButtons;
    public TextMeshProUGUI upgradeCostText;
    public TextMeshProUGUI[] upgradeTexts;
    public GameObject confirmButton;
    public int aspectIndex;
    public bool isSeingUpgrades;
    public GameObject cancelButton;
    public GameObject[] icons3;
    public TextMeshProUGUI[] valuesThresHold;
    public TextMeshProUGUI deathMessage;
    public TextMeshProUGUI valuesText;
    private bool isCrumbling;
    public GameObject[] buttons;
    public NodeManager nodeManager;
    public Globe globe;
    public TransportType transportType;
    public bool isSelectingPersistence;
    public bool persistence;
    public GameObject persistentText;
    public GameObject yesButton;
    public GameObject noButton;
    //create variable for the vehicle
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
                isCrumbling = true;
                if (resourcesStored[i] == 0&& isCityAlive) StartCoroutine(Crumbling());
            }
            if (resourcesStored[i] < 0) resourcesStored[i] = 0; //Limit the resource to 0
        }
    }

    public void OnMouseDown()
    {
        if (!nodeManager.isSelectingTarget)
        {
            foreach(CityNode citynode in nodeManager.cityNodes) citynode.canvas.gameObject.SetActive(false);
            canvas.gameObject.SetActive(true);           
        }
        else
        {
            nodeManager.target = this.gameObject;
            nodeManager.isSelectingTarget = false;
   
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

            if (resourcesStored[(int)ResourceType.Fuel] > cityRevivalThreshold[0] && resourcesStored[(int)ResourceType.Food] > cityRevivalThreshold[1])
            isCityAlive = true;
            for(int i =0;i<buttons.Length;i++)
            {
                buttons[i].SetActive(false);
            }
        }
        if(!isCrumbling && isCityAlive)
        {
            for (int i = 0; i < 2; i++)
            {
                icons3[i].SetActive(false);
                valuesThresHold[i].gameObject.SetActive(false);
                valuesText.gameObject.SetActive(false);
                deathMessage.gameObject.SetActive(false);
            }
            for (int i = 0; i < buttons.Length; i++)
            {
                buttons[i].SetActive(true);
            }
        }
        
    }

    public void Death()
    {
        isCityAlive = false;
        for (int i = 0; i < 2; i++)
        {
            icons3[i].SetActive(true);
            valuesThresHold[i].gameObject.SetActive(true);
            valuesThresHold[i].text = cityRevivalThreshold[i].ToString();
            valuesText.gameObject.SetActive(true);
            deathMessage.gameObject.SetActive(true);
            valuesText.text = "Requeriments:";
            deathMessage.text = "City is Dead!";
        }
    }
    public IEnumerator Crumbling ()
    {
        for (int i = 0; i < 2; i++)
        {
            icons3[i].SetActive(true);
            valuesThresHold[i].gameObject.SetActive(true);
            valuesThresHold[i].text = cityNotCrumbleThreshold.ToString();
            valuesText.gameObject.SetActive(true);
            deathMessage.gameObject.SetActive(true);
            valuesText.text = "Requeriments:";
            deathMessage.text = "City is Crumbling!";
        }
        yield return new WaitForSeconds(100f);
        
        if (resourcesStored[(int)ResourceType.Fuel] < cityNotCrumbleThreshold || resourcesStored[(int)ResourceType.Food] < cityNotCrumbleThreshold) 
        Death();
    }

    public override void Selected()
    {
        throw new System.NotImplementedException();
    }

    public void UpgradeAspect(int index)
    {
        if (!cityAspects[index] && (int)cityEra < 2 && resourcesStored[0] > upgradeCost[0] && resourcesStored[1] > upgradeCost[1] && resourcesStored[2] > upgradeCost[2] && resourcesStored[3] > upgradeCost[3] && money > upgradeCost[4])
        {
            cityAspects[index] = true;
            for(int i = 0; i < 4; i++)
            {
                resourcesStored[i] -= upgradeCost[i];
            }
            money -= upgradeCost[4];
        }
    }
    void UpgradeCity()
    {
        if (cityAspects[0] && cityAspects[1] && cityAspects[2] && cityAspects[3])
        {
            cityEra++;
            for (int i = 0; i < initialResourcesConsumption.Length; i++) {
                initialResourcesConsumption[i] = initialResourcesConsumption[i] * eraMultipliers[(int)cityEra];
                upgradeCost[i] = upgradeCost[i] * eraMultipliers[(int)cityEra];
                }
            upgradeCost[4] = upgradeCost[4] * eraMultipliers[(int)cityEra];
            for (int i = 0; i < cityAspects.Length; i++)
            {
                cityAspects[i] = false;
            }
        }
        ResourceNode[] resourceNodes = GetComponentsInChildren<ResourceNode>();
    }

    public void CreateTradeRouteBetweenCities(CityNode c, Node n, Resource r)
    {
        
        c.resourcesStored[(int)r.Type] -= r.Quantity; 
        n.gameObject.GetComponent<CityNode>().resourcesStored[(int)r.Type] += r.Quantity;
 
    }
    public void CreateTradeRouteBetweenCityAndResourceNode (ResourceNode destination)
    {
        float cost = 0;
        float quantity = 0;
        //implement code that creates a trade route
        if (!destination.used)
        {
            TransportRoute TradeRoute = globe.GenerateTradeRoute(this, destination, transportType, true, arrivalCallBack);
            destination.City = this;
            ResourceType type = destination.ResourceType;
            resourcesConsumption[(int)type] += quantity;
            destination.resourceTransmitted += quantity;
            destination.used = true;
            money -= cost;
        }
    }

    private void FixedUpdate()
    {
        moneyText.text = money.ToString();
        foodText.text = resourcesStored[(int)ResourceType.Food].ToString();
        fuelText.text = resourcesStored[(int)ResourceType.Fuel].ToString();
        mineralsText.text = resourcesStored[(int)ResourceType.Minerals].ToString();
        metalText.text = resourcesStored[(int)ResourceType.Metal].ToString();
    }

    public void createTradeRoute()
    {
        
        StartCoroutine(tradeRouteEnumarator());
    }

    IEnumerator tradeRouteEnumarator()
    {
        cancelButton.SetActive(true);
        target = null;
        valueToBeTransfered = 0;
        isSelectingTarget = true;
        message.gameObject.SetActive(true);
        message.text = "Select a Node";
        yield return new WaitUntil(() => !isSelectingTarget);
        message.gameObject.SetActive(false);
        if(nodeManager.target.GetComponent<ResourceNode>() != null)
        {
            CreateTradeRouteBetweenCityAndResourceNode(nodeManager.target.GetComponent<ResourceNode>());
            cancelButton.SetActive(false);
        }
        if(nodeManager.target.GetComponent<CityNode>() != null)
        {
            for (int i = 0; i < icons.Length; i++) icons[i].SetActive(true);
            isSelectingResourceType = true;
            yield return new WaitUntil(() => !isSelectingResourceType);
            for (int i = 0; i < icons.Length; i++) icons[i].SetActive(false);
            //implement method to get player input for the value to be transferred
            IsSelectingVehicle = true;
            for (int i = 0; i < vehiclesButtons.Length; i++) vehiclesButtons[i].SetActive(true);
            yield return new WaitUntil(() => !IsSelectingVehicle);
            for (int i = 0; i < vehiclesButtons.Length; i++) vehiclesButtons[i].SetActive(false);
            isSelectingPersistence = true;
            persistentText.gameObject.SetActive(true);
            yesButton.SetActive(true); noButton.SetActive(true);
            yield return new WaitUntil(() => !isSelectingPersistence);
            isSelectingPersistence = false;
            persistentText.gameObject.SetActive(false);
            yesButton.SetActive(false); noButton.SetActive(false);
            //implement a way for the player to input the value
            Resource resource = new Resource(resourceType, valueToBeTransfered, persistence);
            resource.Type = resourceType;
            TransportRoute transportRoute = globe.GenerateTradeRoute(this, nodeManager.target.GetComponent<CityNode>(), transportType, persistence, resource,CreateTradeRouteBetweenCities);
            
            cancelButton.SetActive(false);

        }
        
    }

    void arrivalCallBack(CityNode c, Node n, Resource r)
    {

    }

    public void SelectResourceType(int type)
    {
        resourceType = (ResourceType)type;
        isSelectingResourceType = false;
    }


    public override void Setup()
    {
        base.Setup();
        Type = NodeType.City;
        passable = true;
    }

    public void UpgradeCityButtom()
    {
        UpgradeCity();
    }
    public void UpgradeAspectButtom(int index)
    {
        aspectIndex = index;
        isSeingUpgrades = true;
        cancelButton.SetActive(true);
        for (int i = 0;i<icons2.Length; i++)
        {
            icons2[i].SetActive(true);
            upgradeTexts[i].gameObject.SetActive(true);
            upgradeTexts[i].text = upgradeCost[i].ToString();
        }
    }
    public void SelectVehicle(int vehicleIndex)
    {
        transportType = (TransportType)vehicleIndex;
        IsSelectingVehicle = false;
    }
    public void ConfirmUpgradeAspect()
    {
        UpgradeAspect(aspectIndex);
        for (int i = 0; i < icons2.Length; i++)
        {
            icons2[i].SetActive(false);
            upgradeTexts[i].gameObject.SetActive(false);
        }
    }
    public void CancelButton()
    {
        if(isSelectingTarget)
        {
            isSelectingTarget = false;
            StopCoroutine(tradeRouteEnumarator());
        }
        if(isSelectingResourceType)
        {
            isSelectingResourceType = false;
            StopCoroutine(tradeRouteEnumarator());
            for (int i = 0; i < icons.Length; i++) icons[i].SetActive(false);
        }
        if(IsSelectingVehicle)
        {
            IsSelectingVehicle=false;
            StopCoroutine(tradeRouteEnumarator());
            for (int i = 0; i < vehiclesButtons.Length; i++) vehiclesButtons[i].SetActive(false);
        }
        if(isSeingUpgrades)
        {
            isSeingUpgrades = false;
            for (int i = 0; i < icons2.Length; i++)
            {
                icons2[i].SetActive(false);
                upgradeTexts[i].gameObject.SetActive(false);
            }
        }
        if(isSelectingPersistence)
        {
            isSelectingPersistence = false;
            persistentText.gameObject.SetActive(false);
            yesButton.SetActive(false); noButton.SetActive(false);
        }
        cancelButton.SetActive(false);
    }

    public void PersistenceButton(int b)
    {
        bool u;
        if(b==1) { u = true; } else { u = false; }
        persistence = u;
        isSelectingPersistence = false;
    }
}