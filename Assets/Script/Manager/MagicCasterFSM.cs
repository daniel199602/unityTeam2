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
    public GameObject LightRays;
    public Transform LaunchPort;

    private MagicCasterState m_NowState;

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

    float ATKRadius;
    float Close_ATKRadius;
    float AwakeRadius;
    [SerializeField]float RotateSpeed;

    Vector3 GetTargetNormalize;

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
        Target = GameManager.Instance().PlayerStart;//§ì¥Xª±®a
        MySelf = this.transform.gameObject;//§ì¥X¦Û¤v

        m_NowState = MagicCasterState.Idle;
        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();
        hpTemporary = State.Hp;
        FrameCount_Roar = 200;

        m_NowState = MagicCasterState.Idle;

        ATKRadius = ThisItemOnMob_State.mobRadius;//WeaponÂÐ»\

        Close_ATKRadius = ATKRadius * 0.8f;
        AwakeRadius = ATKRadius * 1.5f;
        Count = 0;
        RotateSpeed = 10f;
    }

    // Update is called once per frame
    void Update()
    {
        //ª±®a¦º¤`TODO()ÁÙ¨S¼g
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

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, RotateSpeed * Time.deltaTime);

                BugSummon();

                TraceStatus();

                AttackStatus();
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
            capsule.radius = 0f;
        }
    }
    //°lÀ»ª¬ºA
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
    //§ðÀ»ª¬ºA
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
    //µd¿ôª¬ºA
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //¦b»·µ{§ðÀ»½d³ò¤º 
    public bool IsInRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
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
            backing = TooCloseRange_RangedBattleRange(Close_ATKRadius, MySelf, Target);
            if (backing == true)
            {
                m_NowState = MagicCasterState.Back;
                if (m_NowState == MagicCasterState.Back)
                {
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack", false);
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

        Gizmos.color = Color.white;
        Gizmos.DrawLine(MySelf.transform.position, MySelf.transform.forward*ATKRadius);
    }
    private void AnimationSpeed_Attack()
    {
        MubAnimator.speed = 0.2f;
        ParticleSystem ps = LightRays.GetComponent<ParticleSystem>();
        ps.Play();
        //Instantiate(TestCube,LaunchPort.position,MySelf.transform.rotation,MySelf.transform);
        RotateSpeed = RotateSpeed * .1f;
    }
    private void AnimationSpeed_AttackEnd()
    {
        MubAnimator.speed = 1f;
        Close_ATKRadius = ATKRadius * .8f;
        CDs = 4;
        RotateSpeed = RotateSpeed * 10f;
        StartCoroutine(AttackCooldown());
        ParticleSystem ps = LightRays.GetComponent<ParticleSystem>();
        ps.Stop();
    }
    public void ZoneOpen()
    {
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
