using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour, IDataPersistence
{

    public delegate void SpawnedBricks();
    public SpawnedBricks OnSpawnedBricks;

    private static BrickManager instance;

    public static BrickManager Instance { get { return instance; } }

    private SpriteRenderer areaSR;
    private float areaWidth;
    private float areaHeight;

    [SerializeField] private BoolSO IsGameLoaded;

    [SerializeField] private GameObject bricksArea;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject bricksParent;

    [SerializeField] private int cols;
    [SerializeField] private int rows;

    [SerializeField] private int finalCols;
    [SerializeField] private int finalRows;


    [SerializeField] private float spacing = 0.2f;
    [SerializeField] private float brickWidth;
    [SerializeField] private float brickHeight;
    [SerializeField] private float widthDelta;
    [SerializeField] private float heightDelta;

    [SerializeField] private int[,] bricksMap;
    [SerializeField] private List<int[,]> listOfBricksMaps = new List<int[,]>();

    [SerializeField] private List<GameObject> bricksList = new List<GameObject>();

    private Vector2 startingPos;
    private Vector2 spawnPos;
    private Vector2 bricksAreaPos;


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

        areaSR = bricksArea.GetComponent<SpriteRenderer>();
        areaWidth = areaSR.bounds.size.x;
        areaHeight = areaSR.bounds.size.y;

        finalCols = cols;
        finalRows = rows;
    }

    private void OnEnable()
    {
        cols += LevelManager.Instance.Level - 1;
        rows += LevelManager.Instance.Level - 1;
        bricksAreaPos = bricksArea.transform.position;


        MainMenuManager.OnPlayGame += CreateNewLevel;
        MainMenuManager.OnLoadGame += InitLoad;

    }

    private void OnDisable()
    {
        MainMenuManager.OnPlayGame -= CreateNewLevel;
        MainMenuManager.OnLoadGame -= InitLoad;
    }

    private void Start()
    {
        startingPos = new Vector2(-(bricksAreaPos.x + areaWidth / 2), (bricksAreaPos.y + areaHeight / 2)); //left side
        spawnPos = startingPos;

        if (IsGameLoaded.Value)
        {
            InitLoad();
        }
        else
        {
            CreateNewLevel();
        }
    }



    void GenerateBricks()
    {

        startingPos = new Vector2(-(bricksAreaPos.x + areaWidth / 2), (bricksAreaPos.y + areaHeight / 2)); //left side
        spawnPos = startingPos;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (bricksMap[i, j] != 0)
                {
                    var go = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                    if (go.GetComponent<BrickScript>() != null)
                    {
                        go.GetComponent<BrickScript>().SetHits(bricksMap[i, j]);
                    }
                }
                spawnPos.y -= heightDelta;
            }

            spawnPos.x += widthDelta;

            spawnPos.y = startingPos.y;
        }

        startingPos = new Vector2((bricksAreaPos.x + areaWidth / 2), (bricksAreaPos.y + areaHeight / 2)); //right side
        spawnPos = startingPos;

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                if (bricksMap[i, j] != 0)
                {
                    var go = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                    if (go.GetComponent<BrickScript>() != null)
                    {
                        go.GetComponent<BrickScript>().SetHits(bricksMap[i, j]);
                    }
                }
                spawnPos.y -= heightDelta;
            }

            spawnPos.x -= widthDelta;

            spawnPos.y = startingPos.y;
        }

        OnSpawnedBricks?.Invoke();

    }

    void SetBricksSize()
    {

        spacing = 0.2f;

        brickWidth = (areaWidth / cols) - (spacing / 2f);
        brickHeight = (areaHeight / rows) - (spacing / 2f);

        widthDelta = (areaWidth - (cols * brickWidth) / cols) / ((1.4f * LevelManager.Instance.Level));
        heightDelta = (areaHeight - (rows * brickHeight) / rows) / ((1.4f * LevelManager.Instance.Level));

        brickPrefab.transform.localScale = new Vector3(brickWidth, brickHeight, 0f);
    }

    void InitBrickMap()
    {
        bricksMap = new int[rows, cols];

        for (int i = 0; i < rows; i++)
        {
            for (int j = 0; j < cols; j++)
            {
                bricksMap[i, j] = Random.Range(0, 5);
            }
        }

        listOfBricksMaps.Add(bricksMap);
    }

    public void CreateNewLevel()
    {

        rows = finalRows;
        cols = finalCols;

        cols += LevelManager.Instance.Level - 1;
        rows += LevelManager.Instance.Level - 1;

        SetBricksSize();

        InitBrickMap();

        rows /= 2;
        cols /= 2;

        GenerateBricks();
    }

    public void AddToBricksList(GameObject obj)
    {
        bricksList.Add(obj);
    }

    public void DeleteBrickFromList(GameObject obj)
    {
        bricksList.Remove(obj);
    }

    public void CheckBricksOnScene()
    {
        if (bricksList.Count == 0)
        {
            GameManager.Instance.RestartBall();
            LevelManager.Instance.LevelUp();

            foreach (var obj in GameObject.FindObjectsOfType<PowerupScript>())
            {
                Destroy(obj);
            }

            CreateNewLevel();
        }
    }

    void InitLoad()
    {
        Debug.Log("Init Load");
        DataPersistenceManager.Instance.UpdatedDataPersistanceObj();
        DataPersistenceManager.Instance.LoadGame();
    }

    private List<BrickData> GetBricksDataFromList()
    {

        List<BrickData> bricksDataList = new List<BrickData>();

        foreach (var go in bricksList)
        {
            Debug.Log($"{go.name}, {go.transform.position}, {go.GetComponent<BrickScript>().Hits}");
            bricksDataList.Add(new BrickData(go.transform.position, go.GetComponent<BrickScript>().Hits));
        }

        return bricksDataList;
    }

    private void LoadFromData(List<BrickData> brickDatas)
    {
        bricksList.Clear();
        foreach (var brickData in brickDatas)
        {
            var go = Instantiate(brickPrefab, brickData.position, Quaternion.identity);
            go.GetComponent<BrickScript>().SetHits(brickData.hits);
            bricksList.Add(go);
        }
        OnSpawnedBricks?.Invoke();
    }

    public void LoadData(GameData data)
    {
        //bricksList = data.bricks;

        LoadFromData(data.bricksData);
    }

    public void SaveData(ref GameData data)
    {
        data.bricksData = GetBricksDataFromList();
    }
}
