using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogController : MonoBehaviour
{
    public static DialogController instance = null;

    void Awake()
    {
        instance = this;
    }
    #region dialogs

    public ShopHeroDialog ShopHeroDialog;
    public ShopEquipDialog ShopEquipDialog;
    /// <summary>
    /// 队伍中选择自己拥有的英雄
    /// </summary>
    public SelfHeroSelectDialog SelfHeroSelectDialog;
    /// <summary>
    /// 队伍中选择后点击equip
    /// </summary>
    public SelfHeroEquipSelectDialog SelfHeroEquipSelectDialog;

    /// <summary>
    /// 选择穿戴的装备的dialog
    /// </summary>
    public SelfHeroSelectChangeEquipDialog selfHeroSelectChangeEquipDialog;

    /// <summary>
    /// 更换队伍的dialog
    /// </summary>
    public HeroFormationDialog heroFormationDialog;
    #endregion
    public enum DialogType
    {
        back,
        dontShow,
        wait,
        stack
    }
    private Dialog currentDialog = null;
    private Stack<Dialog> dialogQueue = new Stack<Dialog>();
    public Dialog ShowDialog(Dialog dialog, DialogType type)
    {
        Dialog cdialog = null;
        switch (type)
        {
            case DialogType.back:
                Back(dialog);
                break;
            case DialogType.dontShow:
                break;
            case DialogType.wait:
                cdialog = Wait(dialog);
                break;
            case DialogType.stack:
                cdialog = Stack(dialog);
                break;
        }

        return cdialog;
    }

    Dialog Back(Dialog dialog)
    {
        currentDialog = dialog;
        dialog.gameObject.SetActive(true);
        return dialog;
    }


    Dialog Wait(Dialog dialog)
    {
        if (currentDialog != null)
        {
            Dialog cDialog = CreatDialog(dialog);
            cDialog.gameObject.SetActive(false);
            dialogQueue.Push(cDialog);
            return cDialog;
        }
        else
        {
            currentDialog = CreatDialog(dialog);
            return currentDialog;
        }
    }

    Dialog Stack(Dialog dialog)
    {
        if (currentDialog != null)
        {
            currentDialog.gameObject.SetActive(false);
            dialogQueue.Push(currentDialog);
            currentDialog = CreatDialog(dialog);
            return currentDialog;
        }
        else
        {
            currentDialog = CreatDialog(dialog);
            return currentDialog;
        }
    }

    Dialog CreatDialog(Dialog dialog)
    {
        Dialog insDialog = null;

        insDialog = Instantiate(dialog);
        insDialog.name += dialogQueue.Count;
        insDialog.Init();


        insDialog.close += Close;
        return insDialog;
    }

    /// <summary>
    /// 关闭当前的
    /// </summary>
    void Close()
    {
        if (currentDialog != null)
        {
            currentDialog = null;
        }
        if (dialogQueue.Count > 0)
        {
            Dialog dialog = dialogQueue.Pop();
            if (dialog != null)
            {
                currentDialog = dialog;
                ShowDialog(dialog, DialogType.back);
            }
        }


    }
}

public class DialogData
{
    public GameObject CreaatedGameObject = null;
    public Dialog dialog = null;
    public object obj = null;
}
