using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour, IUIHandler
{
    public delegate void GameManagerEvents();
    public GameManagerEvents OnRunOutOHP;

    private static GameManager instance;

    public static GameManager Instance { get { return instance; } }

    public bool GameOn { get; set; } = false;

    [SerializeField] private IntSO hiScore;
    [SerializeField] private IntSO score;
    [SerializeField] private IntSO hp;
    [SerializeField] private int startingHP;

    [SerializeField] private GameObject ballPrefab;
    [SerializeField] private GameObject ballGO;

    [SerializeField] private List<GameObject> powerupsList = new List<GameObject>(); public List<GameObject> PowerUpsList { get { return powerupsList; } }




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

        GameOn = false;

        hp.Value = startingHP;
        score.Value = 0;

        SetHiScore();


    }

    private void Start()
    {

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
        if (ballGO == null)
            ballGO = Instantiate(ballPrefab);
        UIManager.Instance.UpdateUI();

    }

    void LoseHealth()
    {
        hp.Value -= 1;
        UIManager.Instance.UpdateUI();
        //Debug.Log($"hp: {hp.Value}");
        CheckHP();
    }

    public void AddHealth()
    {
        //Debug.Log($"hp: {hp.Value}");
        hp.Value += 1;
        //Debug.Log($"hp gained: {hp.Value}");
        UIManager.Instance.UpdateUI();
    }

    void CheckHP()
    {
        if (hp.Value <= 0)
        {
            OnRunOutOHP?.Invoke();
            Debug.Log($"Out of HPs! You Lost! HP: {hp.Value}");
            if (score.Value > hiScore.Value)
            {
                PlayerPrefs.SetInt("BB-HiScore", score.Value);
                hiScore.Value = score.Value;
                UIManager.Instance.UpdateUI();

            }
            Time.timeScale = 0f;
        }
    }

    public void RestartBall()
    {
        PauseGame();
        GameOn = false;
        ballGO.GetComponent<BallController>().RestartBall();
    }

    public void PauseGame()
    {
        Time.timeScale = 0f;
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
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

    private void SetHiScore()
    {

        if (PlayerPrefs.HasKey("BB-HiScore"))
        {
            hiScore.Value = PlayerPrefs.GetInt("BB-HiScore");
        }
    }


}
