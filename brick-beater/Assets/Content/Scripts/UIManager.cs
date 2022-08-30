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

    [SerializeField] private GameObject pauseMenu; public bool PauseMenuOpened { get { return pauseMenu.activeSelf; } }

    [SerializeField] private TMP_Text hpText;
    [SerializeField] private TMP_Text hiScoreText;
    [SerializeField] private TMP_Text scoreText;


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

    }

    void InitUI()
    {
        pauseMenu.SetActive(false);
        hpText.gameObject.SetActive(true);
        hiScoreText.gameObject.SetActive(true);
        scoreText.gameObject.SetActive(true);
    }



}
