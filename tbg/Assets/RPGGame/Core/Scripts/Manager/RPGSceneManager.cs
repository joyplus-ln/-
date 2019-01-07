using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RPGSceneManager : MonoBehaviour
{
    public const string LoginScene = "LoginScene";
    public const string ManagerScene = "ManageScene";
    public const string BattleScene = "BattleScene";

    public static void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }
}
