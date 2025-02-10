using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReplaySaver : MonoBehaviour
{
    [SerializeField] float saveInterval;

    List<float> times = new();
    List<Vector2> positions = new();

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
            positions.Add(transform.position);
            lastSnapshot = time;
        }
    }

    public void OnDonePlacingPlatfroms() { save = true; }


    public string GetReplay()
    {
        times.Add(Time.time);
        positions.Add(transform.position);
        float startTime = times[0];
        for (int i = 0; i < times.Count; i++)
        {
            times[i] -= startTime;
        }
        return JsonUtility.ToJson(new Ghost(times.ToArray(), positions.ToArray()));
    }

    private void OnDisable()
    {
        ObjectPanel.Instance.DonePlacingPlatforms -= OnDonePlacingPlatfroms;
    }
}