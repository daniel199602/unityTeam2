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
    int TraceRadius;//����n������Z����
    int ATKRadius;

    float direction;  
    public float Speed;
    float radius = 10f; 
    Vector3 Get3;
    float Get;   
    public float mSpeed;
    void Start()
    {
        m_NowState = currentState.Idle;
        InRange = IsInRange_MeleeBattleRange(DisRange, MySelf, Target);
        //MubAnimator = GetComponent<Animator>();
        hpTemporary = State.Hp;
        //capsule = GetComponent<CapsuleCollider>();
        TraceRadius = 15 * 2;
        ATKRadius = 15;

        direction = Vector3.Distance(transform.position, Target.transform.position);
        Debug.Log(direction);        
        Speed = .01f;
    }

    // Update is called once per frame
    void Update()
    {
        if (State.Hp <= 0)//���`���L���A
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
           
            tracing = IsInRange_TraceRange(TraceRadius, MySelf, Target);
            if (m_NowState == currentState.Trace && tracing == true)
            {
                MySelf.transform.position -= Target.transform.position;
                InATKrange = IsInRange_MeleeBattleRange(ATKRadius, MySelf, Target);
                MubAnimator.SetBool("Trace", true);
                if (InATKrange == true)
                {
                    m_NowState = currentState.Attack;
                    MubAnimator.SetBool("Trace", false);
                    if (m_NowState == currentState.Attack)
                    {
                        MubAnimator.SetBool("Attack", true);
                    }
                    else
                    {
                        MubAnimator.SetBool("Attack", false);
                    }
                }
            }


            Get3 = (Target.transform.position - transform.position).normalized;
            Get = (Target.transform.position - transform.position).magnitude;            
            transform.forward = Get3;            
            transform.position = Vector3.Lerp(transform.position, Target.transform.position, Speed);
            if (Get <= radius)
            {
                Speed = 0f;
            }
            else
            {
                Speed = .01f;
            }
        }
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
        tracing = IsInRange_TraceRange(TraceRadius, MySelf, Target);
        if (tracing == true)
        {
            m_NowState = currentState.Trace;
            if (m_NowState==currentState.Trace)
            {
                MySelf.transform.position -= Target.transform.position;            
                MubAnimator.SetBool("Trace", true);            
            }           
        }
    }
    public bool IsInRange_MeleeBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    /*�d��P�w_�l��*/
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > Radius;
    }
    public void Move()
    {
        

        
    }
}
