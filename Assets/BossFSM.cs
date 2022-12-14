using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle, Trace, Back, Attack, RangeAttack,GetHit, Dead,
}
public class BossFSM : MonoBehaviour
{
    private BossState m_NowState;

    private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    private GameObject TeleportPoint01;
    private GameObject TeleportPoint02;

    public GameObject AxeOnHand;
    public GameObject BigSwordOnHand;
    public GameObject AxeOnBack;
    public GameObject BigSwordOnBack;

    MubHpData State;
    Animator MubAnimator;
    int hpTemporary;
    int hpTemporaryMax;
    CharacterController capsule;

    bool backing;
    bool tracing;
    bool InRangeATKrange;
    bool RangeCDtodo;
    bool InATKrange;
    bool GetHit;
    bool StageTwo=false;
    bool IK=false;//2階段單次判定，不會重複進入2階段

    float TraceRadius;
    float ATKRadius;
    float RangedRadius;

    int RCount;
    int Count;
    int UCount;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
   
    int RandomChoose;
    int RandomChooseHit;
    bool RandomChooseCd = false;

    ItemOnMob ThisItemOnMob_State;

    // Start is called before the first frame update
    private void Awake()
    {
        ThisItemOnMob_State = GetComponent<ItemOnMob>();
        TeleportPoint01 = GameObject.Find("TeleportPoint01");
        TeleportPoint02 = GameObject.Find("TeleportPoint02");
    }
    void Start()
    {
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己

        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();

        //血量區
        hpTemporary = State.Hp;
        hpTemporaryMax = State.Hp;


        //初始狀態
        m_NowState = BossState.Idle;

        //範圍設定
        ATKRadius = ThisItemOnMob_State.mobRadius; ;//Weapon覆蓋
        RangedRadius = ATKRadius * 1.8f;
        TraceRadius = ATKRadius * 2f;

        GetHit = false;

        //初始冷卻設定
        Count = 0;
        RCount = 0;
        UCount = 40;
    }

    // Update is called once per frame
    void Update()
    {
       
        if (State.Hp <= 0)//死亡→無狀態
        {
            m_NowState = BossState.Dead;
            DeadStatus();
            return;
        }
        else if ((float)hpTemporary/ (float)hpTemporaryMax <=0.5f&& IK == false)
        {            
            StageTwo = true;
            IK = true;
            StageTwoCost();
        }
        else if (State.Hp != hpTemporary)
        {
            hpTemporary = State.Hp;
            if (GetHit==false)
            {
                RandomChooseHit = UnityEngine.Random.Range(1, 2);
                if (RandomChooseHit == 1)
                {
                    MubAnimator.SetBool("GetHit01", true);
                    GetHit = true;
                    return;
                }
                if (RandomChooseHit == 2)
                {
                    MubAnimator.SetBool("GetHit02", true);
                    GetHit = true;
                    return;
                }
            }
        }        
        else
        {
            MubAnimator.SetBool("GetHit01", false);
            MubAnimator.SetBool("GetHit02", false);
            GetHit = false;

            LookPoint();

            TraceStatus();

            AttackStatus();

            Ultimate();
        }
        Debug.Log(m_NowState);
    }
    public void StageTwoCost()
    {
        if (StageTwo == true)
        {
            MubAnimator.SetTrigger("T2");
        }                
    }
    public void LookPoint() 
    {
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

        transform.rotation = Quaternion.Slerp(this.transform.rotation, Look, 7f * Time.deltaTime);
    }
    public void DeadStatus()
    {
        if (m_NowState == BossState.Dead)
        {
            MubAnimator.SetTrigger("Die");
            capsule.radius = 0f;
        }
    }
    //追擊狀態
    public void TraceStatus()
    {        
        tracing = IsInRange_TraceRange(RangedRadius, MySelf, Target);
        if (tracing == true)
        {
            m_NowState = BossState.Trace;
            Debug.Log("NowInT");
            if (m_NowState == BossState.Trace)
            {
                MubAnimator.SetBool("Trace", true);
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
                MubAnimator.SetBool("Attack04", false);
                MubAnimator.SetBool("R_Attack", false);
            }
        }
    }
    public void RangedAttackStatus()
    {
        InRangeATKrange = IsInRange_RangedBattleRange(RangedRadius, MySelf, Target);
        if (InRangeATKrange == true)
        {          
            m_NowState = BossState.RangeAttack;
            MubAnimator.SetBool("Trace", false);
            RangeAttack();
        }
    }
    //攻擊狀態
    public void AttackStatus()
    {
        InATKrange = IsInRange_BattleRange(ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            m_NowState = BossState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            Attack();
        }
    }
   
    public void RangeAttack()
    {
        if (m_NowState == BossState.RangeAttack && RCount == 0)
        {
            MubAnimator.SetBool("R_Attack", true);
        }
        else if (m_NowState == BossState.RangeAttack&&RCount != 0)
        {
            RangeCDtodo = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (RangeCDtodo == true)
            {
                m_NowState = BossState.Trace;;
                if (m_NowState == BossState.Trace)
                {
                    MubAnimator.SetBool("Trace", true);                    
                    MubAnimator.SetBool("R_Attack", false);
                }
            }         
        }
    }

    public void Attack()
    {
        if (m_NowState == BossState.Attack && Count == 0)
        {
            if (RandomChooseCd == false)
            {
                RandomChoose = UnityEngine.Random.Range(1, 4);
                RandomChooseCd = true;
            }
            Debug.Log("隨機數:" + RandomChoose);
            if (RandomChoose == 1)
            {
                MubAnimator.SetBool("Attack01", true);
            }
            else if (RandomChoose == 2)
            {
                MubAnimator.SetBool("Attack02", true);
            }
            else if (RandomChoose == 3)
            {
                MubAnimator.SetBool("Attack03", true);
            }
            else if (RandomChoose == 4)
            {
                MubAnimator.SetBool("Attack04", true);
            }
        }
        else if (m_NowState == BossState.Attack && Count != 0)
        {
            backing = TooCloseRange_RangedBattleRange(ATKRadius, MySelf, Target);
            if (backing == true)
            {
                m_NowState = BossState.Back;
                if (m_NowState == BossState.Back)
                {
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                    MubAnimator.SetBool("Attack04", false);
                }
            }
        }
    }
    public void Ultimate()
    {
        if (UCount <= 0)
        {
            MubAnimator.SetTrigger("Ulti");
        }
    }

    //甦醒狀態
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //在遠程攻擊範圍內 
    public bool IsInRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //在攻擊範圍內 
    public bool IsInRange_BattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //範圍判定_後退
    public bool TooCloseRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude < Radius;
    }

    //範圍判定_追蹤
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > Radius;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color = InRangeATKrange ? Color.gray : Color.blue;
        Gizmos.DrawWireSphere(transform.position,RangedRadius);
    }
    private void Animation_Attack()
    {
        MubAnimator.speed = 2f;
    }
    
    private void Animation_RangedAttack()
    {
        MubAnimator.speed = 0.2f;
    }
    private void AnimationSpeed_R_AttackEnd()
    {
        MubAnimator.speed = 1f;
        TraceRadius = RangedRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
    }
    private void AnimationSpeed_AttackEnd03()
    {
        MubAnimator.speed = 1f;
    }
    private void AnimationSpeed_AttackEnd04()
    {
        MubAnimator.speed = 1f;
    }
    public void R_ZoneOpen()
    {
        TraceRadius = RangedRadius * .1f;
        RCount = 10;
        StartCoroutine(RangerCooldown());
    }
    public void ZoneOpen()
    {
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    public void ZoneOpen01()
    {
        Count = 4;      
    }

    public void ZoneOpen02()
    {
        Count = 4;
    }
    private void StageTwoEventSwitch()
    {
        MubAnimator.ResetTrigger("T2");
    }
    private void TeleportEvent()
    {
        float TeleoortChoose01 = (TeleportPoint01.transform.position - transform.position).magnitude;
        float TeleoortChoose02 = (TeleportPoint02.transform.position - transform.position).magnitude;
        if (TeleoortChoose01> TeleoortChoose02)
        {
            MubAnimator.ResetTrigger("Ulti");
            transform.position = TeleportPoint01.transform.position;
            MubAnimator.SetBool("ChargeUp", true);
        }
        else
        {
            MubAnimator.ResetTrigger("Ulti");
            transform.position = TeleportPoint02.transform.position;
            MubAnimator.SetBool("ChargeUp", true);
        }        
    }
    private void Animation_UltimateCoolDown()
    {
        MubAnimator.SetBool("ChargeUp", false);
        UCount = 80;
        StartCoroutine(UltimateCooldown());
    }
    IEnumerator SummonCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;         
        }
    }
    IEnumerator RangerCooldown()
    {
        while (RCount > 0)
        {
            yield return new WaitForSeconds(1);
            RCount--;      
        }
    }
    IEnumerator UltimateCooldown()
    {
        while (UCount > 0)
        {
            yield return new WaitForSeconds(1);
            UCount--;
        }
    }
}
