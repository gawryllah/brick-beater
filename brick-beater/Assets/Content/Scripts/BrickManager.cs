using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private static BrickManager instance;

    public static BrickManager Instance { get { return instance; } }


    [SerializeField] private GameObject bricksArea;
    [SerializeField] private GameObject brickPrefab;

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
    }

    private void Start()
    {
        GenerateBricks();
    }

    void GenerateBricks()
    {
        cols *= LevelManager.Instance.Level;
        rows *= LevelManager.Instance.Level;

        for (int i = 0; i < cols + rows; i++)
        {
            Vector3 position;
            position = new Vector3(x_Start + (x_Space * (i % cols)), y_Start + (y_Space * (i / cols)));
            Instantiate(brickPrefab, position, Quaternion.identity);
        }

        /*

        bricksMap = new int[rows, cols];

        for (int i = 0; i < cols; i++)
        {
            for (int j = 0; j < rows; j++)
            {
                bricksMap[j, i] = (int)Random.Range(0, 5);


                Debug.Log($"row {j}, col {i}, val: {bricksMap[j, i]}");
            }
        }

        */


    }

    Bounds getBounds(GameObject obj)
    {
        Bounds bounds;
        Renderer childRender;
        bounds = getRenderBounds(obj);
        if (bounds.extents.x == 0)
        {
            bounds = new Bounds(obj.transform.position, Vector3.zero);
            foreach (Transform child in obj.transform)
            {
                childRender = child.GetComponent<Renderer>();
                if (childRender)
                {
                    bounds.Encapsulate(childRender.bounds);
                }
                else
                {
                    bounds.Encapsulate(getBounds(child.gameObject));
                }
            }
        }
        return bounds;
    }

    Bounds getRenderBounds(GameObject obj)
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        Renderer render = obj.GetComponent<Renderer>();
        if (render != null)
        {
            return render.bounds;
        }
        return bounds;
    }


}
