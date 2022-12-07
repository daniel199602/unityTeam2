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
    int TraceRadius;//¤§«á­n¶ë§ðÀ»¶ZÂ÷¥Î
    int ATKRadius;
    public float Speed;
    float radius = 10f; 
    Vector3 Get3;
    float Get;   
    float mSpeed;
    void Start()
    {
        m_NowState = currentState.Idle;
        InRange = IsInRange_MeleeBattleRange(DisRange, MySelf, Target);
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<PlayerState>();
        hpTemporary = State.Hp;
        capsule = GetComponent<CapsuleCollider>();
        TraceRadius = 15 * 2;
        ATKRadius = 15;
        

    }

    // Update is called once per frame
    void Update()
    {
        if (State.Hp <= 0)//¦º¤`¡÷µLª¬ºA
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

            TraceStatus();

            AttackStatus();   
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
        tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
        if (tracing == true)
        {
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
            m_NowState = currentState.Attack;
            MubAnimator.SetBool("Trace", false);
            Attack();
            Debug.Log("NowInA");
        }
        if (InATKrange == false)
        {

        }
    }
    public bool IsInRange_MeleeBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    /*½d³ò§P©w_°lÂÜ*/
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

        if (Get > TraceRadius)
        {
            int i = 0;
            if (i != 0)
            {
                return;
            }
            else if (i == 0)
            {
                mSpeed *= 2;
                MubAnimator.SetFloat("Blend", 1);
                i++;
            }
        }
        else
        {
            mSpeed *= 1;
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
        MubAnimator.SetBool("Roar",true);
        mSpeed = Speed * 0f;
        MubAnimator.SetBool("Roar", false);
    }
    public void Attack()
    {
        if (m_NowState == currentState.Attack)
        {
            MubAnimator.SetBool("Attack", true);
            mSpeed = Speed * 0f;
        }
        else if (m_NowState == currentState.Trace)
        {
            MubAnimator.SetBool("Attack", false);
        }
       
    }
}
