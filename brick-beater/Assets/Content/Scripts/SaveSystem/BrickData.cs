using UnityEngine;

[System.Serializable]
public class BrickData
{
    public Vector3 position;
    public int hits;

    public BrickData(Vector3 position, int hits)
    {
        this.position = position;
        this.hits = hits;
    }
}
