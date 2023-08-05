﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MagicCasterState
{
    Idle, Trace, Back, Attack, GetHit, Dead, Summon,
}
public class MagicCasterFSM : MonoBehaviour
{
    public GameObject Bug;

    public LayerMask layerMask_BugSensor;

    public GameObject LightRays;
    public GameObject RoarLight;
    public Transform LaunchPort;

    private MagicCasterState m_NowState;

    ParticleSystem ps;
    ParticleSystem pc;

    private GameObject Target;
    private GameObject MySelf;

    MubHpData State;
    Animator MubAnimator;
    int hpTemporary;
    CharacterController capsule;

    bool backing;
    bool tracing;
    bool InATKrange;

    bool AwakeBool = false;
    bool Awaken;
    bool TA = false;

    float ATKRadius;
    float Close_ATKRadius;
    float AwakeRadius;
    [SerializeField] float RotateSpeed;

    Vector3 GetTargetNormalize;
    int THp;
    int Count;
    int CDs;
    ItemOnMob ThisItemOnMob_State;

    bool RoarBool = false;

    int FrameCount_Roar;

    // Start is called before the first frame update
    private void Awake()
    {
        ThisItemOnMob_State = GetComponent<ItemOnMob>();
    }
    void Start()
    {
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己

        m_NowState = MagicCasterState.Idle;
        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();


        hpTemporary = State.Hp;
        FrameCount_Roar = 200;
        ps = LightRays.GetComponent<ParticleSystem>();
        pc = RoarLight.GetComponent<ParticleSystem>();

        m_NowState = MagicCasterState.Idle;

        ATKRadius = ThisItemOnMob_State.mobRadius;//Weapon覆蓋

        Close_ATKRadius = ATKRadius * 0.8f;
        AwakeRadius = ATKRadius * 2.5f;
        Count = 0;
        RotateSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        THp = Target.GetComponent<PlayerHpData>().Hp;
        AwakeSensor();
        Quaternion c = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        transform.rotation = c;
        if (AwakeBool == true)
        {
            if (FrameCount_Roar > 0)
            {
                FrameCount_Roar--;
            }
            if (RoarBool == false)
            {
                pc.Play();
                Roar();
                RoarBool = true;
            }
            if (State.Hp <= 0)//死亡→無狀態
            {
                m_NowState = MagicCasterState.Dead;
                DeadStatus();
                return;
            }
            else if (THp <= 1)
            {
                MubAnimator.SetTrigger("IDLE");
                return;
            }
            else if (State.Hp > 0)
            {
                if (State.Hp != hpTemporary)
                {
                    if (hpTemporary - State.Hp < 50)
                    {
                        hpTemporary = State.Hp;
                    }
                    else if (hpTemporary - State.Hp >= 50)
                    {
                        hpTemporary = State.Hp;
                        MubAnimator.SetBool("GetHit", true);
                    }
                }
                else if (FrameCount_Roar <= 0)
                {
                    MubAnimator.SetBool("GetHit", false);

                    MubAnimator.SetBool("Warn", false);

                    GetTargetNormalize = (Target.transform.position - transform.position).normalized;

                    Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

                    transform.rotation = Quaternion.Slerp(transform.rotation, Look, RotateSpeed * Time.deltaTime);

                    BugSummon();

                    TraceStatus();

                    AttackStatus();
                }
            }

        }
    }
    public void AwakeSensor()
    {
        {
            if (AwakeBool == false)
            {
                Awaken = IsInRange_AwakeRange(AwakeRadius, MySelf, Target);
                if (Awaken == true)
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
            MubAnimator.SetBool("KnockBack", false);
            MubAnimator.speed = 1f;
            ps.Stop();
            pc.Stop();
            capsule.radius = 0f;
        }
    }
    //追擊狀態
    public void TraceStatus()
    {
        if (m_NowState != MagicCasterState.Summon)
        {
            tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = MagicCasterState.Trace;

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
    //攻擊狀態
    public void AttackStatus()
    {
        InATKrange = IsInRange_RangedBattleRange(ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            m_NowState = MagicCasterState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            Attack();
        }
    }
    //甦醒狀態
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //在遠程攻擊範圍內 
    public bool IsInRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
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
    public void Roar()
    {
        MubAnimator.SetBool("Warn", true);
    }
    public void Attack()
    {
        if (m_NowState == MagicCasterState.Attack)
        {
            
            if (CDs == 0)
            {
                Vector3 mePos = this.transform.forward;
                Ray r = new Ray(this.transform.position, mePos);
                RaycastHit rh;
                if (Physics.Raycast(r, out rh, 15f, layerMask_BugSensor))
                {
                    return;
                }
                else
                {
                    MubAnimator.SetBool("Attack", true);
                    MubAnimator.SetBool("Back", false);
                    MubAnimator.SetBool("Wondering01", false);
                    MubAnimator.SetBool("Wondering02", false);
                    TA = false;
                }                 
            }
            else if (CDs != 0)
            {
                backing = TooCloseRange_RangedBattleRange(Close_ATKRadius, MySelf, Target);
                if (backing == true)
                {
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack", false);
                }
                else
                {
                    if (TA == false)
                    {
                        int RA = Random.Range(0, 2);
                        if (RA == 0)
                        {
                            MubAnimator.SetBool("Wondering01", true);
                            TA = true;
                        }
                        else if (RA == 1)
                        {
                            MubAnimator.SetBool("Wondering02", true);
                            TA = true;
                        }
                    }
                }
            }
        }
    }
    public void BugSummon()
    {
        if (Count <= 0)
        {
            m_NowState = MagicCasterState.Summon;
            if (m_NowState == MagicCasterState.Summon)
            {
                MubAnimator.SetBool("GrenerateBug", true);
                Debug.Log("產生蟲蟲");
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
    }
    private void AnimationSpeed_Attack()
    {
        MubAnimator.speed = 0.2f;

        ps.Play();
        RotateSpeed = RotateSpeed * .1f;
    }
    private void AnimationSpeed_AttackEnd()
    {
        MubAnimator.speed = 1f;
        Close_ATKRadius = ATKRadius * .8f;
        CDs = 4;
        RotateSpeed = RotateSpeed * 10f;
        StartCoroutine(AttackCooldown());

        ps.Stop();
    }
    public void ZoneOpen()
    {
        Close_ATKRadius = ATKRadius * .1f;

    }

    public void Recover()
    {
        MubAnimator.SetBool("KnockBack", false);
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
        Vector3 mePos = this.transform.forward;
        Ray r = new Ray(this.transform.position, mePos);
        RaycastHit rh;
        if (Physics.Raycast(r, out rh, 15f, layerMask_BugSensor))
        {
            Instantiate(Bug, (MySelf.transform.position + -(MySelf.transform.forward * 10)), MySelf.transform.rotation);
        }
        else
        {
            Instantiate(Bug, (MySelf.transform.position + MySelf.transform.forward * 10), MySelf.transform.rotation);
        }
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
