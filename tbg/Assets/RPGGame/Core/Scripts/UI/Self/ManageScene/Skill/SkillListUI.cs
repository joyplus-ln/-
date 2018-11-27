using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillListUI : MonoBehaviour
{
    public Transform content;
    private List<CustomSkill> skills;
    public UiSkillListItem skillitem;

    public GameObject SkillDesPanel;

    public Text SkillDesText;
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
            item.GetComponent<UiSkillListItem>().SetData(skill, SkillDes);
        }
    }

    public void SkillDes(string skilldes)
    {
        SkillDesText.text = skilldes;
        SkillDesPanel.SetActive(true);
    }

    void DeleteChild()
    {
        foreach (Transform child in content)
        {
            Destroy(child.gameObject);
        }
    }
}
