using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RpguiCharacterActionAttack : RpguiCharacterAction
{
    protected override void OnActionSelected()
    {
        ActionManager.ActiveCharacter.SetAction(CharacterEntity.ACTION_ATTACK,Const.SkillType.Attack);
    }
}
