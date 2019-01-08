using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillManager : MonoBehaviour
{
    public List<UseSKillItem> useSKillItems = new List<UseSKillItem>();
    public Transform SelectedTransform;

    public SkillPower sikllPower;

    public CharacterEntity ActiveCharacter { set; get; }
    // Use this for initialization
    void Start()
    {
    }

    public void SetData(CharacterEntity ActiveCharacter)
    {
        this.ActiveCharacter = ActiveCharacter;
        int index = 0;
        foreach (string key in ActiveCharacter.Item.GetCustomSkills().Keys)
        {
            useSKillItems[index].SetData(this, key, ActiveCharacter.Item.GetCustomSkills()[key]);
            useSKillItems[index].gameObject.SetActive(true);
            index++;
        }

        for (int i = index; i < useSKillItems.Count; i++)
        {
            useSKillItems[i].gameObject.SetActive(false);
        }
        //for (int i = 0; i < useSKillItems.Count; i++)
        //{
        //    if (i < ActiveCharacter.Item.GetCustomSkills().Count)
        //    {
        //        useSKillItems[i].SetData(this, i, ActiveCharacter.CustomSkills[i]);
        //        useSKillItems[i].gameObject.SetActive(true);
        //    }
        //    else
        //    {
        //        useSKillItems[i].gameObject.SetActive(false);
        //    }
        //}
        //刷新
        useSKillItems[0].RestartAndSelected();
    }


    public void Hide()
    {
        for (int i = 0; i < useSKillItems.Count; i++)
        {
            useSKillItems[i].gameObject.SetActive(false);
        }
    }

}
