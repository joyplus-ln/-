using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpguiHistoryManager : RPGUIBase
{
    public RPGUIBase defaultEntry;
    public GameObject[] showingObjectsWhenStayAtDefaultEntry;
    public GameObject[] hiddingObjectsWhenStayAtDefaultEntry;
    public readonly Stack<RPGUIBase> ShownUis = new Stack<RPGUIBase>();

    protected override void Awake()
    {
        base.Awake();
        UpdateShowingAndHiddingObjects();
        if (defaultEntry != null)
            defaultEntry.Show();
    }

    public void UpdateShowingAndHiddingObjects()
    {
        foreach (var obj in showingObjectsWhenStayAtDefaultEntry)
        {
            obj.SetActive(ShownUis.Count <= 0);
        }
        foreach (var obj in hiddingObjectsWhenStayAtDefaultEntry)
        {
            obj.SetActive(ShownUis.Count > 0);
        }
    }

    public void Back()
    {
        if (ShownUis.Count > 0)
        {
            var showUi = ShownUis.Pop();
            showUi.Hide();
        }
        if (ShownUis.Count > 0)
            ShownUis.Peek().Show();
        else if (defaultEntry != null)
            defaultEntry.Show();
        UpdateShowingAndHiddingObjects();
    }

    public void Next(RPGUIBase Rpgui)
    {
        if (Rpgui == null)
            return;
        if (ShownUis.Count > 0)
            ShownUis.Peek().Hide();
        else if (defaultEntry != null)
            defaultEntry.Hide();
        ShownUis.Push(Rpgui);
        Rpgui.Show();
        UpdateShowingAndHiddingObjects();
    }
}
