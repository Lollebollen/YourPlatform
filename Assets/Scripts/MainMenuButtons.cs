using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuButtons : MonoBehaviour
{
    FirebaseAuth auth;
    FirebaseDatabase database;

    [SerializeField] Button startButton;
    [SerializeField] GameDatas gameDatasForTesting;
    [SerializeField] GameStates GameStatesForTestting;

    int gameDataindex = -1;
    string gameData;
    int gameStateindex = -2;
    long startTime;

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

            DataSnapshot snapshot = task.Result;
            CheckForOpenGame(snapshot);
        });
    }

    private void CheckForOpenGame(DataSnapshot snapShot)
    {
        GameStates gameStates = JsonUtility.FromJson<GameStates>(snapShot.GetRawJsonValue());

        for (int i = 0; i < gameStates.activeStatus.Length; i++) // TODO make it go in a random order
        {
            long time = gameStates.activeStatus[i].timeSetActive;
            if (!gameStates.activeStatus[i].isActive || (time < System.DateTime.UtcNow.Ticks && new System.DateTime(time).Day != System.DateTime.UtcNow.Day))
            {
                SetGameActive(i);
                FetchGameData(i);
                // things to save to game scene game index, time and platformdata
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
                TryToStartGame();
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
                TryToStartGame();
            }
        });
    }

    private void TryToStartGame()
    {
        if (gameDataindex == gameStateindex && gameData != null && gameData.Length > 1 && startTime != default)
        {
            Debug.Log("starting new game");
            LevelManager.Instance.gameIndex = gameDataindex;
            LevelManager.Instance.oldLevelData = gameData;
            LevelManager.Instance.startTime = startTime;
            SceneManager.LoadScene(2);
        }
    }
}