using UnityEngine;

public class GameManager : MonoBehaviour, IUIHandler
{
    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public bool GameOn { get; set; } = false;

    [SerializeField] private IntSO hiScore;
    [SerializeField] private IntSO score;
    [SerializeField] private IntSO hp;


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
        hp.Value = 5;
        score.Value = 0;

    }

    private void Start()
    {
        GameOn = true;
        BrickManager.Instance.OnSpawnedBricks += StartGame;
        BallController.TouchedGround += LoseHealth;
    }

    public void AddScore()
    {
        score.Value += 10;
        UIManager.Instance.UpdateUI();
        //Debug.Log($"Score: {score.Value}");

    }

    void StartGame()
    {
        if (GameOn)
        {
            Instantiate(ballPrefab);
            UIManager.Instance.UpdateUI();
        }
    }

    void LoseHealth()
    {
        hp.Value -= 1;
        UIManager.Instance.UpdateUI();
        //Debug.Log($"hp: {hp.Value}");
        CheckHP();
    }

    void CheckHP()
    {
        if (hp.Value <= 0)
        {

            Debug.Log($"Out of HPs! You Lost! HP: {hp.Value}");
            if (score.Value > hiScore.Value)
            {
                hiScore.Value = score.Value;
                UIManager.Instance.UpdateUI();
            }
            Time.timeScale = 0f;
        }
    }

    public void OpenPauseMenu()
    {
        Time.timeScale = 0f;
        UIManager.Instance.OpenPauseMenu();
    }

    public void ClosePauseMenu()
    {
        Time.timeScale = 1f;
        UIManager.Instance.ClosePauseMenu();
    }


}
