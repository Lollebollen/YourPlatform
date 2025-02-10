using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CredentialsFunctions : MonoBehaviour
{
    StartMenuButtons menu;
    Animator animator;

    private void Awake()
    {
        menu = transform.root.GetComponentInChildren<StartMenuButtons>();
        animator = GetComponent<Animator>();
    }

    public void AnimationDone()
    {
        animator.enabled = false;
    }

    public void Out()
    {
        menu.credentialActive = false;
    }
}
