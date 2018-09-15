using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//[RequireComponent(typeof(Rigidbody))]
//[RequireComponent(typeof(CapsuleCollider))]
//[RequireComponent(typeof(TargetingRigidbody))]
public class CharacterEntity : BaseCharacterEntity
{
    public const string ANIM_KEY_IS_DEAD = "IsDead";
    public const string ANIM_KEY_SPEED = "Speed";
    public const string ANIM_KEY_ACTION_STATE = "ActionState";
    public const string ANIM_KEY_DO_ACTION = "DoAction";
    public const string ANIM_KEY_HURT = "Hurt";
    public const int ACTION_ATTACK = -100;
    [HideInInspector]
    public bool forcePlayMoving;
    [HideInInspector]
    public bool forceHideCharacterStats;
    [HideInInspector]
    public int currentTimeCount;
    [HideInInspector]
    public bool selectable;
    [HideInInspector]
    public UICharacterStats uiCharacterStats;

    public GamePlayFormation CastedFormation { get { return Formation as GamePlayFormation; } }
    public TowerGamePlayFormation TowerCastedFormation { get { return Formation as TowerGamePlayFormation; } }
    public GamePlayManager Manager { get { return GamePlayManager.Singleton; } }
    public TowerGamePlayManager TowerManager { get { return TowerGamePlayManager.Singleton; } }
    public bool IsActiveCharacter { get { return Manager.ActiveCharacter == this; } }
    public bool IsPlayerCharacter { get { return Formation != null && (CastedFormation != null && CastedFormation.isPlayerFormation || TowerCastedFormation != null && TowerCastedFormation.isPlayerFormation); } }
    public int Action { get; set; }
    public bool IsDoingAction { get; set; }
    public Const.SkillType skilltype;
    public bool IsMovingToTarget { get; private set; }
    public CharacterSkill SelectedSkill { get { return Skills[Action] as CharacterSkill; } }

    public CustomSkill SelectedCustomSkill
    {
        get
        {
            if (Action < 0) return null;
            if (Action - Skills.Count >= CustomSkills.Count)
                return null;
            return CustomSkills[Action - Skills.Count];
        }
    }

    public CharacterEntity ActionTarget { get; set; }
    public readonly List<Damage> Damages = new List<Damage>();
    private Vector3 targetPosition;
    private CharacterEntity targetCharacter;
    private Coroutine movingCoroutine;
    public bool isReachedTargetCharacter;

    private NormalSkillLogic normalSkillLogic;
    private CustomSkillActionLogic customSkillActionLogic;
    private CustomBody customBody;

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
            if (uiCharacterStats != null)
            {
                if (forceHideCharacterStats)
                    uiCharacterStats.Hide();
                else
                    uiCharacterStats.Show();
            }
        }
        else
        {
            if (uiCharacterStats != null)
                uiCharacterStats.Hide();
        }
    }

    //private void OnTriggerStay(Collider other)
    //{
    //    if (targetCharacter != null && targetCharacter == other.GetComponent<CharacterEntity>())
    //        isReachedTargetCharacter = true;
    //}

    private void OnDestroy()
    {
        if (uiCharacterStats != null)
            Destroy(uiCharacterStats.gameObject);
    }
    #endregion




    private void Start()
    {
        normalSkillLogic = new NormalSkillLogic(this);
        customSkillActionLogic = new CustomSkillActionLogic(this);
        customBody = new CustomBody(this);
    }
    #region Damage/Dead/Revive/Turn/Buff
    public void Attack(CharacterEntity target, float pAtkRate = 1f, float mAtkRate = 1f, int hitCount = 1, int fixDamage = 0)
    {
        customBody.Attack(target, pAtkRate, mAtkRate, hitCount, fixDamage);
    }

    public AttackInfo ReceiveDamage(int pAtk, int mAtk, int acc, float critChance, float critDamageRate,
        int hitCount = 1, int fixDamage = 0)
    {
        return customBody.ReceiveDamage(pAtk, mAtk, acc, critChance, critDamageRate, hitCount, fixDamage);
    }
    public override void InitPassiveSkill()
    {
        base.InitPassiveSkill();
        foreach (var passiveskill in PassiveSkills)
        {
            passiveskill.Init(this);
        }
    }


    public void Attack(CharacterEntity target, Damage damagePrefab, float pAtkRate = 1f, float mAtkRate = 1f, int hitCount = 1, int fixDamage = 0)
    {
        if (damagePrefab == null)
        {
            Attack(target, pAtkRate, mAtkRate, hitCount, fixDamage);
        }
        else
        {
            var damage = Instantiate(damagePrefab, damageContainer.position, damageContainer.rotation);
            damage.transform.SetParent(transform, false);
            damage.Setup(this, target, pAtkRate, mAtkRate, hitCount, fixDamage);
        }
    }



    public override void Dead()
    {
        base.Dead();
        ClearActionState();
    }

    public bool IsStun()
    {
        var keys = new List<string>(Buffs.Keys);
        for (var i = keys.Count - 1; i >= 0; --i)
        {
            var key = keys[i];
            if (!Buffs.ContainsKey(key))
                continue;

            var buff = Buffs[key] as CharacterBuff;
            if (buff.GetIsStun())
                return true;
        }
        return false;
    }
    public void DecreaseBuffsTurn()
    {
        var keys = new List<string>(Buffs.Keys);
        for (var i = keys.Count - 1; i >= 0; --i)
        {
            var key = keys[i];
            if (!Buffs.ContainsKey(key))
                continue;

            var buff = Buffs[key] as CharacterBuff;
            buff.IncreaseTurnsCount();
            if (buff.IsEnd())
            {
                buff.BuffRemove();
                Buffs.Remove(key);
            }
        }
    }

    public void DecreaseSkillsTurn()
    {
        for (var i = Skills.Count - 1; i >= 0; --i)
        {
            var skill = Skills[i] as CharacterSkill;
            skill.IncreaseTurnsCount();
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

    public void TurnToEnemyFormation()
    {
        Quaternion headingRotation;
        if (CastedFormation != null && CastedFormation.TryGetHeadingToFoeRotation(out headingRotation))
            TempTransform.rotation = headingRotation;
        if (TowerCastedFormation != null && TowerCastedFormation.TryGetHeadingToFoeRotation(out headingRotation))
            TempTransform.rotation = headingRotation;
    }

    public void ClearActionState()
    {
        //CacheAnimator.SetInteger(ANIM_KEY_ACTION_STATE, 0);
        //CacheAnimator.SetBool(ANIM_KEY_DO_ACTION, false);
    }

    public bool SetAction(int action, Const.SkillType skilltype)
    {
        if (action == ACTION_ATTACK || (action >= 0))//action >= 0&& action < Skills.Count
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
        Action = ACTION_ATTACK;
        ActionTarget = null;
        IsDoingAction = false;
    }

    public void NotifyEndAction()
    {
        ApplySkillAndBuff(TriggerType.afterfight);
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

    public override BaseCharacterSkill NewSkill(int level, BaseSkill skill)
    {
        return new CharacterSkill(level, skill);
    }

    public override BaseCharacterBuff NewBuff(int level, BaseSkill skill, int buffIndex, BaseCharacterEntity giver, BaseCharacterEntity receiver)
    {
        return new CharacterBuff(level, skill, buffIndex, giver, receiver);
    }

    public override BaseCharacterBuff NewPassiveBuff(int level, PassiveSkill skill, int buffIndex, BaseCharacterEntity giver, BaseCharacterEntity receiver)
    {
        return new CharacterBuff(level, skill, buffIndex, giver, receiver);
    }

    #endregion


    public void ApplySkillAndBuff(TriggerType type)
    {
        foreach (var skill in PassiveSkills)
        {
            if (skill.type == type)
                skill.ApplyPassiveSkill();
        }
        foreach (var custombuff in Buffs_custom.Values)
        {
            custombuff.Trigger(type);
        }
    }




    public bool IsSameTeamWith(CharacterEntity target)
    {
        return target != null && Formation == target.Formation;
    }

    public void RandomAction()
    {
        normalSkillLogic.RandomAction();
    }

    //返回true是执行，返回false是不可执行
    public bool DoAction(CharacterEntity target)
    {
        if (target == null || target.Hp <= 0)
            return false;
        if (Action == CharacterEntity.ACTION_ATTACK)
        {
            // Cannot attack self or same team character
            if (target == this || IsSameTeamWith(target))
                return false;
            ActionTarget = target;
            normalSkillLogic.DoAction();
            DoAttackAction();
            return true;
        }
        if (skilltype == Const.SkillType.Normal)
            return normalSkillLogic.DoAction(target);
        else
            return customSkillActionLogic.DoAction(target);

    }

    void DoAttackAction()
    {
        if (IsDoingAction)
            return;
        StartCoroutine(DoAttackActionRoutine());
    }
    public IEnumerator DoAttackActionRoutine()
    {
        IsDoingAction = true;
        // Move to target character
        yield return MoveTo(ActionTarget, Manager.doActionMoveSpeed);
        // Play attack animation
        // Apply damage
        Attack(ActionTarget,null);
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

}
