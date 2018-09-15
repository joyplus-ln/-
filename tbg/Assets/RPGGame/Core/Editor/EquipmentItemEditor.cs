using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(EquipmentItem))]
[CanEditMultipleObjects]
public class EquipmentItemEditor : BaseCustomEditor
{
    private EquipmentItem cacheItem;
    protected override void SetFieldCondition()
    {
        //if (cacheItem == null)
            //cacheItem = CreateInstance<EquipmentItem>();
    }
}
