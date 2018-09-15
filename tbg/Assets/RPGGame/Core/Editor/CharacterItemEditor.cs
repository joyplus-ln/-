using System.Collections;
using System.Collections.Generic;
using UnityEditor;

[CustomEditor(typeof(CharacterItem))]
[CanEditMultipleObjects]
public class CharacterItemEditor : BaseCustomEditor
{
    private CharacterItem cacheItem;
    protected override void SetFieldCondition()
    {
        //if (cacheItem == null)
            //cacheItem = CreateInstance<CharacterItem>();
    }
}
