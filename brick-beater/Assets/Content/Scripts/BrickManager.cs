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

    [SerializeField] private float x_Start, y_Start;
    [SerializeField] private float x_Space, y_Space;

    [SerializeField] private int cols = 3;
    [SerializeField] private int rows = 3;
    [SerializeField] private int[,] bricksMap;
    [SerializeField] private List<int[,]> listOfBricksMaps;


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
        InitBrickMap();
        GenerateBricks();
    }

    void GenerateBricks()
    {


        float spacing = 0.2f;

        float brickWidth = (areaWidth / cols) - spacing / 2f;
        float brickHeight = (areaHeight / rows) - spacing / 2f;

        float widthDelta = (areaWidth - (cols * brickWidth) / cols) / ((1.4f * LevelManager.Instance.Level));
        float heightDelta = (areaHeight - (rows * brickHeight) / rows) / ((1.4f * LevelManager.Instance.Level));

        brickPrefab.transform.localScale = new Vector3(brickWidth, brickHeight, 0f);



        var startingPos = new Vector2(-(bricksArea.transform.position.x + areaWidth / 2), (bricksArea.transform.position.y + areaHeight / 2)); //left side
        var spawnPos = startingPos;

        // Debug.Log($"x:{brickWidth}, y: {brickHeight}, deltaX {widthDelta}, deltaY {heightDelta}");

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
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
                //Debug.Log($"i: {i}, j: {j}, Spawned {go.name}, at: {go.transform.position}, spawnX: {spawnPos.x}, spawnY: {spawnPos.y}");

            }

            if (i < cols - 1)
                spawnPos.x += widthDelta;

            spawnPos.y = startingPos.y;
        }

        OnSpawnedBricks?.Invoke();

    }

    void InitBrickMap()
    {
        bricksMap = new int[rows, cols];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                bricksMap[j, i] = (int)Random.Range(0, 5);


                //Debug.Log($"row {j}, col {i}, val: {bricksMap[j, i]}");
            }
        }
    }

}
