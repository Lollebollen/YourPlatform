using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceingButtons : MonoBehaviour
{
    [SerializeField] ObjectPanel panel;

    public void Accept()
    {
        panel.PlacingDone();
    }

    public void Cancel()
    {
        panel.PlacingCancelled();
    }
}
