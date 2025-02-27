using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectPanel : MonoBehaviour
{
    Animator animator;
    Dictionary<Button, ObjectBase> buttons = new();
    Button placingButton;
    GameObject currentPlatformObject;

    int platformTotal;
    int platformCurrent = 0;

    [SerializeField] GameObject platformButtonsParent;
    [SerializeField] GameObject platformBase;
    [SerializeField] Button[] platfromButtons;

    public Action DonePlacingPlatforms;
    public Action StartedPlacingPlatforms;
    public Action PlacedOnePlatform;

    static ObjectPanel instance;
    public static ObjectPanel Instance { get { return instance; } }

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(gameObject); }
        animator = GetComponent<Animator>();
    }

    private void Start()
    {
        DisableAnimator();
        //RandomPlatforms();
    }

    private void RandomPlatforms()
    {
        foreach (Button button in platfromButtons)
        {
            ObjectBase objectBase = ObjectRefrenceTable.Instance.RandomPlatform();
            SetupButton(button, objectBase);
        }

    }

    /// <summary>
    /// Returns true if there are more buttons to define.
    /// </summary>
    /// <param name="objectBase"></param>
    /// <returns></returns>
    public bool AddButtonData(ObjectBase objectBase)
    {
        if (platfromButtons.Length <= 0) { return false; }
        SetupButton(platfromButtons[0], objectBase);
        platfromButtons = platfromButtons[1..];
        return platfromButtons.Length > 0;
    }

    private void SetupButton(Button button, ObjectBase objectBase)
    {
        var image = button.transform.GetChild(0).GetComponent<Image>();
        image.sprite = objectBase.image;
        image.type = Image.Type.Simple;
        image.SetNativeSize();
        buttons.Add(button, objectBase);
        platformTotal = buttons.Count;
    }

    public void PlacePlatform(Button button)
    {
        button.interactable = false;
        placingButton = button;
        Vector3 placePos = Camera.main.transform.position;
        placePos.z = 0;
        currentPlatformObject = Instantiate(platformBase, placePos, Quaternion.identity);
        buttons[button].InstantiateNewObject(currentPlatformObject, this, out _);

        RemovePanel();
        placingButton.enabled = true;
    }

    public void PlacingDone()
    {
        var drag = currentPlatformObject.GetComponent<Drag>();
        if (!drag.CanPlace()) { return; }
        platformButtonsParent.SetActive(false);
        Destroy(drag);
        Destroy(placingButton.gameObject);

        if (platformTotal <= ++platformCurrent && DonePlacingPlatforms != null) { DonePlacingPlatforms(); }
        else { ShowPanel(); }
    }

    public void PlacingCancelled()
    {
        placingButton.interactable = true;
        platformButtonsParent.SetActive(false);
        ShowPanel();
        Destroy(currentPlatformObject);
    }

    private void RemovePanel()
    {
        StartedPlacingPlatforms?.Invoke();
        animator.enabled = true;
        animator.Play("Remove");
    }

    public void ShowPanel()
    {
        PlacedOnePlatform?.Invoke();
        animator.enabled = true;
        animator.Play("Add");
    }

    public void DisableAnimator()
    {
        animator.enabled = false;
    }

    public void ShowPlaceingButton()
    {
        platformButtonsParent.SetActive(true);
    }
}
