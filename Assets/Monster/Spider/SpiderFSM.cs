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

        FrameCount_Roar = 420;

        ATKRadius = ThisItemOnMob_State.mobRadius;//Weapon覆蓋

        StartExplosionRadius = ATKRadius * 0.5f;

        StartExplosive = false;
    }
    private void Update()
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
        if (StartExplosive== true)//死亡→無狀態
        {           
            DeadStatus();
            return;
        }
        else if (FrameCount_Roar <= 0)
        {
            MubAnimator.SetBool("Roar", false);

            AttackStatus();
            if (m_NowState!= SpiderState.Attack)
            {
                GetTargetNormalize = (Target.transform.position - transform.position).normalized;

                Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, Speed * Time.deltaTime);

                TraceStatus();
            }
        }

        Boon.transform.position = SpiderSelf.transform.position;
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
    public void TraceStatus()
    {
        tracing = IsInRange_TraceRange(StartExplosionRadius, MySelf, Target);
        if (tracing == true)
        {
            m_NowState = SpiderState.Trace;
            if (m_NowState == SpiderState.Trace)
            {
                Move();
                MubAnimator.SetBool("Trace", true);
                MubAnimator.SetBool("Attack", false);
            }
        }
    }
    public void AttackStatus()
    {
        InATKrange = IsInRange_MeleeBattleRange(StartExplosionRadius, MySelf, Target);
        if (InATKrange == true)
        {           
            MoveSpeed = 0f;
            m_NowState = SpiderState.Attack;
            MubAnimator.SetBool("Trace", false);
            Attack();            
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
