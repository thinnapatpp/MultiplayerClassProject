using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Authentication.PlayerAccounts;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/*
* Note that you need to have a published script in order to use the Cloud Code SDK.
* You can do that from the Unity Dashboard - https://dashboard.unity3d.com/
*/
public class CloudCodeExample : MonoBehaviour
{
    [SerializeField] private TMP_InputField usernameInputField;
    [SerializeField] private TMP_InputField passwordInputField;
    [SerializeField] private TextMeshProUGUI welcomeMessageText;
    [SerializeField] private TextMeshProUGUI errorMessageText;
    [SerializeField] private GameObject authenSection;
    [SerializeField] private GameObject loginSection;
    private string currentUsername;
    private string currentPassword;
    private string oldPassword;
    private string newPassword;
    private class CloudCodeResponse
    {
        public string welcomeMessage;
    }
    async void Awake()
    {
        try
        {
            await UnityServices.InitializeAsync();
        }
        catch (Exception e)
        {
            Debug.LogException(e);
        }
    }

    void Start()
    {
        if (usernameInputField != null && passwordInputField != null)
        {
            usernameInputField.onEndEdit.AddListener(OnUsernameInputFieldEndEdit);
            passwordInputField.onEndEdit.AddListener(OnPasswordInputFieldEndEdit);
        }
    }
    void OnUsernameInputFieldEndEdit(string inputText)
    {
        currentUsername = inputText;
    }
    void OnPasswordInputFieldEndEdit(string inputText)
    {
        currentPassword = inputText;
    }
    public void SignUp()
    {
        Task task = SignUpWithUsernamePasswordAsync(currentUsername, currentPassword);
    }

    public void SignIn()
    {
        Task task = SignInWithUsernamePasswordAsync(currentUsername, currentPassword);
    }

    public void ChangePassword()
    {
        Task task = UpdatePasswordAsync(oldPassword, newPassword);
    }

    public void SignOut()
    {
        AuthenticationService.Instance.SignedOut();
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            errorMessageText.text = "SignUp is successful.";
            errorMessageText.color = Color.green;
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            Debug.LogException(ex);
        }
        OnPlayerRegister(username);
    }

    async Task SignInWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignInWithUsernamePasswordAsync(username, password);
            errorMessageText.text = "SignIn is successful.";
            errorMessageText.color = Color.green;
        }
        catch (AuthenticationException ex)
        {
            // Compare error code to AuthenticationErrorCodes
            // Notify the player with the proper error message
            //Debug.LogException(ex);
        }
        catch (RequestFailedException ex)
        {
            // Compare error code to CommonErrorCodes
            // Notify the player with the proper error message
            //Debug.LogException(ex);
        }
        OnPlayerLogin(username);
    }
    async Task UpdatePasswordAsync(string currentPassword, string newPassword)
    {
        try
        {
            await AuthenticationService.Instance.UpdatePasswordAsync(currentPassword, newPassword);
            Debug.Log("Password updated.");
        }
        catch (AuthenticationException ex)
        {

        }
        catch (RequestFailedException ex)
        {

        }
    }

    public async void OnPlayerRegister(string name)
    {
        var arguments = new Dictionary<string, object> { { "name", name } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<CloudCodeResponse>("TestScript", arguments);
        welcomeMessageText.text = response.welcomeMessage;
        OnLoginSection();
    }
    public async void OnPlayerLogin(string name)
    {
        var arguments = new Dictionary<string, object> { { "name", name } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<CloudCodeResponse>("TestScript", arguments);
        welcomeMessageText.text = response.welcomeMessage;
        OnLoginSection();
    }

    public void OnLoginSection()
    {
        authenSection.SetActive(!authenSection.activeSelf);
        loginSection.SetActive(!loginSection.activeSelf);
    }
}
