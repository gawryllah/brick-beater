using UnityEngine;

public class LevelManager : MonoBehaviour, IDataPersistence
{
    private static LevelManager instance;

    public static LevelManager Instance { get { return instance; } }

    [SerializeField] private int level; public int Level { get { return level; } }

    public delegate void LevelEvenets();
    public LevelEvenets LevelLoaded;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        DontDestroyOnLoad(instance);
        level = 1;

    }

    public void LevelUp()
    {
        level++;
    }

    public void LoadData(GameData data)
    {
        level = data.level;
    }

    public void SaveData(ref GameData data)
    {
        data.level = level;
    }
}
