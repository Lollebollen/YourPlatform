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
    public bool isReplay;
    public Ghost ghost;

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
        LoadLevel();
    }

    public void OnLevelComplete()
    {
        if (gameIndex == -1) { return; }
        database.RootReference.Child("games").Child("gameData").Child(gameIndex.ToString()).
            SetRawJsonValueAsync(SaveLevel(out PlatformDataCollection replayMap)).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
        });
        database.RootReference.Child("gameStates").Child("activeStatus").Child(gameIndex.ToString()).
            SetRawJsonValueAsync(JsonUtility.ToJson(new Active(false, startTime))).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
        });

        database.RootReference.Child("replays").Child(gameData.user).Child(auth.CurrentUser.UserId).SetRawJsonValueAsync(JsonUtility.
            ToJson(new GhostWithMap(replayMap, ReplaySaver.Instance.GetReplay()))).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
        });

        SceneManager.LoadScene(1); // TODO add more effects/juice
    }

    public void OnReplayComplete()
    {
        SceneManager.LoadScene(1); // TODO add more effects/juice
    }

    #region debug Functions
    public void DebugLoad()
    {
        foreach (Platform platform in FindObjectsOfType<Platform>())
        {
            Destroy(platform.gameObject);
        }

        LoadData();
        if (gameData.collection == null) { return; }
        LoadLevel();
    }

    public void DebugOverwrite()
    {
        oldLevelData = newLevelData;
    }

    public void DebugSave()
    {
        newLevelData = SaveLevel(out _);
    }
    #endregion

    public void LoadData()
    {
        if (string.IsNullOrEmpty(oldLevelData)) { return; }
        var oldPlatformData = JsonUtility.FromJson<PlatformDataCollection>(oldLevelData);
        gameData = oldPlatformData;
    }

    public void LoadLevel()
    {
        Instantiate(maps[gameData.map]);
        ObjectPanel panel = FindObjectOfType<ObjectPanel>();

        if (gameData.collection == null) { return; }
        foreach (PlatformData platformData in gameData.collection)
        {
            var platfromObject = Instantiate(basePlatformObject, new Vector3(platformData.x, platformData.y, 0), Quaternion.identity);
            ObjectRefrenceTable.Instance.objectBases[platformData.ID].InstantiateOldObject(platfromObject, platformData.state, panel, out _);
        }
    }

    public string SaveLevel(out PlatformDataCollection replayMap)
    {
        List<PlatformData> platformDatas = new();
        Platform[] allPlatforms = FindObjectsOfType<Platform>();
        foreach (Platform platform in allPlatforms)
        {
            if (!platform.isNew) { continue; }
            platformDatas.Add(new PlatformData((Vector2)platform.transform.position, platform.ID, platform.state));
        }
        PlatformDataCollection platformDataCollection = new PlatformDataCollection();
        platformDataCollection.collection = platformDatas.ToArray();
        platformDataCollection.map = gameData.map;
        platformDataCollection.user = auth.CurrentUser.UserId;

        if (ReplaySaver.Instance != null) { replayMap = ReplaySaver.Instance.SaveMap(allPlatforms, platformDataCollection); }
        else { replayMap = new(); }

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