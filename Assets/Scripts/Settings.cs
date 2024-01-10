using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

[CreateAssetMenu]
public class Settings : ScriptableObject
{
    [Range(0.1f, 1)] public float RotationSpeed;
    [Range(0.1f, 1)] public float ZoomSpeed;
    public int NumStartCities;
    public int MaxCityResourceStartDist;
}
