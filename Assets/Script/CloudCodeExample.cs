using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.CloudCode;
using Unity.Services.Core;
using UnityEngine;

/*
* Note that you need to have a published script in order to use the Cloud Code SDK.
* You can do that from the Unity Dashboard - https://dashboard.unity3d.com/
*/
public class CloudCodeExample : MonoBehaviour
{

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


    public void SignUp()
    {
        Task task = SignUpWithUsernamePasswordAsync("NewUser1", "DDct@me1234");
    }

    public void SignIn()
    {
        Task task = SignInWithUsernamePasswordAsync("NewUser1", "DDct@me1234");
    }

    async Task SignUpWithUsernamePasswordAsync(string username, string password)
    {
        try
        {
            await AuthenticationService.Instance.SignUpWithUsernamePasswordAsync(username, password);
            Debug.Log("SignUp is successful.");
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
            Debug.Log("SignIn is successful.");
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
        OnPlayerLogin(username);
    }

    public async void OnPlayerRegister(string name)
    {
        var arguments = new Dictionary<string, object> { { "name", name } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<CloudCodeResponse>("TestScript", arguments);
        Debug.Log(response.welcomeMessage);
    }
    public async void OnPlayerLogin(string name)
    {
        var arguments = new Dictionary<string, object> { { "name", name } };
        var response = await CloudCodeService.Instance.CallEndpointAsync<CloudCodeResponse>("TestScript", arguments);
        Debug.Log(response.welcomeMessage);
    }
}
