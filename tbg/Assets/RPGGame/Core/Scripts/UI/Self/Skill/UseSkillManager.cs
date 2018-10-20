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
        for (int i = 0; i < useSKillItems.Count; i++)
        {
            if (i < ActiveCharacter.CustomSkills.Count)
            {
                useSKillItems[i].SetData(this, i, ActiveCharacter.CustomSkills[i]);
                useSKillItems[i].gameObject.SetActive(true);
            }
            else
            {
                useSKillItems[i].gameObject.SetActive(false);
            }
        }
    }

    public void Hide()
    {

    }

}
