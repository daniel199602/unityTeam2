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
    }

    // Update is called once per frame
    void Update()
    {
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
        else
        {
            MubAnimator.SetBool("GetHit", false);

            Get3 = (Target.transform.position - transform.position).normalized;

            transform.forward = Get3;

            TraceStatus();

            AttackStatus();
            
            
            
                LeaveAttackStatus();
            
        }           
        Debug.Log(m_NowState);
    }
    public void DeadStatus()
    {
        if (m_NowState== currentState.Dead)
        {
            MubAnimator.SetTrigger("isTriggerDie");
            capsule.radius = 0f;
        }
    }
    public void TraceStatus()
    {
        MubAnimator.speed = 1;
        tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
        if (tracing == true)
        {
            ATT = false;
            m_NowState = currentState.Trace;
            Debug.Log("NowInT");
            if (m_NowState==currentState.Trace)
            {
                Move();
                MubAnimator.SetBool("Trace", true);
                MubAnimator.SetBool("Attack", false);
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
        } 
    }
    public bool IsInRange_MeleeBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    public bool IsOutRange_MeleeBattleRange(float RadiusMax,float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude < RadiusMax&& direction.magnitude>= RadiusMin;
    }
    /*範圍判定_追蹤*/
    public bool IsInRange_TraceRange(float RadiusMax, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > RadiusMax;
    }
    public void Move()
    {
        Get3 = (Target.transform.position - transform.position).normalized;

        Get = (Target.transform.position - transform.position).magnitude;

        transform.forward = Get3;

        Debug.Log("當前速度"+mSpeed);

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
    //public void Roar()
    //{
    //    MubAnimator.SetBool("Roar",true);
    //    mSpeed = Speed * 0f;
    //    MubAnimator.SetBool("Roar", false);
    //}
    public void Attack()
    {       
        if (m_NowState == currentState.Attack)
        {            
            MubAnimator.SetBool("Attack", true);
            MubAnimator.speed = 2;
        }
        else if (m_NowState == currentState.Trace)
        {
            MubAnimator.SetBool("Attack", false);
            MubAnimator.speed = 1;
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
        Gizmos.DrawWireSphere(transform.position,ATKRadius);


         Gizmos.color = OutATKrange ? Color.blue : Color.black;
         Gizmos.DrawWireSphere(transform.position, LeaveATKRadius);
        


    }
}
