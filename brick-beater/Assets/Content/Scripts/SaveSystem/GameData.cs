using System.Collections.Generic;

[System.Serializable]
public class GameData
{
    public List<BrickData> bricksData;
    public int score;
    public int hp;
    public int level;

    public GameData()
    {
        bricksData = new List<BrickData>();
        score = 0;
        hp = 10;
        level = 1;
    }
}
