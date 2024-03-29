﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(TargetingRigidbody))]
public class CharacterEntity : BaseCharacterEntity
{
    public bool forcePlayMoving;
    public bool forceHideCharacterStats;
    public int currentTimeCount;
    public bool selectable;
    public RpguiCharacterStats RpguiCharacterStats;

    public GamePlayFormation CastedFormation { get { return Formation as GamePlayFormation; } }
    public GamePlayManager Manager { get { return GamePlayManager.Singleton; } }
    public bool IsActiveCharacter { get { return Manager.ActiveCharacter == this; } }
    public bool IsPlayerCharacter { get { return Formation != null && (CastedFormation != null && CastedFormation.isPlayerFormation); } }
    public string Action { get; set; }
    public bool IsDoingAction { get; set; }
    public Const.SkillType skilltype;
    public bool IsMovingToTarget { get; private set; }

    public CustomSkill SelectedCustomSkill
    {
        get
        {
            if (Item.GetCustomSkills().ContainsKey(Action))
            {
                return Item.GetCustomSkills()[Action];
            }

            return null;
        }
    }

    public CharacterEntity ActionTarget { get; set; }
    public readonly List<Damage> Damages = new List<Damage>();
    private Vector3 targetPosition;
    private CharacterEntity targetCharacter;
    private Coroutine movingCoroutine;
    public bool isReachedTargetCharacter;

    public CustomSkillActionLogic customSkillActionLogic;
    private CustomBody customBody;

    public CustomBody Custombody
    {
        get { return customBody; }
    }

    #region Temp components
    private TargetingRigidbody cacheTargetingRigidbody;
    public TargetingRigidbody CacheTargetingRigidbody
    {
        get
        {
            if (cacheTargetingRigidbody == null)
                cacheTargetingRigidbody = GetComponent<TargetingRigidbody>();
            return cacheTargetingRigidbody;
        }
    }
    #endregion

    #region Unity Functions
    protected override void Awake()
    {
        base.Awake();
        customSkillActionLogic = new CustomSkillActionLogic(this);
        customBody = new CustomBody(this);
        //CacheCapsuleCollider.isTrigger = true;
    }

    private void Update()
    {
        if (Item == null)
        {
            // For show in viewers
            //CacheAnimator.SetBool(ANIM_KEY_IS_DEAD, false);
            //CacheAnimator.SetFloat(ANIM_KEY_SPEED, 0);
            return;
        }
        //CacheAnimator.SetBool(ANIM_KEY_IS_DEAD, Hp <= 0);
        if (Hp > 0)
        {
            var moveSpeed = 1;//CacheRigidbody.velocity.magnitude;
            // Assume that character is moving by set moveSpeed = 1
            if (forcePlayMoving)
                moveSpeed = 1;
            //CacheAnimator.SetFloat(ANIM_KEY_SPEED, moveSpeed);
            if (RpguiCharacterStats != null)
            {
                if (forceHideCharacterStats)
                    RpguiCharacterStats.Hide();
                else
                    RpguiCharacterStats.Show();
            }
        }
        else
        {
            if (RpguiCharacterStats != null)
                RpguiCharacterStats.Hide();
        }
    }


    private void OnDestroy()
    {
        if (RpguiCharacterStats != null)
            Destroy(RpguiCharacterStats.gameObject);
    }
    #endregion




    private void Start()
    {

    }
    #region Damage/Dead/Revive/Turn/Buff
    public AttackInfo Attack(CharacterEntity target, float pAtkRate = 1f, float mAtkRate = 1f, int hitCount = 1, int fixDamage = 0)
    {
        return customBody.Attack(target, pAtkRate, mAtkRate, hitCount, fixDamage);
    }

    public AttackInfo ReceiveDamage(CharacterEntity attacker, int pAtk, int mAtk, int acc, float critChance, float critDamageRate,
        int hitCount = 1, int fixDamage = 0)
    {
        return customBody.ReceiveDamage(attacker, pAtk, mAtk, acc, critChance, critDamageRate, hitCount, fixDamage);
    }


    public override void Dead()
    {
        base.Dead();
        ClearActionState();
    }

    public bool IsStun()
    {
        var keys = new List<string>(Item.GetBuffs().Keys);
        for (var i = keys.Count - 1; i >= 0; --i)
        {
            var key = keys[i];
            if (!Item.GetBuffs().ContainsKey(key))
                continue;

            var buff = Item.GetBuffs()[key];
            if (buff.CanDoAction())
                return true;
        }
        return false;
    }
    public void DecreaseBuffsTurn()
    {
        var keys = new List<string>(Item.GetBuffs().Keys);
        for (var i = keys.Count - 1; i >= 0; --i)
        {
            var key = keys[i];
            if (!Item.GetBuffs().ContainsKey(key))
                continue;

            var buff = Item.GetBuffs()[key];
            buff.ReduceTurnsCount();
            if (buff.IsEnd())
            {
                buff.BuffRemove();
                Item.GetBuffs().Remove(key);
            }
        }
    }

    public void DecreaseSkillsTurn()
    {
        foreach (CustomSkill customSkill in Item.GetCustomSkills().Values)
        {
            customSkill.IncreaseTurnsCount();
        }
    }
    #endregion

    #region Movement/Actions
    public Coroutine MoveTo(Vector3 position, float speed)
    {
        if (IsMovingToTarget)
            StopCoroutine(movingCoroutine);
        IsMovingToTarget = true;
        isReachedTargetCharacter = false;
        targetPosition = position;
        movingCoroutine = StartCoroutine(MoveToRoutine(position, speed));
        return movingCoroutine;
    }

    IEnumerator MoveToRoutine(Vector3 position, float speed)
    {
        CacheTargetingRigidbody.StartPositionMove(position, speed, (ok) =>
        {
            //isReachedTargetCharacter = true;
            if (!CacheTargetingRigidbody.IsMoving || isReachedTargetCharacter)
            {
                IsMovingToTarget = false;
                if (targetCharacter == null)
                {
                    //TurnToEnemyFormation();
                    //TempTransform.localPosition = targetPosition;
                }
                targetCharacter = null;
            }
        });

        while (IsMovingToTarget)
        {
            yield return new WaitForSeconds(0.2f);
        }
        //while (true)
        //{
        //    if (!CacheTargetingRigidbody.IsMoving || isReachedTargetCharacter)
        //    {
        //        IsMovingToTarget = false;
        //        CacheTargetingRigidbody.StopMove();
        //        if (targetCharacter == null)
        //        {
        //            TurnToEnemyFormation();
        //            TempTransform.position = targetPosition;
        //        }
        //        targetCharacter = null;
        //        break;
        //    }
        //    yield return 0;
        //}
        yield return 0;
    }

    public Coroutine MoveTo(CharacterEntity character, float speed)
    {
        targetCharacter = character;
        return MoveTo(character.TempTransform.position, speed);
    }

    public void ClearActionState()
    {
        //CacheAnimator.SetInteger(ANIM_KEY_ACTION_STATE, 0);
        //CacheAnimator.SetBool(ANIM_KEY_DO_ACTION, false);
    }

    public bool SetAction(string action, Const.SkillType skilltype)
    {
        if (!string.IsNullOrEmpty(action))//action >= 0&& action < Skills.Count
        {
            Action = action;
            this.skilltype = skilltype;
            Manager.ShowTargetScopesOrDoAction(this);
            return true;
        }
        return false;
    }










    public void ResetStates()
    {
        Action = Const.NormalAttack;
        ActionTarget = null;
        IsDoingAction = false;
    }

    public void NotifyEndAction()
    {
        ApplySkillAndBuff(CustomSkill.TriggerType.afterfight);
        Manager.NotifyEndAction(this);
    }
    #endregion

    #region Misc

    public override void SetFormation(BaseGamePlayFormation formation, int position)
    {
        base.SetFormation(formation, position);

        if (formation == null || position < 0 || position >= formation.containers.Length)
            return;

        Quaternion headingRotation;
        if (CastedFormation.TryGetHeadingToFoeRotation(out headingRotation))
        {
            TempTransform.rotation = headingRotation;
            if (Manager != null)
                TempTransform.position -= Manager.spawnOffset * TempTransform.forward;
        }
    }






    #endregion


    public void ApplySkillAndBuff(CustomSkill.TriggerType type)
    {

        foreach (var custombuff in Item.GetBuffs().Values)
        {
            custombuff.Trigger(type);
        }
        foreach (var customskill in Item.GetCustomSkills().Values)
        {
            customskill.Trigger(type);
        }
    }




    public bool IsSameTeamWith(CharacterEntity target)
    {
        return target != null && Formation == target.Formation;
    }

    public void RandomAction()
    {
        //normalSkillLogic.RandomAction();
        customSkillActionLogic.RandomAction();
    }

    //返回true是执行，返回false是不可执行
    public bool DoAction(CharacterEntity target)
    {
        if (target == null || target.Hp <= 0)
            return false;
        //if (Action == Const.NormalAttack)
        //{
        //    // Cannot attack self or same team character
        //    if (target == this || IsSameTeamWith(target))
        //        return false;
        //    ActionTarget = target;
        //    DoAttackAction();
        //    return true;
        //}
        return customSkillActionLogic.DoAction(target);

    }

    //void DoAttackAction()
    //{
    //    if (IsDoingAction)
    //        return;
    //    StartCoroutine(DoAttackActionRoutine());
    //}
    public IEnumerator DoAttackActionRoutine()
    {
        IsDoingAction = true;
        // Move to target character
        yield return MoveTo(ActionTarget, Manager.doActionMoveSpeed);
        // Play attack animation
        // Apply damage
        Attack(ActionTarget);
        // Wait damages done
        while (Damages.Count > 0)
        {
            yield return 0;
        }

        ClearActionState();
        yield return MoveTo(Container.position, Manager.actionDoneMoveSpeed);
        NotifyEndAction();
        IsDoingAction = false;
    }
}

public class AttackInfo
{
    public bool baoji;
    public bool shanbi;
    public bool gedang;
    public int totalDamage;
    public bool die;
    public float lastHP;

}
