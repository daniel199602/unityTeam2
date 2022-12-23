using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SpiderState
{
    Idle, Trace, Attack, Dead,
}
public class SpiderFSM : MonoBehaviour
{
    private SpiderState m_NowState;//狀態機

    public Mesh mesh;//外型
    public Material mat;//變色
    public int matIndex;//變色參數
    public float offset;//變色參數

    [SerializeField]private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    ItemOnMob ThisItemOnMob_State;

    //TestUse
    public GameObject DangerZone;
    public GameObject ExplosionZone;
    public GameObject Boon;
    public GameObject SpiderSelf;
    

    Animator MubAnimator;

    RecoilShake recoilShake;

    CharacterController Selfcapsule;

    CharacterController TargetCapsule;

    bool StartExplosive = false;

    bool RoarBool = false;

    bool StartCountDown;

    bool tracing;

    bool InATKrange;

    float ATKRadius;

    float StartExplosionRadius;

    public float Speed;

    Vector3 GetTargetNormalize;

    float GetTargetMegnitude;

    float MoveSpeed;

    int FrameCount_Roar;

    int ColorChange;

    int TargetHp;

    int CountDown;

    float ColorChangeTime;
    private void Awake()
    {
        ThisItemOnMob_State = GetComponent<ItemOnMob>();
    }
    private void Start()
    {
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己

        recoilShake = GetComponent<RecoilShake>();

        mesh = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        mat = gameObject.GetComponent<SkinnedMeshRenderer>().materials[matIndex];

        m_NowState = SpiderState.Idle;

        Selfcapsule = GetComponent<CharacterController>();

        TargetCapsule = Target.GetComponent<CharacterController>();

        MubAnimator = GetComponent<Animator>();

        FrameCount_Roar = 390;

        ATKRadius = ThisItemOnMob_State.mobRadius;//Weapon覆蓋

        StartExplosionRadius = ATKRadius * 0.5f;

        StartExplosive = false;

        StartCountDown = false;
    }
    private void Update()
    {
        TargetHp = Target.GetComponent<PlayerHpData>().Hp;
        if (FrameCount_Roar > 0)
        {
            FrameCount_Roar--;
        }
        if (RoarBool == false)
        {
            Roar();
            RoarBool = true;
        }
        if (StartExplosive== true)//死亡→無狀態
        {
            m_NowState = SpiderState.Dead;
            DeadStatus();
            return;
        }
        else if (TargetHp <= 1)
        {
            m_NowState = SpiderState.Dead;
            DeadStatus();
            return;
        }        
        else if (FrameCount_Roar <= 0)
        {
            MubAnimator.SetBool("Roar", false);

            AttackStatus();           
        }
        Vector3 cc = new Vector3(transform.position.x, 0, transform.position.z);
        transform.position = cc;
        Boon.transform.position = SpiderSelf.transform.position;
    }
    public void LookTarget()
    {
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

        transform.rotation = Quaternion.Slerp(transform.rotation, Look, Speed * Time.deltaTime);
    }
    public void DeadStatus()
    {
        if (m_NowState == SpiderState.Dead)
        {
            MubAnimator.SetTrigger("isTriggerDie");
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Attack", false);
            Selfcapsule.radius = 0f;
        }
    }    
    public void AttackStatus()
    {
        InATKrange = IsInRange_MeleeBattleRange(StartExplosionRadius, MySelf, Target);        
        if (InATKrange == true)
        {
            MoveSpeed = 0f;
            m_NowState = SpiderState.Attack;
            Attack();
        }        
        else if (m_NowState != SpiderState.Attack)
        {
            tracing = IsInRange_TraceRange(StartExplosionRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = SpiderState.Trace;
                if (m_NowState == SpiderState.Trace)
                {
                    if (StartCountDown == false)
                    {
                        CountDown = 4;
                        
                        StartCountDown = true;
                        StartCoroutine(CountTimeDown());
                    }
                    if (CountDown <= 0)
                    {
                        m_NowState = SpiderState.Attack;
                        Attack();
                    }
                    else if (CountDown > 0)
                    {
                        Move();
                        MubAnimator.SetBool("Trace", true);
                        LookTarget();
                    }
                }
            }            
        }
    }
    public void Move()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;

        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Selfcapsule.SimpleMove(GetTargetNormalize * MoveSpeed);

        if (GetTargetMegnitude == ATKRadius+Selfcapsule.radius){MoveSpeed = Speed * 0f;}

        else{ MoveSpeed = Speed;}
    }
    public void Roar()
    {
        MubAnimator.SetBool("Roar", true);
        MoveSpeed = Speed * 0f;
    }
    public void Attack()
    {
        if (m_NowState == SpiderState.Attack)
        {
            MubAnimator.SetBool("Attack", true);
            MubAnimator.SetBool("Trace", false);
        }        
    }
    IEnumerator CountTimeDown()
    {
        if (CountDown>0)
        {
            while (CountDown > 0)
            {
                yield return new WaitForSeconds(1);
                CountDown--;
                Debug.Log(CountDown);
            }           
        }       
    }
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude + TargetCapsule.radius > Radius;
    }
    public bool IsInRange_MeleeBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude + TargetCapsule.radius <= Radius;
    }
    IEnumerator changShader()
    {
        while (ColorChange>0)
        {            
            offset = -3;
            mat.SetFloat("_offset", offset);
            yield return new WaitForSeconds(ColorChangeTime);
            offset = 5;
            mat.SetFloat("_offset", offset);
            yield return new WaitForSeconds(ColorChangeTime);
            ColorChangeTime /= 1.5f;
            ColorChange--;
        }
    }
    public void ZoneOpen()
    {
        ATKRadius *=6;
        MubAnimator.speed = .5f;
        //Instantiate(DangerZone,MySelf.transform.position,Quaternion.identity,MySelf.transform);
        ColorChangeTime = .4f;
        ColorChange = 5;
        StartCoroutine(changShader());
    }
    private void Animation_AttackEventTest()
    {
        //Instantiate(ExplosionZone, MySelf.transform.position, Quaternion.identity, MySelf.transform);
        recoilShake.camraBearSake();
        ParticleSystem ps = Boon.GetComponent<ParticleSystem>();
        ps.Play();
    }
    private void AnimationSpeed_AttackEnd()
    {
        m_NowState = SpiderState.Dead;
        
        ATKRadius *=1f;       
        Destroy(SpiderSelf);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);       
    }
}
