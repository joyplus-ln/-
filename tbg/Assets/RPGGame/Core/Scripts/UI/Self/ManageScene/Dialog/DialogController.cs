﻿using System.Collections;
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
    #endregion
    public enum DialogType
    {
        replace,
        dontShow,
        wait,
        stack
    }
    private DialogData currentDialog = null;
    private Stack<DialogData> dialogQueue = new Stack<DialogData>();
    public void ShowDialog(DialogData dialog, DialogType type)
    {
        switch (type)
        {
            case DialogType.replace:
                break;
            case DialogType.dontShow:
                break;
            case DialogType.wait:
                Wait(dialog);
                break;
            case DialogType.stack:
                Stack(dialog);
                break;
        }
    }

    void Replace(DialogData dialog)
    {
        if (currentDialog != null)
        {
            currentDialog.dialog.Close();
            currentDialog = CreatDialog(dialog);
        }
    }

    void DontShow(DialogData dialog)
    {
        if (currentDialog != null)
        {
            return;
        }
    }

    void Wait(DialogData dialog)
    {
        if (currentDialog != null)
        {
            dialogQueue.Push(dialog);
        }
        else
        {
            currentDialog = CreatDialog(dialog);
        }
    }

    void Stack(DialogData dialog)
    {
        if (currentDialog != null)
        {
            currentDialog.CreaatedGameObject.SetActive(false);
            dialogQueue.Push(currentDialog);
            currentDialog = CreatDialog(dialog);
        }
        else
        {
            currentDialog = CreatDialog(dialog);
        }
    }

    DialogData CreatDialog(DialogData dialog)
    {
        Dialog insDialog = null;
        if (dialog.CreaatedGameObject != null)
        {
            insDialog = dialog.CreaatedGameObject.GetComponent<Dialog>();
        }
        else
        {
            insDialog = Instantiate(dialog.dialog);
            insDialog.name += dialogQueue.Count;
            dialog.CreaatedGameObject = insDialog.gameObject;
        }

        insDialog.close += Close;
        return dialog;
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
        DialogData dialog = dialogQueue.Pop();
        if (dialog != null)
        {
            if (dialog.CreaatedGameObject != null)
            {
                currentDialog = dialog;
                dialog.CreaatedGameObject.SetActive(true);
            }
            else
            {
                ShowDialog(dialog, DialogType.wait);
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
