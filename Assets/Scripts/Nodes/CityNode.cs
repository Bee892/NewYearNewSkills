using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static Constants;
using TMPro;
using UnityEngine.UI;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

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
    public GameObject valueContainer;
    public TMP_InputField value;
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
    public bool isInputingTransferValue;
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
    public GameObject[] visuals;
    public delegate void cityUpgrade();
    public static event cityUpgrade OnCityUpgrade;
    public List<ResourceNode> resourceNodes1;
    public GameObject GenericNodePrefabs;
    float[] transportTypeMultiplicators = { 1, 4, 2, 5 };
    float[] fuelConsumption = { 100, 300, 200, 500 };
    public bool isOriginal = true;
    public float cost;
    public Era CityEra
    {
        get
        {
            return cityEra;
        }
    }

    public void Consumption()
    {
        if (!isOriginal)
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
                    if (resourcesStored[i] == 0 && isCityAlive) StartCoroutine(Crumbling());
                }
                if (resourcesStored[i] < 0) resourcesStored[i] = 0; //Limit the resource to 0
            }
        }
    }

    public void OnMouseDown()
    {
        if (!isOriginal)
        {
            if (!nodeManager.isSelectingTarget)
            {
                foreach (CityNode citynode in nodeManager.cityNodes) citynode.canvas.enabled = false;
                canvas.enabled = true;
            }
            else
            {
                nodeManager.target = this.gameObject;
                nodeManager.isSelectingTarget = false;
                
            }
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        if (!isOriginal)
        {
            
            base.Setup();
            Type = NodeType.City;
            passable = true;
            GameObject manager = GameObject.FindGameObjectWithTag("Manager");
            nodeManager = manager.GetComponent<NodeManager>();
            canvas.enabled = false;
            globe = GetComponentInParent<Globe>();
            InvokeRepeating("Consumption", 0f, 1f);
            for (int i = 0; i < 2; i++)
            {
                valuesThresHold[i].enabled = false;
                valuesText.enabled = false;
                deathMessage.enabled = false;
            }
            for (int i = 0; i < 5; i++)
            {
                upgradeTexts[i].enabled = false;
            }
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!isOriginal)
        {
            if (!isCityAlive)
            {

                if (resourcesStored[(int)ResourceType.Fuel] > cityRevivalThreshold[0] && resourcesStored[(int)ResourceType.Food] > cityRevivalThreshold[1])
                    isCityAlive = true;
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(false);
                }
            }
            if (!isCrumbling && isCityAlive)
            {
                for (int i = 0; i < 2; i++)
                {
                    icons3[i].SetActive(false);
                    valuesThresHold[i].enabled = false;
                    valuesText.enabled = false;
                    deathMessage.enabled = false;
                }
                for (int i = 0; i < buttons.Length; i++)
                {
                    buttons[i].SetActive(true);
                }
            }
        }
    }

    public void Death()
    {
        isCityAlive = false;
        for (int i = 0; i < 2; i++)
        {
            icons3[i].SetActive(true);
            valuesThresHold[i].enabled = true;
            valuesThresHold[i].text = cityRevivalThreshold[i].ToString();
            valuesText.enabled=true;
            deathMessage.enabled = true;
            valuesText.text = "Requeriments:";
            deathMessage.text = "City is Dead!";
        }
    }
    public IEnumerator Crumbling ()
    {
        for (int i = 0; i < 2; i++)
        {
            icons3[i].SetActive(true);
            valuesThresHold[i].enabled = true; valuesText.enabled = true;
            valuesThresHold[i].text = cityNotCrumbleThreshold.ToString();
            deathMessage.enabled = true;
            valuesText.text = "Requeriments:";
            deathMessage.text = "City is Crumbling!";
        }
        yield return new WaitForSeconds(100f);
        
        if (resourcesStored[(int)ResourceType.Fuel] < cityNotCrumbleThreshold || resourcesStored[(int)ResourceType.Food] < cityNotCrumbleThreshold) 
        Death();
    }

    public override void Selected()
    {
        throw new NotImplementedException();
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
            visuals[(int)cityEra].gameObject.SetActive(false);
            cityEra++;
            visuals[(int)cityEra].gameObject.SetActive(true);
            foreach(ResourceNode node in resourceNodes1) node.updateVisuals();
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

    public void CreateTradeRouteBetweenCities(CityNode destination, float quantity, bool persistence, ResourceType type, TransportType transport)
    {
        if (!isOriginal)
        {
            quantity = quantity * transportTypeMultiplicators[(int)transport];
            float fuelCost = fuelConsumption[(int)transport];
            money -= cost;
            if(persistence)
            {
                
                resourcesStored[(int)type] -= quantity;
                destination.resourcesStored[(int)type] += quantity;
                resourcesStored[(int)ResourceType.Fuel] -= fuelCost;
            }
            else
            {
                resourcesConsumption[(int)type] -= quantity;
                destination.resourcesConsumption[(int)type] += quantity;
                resourcesConsumption[(int)ResourceType.Fuel] -= fuelCost;
            }
        }
    }
    public void CreateTradeRouteBetweenCityAndResourceNode (ResourceNode destination, TransportType transport)
    {
        float fuelCost = fuelConsumption[(int)transport];
        resourceNodes1.Add(destination);
        if (!destination.used)
        {
            destination.City = this;
            ResourceType type = destination.ResourceType;
            destination.resourceTransmitted = destination.resourceTransmitted * transportTypeMultiplicators[(int)transport];
            resourcesConsumption[(int)type] += destination.resourceTransmitted;
            destination.used = true;
            resourcesStored[(int)ResourceType.Fuel] -= fuelCost;
            money -= cost;
        }
    }

    private void FixedUpdate()
    {
        if (!isOriginal)
        {
            moneyText.text = money.ToString();
            foodText.text = resourcesStored[(int)ResourceType.Food].ToString();
            fuelText.text = resourcesStored[(int)ResourceType.Fuel].ToString();
            mineralsText.text = resourcesStored[(int)ResourceType.Minerals].ToString();
            metalText.text = resourcesStored[(int)ResourceType.Metal].ToString();
        }
    }


    public void createTradeRoute()
    {
        
        StartCoroutine(tradeRouteEnumarator());
    }

    IEnumerator tradeRouteEnumarator()
    {
        if (!isOriginal)
        {
            cancelButton.SetActive(true);
            target = null;
            valueToBeTransfered = 0;
            nodeManager.isSelectingTarget = true;
            message.gameObject.SetActive(true);
            message.text = "Select a Node";
            yield return new WaitUntil(() => !nodeManager.isSelectingTarget);
            target = nodeManager.target;
            message.gameObject.SetActive(false);
            if (nodeManager.target.GetComponent<ResourceNode>() != null)
            {
                IsSelectingVehicle = true;
                for (int i = 0; i < vehiclesButtons.Length; i++) vehiclesButtons[i].SetActive(true);
                yield return new WaitUntil(() => !IsSelectingVehicle);
                for (int i = 0; i < vehiclesButtons.Length; i++) vehiclesButtons[i].SetActive(false);
                CreateTradeRouteBetweenCityAndResourceNode(nodeManager.target.GetComponent<ResourceNode>(),transportType);
                cancelButton.SetActive(false);
            }
            if (nodeManager.target.GetComponent<CityNode>() != null)
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
                valueContainer.SetActive(true);
                isInputingTransferValue = true;
                yield return new WaitUntil(() => !isInputingTransferValue);
                valueToBeTransfered = float.Parse(value.text);

                valueContainer.SetActive(false);
                value.text = "";

                CreateTradeRouteBetweenCities(target.GetComponent<CityNode>(),valueToBeTransfered,persistence,resourceType,transportType);

                cancelButton.SetActive(false);

            }
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
        GameObject manager = GameObject.FindGameObjectWithTag("Manager");
        nodeManager = manager.GetComponent<NodeManager>();
        GameObject obj = null;
        if (isOriginal)
        {
            obj = Instantiate(nodeManager.CityNodePrefab);
            obj.GetComponent<CityNode>().isOriginal = false;
            obj.transform.position = this.gameObject.transform.position;
            obj.transform.rotation = this.gameObject.transform.rotation;
            obj.transform.parent = this.gameObject.transform;
            nodeManager.cityNodes.Add(obj.GetComponent<CityNode>());
            nodeManager.cityNodes.Remove(this);

        }
        
    }
    GameObject InstantiatePrefab()
    {
        string prefabPathName = "Assets/Nodes Prefabs/City";
        return Instantiate(Resources.Load<GameObject>(prefabPathName));

    }

    public void UpgradeCityButtom()
    {
        UpgradeCity();
    }
    public void UpgradeAspectButtom(int index)
    {
        aspectIndex = index;
        isSeingUpgrades = true;
        confirmButton.SetActive(true);
        cancelButton.SetActive(true);
        for (int i = 0;i<5; i++)
        {
            icons2[i].SetActive(true);
            upgradeTexts[i].enabled = true;
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
            for (int i = 0; i < 5; i++)
            {
                icons2[i].SetActive(false);
                upgradeTexts[i].enabled = false;

            }
        confirmButton.SetActive(false);
        cancelButton.SetActive(false);
        isSeingUpgrades = false;
    }
    public void CancelButton()
    {
        if (isInputingTransferValue)
        {
            isInputingTransferValue = false;
            StopCoroutine(tradeRouteEnumarator());
        }
        if (isSelectingTarget)
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
            for (int i = 0; i < 5; i++)
            {
                icons2[i].SetActive(false);
                upgradeTexts[i].enabled = false;

            }
            confirmButton.SetActive(false);
            cancelButton.SetActive(false);
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

    public void InputValue()
    {
        if (float.TryParse(value.text, out float inputVal))
        {
            isInputingTransferValue = false;
        }
    }
}