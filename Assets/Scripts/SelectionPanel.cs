using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;

public enum PlatformPoolType : byte
{
    standardPlatforms,
    allPlatforms,
    pickups,
    special,
    all
}

public class SelectionPanel : MonoBehaviour
{
    [SerializeField] WeightedValue<PlatformPoolType>[] pools;
    [SerializeField] WeightedValue<ObjectBase>[] standardPlatforms;
    [SerializeField] WeightedValue<ObjectBase>[] allPlatforms;
    [SerializeField] WeightedValue<ObjectBase>[] pickups;
    [SerializeField] WeightedValue<ObjectBase>[] special;
    [SerializeField] WeightedValue<ObjectBase>[] all;
    [SerializeField] Button[] typeButtons;
    [SerializeField] Button[] objectButtons;

    [SerializeField] Sprite standardPlatformImage;
    [SerializeField] Sprite allPlatformsImage;
    [SerializeField] Sprite pickupsImage;
    [SerializeField] Sprite specialImage;
    [SerializeField] Sprite allImage;

    Dictionary<Button, ButtonData> buttonTypePairs;
    Dictionary<Button, ObjectBase> buttonObjectPairs;
    WeightedList<PlatformPoolType> poolsList;
    WeightedList<ObjectBase> standardPlatformsList;
    WeightedList<ObjectBase> allPlatformsList;
    WeightedList<ObjectBase> pickupsList;
    WeightedList<ObjectBase> specialList;
    WeightedList<ObjectBase> allList;

    Animator animator;
    Animator objectButtonsAnimator;

    static SelectionPanel instance;
    public static SelectionPanel Instance { get { return instance; } }

    private void Awake()
    {
        InitializeLists();
        SetNewOptions();

        if (instance == null) { instance = this; }
        else { Destroy(this); }

        animator = GetComponent<Animator>();
        animator.enabled = false;
    }

    private void InitializeLists()
    {
        poolsList = new WeightedList<PlatformPoolType>(pools.Select(x => x.value).ToArray(), pools.Select(x => x.weight).ToArray());
        standardPlatformsList = new WeightedList<ObjectBase>(standardPlatforms.Select(x => x.value).ToArray(), standardPlatforms.Select(x => x.weight).ToArray());
        allPlatformsList = new WeightedList<ObjectBase>(allPlatforms.Select(x => x.value).ToArray(), allPlatforms.Select(x => x.weight).ToArray());
        pickupsList = new WeightedList<ObjectBase>(pickups.Select(x => x.value).ToArray(), pickups.Select(x => x.weight).ToArray());
        specialList = new WeightedList<ObjectBase>(special.Select(x => x.value).ToArray(), special.Select(x => x.weight).ToArray());
        allList = new WeightedList<ObjectBase>(all.Select(x => x.value).ToArray(), all.Select(x => x.weight).ToArray());

        buttonTypePairs = new();
        buttonObjectPairs = new();
        foreach (Button button in typeButtons) { buttonTypePairs.Add(button, new()); }
        foreach (Button button in objectButtons) { buttonObjectPairs.Add(button, null); }
        objectButtonsAnimator = objectButtons[0].GetComponentInParent<Animator>();
    }

    public void SetNewOptions()
    {
        PlatformPoolType poolType;
        foreach (var button in buttonTypePairs)
        {
            poolType = poolsList.Get(Random.Range(0, poolsList.max));
            buttonTypePairs[button.Key].poolType = poolType;
            Sprite sprite;

            switch (poolType) // TODO visuals
            {
                case PlatformPoolType.standardPlatforms:
                    buttonTypePairs[button.Key].list = standardPlatformsList;
                    sprite = standardPlatformImage;
                    break;
                case PlatformPoolType.allPlatforms:
                    buttonTypePairs[button.Key].list = allPlatformsList;
                    sprite = allPlatformsImage;
                    break;
                case PlatformPoolType.pickups:
                    buttonTypePairs[button.Key].list = pickupsList;
                    sprite = pickupsImage;
                    break;
                case PlatformPoolType.special:
                    buttonTypePairs[button.Key].list = specialList;
                    sprite = specialImage;
                    break;
                default:
                    buttonTypePairs[button.Key].list = allList;
                    sprite = allImage;
                    break;
            }

            button.Key.transform.GetChild(0).GetComponent<Image>().sprite = sprite;
        }
    }

    public void Begin()
    {
        Show(animator);
    }

    public void Show(Animator animator)
    {
        animator.enabled = true;
        animator.Play("Show");
    }

    public void Hide(Animator animator)
    {
        animator.enabled = true;
        animator.Play("Hide");
    }



    public void EnableObjectInteractivity() { MakeInteractable(true, buttonObjectPairs.Keys.ToArray()); }
    public void DisableObjectInteractivity() { MakeInteractable(false, buttonObjectPairs.Keys.ToArray()); }
    public void EnableInteractivity() { MakeInteractable(true, buttonTypePairs.Keys.ToArray()); }
    public void DisableInteractivity() { MakeInteractable(false, buttonTypePairs.Keys.ToArray()); }
    private void MakeInteractable(bool state, Button[] buttons) { foreach (var button in buttons) { button.interactable = state; } }

    public void StopAnimator() { animator.enabled = false; }

    public void ButtonPressed(Button button)
    {
        WeightedList<ObjectBase> list = buttonTypePairs[button].poolType switch
        {
            PlatformPoolType.standardPlatforms => standardPlatformsList,
            PlatformPoolType.allPlatforms => allPlatformsList,
            PlatformPoolType.pickups => pickupsList,
            PlatformPoolType.special => specialList,
            _ => allList,
        };
        foreach (var item in buttonObjectPairs.ToList())
        {
            buttonObjectPairs[item.Key] = list.Get(Random.Range(0, list.max));
            SetButtonSprite(item.Key.transform, buttonObjectPairs[item.Key]);
        }

        Hide(animator);
        Show(objectButtonsAnimator);
    }

    private void SetButtonSprite(Transform transform, ObjectBase objectBase)
    {
        Image image = transform.GetChild(0).GetComponent<Image>();
        image.sprite = objectBase.image;
        image.type = Image.Type.Simple;
        image.SetNativeSize();
    }

    public void AddObjectBase(Button button)
    {
        Hide(objectButtonsAnimator);
        if (ObjectPanel.Instance.AddButtonData(buttonObjectPairs[button])) 
        {
            SetNewOptions();
            Show(animator); 
        }
        else { ObjectPanel.Instance.ShowPanel(); }
    }
}

public class ButtonData
{
    public PlatformPoolType poolType;
    public WeightedList<ObjectBase> list;
}