using UnityEditor;
using UnityEngine;

public class MenuTools
{
    #region Open folder
    [MenuItem("Framework/Open Files/PersistentData Folder", priority = 50)]
    static void OpenPersistentData()
    {
        System.Diagnostics.Process.Start(Application.persistentDataPath);
    }

    [MenuItem("Framework/Open Files/Assets Folder", priority = 53)]
    static void OpenAssets()
    {
        System.Diagnostics.Process.Start(Application.dataPath);
    }

    [MenuItem("Framework/Open Files/StreamingAssets Folder", priority = 55)]
    static void OpenStreamingAssets()
    {
        System.Diagnostics.Process.Start(Application.streamingAssetsPath);
    }
    #endregion

    #region Local data
    [MenuItem("Framework/Local Player Data/Clear PlayerPrefs", priority = 25)]
    static void ClearPlayPrefs()
    {
        PlayerPrefs.DeleteAll();
    }
    #endregion

    #region Run Mode

    private static string[] debugMacroDef = { "AD_SDK", "FB_SDK", "FTDSdk", "IAP_SDK", "DEBUG_MODE" };
   // private static string[] releaseMacroDef = { "AD_SDK", "FB_SDK", "FTDSdk", "IAP_SDK" };
    // private static string editorMacroDef = string.Empty;

    [MenuItem("Framework/Runtime Mode/Debug", true, priority = 70)]
    static bool IsDebugRuntimeMode()
    {
        string[] def = EditorUserBuildSettings.activeScriptCompilationDefines;
        int len = def.Length;
        if (len == debugMacroDef.Length)
        {
            for (int i = 0; i < len; i++)
            {
                bool exist = false;
                for (int j = 0; j < len; j++)
                {
                    if (def[i] == debugMacroDef[j])
                    {
                        exist = true;
                        break;
                    }

                    if (!exist) return true;
                }
            }

            return false;
        }

        return true;
    }

    [MenuItem("Framework/Runtime Mode/Debug", false, priority = 70)]
    static void SetDebugRuntimeMode()
    {
        //EditorUserSettings.
        //EditorUserBuildSettings.activeScriptCompilationDefines = debugMacroDef;
    }

    [MenuItem("Framework/Runtime Mode/Release", priority = 73)]
    static void SetReleaseRuntimeMode()
    {
        PlayerPrefs.DeleteAll();
    }

    [MenuItem("Framework/Runtime Mode/Debug", priority = 75)]
    static void SwitchRuntimeMode()
    {
        PlayerPrefs.DeleteAll();
    }

    #endregion
}