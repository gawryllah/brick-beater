using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public bool GameOn { get; set; } = false;

    [SerializeField] private int score;

    [SerializeField] private GameObject ballPrefab;

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
        score = 0;
    }

    private void Start()
    {
        BrickManager.Instance.OnSpawnedBricks += StartGame;
        GameOn = true;

    }

    public void AddScore()
    {
        score += 10;
    }

    public void StartGame()
    {
        if (GameOn)
        {
            Instantiate(ballPrefab);
        }
    }
}
