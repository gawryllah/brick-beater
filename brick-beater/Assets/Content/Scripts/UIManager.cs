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
        InitUI();

        BallController.BallSpawned += CountDown;
        LevelManager.Instance.LevelLoaded += CountDown;
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
        TurnOffUI();
        GameManager.Instance.ClosePauseMenu();
        SceneManager.LoadScene("MainMenu");
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
    }

    void CountDown()
    {
        StartCoroutine(CountDownText(cdValue));
    }

    private IEnumerator CountDownText(int value)
    {
        for (int i = value; i > 0; i--)
        {
            cdText.text = $"{i}";
            yield return new WaitForSecondsRealtime(1);
        }
        cdGo.SetActive(false);
        CountdownFinished?.Invoke();
    }



}
