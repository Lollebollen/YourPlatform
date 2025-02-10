using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager instance;
    public static LevelManager Instance { get { return instance; } }

    [SerializeField] GameObject basePlatformObject;
    [SerializeField] GameObject[] maps;

    FirebaseAuth auth;
    FirebaseDatabase database;

    public string oldLevelData;
    public string newLevelData;
    public long startTime;
    public int gameIndex = -1;

    PlatformDataCollection gameData;

    private void Awake()
    {
        gameIndex = -1;
        if (instance == null) { instance = this; DontDestroyOnLoad(gameObject); }
        else { Destroy(this); }
        database = FirebaseDatabase.DefaultInstance;
        auth = FirebaseAuth.DefaultInstance;
    }

    public void StartGame() // gets called from a gameObject in game scene
    {
        LoadData();
        if (gameData.Collection == null) { return; }
        LoadLevel();
    }

    public void OnLevelComplete()
    {
        if (gameIndex == -1) { return; }
        bool task1 = false;
        bool task2 = false;
        bool task3 = false;
        database.RootReference.Child("games").Child("gameData").Child(gameIndex.ToString()).
            SetRawJsonValueAsync(SaveLevel()).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
            task1 = task.IsCompleted;
            SyncDataSetting(task1, task2, task3);
        });
        database.RootReference.Child("gameStates").Child("activeStatus").Child(gameIndex.ToString()).
            SetRawJsonValueAsync(JsonUtility.ToJson(new Active(false, startTime))).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
            task2 = task.IsCompleted;
            SyncDataSetting(task1, task2, task3);
        });
        database.RootReference.Child("replays").Child(gameData.user).Child(auth.CurrentUser.UserId).
            SetRawJsonValueAsync(ReplaySaver.Instance.GetReplay()).ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null) { Debug.Log(task.Exception); }
                task3 = task.IsCompleted;
                SyncDataSetting(task1, task3, task2);
            });
    }

    private void SyncDataSetting(bool task1, bool task2, bool task3)
    {
        if (task1 && task2 && task3)
        {
            SceneManager.LoadScene(1);
        }
    }

    #region debug Functions
    public void DebugLoad()
    {
        foreach (Platform platform in FindObjectsOfType<Platform>())
        {
            Destroy(platform.gameObject);
        }

        LoadData();
        if (gameData.Collection == null) { return; }
        LoadLevel();
    }

    public void DebugOverwrite()
    {
        oldLevelData = newLevelData;
    }

    public void DebugSave()
    {
        newLevelData = SaveLevel();
    }
    #endregion

    public void LoadData()
    {
        if (string.IsNullOrEmpty(oldLevelData)) { return; }
        var oldPlatformData = JsonUtility.FromJson<PlatformDataCollection>(oldLevelData);
        if (oldPlatformData.Collection == null) { return; }
        gameData = oldPlatformData;
    }

    public void LoadLevel()
    {
        Instantiate(maps[gameData.map]);
        ObjectPanel panel = FindObjectOfType<ObjectPanel>();

        foreach (PlatformData platformData in gameData.Collection)
        {
            var platfromObject = Instantiate(basePlatformObject, new Vector3(platformData.x, platformData.y, 0), Quaternion.identity);
            ObjectRefrenceTable.Instance.objectBases[platformData.ID].InstantiateOldObject(platfromObject, platformData.state, panel, out _);
        }
    }

    public string SaveLevel()
    {
        List<PlatformData> platformDatas = new();
        foreach (Platform platform in FindObjectsOfType<Platform>())
        {
            if (!platform.isNew) { continue; }
            platformDatas.Add(new PlatformData((Vector2)platform.transform.position, platform.ID, platform.state));
        }
        PlatformDataCollection platformDataCollection = new PlatformDataCollection();
        platformDataCollection.Collection = platformDatas.ToArray();
        platformDataCollection.map = gameData.map;
        platformDataCollection.user = auth.CurrentUser.UserId;

        return JsonUtility.ToJson(platformDataCollection);
    }

    private void OnApplicationQuit()
    {
        if (gameIndex < 0) { return; }
        ResetGameState(gameIndex);
    }

    public void ResetGameState(int i)
    {
        database.RootReference.Child("gameStates").Child("activeStatus").Child(i.ToString())
            .SetRawJsonValueAsync(JsonUtility.ToJson(new Active(false, 0))).ContinueWithOnMainThread(task =>
            {
                if (task.Exception != null) { Debug.Log(task.Exception); }
            });
    }
}