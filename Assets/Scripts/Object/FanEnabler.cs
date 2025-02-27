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
        ObjectPanel.Instance.DonePlacingPlatforms += removeSprite;
        ObjectPanel.Instance.PlacedOnePlatform += removeSprite;
        GhostHandler.Instance.startGhost += removeSprite;
    }

    public void EnableFan()
    {
        effector.enabled = true;
        col.enabled = true;
    }

    public void removeSprite()
    {
        transform.parent.GetComponent<SpriteRenderer>().enabled = false;
    }

    private void OnDestroy()
    {
        ObjectPanel.Instance.DonePlacingPlatforms -= EnableFan;
        ObjectPanel.Instance.DonePlacingPlatforms -= removeSprite;
        ObjectPanel.Instance.PlacedOnePlatform -= removeSprite;
        GhostHandler.Instance.startGhost -= removeSprite;
    }
}
