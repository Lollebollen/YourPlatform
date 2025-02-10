using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SaveManager : MonoBehaviour
{
    private const string PosKey = "POS";
    private const string InputFieldKey = "InputField";
    private const string SliderKey = "Slider";

    TMP_InputField inputField;
    Slider slider;
    PlayerMovement playerMovement;
    SpriteRenderer spriteRenderer;

    private void Awake()
    {
        inputField = GetComponentInChildren<TMP_InputField>();
        slider = GetComponentInChildren<Slider>();
        playerMovement = FindObjectOfType<PlayerMovement>();
        spriteRenderer = playerMovement.transform.GetComponent<SpriteRenderer>();
    }


    public void Save()
    {
        PlayerPrefs.SetString(InputFieldKey, inputField.text);
        PlayerPrefs.SetFloat(SliderKey, slider.value);
        Vector3 pos = playerMovement.transform.position;
        for (int i = 0; i < 3; i++)
        {
            PlayerPrefs.SetFloat(PosKey + i, pos[i]);
        }
    }

    public void Load()
    {
        inputField.text = PlayerPrefs.GetString(InputFieldKey);
        slider.value = PlayerPrefs.GetFloat(SliderKey);
        PlayerColor();
        Vector3 pos = new();
        for (int i = 0; i < 3; i++)
        {
            pos[i] = PlayerPrefs.GetFloat(PosKey + i);
        }
        playerMovement.transform.position = pos;
    }

    public void PlayerColor()
    {
        spriteRenderer.color = Color.HSVToRGB(slider.value, 1, 1);
    }
}
