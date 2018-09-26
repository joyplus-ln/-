using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UseSkillManager : MonoBehaviour
{
    public List<UseSKillItem> useSKillItems = new List<UseSKillItem>();
    public Transform SelectedTransform;
    // Use this for initialization
    void Start()
    {
        SetData(5);
    }

    public void SetData(int num)
    {
        for (int i = 0; i < useSKillItems.Count; i++)
        {
            if (i < num)
            {
                useSKillItems[i].SetData(this);
                useSKillItems[i].gameObject.SetActive(true);
            }
            else
            {
                useSKillItems[i].gameObject.SetActive(false);
            }
        }
    }

}
