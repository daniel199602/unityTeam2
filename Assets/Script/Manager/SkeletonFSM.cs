using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum currentState
{
    Idle, Trace, Attack, GetHit, Dead,
}
public class SkeletonFSM : MonoBehaviour
{
    private currentState m_NowState;
    public GameObject Target;
    public GameObject MySelf;
    float DisRange;
    bool InRange;
    float AttackRangeMiddle;
    PlayerState State;
    Animator MubAnimator;
    int hpTemporary;
    CapsuleCollider capsule;
    bool tracing;
    bool InATKrange;
    bool OutATKrange;
    int TraceRadius;//之後要塞攻擊距離用
    int ATKRadius;
    int LeaveATKRadius;
    public float Speed;
    float radius = 10f;
    Vector3 Get3;
    float Get;
    float mSpeed;
    bool ATT;
    bool RRRR = false;
    int F ;
    void Start()
    {
        m_NowState = currentState.Idle;
        InRange = IsInRange_MeleeBattleRange(DisRange, MySelf, Target);
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<PlayerState>();
        hpTemporary = State.Hp;
        capsule = GetComponent<CapsuleCollider>();
        TraceRadius = ATKRadius * 3;
        ATKRadius = 30;
        LeaveATKRadius = ATKRadius * 2;
        ATT = false;
        F = 160;
    }
    // Update is called once per frame
    void Update()
    {
        F--;
        
        if (RRRR==false)
        {
            Roar();
            RRRR = true;            
        }
        if (State.Hp <= 0)//死亡→無狀態
        {
            m_NowState = currentState.Dead;
            DeadStatus();
            return;
        }
        else if (State.Hp != hpTemporary)
        {
            hpTemporary = State.Hp;
            MubAnimator.SetBool("GetHit", true);
        }        
        else if (F<=0)
        {
            MubAnimator.SetBool("GetHit", false);

            Debug.LogError("f:" + F);

            MubAnimator.SetBool("Roar", false);

            Get3 = (Target.transform.position - transform.position).normalized;

            Quaternion Look = Quaternion.LookRotation(Get3);

            transform.rotation = Quaternion.Slerp(transform.rotation, Look, Speed * Time.deltaTime);

            TraceStatus();

            AttackStatus();

            LeaveAttackStatus();
        }
        Debug.Log(m_NowState);
    }
    public void DeadStatus()
    {
        if (m_NowState == currentState.Dead)
        {
            MubAnimator.SetTrigger("isTriggerDie");
            capsule.radius = 0f;
        }
    }
    public void TraceStatus()
    {
        if (ATT == false)
        {
            MubAnimator.speed = 1;

            tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = currentState.Trace;
                Debug.Log("NowInT");
                if (m_NowState == currentState.Trace)
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
            ATT = true;
            mSpeed = 0f;
            m_NowState = currentState.Attack;
            MubAnimator.SetBool("Trace", false);
            Attack();
            Debug.Log("NowInA");
        }
    }
    public void LeaveAttackStatus()
    {
        if (ATT == true)
        {
            OutATKrange = IsOutRange_MeleeBattleRange(LeaveATKRadius, ATKRadius, MySelf, Target);
            if (OutATKrange == true)
            {
                m_NowState = currentState.Idle;
                Idle();
                Debug.LogError("L is active");
            }
            Get = (Target.transform.position - transform.position).magnitude;
            if (Get > LeaveATKRadius)
            {
                ATT = false;
            }
        }
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
       

        Get = (Target.transform.position - transform.position).magnitude;

        

        Debug.Log("當前速度" + mSpeed);

        if (Get > TraceRadius)
        {
            int i = 0;
            if (i != 0)
            {
                return;
            }
            else if (i == 0)
            {
                mSpeed *= 1.5f;
                MubAnimator.SetFloat("Blend", 1);
                i++;
            }
        }
        else
        {
            mSpeed *= 1f;
            MubAnimator.SetFloat("Blend", 0);
        }

        Vector3 m = Vector3.MoveTowards(transform.position, Target.transform.position, mSpeed);

        transform.position = m;

        Debug.Log(mSpeed);

        if (Get == ATKRadius)
        {
            mSpeed = Speed * 0f;
        }
        else
        {
            mSpeed = Speed * .01f;
        }
    }
    public void Roar()
    {

        MubAnimator.SetBool("Roar", true);
        mSpeed = Speed * 0f;

    }
    public void Attack()
    {
        if (m_NowState == currentState.Attack)
        {
            MubAnimator.SetBool("Attack", true);
        }
        else if (m_NowState == currentState.Trace)
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
        LeaveATKRadius = ATKRadius * 2;
    }
    public void ZoneOpen()
    {
        LeaveATKRadius = ATKRadius * 5;
    }
}
