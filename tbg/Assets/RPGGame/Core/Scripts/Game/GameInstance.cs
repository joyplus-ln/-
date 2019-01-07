using System.Collections;
using System.Collections.Generic;
using System.Linq;
using SQLite3TableDataTmp;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameInstance : MonoBehaviour
{
    public enum LoadAllPlayerDataState
    {
        GoToManageScene,
        GoToBattleScene,
    }
    public GameDatabase gameDatabase;
    public RpguiMessageDialog messageDialog;
    public RpguiItemList rewardItemsDialog;
    public GameObject loadingObject;
    public BaseCharacterEntity model;//公用model
    public static GameInstance Singleton { get; private set; }
    public static GameDatabase GameDatabase { get; private set; }
    private readonly Queue<RpguiMessageDialog.Data> messageDialogData = new Queue<RpguiMessageDialog.Data>();
    private LoadAllPlayerDataState loadAllPlayerDataState;
    private static int countLoading = 0;

    public static DBMapItem dbMapItem;
    public static DBDataUtils dbDataUtils;
    public static DBTableUtils dbTableUtils;
    public static DBLogin dbLogin;
    public static DBPlayerData dbPlayerData;
    public static DBBattle dbBattle;

    private void Awake()
    {
        if (Singleton != null)
        {
            Destroy(gameObject);
            return;
        }
        Singleton = this;
        DontDestroyOnLoad(gameObject);

        GameDatabase = gameDatabase;
        if (GameDatabase == null)
            Debug.LogError("`Game Database` has not been set");
        else
            GameDatabase.Setup();

        DBManager.instance.Init();
        dbTableUtils = new DBTableUtils();
        dbTableUtils.Init();
        dbDataUtils = new DBDataUtils();
        dbDataUtils.Init();
        dbLogin = new DBLogin();
        dbPlayerData = new DBPlayerData();
        dbBattle = new DBBattle();
        dbMapItem = new DBMapItem();
        dbMapItem.Init();

        //GameService.onServiceStart.RemoveListener(OnGameServiceStart);
        //GameService.onServiceStart.AddListener(OnGameServiceStart);
        //GameService.onServiceFinish.RemoveListener(OnGameServiceFinish);
        //GameService.onServiceFinish.AddListener(OnGameServiceFinish);

        HideMessageDialog();
        HideRewardItemsDialog();
        HideLoading();
    }

    private void OnGameServiceStart()
    {
        ShowLoading();
    }

    private void OnGameServiceFinish()
    {
        HideLoading();
    }

    public void OnGameServiceError(string error, UnityAction errorAction)
    {
        Debug.LogError("OnGameServiceError: " + error);
        var errorText = string.IsNullOrEmpty(error) || !RPGLanguageManager.Texts.ContainsKey(error) ? "" : RPGLanguageManager.Texts[error];
        messageDialogData.Enqueue(new RpguiMessageDialog.Data(RPGLanguageManager.Texts[GameText.TITLE_ERROR_DIALOG], errorText, errorAction));
        ShowError();
    }

    public void OnGameServiceError(string error)
    {
        OnGameServiceError(error, null);
    }

    #region Error/Warning/Loading Handler
    private void ShowError()
    {
        if (messageDialogData.Count > 0)
        {
            var data = messageDialogData.Dequeue();
            ShowMessageDialog(data.title, data.content, () =>
            {
                ShowError();
                if (data.actionYes != null)
                    data.actionYes.Invoke();
            }, data.actionNo, data.actionCancel);
        }
    }

    public void ShowMessageDialog(string title,
        string content,
        UnityAction actionYes = null,
        UnityAction actionNo = null,
        UnityAction actionCancel = null)
    {
        if (messageDialog == null)
        {
            Debug.LogWarning("`Message Dialog` has not been set");
            return;
        }
        if (!messageDialog.IsVisible())
        {
            messageDialog.Title = title;
            messageDialog.Content = content;
            messageDialog.actionYes = actionYes;
            messageDialog.actionNo = actionNo;
            messageDialog.actionCancel = actionCancel;
            messageDialog.Show();
        }
    }

    public void HideMessageDialog()
    {
        if (messageDialog == null)
        {
            Debug.LogWarning("`Message Dialog` has not been set");
            return;
        }
        messageDialog.Hide();
    }


    public void HideRewardItemsDialog()
    {
        if (rewardItemsDialog == null)
        {
            Debug.LogWarning("Reward Items Dialog` has not been set");
            return;
        }
        rewardItemsDialog.Hide();
    }

    public void WarnNotEnoughSoftCurrency()
    {
        OnGameServiceError(GameServiceErrorCode.NOT_ENOUGH_SOFT_CURRENCY);
    }

    public void WarnNotEnoughHardCurrency()
    {
        OnGameServiceError(GameServiceErrorCode.NOT_ENOUGH_HARD_CURRENCY);
    }

    public void WarnNotEnoughStageStamina()
    {
        OnGameServiceError(GameServiceErrorCode.NOT_ENOUGH_STAGE_STAMINA);
    }

    public void ShowLoading()
    {
        if (loadingObject == null)
        {
            Debug.LogWarning("`Loading Object` has not been set");
            return;
        }
        ++countLoading;
        if (countLoading > 0)
            loadingObject.SetActive(true);
    }

    public void HideLoading()
    {
        if (loadingObject == null)
        {
            Debug.LogWarning("`Loading Object` has not been set");
            return;
        }
        --countLoading;
        if (countLoading <= 0)
        {
            loadingObject.SetActive(false);
            countLoading = 0;
        }
    }
    #endregion

}
