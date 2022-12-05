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
    int TraceRadius;//之後要塞攻擊距離用
    int ATKRadius;
    void Start()
    {
        m_NowState = currentState.Idle;
        InRange = IsInRange_MeleeBattleRange(DisRange, MySelf, Target);
        //MubAnimator = GetComponent<Animator>();
        hpTemporary = State.Hp;
        //capsule = GetComponent<CapsuleCollider>();
        TraceRadius = 15 * 2;
        ATKRadius = 15;
    }
   
    // Update is called once per frame
    void Update()
    {
        if (State.Hp <= 0)//死亡→無狀態
        {
            MubAnimator.SetTrigger("isTriggerDie");
            capsule.radius = 0f;
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
            if (m_NowState == currentState.Idle)
            {
                m_NowState = currentState.Trace;
                tracing = IsInRange_TraceRange(TraceRadius, MySelf, Target);
                if (m_NowState == currentState.Trace&& tracing ==true)
                {
                    MySelf.transform.position -= Target.transform.position;
                    InATKrange = IsInRange_MeleeBattleRange(ATKRadius,MySelf,Target);
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
            }           
        }
    }
    public bool IsInRange_MeleeBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    /*範圍判定_追蹤*/
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > Radius;
    }
}
