using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ControlsStats", menuName = "ScriptableObjects/ControlsStats")]
public class ControlsStats : ScriptableObject
{
    [SerializeField] private float speed; public float Speed { get { return speed; } }
}
