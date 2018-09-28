using System.Collections;
using System.Collections.Generic;
using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

//[RequireComponent(typeof(Animator))]
public abstract class BaseCharacterEntity : MonoBehaviour
{
    public const string ANIM_ACTION_STATE = "_Action";

    [Header("UIs/Effects/Entities Containers")]
    [Tooltip("The transform where we're going to spawn uis")]
    public Transform uiContainer;
    [Tooltip("The transform where we're going to spawn body effects")]
    public Transform bodyEffectContainer;
    [Tooltip("The transform where we're going to spawn floor effects")]
    public Transform floorEffectContainer;
    [Tooltip("The transform where we're going to spawn damage")]
    public Transform damageContainer;



    private PlayerItem item;
    public PlayerItem Item
    {
        get { return item; }
        set
        {
            if (value == null || item == value || value.CharacterData == null)
                return;
            item = value;

            CustomSkills.AddRange(item.CharacterData.GetBattleCustomSkills());
            for (int i = 0; i < CustomSkills.Count; i++)
            {
                CustomSkills[i].Init();
            }
            Revive();
        }
    }
    public readonly Dictionary<string, CustomBuff> Buffs_custom = new Dictionary<string, CustomBuff>();
    public readonly List<CustomSkill> CustomSkills = new List<CustomSkill>();
    public BaseGamePlayFormation Formation { get; protected set; }
    public int Position { get; protected set; }

    public int MaxHp
    {
        get { return (int)GetTotalAttributes().hp; }
    }

    private float hp;
    public float Hp
    {
        get { return hp; }
        set
        {
            hp = value;
            if (hp <= 0)
                Dead();
            if (hp >= MaxHp)
                hp = MaxHp;
        }
    }

    private Transform container;
    public Transform Container
    {
        get { return container; }
        set
        {
            container = value;
            TempTransform.SetParent(container);
            TempTransform.localPosition = Vector3.zero;
            TempTransform.localEulerAngles = Vector3.zero;
            TempTransform.localScale = Vector3.one;
            gameObject.SetActive(true);
        }
    }

    private Transform tempTransform;
    public Transform TempTransform
    {
        get
        {
            if (tempTransform == null)
                tempTransform = GetComponent<Transform>();
            return tempTransform;
        }
    }

    protected virtual void Awake()
    {
        if (uiContainer == null)
            uiContainer = TempTransform;
        if (bodyEffectContainer == null)
            bodyEffectContainer = TempTransform;
        if (floorEffectContainer == null)
            floorEffectContainer = TempTransform;
        if (damageContainer == null)
            damageContainer = TempTransform;
    }


    public void Revive()
    {
        if (Item == null)
            return;

        Hp = MaxHp;
    }

    public virtual void Dead()
    {
        //var keys = new List<string>(Buffs.Keys);
        //for (var i = keys.Count - 1; i >= 0; --i)
        //{
        //    var key = keys[i];
        //    if (!Buffs.ContainsKey(key))
        //        continue;

        //    var buff = Buffs[key];
        //    buff.BuffRemove();
        //    Buffs.Remove(key);
        //}
        var customkeys = new List<string>(Buffs_custom.Keys);
        for (var i = customkeys.Count - 1; i >= 0; --i)
        {
            var key = customkeys[i];
            if (!Buffs_custom.ContainsKey(key))
                continue;

            var buff_c = Buffs_custom[key];
            buff_c.BuffRemove();
            Buffs_custom.Remove(key);
        }
    }

    public CalculationAttributes GetTotalAttributes()
    {
        var result = Item.Attributes;
        var equipmentBonus = Item.EquipmentBonus;
        result += equipmentBonus;

        //var buffs = new List<BaseCharacterBuff>(Buffs.Values);
        //foreach (var buff in buffs)
        //{
        //    result += buff.Attributes;
        //}

        var Custom_buffs = new List<CustomBuff>(Buffs_custom.Values);
        foreach (var buff in Custom_buffs)
        {
            result += buff.SelfAttributes;
        }

        foreach (var cskill in CustomSkills)
        {
            result += cskill.SelfAttributes;
        }

        // If this is character item, applies rate attributes
        result.hp += Mathf.CeilToInt(result.hpRate * result.hp);
        result.pAtk += Mathf.CeilToInt(result.pAtkRate * result.pAtk);
        result.pDef += Mathf.CeilToInt(result.pDefRate * result.pDef);
        result.mAtk += Mathf.CeilToInt(result.mAtkRate * result.mAtk);
        result.mDef += Mathf.CeilToInt(result.mDefRate * result.mDef);
        result.spd += Mathf.CeilToInt(result.spdRate * result.spd);
        result.eva += Mathf.CeilToInt(result.evaRate * result.eva);
        result.acc += Mathf.CeilToInt(result.accRate * result.acc);
        result.hpRate = 0;
        result.pAtkRate = 0;
        result.pDefRate = 0;
        result.mAtkRate = 0;
        result.mDefRate = 0;
        result.spdRate = 0;
        result.evaRate = 0;
        result.accRate = 0;

        return result;
    }

    public virtual void SetFormation(BaseGamePlayFormation formation, int position)
    {
        if (formation == null || position < 0 || position >= formation.containers.Length)
            return;

        Formation = formation;
        Position = position;
        Container = formation.containers[position];
    }


    public virtual void ApplyCustomBuff(CustomBuff buff)
    {
        buff.SetSelf(this as CharacterEntity);
        if (Buffs_custom.ContainsKey(buff.guid))
        {
            return;
        }
        Buffs_custom.Add(buff.guid, buff);
    }
}
