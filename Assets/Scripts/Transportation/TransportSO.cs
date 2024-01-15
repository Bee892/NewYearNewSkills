using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu]
public class TransportSO : ScriptableObject
{
    [SerializeField] private Era vehicleEra;
    public Era VehicleEra
    {
        get
        {
            return vehicleEra;
        }
    }
    public GameObject[] visual;
    public float FuelConsumptionPerSecond;
    public float ResourceGainPerSecond;
    public LandSeaDesignation LandOrSea;
    public float SecondsBetweenMoves;

    public void SetEra(Era e)
    {
        vehicleEra = e;

    }
}
