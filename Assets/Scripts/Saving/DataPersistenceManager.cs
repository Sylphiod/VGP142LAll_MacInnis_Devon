using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistenceManager : MonoBehaviour
{

    [SerializeField] private string fileName = "Save";
    [SerializeField] private bool useEncryption;

    private GameData gameData;
    private List<IDatatPersistence> datatPersistenceObjects;
    private FileDataHandler dataHandler;

    public static DataPersistenceManager instance { get; private set; }

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogError("Multiple Data Persistence");
        }

        instance = this;
    }

    private void Start()
    {
        this.dataHandler = new FileDataHandler(Application.persistentDataPath, fileName, useEncryption);
        this.datatPersistenceObjects = FindAllDataPersistenceObjects();
    }
    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        this.gameData = dataHandler.Load();

        if (this.gameData == null)
        {
            Debug.Log("No data was found.");
            NewGame();
        }

        foreach (IDatatPersistence dataPersistenceObj in datatPersistenceObjects)
        {
            dataPersistenceObj.LoadData(gameData);
        }

    }

    public void SaveGame()
    {
        foreach (IDatatPersistence dataPersistenceObj in datatPersistenceObjects)
        {
            dataPersistenceObj.SaveData(gameData);
        }

        dataHandler.Save(gameData);
    }

    private List<IDatatPersistence> FindAllDataPersistenceObjects()
    {
        IEnumerable<IDatatPersistence> dataPersistenceObjects = FindObjectOfType<MonoBehaviour>().OfType<IDatatPersistence>;

        return new List<IDatatPersistence>(dataPersistenceObjects);
    }

}

