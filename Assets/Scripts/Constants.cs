using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Constants
{
    // Zooming
    public const float MinZoomLevel = 0;
    public const float MaxZoomLevel = 0.6f;
    public const float ZoomMod = 0.2f;

    // Nodes
    public enum LandSeaDesignation
    {
        Land,
        Sea
    }

    public enum NodeType
    {
        City,
        Resource
    }

    // Resources
    public enum ResourceType
    {
        Food,
        Minerals,
        Metal,
        Fuel
    }

    public enum ResourceYieldCategory
    {
        Sparse = 0,
        Moderate,
        Rich
    }

    public const float MaxYield = 5;
    public const float MinYield = 0.5f;

    public static readonly Dictionary<ResourceYieldCategory, float> ResourceYieldMinValues = new Dictionary<ResourceYieldCategory, float>()
    {
        { ResourceYieldCategory.Sparse, MinYield },
        { ResourceYieldCategory.Moderate, MinYield + ((MaxYield - MinYield) / Enum.GetNames(typeof(ResourceYieldCategory)).Length) },
		{ ResourceYieldCategory.Rich, MinYield + ((MaxYield - MinYield) / Enum.GetNames(typeof(ResourceYieldCategory)).Length * 2) },
	};

    // Cities
    public enum Era
    {
        Primitive,
        Modern,
        Futuristic
    }

    // Transportation
    public enum TransportType
    {
        Train,
        Truck,
        Plane,
        Boat
    }

    public const float ChanceOfDeparturePerSecond = 0.3f;

    public static readonly Dictionary<TransportType, string> TransportPrefabs = new Dictionary<TransportType, string>() 
    {
        { TransportType.Train, "" },
        { TransportType.Truck, "" },
        { TransportType.Plane, "" },
        { TransportType.Boat, "" }
    };
}
