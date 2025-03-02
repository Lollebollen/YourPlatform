using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaySaver : MonoBehaviour
{
    [SerializeField] float saveInterval;

    List<float> times = new();
    List<float> x = new();
    List<float> y = new();

    float lastSnapshot = 0;
    bool save = false;

    static ReplaySaver instance;
    public static ReplaySaver Instance {  get { return instance; } }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        ObjectPanel.Instance.DonePlacingPlatforms += OnDonePlacingPlatfroms;
    }

    private void Update()
    {
        float time = Time.time;
        if (save && time - lastSnapshot > saveInterval)
        {
            times.Add(time);
            AddPosition(transform.position);
            lastSnapshot = time;
        }
    }

    public void OnDonePlacingPlatfroms() { save = true; }


    public Ghost GetReplay()
    {
        times.Add(Time.time);
        AddPosition(transform.position);
        float startTime = times[0];
        for (int i = 0; i < times.Count; i++)
        {
            times[i] -= startTime;
        }
        return new Ghost(times.ToArray(), x.ToArray(), y.ToArray());
    }

    public PlatformDataCollection SaveMap(Platform[] platforms, PlatformDataCollection collection)
    {
        List<PlatformData> list = new List<PlatformData>();
        foreach (Platform platform in platforms)
        {
            list.Add(new PlatformData((Vector2)platform.transform.position, platform.ID, platform.state));
        }
        collection.collection = list.ToArray();
        return collection;
    }

    private void AddPosition(Vector3 pos)
    {
        x.Add(pos.x);
        y.Add(pos.y);
    }

    private void OnDisable()
    {
        ObjectPanel.Instance.DonePlacingPlatforms -= OnDonePlacingPlatfroms;
    }
}