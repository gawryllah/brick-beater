using UnityEngine;

[CreateAssetMenu(fileName = "ControlsStats", menuName = "ScriptableObjects/ControlsStats")]
public class ControlsStats : ScriptableObject
{
    [SerializeField] private float speed; public float Speed { get { return speed; } set { this.speed = value; } }
    [SerializeField] private float maxDelta; public float MaxDelta { get { return maxDelta; } }
}
