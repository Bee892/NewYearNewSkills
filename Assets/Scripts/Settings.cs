using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    // Globe
    [Range(0.1f, 1)] public float RotationSpeed;
    [Range(0.1f, 1)] public float ZoomSpeed;

    // Generation
    public int NumStartCities;
    public int MaxCityResourceStartDist;

    // Transportation
	public float MinTimeArrivalToDeparture;
	public float MaxTimeArrivalToDeparture;
    public float TimeBetweenPrimitiveTruckMovements;
	public float TimeBetweenModernTruckMovements;
	public float TimeBetweenFutureTruckMovements;
	public float TimeBetweenPrimitiveBoatMovements;
	public float TimeBetweenModernBoatMovements;
	public float TimeBetweenFutureBoatMovements;
	public float TimeBetweenPrimitivePlaneMovements;
	public float TimeBetweenModernPlaneMovements;
	public float TimeBetweenFuturePlaneMovements;
	public float TimeBetweenPrimitiveTrainMovements;
	public float TimeBetweenModernTrainMovements;
	public float TimeBetweenFutureTrainMovements;
}
