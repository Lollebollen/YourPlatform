using System;
using UnityEngine;

[Serializable]
public struct PlatformData
{
    public float x;
    public float y;
    public int ID;
    public int state;

    public PlatformData(Vector2 position, int ID, int state)
    {
        x = position.x;
        y = position.y;
        this.ID = ID;
        this.state = state;
    }
}

[Serializable]
public struct PlatformDataCollection
{
    public PlatformData[] Collection;
    public int map;
    public string user;
}

[Serializable]
public struct GameDatas
{
    public PlatformDataCollection[] gameData;
}

[Serializable]
public struct GameStates
{
    public Active[] activeStatus;
}

[Serializable]
public struct Active
{
    public bool isActive;
    public long timeSetActive;

    public Active (bool isActive, long timeSetActive)
    {
        this.isActive = isActive;
        this.timeSetActive = timeSetActive;
    }
}

[Serializable]
public struct WeightedValue <type>
{
    public type value;
    public int weight;
}
