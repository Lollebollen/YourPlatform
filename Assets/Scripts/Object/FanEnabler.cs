using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FanEnabler : MonoBehaviour
{
    [SerializeField] PointEffector2D effector;
    [SerializeField] Collider2D col;
    private void Start()
    {
        ObjectPanel.Instance.DonePlacingPlatforms += EnableFan;
    }

    public void EnableFan()
    {
        effector.enabled = true;
        col.enabled = true;
    }

    private void OnDestroy()
    {
        ObjectPanel.Instance.DonePlacingPlatforms -= EnableFan;
    }
}
