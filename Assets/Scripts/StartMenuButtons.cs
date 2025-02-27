using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using Firebase.Extensions;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class StartMenuButtons : MonoBehaviour
{
    [SerializeField] TMP_InputField Mail;
    [SerializeField] TMP_InputField password;
    [SerializeField] Button continueButton;
    [SerializeField] GameObject credentials;

    FirebaseAuth auth;
    Animator credentialsAnimator;
    Image credentialsPanel;

    [Header("Cred Colors")]
    [SerializeField] Color signInColor;
    [SerializeField] Color registerColor;

    [HideInInspector] public bool credentialActive = false;
    public enum RegisterOrSignIn
    {
        None,
        Register,
        SignIn
    }

    RegisterOrSignIn registerOrSignIn;

    private void Awake()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { throw task.Exception; }

            auth = FirebaseAuth.DefaultInstance;
            FirebaseUser user = auth.CurrentUser;
            if (user != null)
            {
                
                // TODO check if user has been deleted
                LoadMainMenu();
            }
        });
        credentialsAnimator = credentials.GetComponent<Animator>();
        credentialsPanel = credentials.GetComponent<Image>();
    }

    public void OpenRigister()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(Register);
        credentialsPanel.color = registerColor;
        CredentialsPanelToggle(RegisterOrSignIn.Register);
    }

    public void OpenSignIn()
    {
        continueButton.onClick.RemoveAllListeners();
        continueButton.onClick.AddListener(SignIn);
        credentialsPanel.color = signInColor;
        CredentialsPanelToggle(RegisterOrSignIn.SignIn);
    }

    public void CredentialsPanelToggle(RegisterOrSignIn registerOrSignIn)
    {
        if (credentialActive)
        {
            if (!credentialsAnimator.enabled && registerOrSignIn != RegisterOrSignIn.None && this.registerOrSignIn == registerOrSignIn)
            {
                credentialsAnimator.enabled = true;
                credentialsAnimator.Play("Out");
            }
        }
        else
        {
            if (!credentialsAnimator.enabled)
            {
                credentialsAnimator.enabled = true;
                credentialActive = true;
                credentialsAnimator.Play("In");
            }
        }

        this.registerOrSignIn = registerOrSignIn;
    }

    public void Register()
    {
        Debug.LogFormat("Attempting register: {0} {1}", Mail.text, password.text);
        auth.CreateUserWithEmailAndPasswordAsync(Mail.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                //TODO Feedback
                Debug.Log(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("Registered user: {0} ({1})", newUser.DisplayName, newUser.UserId);
                SignIn();
            }
        });
    }

    public void SignIn()
    {
        auth.SignInWithEmailAndPasswordAsync(Mail.text, password.text).ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null)
            {
                //TODO Feedback
                Debug.Log(task.Exception);
            }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("Signed in user: {0} ({1})", newUser.DisplayName, newUser.UserId);
                LoadMainMenu();
            }
        });
    }

    public void AnonymousSignIn()
    {
        auth.SignInAnonymouslyAsync().ContinueWithOnMainThread(task =>
        {
            if (task.Exception != null) { Debug.Log(task.Exception); }
            else
            {
                FirebaseUser newUser = task.Result.User;
                Debug.LogFormat("Signed in user: {0} ({1})", newUser.DisplayName, newUser.UserId);
                LoadMainMenu();
            }
        });
    }

    public void LoadMainMenu()
    {
        if (SceneManager.GetActiveScene().buildIndex != 1) { SceneManager.LoadScene(1); }
    }
}