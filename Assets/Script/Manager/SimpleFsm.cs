using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class SimpleFsm : MonoBehaviour
{
    public enum currentState
    {
        Idle, Seek01, Trace01, Attack01, CastingMagic01,
        Getpower, Seek02, Trace02, Attack02, CastingMagic02,
        GetHit, Dead,
    }
    private currentState m_NowState;
    public GameObject WalkPoint;
    public GameObject Target;
    public GameObject MySelf;
    float DisRange;
    bool InRange;
    float AttackRangeMiddle;
    PlayerState State;
    Animator MubAnimator;
    int hpTemporary;
    CapsuleCollider capsule;
    // Start is called before the first frame update
    void Start()
    {
        m_NowState = currentState.Idle;
        InRange = IsInRange(DisRange, MySelf, Target);
        MubAnimator = GetComponent<Animator>();
        hpTemporary = State.Hp;
        capsule = GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        if (State.Hp <=0)//¦º¤`¡÷µLª¬ºA
        {
            MubAnimator.SetTrigger("isTriggerDie");
            capsule.radius = 0f;
            return;
        }
        else if (State.Hp!=hpTemporary)
        {
            hpTemporary = State.Hp;
            MubAnimator.SetBool("GetHit", true);
        }        
        else
        {
            MubAnimator.SetBool("GetHit", false);
            if (m_NowState == currentState.Idle && InRange == false)
            {
                m_NowState = currentState.Seek01;
                if (m_NowState == currentState.Seek01)
                {
                    IdleThing();
                }
            }
            if (InRange == true)
            {
                m_NowState = currentState.Attack01;
                //Attack();
            }
        }        
    }
    public bool IsInRange( float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;
               
        return direction.magnitude > Radius;
    }
    public void IdleThing()
    {
        var position = new Vector3(transform.position.x+ Random.Range(-10.0f, 10.0f), 0, transform.position.z+Random.Range(-10.0f, 10.0f));
        Instantiate(WalkPoint, position, Quaternion.identity);
        transform.forward = transform.position - WalkPoint.transform.position;
        transform.position = Vector3.MoveTowards(transform.position, position, Time.deltaTime);        
        if ((transform.position - position).magnitude <= 1)
        {
            m_NowState = currentState.Idle;
            //Stop
        }
    }
    public bool InAttackRange(float RadiusMIn,float RadiusMax, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;
        RadiusMax += AttackRangeMiddle;
        RadiusMIn -= AttackRangeMiddle;

        return direction.magnitude > RadiusMIn && direction.magnitude < RadiusMIn; 
    }
}
