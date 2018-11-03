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
#endregion
    public enum DialogType
    {
        replace,
        dontShow,
        wait
    }
    private Dialog currentDialog = null;
    private Queue<DialogData> dialogQueue = new Queue<DialogData>();
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
        }
    }

    void Replace(DialogData dialog)
    {
        if (currentDialog != null)
        {
            currentDialog.Close();
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
            dialogQueue.Enqueue(dialog);
        }
        else
        {
            currentDialog = CreatDialog(dialog);
        }
    }

    Dialog CreatDialog(DialogData dialog)
    {
        Dialog insDialog = Instantiate(dialog.dialog);
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
        DialogData dialog = dialogQueue.Dequeue();
        if (dialog != null)
        {
            ShowDialog(dialog, DialogType.wait);
        }

    }
}

public class DialogData
{
    public Dialog dialog = null;
    public object obj = null;
}
