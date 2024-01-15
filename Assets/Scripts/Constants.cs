using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Constants
{
    // Zooming
    public const float MinZoomLevel = -7;
    public const float MaxZoomLevel = -4;
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
        Resource,
        Barren
    }

	// Resources
	public enum ResourceType
    {
        Food = 0,
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
        Primitive = 0,
        Modern,
        Futuristic
    }

    public static readonly Dictionary<Era, int> EraIndices = new Dictionary<Era, int>()
    {
        { Era.Primitive, 0 },
        { Era.Modern, 1 },
        { Era.Futuristic, 2 },
    };

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
        { TransportType.Train, "Assets/TransportPrefabs/Fast Land.prefab" },
        { TransportType.Truck, "Assets/TransportPrefabs/Slow Land.prefab" },
        { TransportType.Plane, "Assets/TransportPrefabs/Air.prefab" },
        { TransportType.Boat, "Assets/TransportPrefabs/Sea.prefab" }
    };
}
