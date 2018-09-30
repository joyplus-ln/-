using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class WearEquipment : MonoBehaviour
{

    public WearEquipmentItem weapon, cloth, shoot;
    // Use this for initialization
    void Start()
    {

    }

    public void SetData(PlayerItem selectedItem)
    {
        List<PlayerItem> items = PlayerItem.equipDataMap.Values.Where(x => x.EquipItemGuid == selectedItem.CharacterData.itemid).ToList();
        for (int i = 0; i < items.Count; i++)
        {
            switch (items[i].EquipmentData.category)
            {
                case "weapon":
                    weapon.SetData(items[i]);
                    break;
                case "cloth":
                    cloth.SetData(items[i]);
                    break;
                case "shoot":
                    shoot.SetData(items[i]);
                    break;
            }
        }
        if (items.Count > 3)
        {
            Debug.LogError("ERROR !");
        }
    }

}
