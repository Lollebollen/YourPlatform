using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLifes : MonoBehaviour
{
    [SerializeField] int lifes = 3;
    [SerializeField] GameObject lifeIcon;
    [SerializeField] float lifeIconsOffset;
    [SerializeField] GameObject resetButton;

    List<GameObject> lifeContainers = new();

    PlayerMovement playerMovement;

    static PlayerLifes instance;
    static public PlayerLifes Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
    }

    private void Start()
    {
        playerMovement = FindObjectOfType<PlayerMovement>();
        ObjectPanel.Instance.DonePlacingPlatforms += Begin;
    }

    public void Begin()
    {
        ChangeLifeIcons();
        resetButton.SetActive(true);
    }

    public void GainLife()
    {
        lifes++;
        ChangeLifeIcons();
    }

    public void LoseLife()
    {
        if (--lifes < 1) { LoseGame(); }
        else
        {
            playerMovement.ResetPlayer();
        }
        ChangeLifeIcons();
    }

    private void ChangeLifeIcons()
    {
        for (int i = 0; i < lifes; i++)
        {
            if (lifeContainers.Count > i) { continue; }
            lifeContainers.Add(Instantiate(lifeIcon, transform));
        }
        for (int i = lifeContainers.Count - 1; i >= lifes && 0 < lifeContainers.Count; i--)
        {
            Destroy(lifeContainers[i]);
            lifeContainers.RemoveAt(i);
        }
        for (int i = 0; i < lifes; i++)
        {
            lifeContainers[i].GetComponent<RectTransform>().anchoredPosition = new Vector3(lifeIconsOffset * i, 0, 0);
        }
    }

    private void LoseGame()
    {
        FindObjectOfType<LevelManager>().OnLevelComplete();
    }
}
