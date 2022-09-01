using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public delegate void MainMenuEvents();
    public static event MainMenuEvents OnPlayGame;
    public static event MainMenuEvents OnLoadGame;

    private static MainMenuManager instance;

    public static MainMenuManager Instance { get { return instance; } }

    [SerializeField] private BoolSO gameLoaded;

    [SerializeField] private GameObject logo;
    [SerializeField] private TMP_Text hiScoreText;

    [SerializeField] private GameObject continueBtn;

    private bool newGame;

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

        if (PlayerPrefs.HasKey("BB-HiScore"))
        {
            hiScoreText.text = $"HiScore: {PlayerPrefs.GetInt("BB-HiScore")}";
        }
    }

    private void OnEnable()
    {
        newGame = false;
    }

    private void Start()
    {
        if (DataPersistenceManager.Instance.IsGameSaved())
        {
            continueBtn.SetActive(true);
        }
        else
        {
            continueBtn.SetActive(false);
        }
        StartCoroutine(logoAnim());
    }

    public void PlayGame()
    {
        newGame = true;
        StopAllCoroutines();
        OnPlayGame?.Invoke();
        gameLoaded.Value = false;
        SceneManager.LoadScene("GameScene");
    }


    public void LoadGame()
    {
        Debug.Log("Loaded Game");
        newGame = true;
        StopAllCoroutines();
        gameLoaded.Value = true;
        SceneManager.LoadScene("GameScene");
        OnLoadGame?.Invoke();
    }

    public void InstructionView()
    {

    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator logoAnim()
    {

        while (!newGame)
        {
            yield return new WaitForSecondsRealtime(1.75f);
            logo.SetActive(false);
            yield return new WaitForSecondsRealtime(1f);
            logo.SetActive(true);
        }
    }
}
