using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{

    public delegate void SpawnedBricks();
    public SpawnedBricks OnSpawnedBricks;

    private static BrickManager instance;

    public static BrickManager Instance { get { return instance; } }

    private SpriteRenderer areaSR;
    private float areaWidth;
    private float areaHeight;

    [SerializeField] private GameObject bricksArea;
    [SerializeField] private GameObject brickPrefab;
    [SerializeField] private GameObject bricksParent;

    [SerializeField] private int cols = 3;
    [SerializeField] private int rows = 3;

    [SerializeField] private float spacing = 0.2f;
    [SerializeField] private float brickWidth;
    [SerializeField] private float brickHeight;
    [SerializeField] private float widthDelta;
    [SerializeField] private float heightDelta;

    [SerializeField] private int[,] bricksMap;
    [SerializeField] private List<int[,]> listOfBricksMaps;

    [SerializeField] private List<GameObject> bricksList = new List<GameObject>();


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

        cols += LevelManager.Instance.Level - 1;
        rows += LevelManager.Instance.Level - 1;
    }

    private void Start()
    {
        Debug.Log($"{areaSR.bounds.size}, x: {areaWidth}, y: {areaHeight}");
      
        SetBricksSize();
        InitBrickMap();

        rows /= 2;
        cols /= 2;

        GenerateBricks();
        CheckBricksOnScene();
    }

    void GenerateBricks()
    {


        var startingPos = new Vector2(-(bricksArea.transform.position.x + areaWidth / 2), (bricksArea.transform.position.y + areaHeight / 2)); //left side
        var spawnPos = startingPos;

        // Debug.Log($"x:{brickWidth}, y: {brickHeight}, deltaX {widthDelta}, deltaY {heightDelta}");

        for (int i = 0; i < rows; i++)
        {
          
            for (int j = 0; j < cols; j++)
            {
                Debug.Log($"1 for, i: {i}, j: {j}");
                if (bricksMap[i, j] != 0)
                {
                    var go = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                    go.transform.SetParent(bricksParent.transform);
                    if (go.GetComponent<BrickScript>() != null)
                    {
                        go.GetComponent<BrickScript>().SetHits(bricksMap[i, j]);
                    }
                }
                spawnPos.y -= heightDelta;
                

            }

            //if (i < cols - 1)
            spawnPos.x += widthDelta;

            spawnPos.y = startingPos.y;
        }

        startingPos = new Vector2((bricksArea.transform.position.x + areaWidth / 2), (bricksArea.transform.position.y + areaHeight / 2)); //right side
        spawnPos = startingPos;


        for (int i = 0; i < rows; i++)
        {

            for (int j = 0; j < cols; j++)
            {
                Debug.Log($"1 for, i: {i}, j: {j}");
                if (bricksMap[i, j] != 0)
                {
                    var go = Instantiate(brickPrefab, spawnPos, Quaternion.identity);
                    go.transform.SetParent(bricksParent.transform);
                    if (go.GetComponent<BrickScript>() != null)
                    {
                        go.GetComponent<BrickScript>().SetHits(bricksMap[i, j]);
                    }
                }
                spawnPos.y -= heightDelta;


            }

            //if (i < cols - 1)
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
                bricksMap[i, j] = (int)Random.Range(0, 5);


                //Debug.Log($"row {j}, col {i}, val: {bricksMap[j, i]}");
            }
        }
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
            Debug.Log($"Level finished! Level: {LevelManager.Instance.Level}");
            Time.timeScale = 0f;
        }
    }

}
