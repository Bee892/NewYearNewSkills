using System.Collections;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor.Timeline.Actions;
#endif
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
    public int NumStartFuel;
    public int NumStartFood;
    public int NumStartMetal;
    public int NumStartMinerals;

    // Transportation
	public float MinTimeArrivalToDeparture;
	public float MaxTimeArrivalToDeparture;
}
