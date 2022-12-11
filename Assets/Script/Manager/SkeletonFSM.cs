using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SkeletonState
{
    Idle, Trace, Attack, GetHit, Dead,
}
public class SkeletonFSM : MonoBehaviour
{
    private SkeletonState m_NowState;

    private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    PlayerState State;
    Animator MubAnimator;
    int hpTemporary;
    CapsuleCollider capsule;
    bool LeaveAttackRangeBool;
    bool RoarBool = false;
    bool tracing;
    bool InATKrange;
    bool OutATKrange;
    bool AwakeBool = false;
    bool Awake;

    float TraceRadius;
    float ATKRadius;
    float LeaveATKRadius;
    float AwakeRadius;

    public float Speed = 15f;    
    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
    float MoveSpeed;

    int CDs;

    int FrameCount_Roar;
    void Start()
    {
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己

        m_NowState = SkeletonState.Idle;
        capsule = GetComponent<CapsuleCollider>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<PlayerState>();
        hpTemporary = State.Hp;
        FrameCount_Roar = 160;
        LeaveAttackRangeBool = false;
        
        ATKRadius = 30;//Weapon覆蓋

        TraceRadius = ATKRadius * 2;

        AwakeRadius = ATKRadius * 1.5f;

        LeaveATKRadius = ATKRadius * 1.5f;
        
    }
    // Update is called once per frame
    void Update()
    {
        AwakeSensor();
        if (AwakeBool ==true)
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
            if (State.Hp <= 0)//死亡→無狀態
            {
                m_NowState = SkeletonState.Dead;
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

                Debug.LogError("f:" + FrameCount_Roar);

                MubAnimator.SetBool("Roar", false);

                GetTargetNormalize = (Target.transform.position - transform.position).normalized;

                Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, Speed * Time.deltaTime);

                TraceStatus();

                AttackStatus();

                LeaveAttackStatus();
            }
        }        
        Debug.Log(m_NowState);
    }
    public void AwakeSensor()
    {
        {
            if (AwakeBool==false)
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
        if (m_NowState == SkeletonState.Dead)
        {
            MubAnimator.SetTrigger("isTriggerDie");
            capsule.radius = 0f;
        }
    }
    public void TraceStatus()
    {
        if (LeaveAttackRangeBool == false)
        {
            MubAnimator.speed = 1;

            tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = SkeletonState.Trace;
                Debug.Log("NowInT");
                if (m_NowState == SkeletonState.Trace)
                {
                    Move();
                    MubAnimator.SetBool("Trace", true);
                    MubAnimator.SetBool("Attack", false);
                }
            }
        }
    }
    public void AttackStatus()
    {
        InATKrange = IsInRange_MeleeBattleRange(ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            LeaveAttackRangeBool = true;
            MoveSpeed = 0f;
            m_NowState = SkeletonState.Attack;
            MubAnimator.SetBool("Trace", false);
            Attack();
            Debug.Log("NowInA");
        }
    }
    public void LeaveAttackStatus()
    {
        if (LeaveAttackRangeBool == true)
        {
            OutATKrange = IsOutRange_MeleeBattleRange(LeaveATKRadius, ATKRadius, MySelf, Target);
            if (OutATKrange == true)
            {
                m_NowState = SkeletonState.Idle;
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
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    public bool IsInRange_MeleeBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    public bool IsOutRange_MeleeBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    /*範圍判定_追蹤*/
    public bool IsInRange_TraceRange(float RadiusMax, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > RadiusMax;
    }
    public void Move()
    {

        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;

        Debug.Log("當前速度" + MoveSpeed);

        if (GetTargetMegnitude > TraceRadius)
        {
            int i = 0;
            if (i != 0)
            {
                return;
            }
            else if (i == 0)
            {
                MoveSpeed *= 1.5f;
                MubAnimator.SetFloat("Blend", 1);
                i++;
            }
        }
        else
        {
            MoveSpeed *= 1f;
            MubAnimator.SetFloat("Blend", 0);
        }

        Vector3 m = Vector3.MoveTowards(transform.position, Target.transform.position, MoveSpeed);

        transform.position = m;

        Debug.Log(MoveSpeed);

        if (GetTargetMegnitude == ATKRadius)
        {
            MoveSpeed = Speed * 0f;
        }
        else
        {
            MoveSpeed = Speed * .01f;
        }
    }
    public void Roar()
    {

        MubAnimator.SetBool("Roar", true);
        MoveSpeed = Speed * 0f;

    }
    public void Attack()
    {
        if (m_NowState == SkeletonState.Attack&&CDs==0)
        {
            MubAnimator.SetBool("Attack", true);
        }
        else if (CDs != 0)
        {
            MubAnimator.SetBool("Attack", false);
        }
    }
    public void Idle()
    {
        MubAnimator.SetBool("Attack", false);
        MubAnimator.SetBool("Trace", false);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);


        Gizmos.color = OutATKrange ? Color.blue : Color.black;
        Gizmos.DrawWireSphere(transform.position, LeaveATKRadius);
    }
    private void AnimationSpeed_PrepareAttack()
    {
        MubAnimator.speed = 2f;
    }
    private void AnimationSpeed_Attack()
    {
        MubAnimator.speed = 4f;
    }
    private void AnimationSpeed_AttackEnd()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.5f;
        CDs = 3;
        StartCoroutine(AttackCooldown());
    }
    public void ZoneOpen()
    {
        LeaveATKRadius = ATKRadius * 6;
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
