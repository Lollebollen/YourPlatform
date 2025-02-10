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
    public PlatformData[] Collection;
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

    public Active (bool isActive, long timeSetActive)
    {
        this.isActive = isActive;
        this.timeSetActive = timeSetActive;
    }
}

[System.Serializable]
public struct WeightedValue <type>
{
    public type value;
    public int weight;
}

[System.Serializable]
public struct Ghost
{
    public float[] times;
    public Vector2[] positions;

    public Ghost(float[] times, Vector2[] positions)
    {
        this.times = times;
        this.positions = positions;
    }
}
