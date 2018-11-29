using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class RpguiAuthentication : RPGUIBase
{
    public InputField inputUsername;
    public InputField inputPassword;
    public UnityEvent eventLogin;
    public UnityEvent eventRegister;
    public UnityEvent eventError;

    public string Username
    {
        get { return inputUsername == null ? "" : inputUsername.text; }
        set { if (inputUsername != null) inputUsername.text = value; }
    }

    public string Password
    {
        get { return inputPassword == null ? "" : inputPassword.text; }
        set { if (inputPassword != null) inputPassword.text = value; }
    }

    public void OnClickLogin()
    {
        GameInstance.dbLogin.DoLogin(Username, Password, OnLoginSuccess);
    }

    public void OnClickRegister()
    {
        GameInstance.dbLogin.DoRegister(Username, Password, OnRegisterSuccess);
    }

    public void OnClickRegisterOrLogin()
    {
        GameInstance.dbLogin.DoRegisterOrLogin(Username, Password, OnLoginSuccess);
    }

    public void OnClickGuestLogin()
    {
        var duid = SystemInfo.deviceUniqueIdentifier;
        GameInstance.dbLogin.DoGuestLogin(duid, OnLoginSuccess);
    }

    private void OnLoginSuccess(PlayerResult result)
    {
        var gameInstance = GameInstance.Singleton;
        gameInstance.OnGameServiceLogin(result);
        eventLogin.Invoke();
    }

    private void OnRegisterSuccess(PlayerResult result)
    {
        var gameInstance = GameInstance.Singleton;
        eventRegister.Invoke();
    }

    private void OnError(string error)
    {
        var gameInstance = GameInstance.Singleton;
        gameInstance.OnGameServiceError(error);
        eventError.Invoke();
    }
}
