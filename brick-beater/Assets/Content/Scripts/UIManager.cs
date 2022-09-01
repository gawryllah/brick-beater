using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour, IUIHandler
{
    private static UIManager instance;

    public static UIManager Instance { get { return instance; } }

    [SerializeField] private IntSO hiScore;
    [SerializeField] private IntSO score;
    [SerializeField] private IntSO hp;

    [SerializeField] private int cdValue;

    [SerializeField] private GameObject cdGo;
    [SerializeField] private TMP_Text cdText;
    [SerializeField] private GameObject pauseMenu; public bool PauseMenuOpened { get { return pauseMenu.activeSelf; } }

    [SerializeField] private GameObject gameOverView; public bool GameOverViewOpened { get { return gameOverView.activeSelf; } }
    [SerializeField] protected TMP_Text gameOverText;

    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text hiScoreText;
    [SerializeField] private TMP_Text scoreText;
    [SerializeField] private TMP_Text levelText;

    public delegate void UIEvent();
    public UIEvent CountdownFinished;


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

    private void OnEnable()
    {

        GameManager.Instance.OnRunOutOHP += ShowGameOver;

        BallController.BallSpawned += CountDown;
    }

    private void OnDisable()
    {
        GameManager.Instance.OnRunOutOHP -= ShowGameOver;

        BallController.BallSpawned -= CountDown;
    }

    private void Start()
    {
        InitUI();
    }

    public void Resume()
    {
        GameManager.Instance.ClosePauseMenu();
    }

    public void Save()
    {

    }

    public void QuitToMenu()
    {
        SceneManager.LoadScene("MainMenu");
        TurnOffUI();
        GameManager.Instance.ClosePauseMenu();
        DeleteAll();


    }

    public void OpenPauseMenu()
    {
        pauseMenu.SetActive(true);
    }

    public void ClosePauseMenu()
    {
        pauseMenu.SetActive(false);
    }

    public void UpdateUI()
    {
        hiScoreText.text = $"HiScore: {hiScore.Value}";
        scoreText.text = $"Score: {score.Value}";
        hpText.text = $"{hp.Value} x ";
        levelText.text = $"Level: {LevelManager.Instance.Level}";

    }

    void InitUI()
    {
        pauseMenu.SetActive(false);
        gameOverView.SetActive(false);

        hpText.gameObject.SetActive(true);
        hiScoreText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
        levelText.gameObject.SetActive(true);

        cdGo.SetActive(true);
    }

    void TurnOffUI()
    {
        pauseMenu.SetActive(false);
        hpText.gameObject.SetActive(false);
        hiScoreText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
        levelText.gameObject.SetActive(false);
        gameOverView.SetActive(false);
    }

    void CountDown()
    {
        StartCoroutine(CountDownText(cdValue));
    }

    private IEnumerator CountDownText(int value)
    {
        cdGo.SetActive(true);
        for (int i = value; i > 0; i--)
        {
            cdText.text = $"{i}";
            yield return new WaitForSecondsRealtime(1);
        }
        cdGo.SetActive(false);

        UpdateUI();
        CountdownFinished?.Invoke();
    }

    void ShowGameOver()
    {
        gameOverText.text = scoreText.text;
        UpdateUI();
        gameOverView.SetActive(true);
    }

    public void DeleteAll()
    {
        foreach (GameObject o in Object.FindObjectsOfType<GameObject>())
        {
            Destroy(o);
        }
    }



}
