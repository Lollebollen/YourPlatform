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

    Dictionary<Button, ButtonData> buttonTypePairs;
    Dictionary<Button, ObjectBase> buttonObjectPairs;
    WeightedList<PlatformPoolType> poolsList;
    WeightedList<ObjectBase> standardPlatformsList;
    WeightedList<ObjectBase> allPlatformsList;
    WeightedList<ObjectBase> pickupsList;
    WeightedList<ObjectBase> specialList;
    WeightedList<ObjectBase> allList;

    private void Awake()
    {
        InitializeLists();
        SetNewOptions();
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
        foreach (Button button in objectButtons) { buttonObjectPairs.Add(button,  null); }
    }

    public void SetNewOptions()
    {
        for (int i = 0; i < poolsList.max; i++)
        {
            Debug.Log(poolsList.Get(i));
        }
        PlatformPoolType poolType;
        foreach (var button in buttonTypePairs)
        {
            poolType = poolsList.Get(Random.Range(0, poolsList.max + 1));
            buttonTypePairs[button.Key].poolType = poolType;
            Color color;

            switch (poolType) // TODO visuals
            {
                case PlatformPoolType.standardPlatforms:
                    buttonTypePairs[button.Key].list = standardPlatformsList;
                    color = Color.white;
                    break;
                case PlatformPoolType.allPlatforms:
                    buttonTypePairs[button.Key].list = allPlatformsList;
                    color = Color.red;
                    break;
                case PlatformPoolType.pickups:
                    buttonTypePairs[button.Key].list = pickupsList;
                    color = Color.green;
                    break;
                case PlatformPoolType.special:
                    buttonTypePairs[button.Key].list = specialList;
                    color = Color.blue;
                    break;
                default:
                    buttonTypePairs[button.Key].list = allList;
                    color = Color.yellow;
                    break;
            }

            button.Key.gameObject.GetComponent<Image>().color = color;
        }
    }

    public void ButtonPressed(Button button)
    {
        WeightedList<ObjectBase> list;
        switch (buttonTypePairs[button].poolType)
        {
            case PlatformPoolType.standardPlatforms:
                list = standardPlatformsList;
                break;
            case PlatformPoolType.allPlatforms:
                list = allPlatformsList;
                break;
            case PlatformPoolType.pickups:
                list = pickupsList;
                break;
            case PlatformPoolType.special:
                list = specialList;
                break;
            default:
                list = allList;
                break;
        }

        foreach (var item in buttonObjectPairs)
        {
            buttonObjectPairs[item.Key] = list.Get(Random.Range(0, list.max));
            SetButtonSprite(item.Key.transform, buttonObjectPairs[item.Key]);
        }

        // TODO animation to other panel
    }

    private void SetButtonSprite(Transform transform, ObjectBase objectBase)
    {
        Image image = transform.GetChild(0).GetComponent<Image>();
        image.sprite = objectBase.image;
        image.type = Image.Type.Simple;
        image.SetNativeSize();
    }

    public void AddObjectBase(Button button) { ObjectPanel.Instance.AddButtonData(buttonObjectPairs[button]); }
}

public class ButtonData
{
    public PlatformPoolType poolType;
    public WeightedList<ObjectBase> list;
}