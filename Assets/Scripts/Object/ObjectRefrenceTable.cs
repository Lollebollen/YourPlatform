using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRefrenceTable : MonoBehaviour
{
    private static ObjectRefrenceTable instance;
    public static ObjectRefrenceTable Instance { get { return instance; } }

    [SerializeField] ObjectBase[] objectBasesArray = new ObjectBase[0];
    public Dictionary<int, ObjectBase> objectBases = new();

    private void Awake()
    {
        if (instance == null) { instance = this; }
        else { Destroy(this); }
        PopulateObjectBases();
    }

    private void PopulateObjectBases()
    {
        foreach (ObjectBase item in objectBasesArray)
        {
            objectBases.Add(item.ID, item);
        }
    }

    public ObjectBase RandomPlatform()
    {
        return objectBasesArray[Random.Range(0, objectBasesArray.Length)];
    }
}
