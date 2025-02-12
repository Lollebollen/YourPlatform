using UnityEngine;

[System.Serializable]
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

[System.Serializable]
public struct PlatformDataCollection
{
    public PlatformData[] collection;
    public int map;
    public string user;
}

[System.Serializable]
public struct GameDatas
{
    public PlatformDataCollection[] gameData;
}

[System.Serializable]
public struct GameStates
{
    public Active[] activeStatus;
}

[System.Serializable]
public struct Active
{
    public bool isActive;
    public long timeSetActive;

    public Active(bool isActive, long timeSetActive)
    {
        this.isActive = isActive;
        this.timeSetActive = timeSetActive;
    }
}

[System.Serializable]
public struct WeightedValue<type>
{
    public type value;
    public int weight;
}

[System.Serializable]
public class Ghost
{
    public float[] times;
    public float[] x;
    public float[] y;

    public Ghost(float[] times, float[] x, float[] y)
    {
        this.times = times;
        this.x = x;
        this.y = y;
    }
}

public class GhostWithMap
{
    public PlatformDataCollection platformDataCollection;
    public float[] times;
    public float[] x;
    public float[] y;

    public GhostWithMap(PlatformDataCollection platformDataCollection, float[] times, float[] x, float[] y)
    {
        this.platformDataCollection = platformDataCollection;
        this.times = times;
        this.x = x;
        this.y = y;
    }
}
