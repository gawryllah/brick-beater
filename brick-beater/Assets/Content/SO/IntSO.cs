using UnityEngine;


[CreateAssetMenu(fileName = "IntSO", menuName = "ScriptableObjects/IntSO")]
public class IntSO : ScriptableObject
{

    [SerializeField] private int value; public int Value { get { return value; } set { this.value = value; } }

}
