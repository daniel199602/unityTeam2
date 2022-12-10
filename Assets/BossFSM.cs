using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BossState
{
    Idle, Trace, Back, Attack, GetHit, Dead,
}
public class BossFSM : MonoBehaviour
{
    private BossState m_NowState;
    public GameObject Target;
    public GameObject MySelf;
    public GameObject TempPoint;
    PlayerState State;
    Animator MubAnimator;
    int hpTemporary;
    CapsuleCollider capsule;
    bool backing;
    bool tracing;
    bool InATKrange;
    bool InATKrange_Close;
    bool OutATKrange;
    float TraceRadius;
    float RunRadius;
    float ATKRadius;
    float LeaveATKRadius;
    float Close_ATKRadius;

    public float Speed;
    float MoveSpeed;
    float BackSpeed;

    float CDs;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
    int Count;
    bool AttackCBool = false;

    bool LeaveAttackRangeBool;
    bool InAttackRangeBool;
    bool RoarBool = false;
    int FrameCount_Roar;

    int RandomChoose;
    int i = 0;
    // Start is called before the first frame update
    void Start()
    {
        m_NowState = BossState.Idle;
        capsule = GetComponent<CapsuleCollider>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<PlayerState>();
        hpTemporary = State.Hp;
        FrameCount_Roar = 400;
        LeaveAttackRangeBool = false;
        InAttackRangeBool = false;

        m_NowState = BossState.Idle;

        ATKRadius = 40;//WeaponÂÐ»\

        Close_ATKRadius = ATKRadius * 0.5f;
        TraceRadius = ATKRadius * 2.5f;
        LeaveATKRadius = ATKRadius * 1.3f;
        RunRadius = ATKRadius * 2.7f;
        AttackCBool = false;
        Count = 0;
    }

    // Update is called once per frame
    void Update()
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
            m_NowState = BossState.Dead;
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

            transform.rotation = Quaternion.Slerp(transform.rotation, Look, 7f * Time.deltaTime);


            Debug.Log("CDs:" + Count);

            TraceStatus();

            AttackStatus();

            LeaveAttackStatus();

            TooCloseAttackStatus_York();

            BackStatus();

        }
        Debug.Log(m_NowState);
    }
    public void DeadStatus()
    {
        if (m_NowState == BossState.Dead)
        {
            MubAnimator.SetTrigger("Die");
            capsule.radius = 0f;
        }
    }
    //°lÀ»ª¬ºA
    public void TraceStatus()
    {
        if (LeaveAttackRangeBool == false)
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
                }
            }
            else
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
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
            MoveSpeed = 0f;
            m_NowState = BossState.Attack;
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
                m_NowState = BossState.Idle;
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
                m_NowState = BossState.Attack;
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
            if (AttackCBool == false)
            {
                backing = TooCloseRange_RangedBattleRange(Close_ATKRadius, MySelf, Target);
                AttackCBool = true;
            }
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
                }
            }
            else
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
            }
        }
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
        if (m_NowState == BossState.Attack && Count == 0)
        {
            if (i == 0)
            {
                RandomChoose = UnityEngine.Random.Range(1, 4);
                i++;
            }
            Debug.Log("ÀH¾÷¼Æ:" + RandomChoose);
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
    }
    private void Animation_Attack()
    {
        MoveSpeed = Speed * .01f;
        Vector3 m = Vector3.MoveTowards(transform.position, Target.transform.position, MoveSpeed * 5);
        transform.position = m;
        MubAnimator.speed = 2f;

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
        Instantiate(TempPoint, transform.position, Quaternion.identity, MySelf.transform);
    }

    public void ZoneOpen02()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 4;
        Instantiate(TempPoint, transform.position, Quaternion.identity, MySelf.transform);
    }
    IEnumerator SummonCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;
            if (Count == 1)
            {
                i = 0;
            }
        }
    }
}
