using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class SkillListShow : MonoBehaviour
{

    public CharacterEntity entity;
    public Text SkillText;
    private StringBuilder textBuilder;

    // Use this for initialization
    void Start()
    {
        //entity.Skills
        //entity.Skills[0].GetCoolDownDurationRate();
        textBuilder = new StringBuilder();
    }

    private void Update()
    {
        if (entity != null)
        {
            textBuilder.Remove(0, textBuilder.Length);
            for (int i = 0; i < entity.Skills.Count; i++)
            {
                textBuilder.Append(entity.Skills[i].GetCoolDownDuration());
            }
            SkillText.text = textBuilder.ToString();
        }
    }



}
