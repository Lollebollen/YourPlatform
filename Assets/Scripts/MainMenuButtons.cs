using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseDatabase database;

    [SerializeField] Button startButton;
    [SerializeField] Button replayButton;
    [SerializeField] GameDatas gameDatasForTesting;
    [SerializeField] GameStates GameStatesForTestting;

    int gameDataindex = -1;
    string gameData;
    int gameStateindex = -2;
    long startTime;
    Ghost ghost;

    private void Awake()
    {
        auth = FirebaseAuth.DefaultInstance;
        database = FirebaseDatabase.DefaultInstance;
        database.SetPersistenceEnabled(false);
    }

    public void SignOut()
    {
        auth.SignOut();
        SceneManager.LoadScene(0);
    }

    public void Play()
    {
        startButton.interactable = false;
        database.RootReference.Child("gameStates").GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }

            CheckForOpenGame(task.Result);
        });
    }

    public void WatchReplay()
    {
        replayButton.interactable = false;

        database.RootReference.Child("replays").Child(auth.CurrentUser.UserId).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
            DataSnapshot snapshot = task.Result;

            if (snapshot.Exists && snapshot.ChildrenCount > 0) { BeginReplay(snapshot); }
            else { FaildReplayFetch(); }
        });
    }

    private void BeginReplay(DataSnapshot snapshot)
    {
        GhostWithMap ghostWithMap = new();
        foreach (DataSnapshot item in snapshot.Children)
        {
            ghostWithMap = JsonUtility.FromJson<GhostWithMap>(item.GetRawJsonValue());
            if (ghostWithMap != null) { break; }
        }
        if (ghostWithMap == null) { FaildReplayFetch(); }
        gameData = JsonUtility.ToJson(ghostWithMap.platformDataCollection);
        ghost = ghostWithMap.ghost;
        StartGame(true);
    }

    private void FaildReplayFetch()
    {
        Debug.LogWarning("There was no replay to watch");
        replayButton.interactable = true;
    }

    private void CheckForOpenGame(DataSnapshot snapShot)
    {
        GameStates gameStates = JsonUtility.FromJson<GameStates>(snapShot.GetRawJsonValue());

        for (int i = 0; i < gameStates.activeStatus.Length; i++) // TODO make it random order
        {
            long time = gameStates.activeStatus[i].timeSetActive;
            if (!gameStates.activeStatus[i].isActive || (time < System.DateTime.UtcNow.Ticks && new System.DateTime(time).Day != System.DateTime.UtcNow.Day))
            {
                SetGameActive(i);
                FetchGameData(i);
                return;
            }
        }

        startButton.interactable = true;
        Debug.LogWarning("No game is active, Try again later");
    }

    private void SetGameActive(int i)
    {
        Active active = new Active(true, System.DateTime.UtcNow.Ticks);
        database.RootReference.Child("gameStates").Child("activeStatus").Child(i.ToString()).SetRawJsonValueAsync(JsonUtility.ToJson(active)).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
            else
            {
                gameStateindex = i;
                startTime = active.timeSetActive;
                TryToStartGame(false);
            }
        });
    }

    private void FetchGameData(int i)
    {
        database.RootReference.Child("games").Child("gameData").Child(i.ToString()).GetValueAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
            else
            {
                gameDataindex = i;
                gameData = task.Result.GetRawJsonValue();
                TryToStartGame(false);
            }
        });
    }

    private void TryToStartGame(bool isReplay)
    {
        if (gameDataindex == gameStateindex && gameData != null && gameData.Length > 1 && startTime != default)
        {
            StartGame(isReplay);
        }
    }

    private void StartGame(bool isReplay)
    {
        Debug.Log("starting new game");
        LevelManager.Instance.oldLevelData = gameData;
        LevelManager.Instance.isReplay = isReplay;
        if (isReplay)
        {
            LevelManager.Instance.ghost = ghost;
        }
        else
        {
            LevelManager.Instance.startTime = startTime;
            LevelManager.Instance.gameIndex = gameDataindex;
        }
        SceneManager.LoadScene(2);
    }
}