using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FormationItemShow : MonoBehaviour
{

    public Text nameText;
    // Use this for initialization
    void Start()
    {

    }

    public void Show(PlayerFormation item)
    {
        nameText.text = PlayerItem.characterDataMap[item.characterGuid].CharacterData.title;
    }
}
