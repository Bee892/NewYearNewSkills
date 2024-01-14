using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants;

[CreateAssetMenu]
public class TransportSO : ScriptableObject
{
    public Era VehicleEra;
    public GameObject[] visual;
    public float FuelConsumptionPerSecond;
    public float ResourceGainPerSecond;
    public LandSeaDesignation LandOrSea;
    public float SecondsBetweenMoves;
}
