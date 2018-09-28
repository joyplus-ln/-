using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillListUI : MonoBehaviour
{
    public Transform content;
    private List<CustomSkill> skills;
    public UiSkillListItem skillitem;
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(List<CustomSkill> skills)
    {
        this.skills = skills;
        DeleteChild();
        StartCoroutine(CreatSkill());
    }

    IEnumerator CreatSkill()
    {
        GameObject item = null;
        yield return null;
        foreach (var skill in skills)
        {
            item = Instantiate(skillitem.gameObject);
            item.transform.SetParent(content, false);
            item.GetComponent<UiSkillListItem>().SetData(skill);
        }
    }

    void DeleteChild()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
