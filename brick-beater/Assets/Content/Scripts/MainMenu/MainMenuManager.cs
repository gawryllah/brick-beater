using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public delegate void MainMenuEvents();
    public static event MainMenuEvents OnPlayGame;

    private static MainMenuManager instance;

    public static MainMenuManager Instance { get { return instance; } }

    [SerializeField] private GameObject logo;
    [SerializeField] private TMP_Text hiScoreText;

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

        //DontDestroyOnLoad(instance);

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
        StartCoroutine(logoAnim());
    }

    public void LoadGame()
    {
        newGame = true;
        StopAllCoroutines();
        SceneManager.LoadScene("GameScene");
        OnPlayGame?.Invoke();

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
            yield return new WaitForSeconds(1.75f);
            logo.SetActive(false);
            yield return new WaitForSeconds(1f);
            logo.SetActive(true);
        }
    }
}
