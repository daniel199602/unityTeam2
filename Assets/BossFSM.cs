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

    MubHpData State;
    Animator MubAnimator;
    int hpTemporary;
    int hpTemporaryMax;
    CharacterController capsule;

    bool backing;
    bool tracing;
    bool InRangeATKrange;
    bool InATKrange;
    bool InATKrange_Close;
    bool OutATKrange;
    bool GetHit;
    bool StageTwo=false;
    bool IK=false;

    float TraceRadius;
    float RunRadius;
    float ATKRadius;
    float RangedRadius;
    float LeaveATKRadius;
    float Close_ATKRadius;

    int RCount;
    int Count;
    int UCount;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
   
    int RandomChoose;
    int i = 0;

    // Start is called before the first frame update
    void Start()
    {
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己
        Debug.LogWarning(Target);
        m_NowState = BossState.Idle;
        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();
        hpTemporary = State.Hp;
        hpTemporaryMax = State.Hp;

        m_NowState = BossState.Idle;

        ATKRadius = 40;//Weapon覆蓋

        Close_ATKRadius = ATKRadius * 0.5f;

        RangedRadius = ATKRadius * 1.8f;

        LeaveATKRadius = ATKRadius * 1.3f;

        TraceRadius = ATKRadius * 2f;

        Count = 0;

        GetHit = false;

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
        else if ((float)hpTemporary/ (float)hpTemporaryMax ==0.5f&& IK == false)
        {
            StageTwo = true;
            IK = true;
        }
        else if (State.Hp != hpTemporary)
        {
            hpTemporary = State.Hp;
            if (GetHit==false)
            {
                i = UnityEngine.Random.Range(1, 2);
                if (i== 1)
                {
                    MubAnimator.SetBool("GetHit01", true);
                    GetHit = true;
                    return;
                }
                if (i == 2)
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

            LeaveAttackStatus();

            TooCloseAttackStatus_York();

            BackStatus();

            Ultimate();
        }
        Debug.Log(m_NowState);
    }
    public void StageTwoCost()
    {
        if (StageTwo == true)
        {
            MubAnimator.SetBool("T2", true);
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
        tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
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
        else
        {
            MubAnimator.SetBool("Attack01", false);
            MubAnimator.SetBool("Attack02", false);
            MubAnimator.SetBool("Attack03", false);
            MubAnimator.SetBool("Attack04", false);
            MubAnimator.SetBool("R_Attack", false);
        }
    }
    public void RangedAttackStatus()
    {
        InRangeATKrange = IsInRange_RangedBattleRange(RangedRadius, LeaveATKRadius, MySelf, Target);
        if (InRangeATKrange == true)
        {          
            m_NowState = BossState.RangeAttack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            RangeAttack();
            Debug.Log("NowInR");
        }
    }
    //攻擊狀態
    public void AttackStatus()
    {
        InATKrange = IsInRange_BattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            m_NowState = BossState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            Attack();
            Debug.Log("NowInA");
        }
    }

    //在攻擊蛋黃區(外圈)狀態
    public void LeaveAttackStatus()
    {
        OutATKrange = IsOutRange_RangedBattleRange(LeaveATKRadius, ATKRadius, MySelf, Target);
        if (OutATKrange == true)
        {
            m_NowState = BossState.Idle;
            Idle();
            Debug.LogError("L is active");
        }
    }
    //在攻擊蛋黃區(內圈)狀態
    public void TooCloseAttackStatus_York()
    {
        InATKrange_Close = CloseRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
        if (InATKrange_Close == true)
        {
            m_NowState = BossState.Attack;
            Debug.LogError("LI is active");
        }
    }
    //後退狀態
    public void BackStatus()
    {
        backing = TooCloseRange_RangedBattleRange(Close_ATKRadius, MySelf, Target);
        if (backing == true)
        {
            m_NowState = BossState.Back;
            Debug.Log("NowInB");
            if (m_NowState == BossState.Back)
            {
                MubAnimator.SetBool("Back", true);
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
                MubAnimator.SetBool("Attack04", false);
                MubAnimator.SetBool("R_Attack", false);
            }
        }
        else
        {
            MubAnimator.SetBool("Attack01", false);
            MubAnimator.SetBool("Attack02", false);
            MubAnimator.SetBool("Attack03", false);
            MubAnimator.SetBool("Attack04", false);
            MubAnimator.SetBool("R_Attack", false);
        }
    }
    //在遠程攻擊範圍內 
    public bool IsInRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //在攻擊範圍內 
    public bool IsInRange_BattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //在遠程攻擊蛋黃區範圍(外圈) 
    public bool IsOutRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //在遠程攻擊蛋黃區範圍(內圈) 
    public bool CloseRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude >= RadiusMin;
    }
    //範圍判定_後退
    public bool TooCloseRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude < Radius;
    }

    /*範圍判定_追蹤*/
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > Radius;
    }    
    public void RangeAttack()
    {
        if (m_NowState == BossState.Attack && RCount == 0)
        {
            MubAnimator.SetBool("R_Attack", true);
        }
        else if (Count != 0)
        {
            MubAnimator.SetBool("R_Attack", false);
        }
    }
    public void Attack()
    {
        if (m_NowState == BossState.Attack && Count == 0)
        {
            if (i == 0)
            {
                RandomChoose = UnityEngine.Random.Range(1, 4);
                i++;
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
        else if (Count != 0)
        {
            MubAnimator.SetBool("Attack01", false);
            MubAnimator.SetBool("Attack02", false);
            MubAnimator.SetBool("Attack03", false);
            MubAnimator.SetBool("Attack04", false);
        }
    }
    public void Ultimate()
    {
        if (UCount <= 0)
        {
            MubAnimator.SetBool("Ulti", true);
        }
    }
    public void Idle()
    {
        MubAnimator.SetBool("Trace", false);
    }
    
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color = OutATKrange ? Color.blue : Color.black;
        Gizmos.DrawWireSphere(transform.position, LeaveATKRadius);

        Gizmos.color = InATKrange_Close ? Color.green : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Close_ATKRadius);

        Gizmos.color = InRangeATKrange ? Color.gray : Color.blue;
        Gizmos.DrawWireSphere(transform.position, Close_ATKRadius);
    }
    private void Animation_Attack()
    {
        MubAnimator.speed = 2f;
    }
    private void Animation_Ultimate()
    {
        UCount = 80;
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        UCount = 80;
        StartCoroutine(UltimateCooldown());
    }
    private void Animation_RangedAttack()
    {
        MubAnimator.speed = 0.2f;
    }
    private void AnimationSpeed_R_AttackEnd()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = RangedRadius * 1.3f;
        TraceRadius = RangedRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd03()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd04()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    public void R_ZoneOpen()
    {
        LeaveATKRadius = RangedRadius * 6;
        TraceRadius = RangedRadius * .1f;
        RCount = 10;
        StartCoroutine(RangerCooldown());
    }
    public void ZoneOpen()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    public void ZoneOpen01()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 4;      
    }

    public void ZoneOpen02()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 4;
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
