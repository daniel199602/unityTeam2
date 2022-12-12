using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicCasterState
{
    Idle, Trace, Back, Attack, GetHit, Dead, Summon,
}
public class MagicCasterFSM : MonoBehaviour
{
    public GameObject Bug;
    public GameObject TestCube;
    public Transform LaunchPort;

    private MagicCasterState m_NowState;

    private GameObject Target;
    private GameObject MySelf;

    PlayerState State;
    Animator MubAnimator;
    int hpTemporary;
    CapsuleCollider capsule;
    bool backing;
    bool tracing;
    bool InATKrange;
    bool InATKrange_Close;
    bool OutATKrange;
    bool AwakeBool = false;
    bool Awake;
    float TraceRadius;
    float ATKRadius;
    float LeaveATKRadius;
    float Close_ATKRadius;
    float AwakeRadius;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
    int Count;
    int CDs;
    bool LeaveAttackRangeBool;
    bool InAttackRangeBool;
    bool RoarBool = false;
    int FrameCount_Roar;
    // Start is called before the first frame update
    void Start()
    {
        Target = GameManager.Instance().PlayerStart;//§ì¥Xª±®a
        MySelf = this.transform.gameObject;//§ì¥X¦Û¤v

        m_NowState = MagicCasterState.Idle;
        capsule = GetComponent<CapsuleCollider>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<PlayerState>();
        hpTemporary = State.Hp;
        FrameCount_Roar = 250;
        LeaveAttackRangeBool = false;
        InAttackRangeBool = false;

        m_NowState = MagicCasterState.Idle;

        ATKRadius = 55;//WeaponÂÐ»\

        Close_ATKRadius = ATKRadius * 0.8f;
        TraceRadius = ATKRadius * 2;
        LeaveATKRadius = ATKRadius * 1.2f;
        AwakeRadius = ATKRadius * 1.5f;
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AwakeSensor();
        if (AwakeBool == true)
        {
            if (FrameCount_Roar > 0)
            {
                FrameCount_Roar--;
            }
            if (RoarBool == false)
            {
                Roar();
                RoarBool = true;
            }
            if (State.Hp <= 0)//¦º¤`¡÷µLª¬ºA
            {
                m_NowState = MagicCasterState.Dead;
                DeadStatus();
                return;
            }
            else if (State.Hp != hpTemporary)
            {
                hpTemporary = State.Hp;
                MubAnimator.SetBool("GetHit", true);
            }
            else if (FrameCount_Roar <= 0)
            {
                MubAnimator.SetBool("GetHit", false);

                MubAnimator.SetBool("Warn", false);

                GetTargetNormalize = (Target.transform.position - transform.position).normalized;

                Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, 14f * Time.deltaTime);

                BugSummon();

                //Debug.Log("CDs:" + Count);

                TraceStatus();

                AttackStatus();

                LeaveAttackStatus();

                TooCloseAttackStatus_York();

                BackStatus();
            }
            Debug.Log(m_NowState);
        }
       
    }
    public void AwakeSensor()
    {
        {
            if (AwakeBool == false)
            {
                Awake = IsInRange_AwakeRange(AwakeRadius, MySelf, Target);
                if (Awake == true)
                {
                    AwakeBool = true;
                }
            }
        }
    }
    public void DeadStatus()
    {
        if (m_NowState == MagicCasterState.Dead)
        {
            MubAnimator.SetTrigger("Die");
            capsule.radius = 0f;
        }
    }
    //°lÀ»ª¬ºA
    public void TraceStatus()
    {
        if (LeaveAttackRangeBool == false && m_NowState != MagicCasterState.Summon)
        {
            tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = MagicCasterState.Trace;
                Debug.Log("NowInT");
                if (m_NowState == MagicCasterState.Trace)
                {
                    MubAnimator.SetBool("Trace", true);
                    MubAnimator.SetBool("Attack", false);
                }
            }
            else
            {
                MubAnimator.SetBool("Attack", false);
            }
        }
    }
    //§ðÀ»ª¬ºA
    public void AttackStatus()
    {
        InATKrange = IsInRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            LeaveAttackRangeBool = true;
            InAttackRangeBool = true;
            m_NowState = MagicCasterState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            Attack();
            Debug.Log("NowInA");
        }
    }
    //¦b§ðÀ»³J¶À°Ï(¥~°é)ª¬ºA
    public void LeaveAttackStatus()
    {
        if (LeaveAttackRangeBool == true)
        {
            OutATKrange = IsOutRange_RangedBattleRange(LeaveATKRadius, ATKRadius, MySelf, Target);
            if (OutATKrange == true)
            {
                m_NowState = MagicCasterState.Idle;
                Idle();
                Debug.LogError("L is active");
            }
            GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
            if (GetTargetMegnitude > LeaveATKRadius)
            {
                LeaveAttackRangeBool = false;
            }
        }
    }
    //¦b§ðÀ»³J¶À°Ï(¤º°é)ª¬ºA
    public void TooCloseAttackStatus_York()
    {
        if (InAttackRangeBool == true)
        {
            InATKrange_Close = CloseRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
            if (InATKrange_Close == true)
            {
                m_NowState = MagicCasterState.Attack;                
                Debug.LogError("LI is active");
            }
            GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
            if (GetTargetMegnitude < Close_ATKRadius)
            {
                InAttackRangeBool = false;
            }
        }
    }
    //«á°hª¬ºA
    public void BackStatus()
    {
        if (InAttackRangeBool == false)
        {
            backing = TooCloseRange_RangedBattleRange(Close_ATKRadius, MySelf, Target);
            if (backing == true)
            {
                m_NowState = MagicCasterState.Back;
                Debug.Log("NowInB");
                if (m_NowState == MagicCasterState.Back)
                {
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack", false);
                }
            }
            else
            {
                MubAnimator.SetBool("Attack", false);
            }
        }
    }
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //¦b»·µ{§ðÀ»½d³ò¤º 
    public bool IsInRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //¦b»·µ{§ðÀ»³J¶À°Ï½d³ò(¥~°é) 
    public bool IsOutRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //¦b»·µ{§ðÀ»³J¶À°Ï½d³ò(¤º°é) 
    public bool CloseRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude >= RadiusMin;
    }
    //½d³ò§P©w_«á°h
    public bool TooCloseRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude < Radius;
    }

    /*½d³ò§P©w_°lÂÜ*/
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > Radius;
    }
    public void Roar()
    {
        MubAnimator.SetBool("Warn", true);
    }
    public void Attack()
    {
        if (m_NowState == MagicCasterState.Attack&&CDs==0)
        {
            MubAnimator.SetBool("Attack", true);
        }
        else if (Count != 0)
        {
            MubAnimator.SetBool("Attack", false);
        }
    }
    public void Idle()
    {

        MubAnimator.SetBool("Trace", false);
    }

    public void BugSummon()
    {
        if (Count <= 0)
        {
            m_NowState = MagicCasterState.Summon;
            if (m_NowState == MagicCasterState.Summon)
            {
                MubAnimator.SetBool("GrenerateBug", true);
                Debug.Log("²£¥ÍÂÎÂÎ");
                MubAnimator.SetBool("Trace", false);
                MubAnimator.SetBool("Attack", false);
                MubAnimator.SetBool("Back", false);
            }
        }
        else if (Count != 0)
        {
            MubAnimator.SetBool("GrenerateBug", false);
            m_NowState = MagicCasterState.Idle;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color = OutATKrange ? Color.blue : Color.black;
        Gizmos.DrawWireSphere(transform.position, LeaveATKRadius);

        Gizmos.color = InATKrange_Close ? Color.green : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Close_ATKRadius);
    }
    private void AnimationSpeed_Attack()
    {
        MubAnimator.speed = 0.2f;        
        Instantiate(TestCube,LaunchPort.position,MySelf.transform.rotation);
    }
    private void AnimationSpeed_AttackEnd()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.2f;
        Close_ATKRadius = ATKRadius * .8f;
        CDs = 6;
        StartCoroutine(AttackCooldown());
    }
    public void ZoneOpen()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;

    }
    IEnumerator SummonCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;
        }
    }
    private void Summon()
    {
        Instantiate(Bug,(MySelf.transform.position+ MySelf.transform.forward*10), MySelf.transform.rotation);
        Count = 20;
        StartCoroutine(SummonCooldown());
    }
    IEnumerator AttackCooldown()
    {
        while (CDs > 0)
        {
            yield return new WaitForSeconds(1);
            CDs--;
        }
    }
}
