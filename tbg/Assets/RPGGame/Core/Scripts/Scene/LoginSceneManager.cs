using System.Collections;
using System.Collections.Generic;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.UI;

public class LoginSceneManager : MonoBehaviour
{
    public static LoginSceneManager Singleton { get; private set; }
    public GameObject clickStartObject;
    public GameObject InputNameGameObject;
    public Text inputText;
    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        ShowClickStart();
    }

    public void OnClickStart()
    {
        //GameInstance.dbLogin.DoValidateLoginToken(true, OnValidateLoginTokenSuccess);
        if (IPlayer.CurrentPlayer == null)
        {
            InputNameGameObject.SetActive(true);
        }
        else
        {
            RPGSceneManager.LoadScene(RPGSceneManager.ManagerScene);

        }
        HideClickStart();
    }




    public void ShowClickStart()
    {
        if (clickStartObject != null)
            clickStartObject.SetActive(true);
    }

    public void HideClickStart()
    {
        if (clickStartObject != null)
            clickStartObject.SetActive(false);
    }


    public void OnClickGuestLogin()
    {
        var duid = SystemInfo.deviceUniqueIdentifier;
        string name = inputText.text;
        if (!string.IsNullOrEmpty(name))
        {
            IPlayer player = new IPlayer();
            player.guid = duid;
            player.Level = 1;
            player.exp = 0;
            player.profileName = name;
            IPlayer.InsertNewPlayer(player);
            IPlayerStamina iPlayerStamina = new IPlayerStamina(1, duid, duid, 1, 1, duid);
            IPlayerStamina.SetData(iPlayerStamina);
            IPlayerStamina.UpdataDataMap();
            RPGSceneManager.LoadScene(RPGSceneManager.ManagerScene);
        }
        else
        {
            Debug.LogError("名字不可以为空");
        }

    }

}
