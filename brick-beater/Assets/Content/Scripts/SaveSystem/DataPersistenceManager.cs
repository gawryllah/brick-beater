using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DataPersistenceManager : MonoBehaviour
{

    [Header("File Storage Config")]

    [SerializeField] private string fileName;

    private static DataPersistenceManager instance;

    public static DataPersistenceManager Instance { get { return instance; } }

    private GameData gameData;

    private List<IDataPersistence> dataPersistanceObj;
    private FileDataHandler dataHandler;

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
        dataHandler = new FileDataHandler(Application.persistentDataPath, fileName);
        dataPersistanceObj = FindAllDataPersistenceObjects();
        LoadGame();
    }


    public void LoadGame()
    {
        gameData = dataHandler.Load();

        UpdatedDataPersistanceObj();

        foreach (IDataPersistence dataPersistance in dataPersistanceObj)
        {
            dataPersistance.LoadData(gameData);
        }
    }

    public void SaveGame()
    {
        if (gameData == null)
            gameData = new GameData();

        UpdatedDataPersistanceObj();

        foreach (IDataPersistence dataPersistance in dataPersistanceObj)
        {
            dataPersistance.SaveData(ref gameData);
        }

        dataHandler.Save(gameData);
    }

    private List<IDataPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDataPersistence> dataPersistancesObj = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistence>();

        return new List<IDataPersistence>(dataPersistancesObj);
    }

    public void UpdatedDataPersistanceObj()
    {
        dataPersistanceObj = FindAllDataPersistenceObjects();
    }

    public bool IsGameSaved()
    {
        return dataHandler.IsGameSaved();
    }

}
