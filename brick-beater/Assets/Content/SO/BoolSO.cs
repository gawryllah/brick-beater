using UnityEngine;

[CreateAssetMenu(fileName = "BoolSO", menuName = "ScriptableObjects/BoolSO")]
public class BoolSO : ScriptableObject
{
    [SerializeField] private bool value; public bool Value { get { return value; } set { this.value = value; } }
}
