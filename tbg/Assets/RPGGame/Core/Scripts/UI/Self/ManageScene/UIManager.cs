using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public List<BaseUI> uilist;
    private BaseUI currentUI;

    // Use this for initialization
    void Start()
    {

    }

    public void OpenUI(BaseUI ui)
    {
        if (uilist.Contains(ui))
        {
            for (int i = 0; i < uilist.Count; i++)
            {
                if (uilist[i] != ui) uilist[i].Hide();
            }
            ui.Show();
            currentUI = ui;
        }
    }

    public void BackUI()
    {
        if (currentUI) currentUI.Hide();
    }

}
