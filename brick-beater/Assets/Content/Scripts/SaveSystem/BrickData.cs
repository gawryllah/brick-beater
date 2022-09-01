using UnityEngine;

[System.Serializable]
public class BrickData
{
    public Vector3 position;
    public Vector3 scale;
    public int hits;

    public BrickData(Vector3 position, int hits, Vector3 scale)
    {
        this.position = position;
        this.hits = hits;
        this.scale = scale;
    }
}
