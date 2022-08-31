using UnityEngine;

public class LevelManager : MonoBehaviour
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


    }


}
