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

    public GameObject Target;
    public GameObject MySelf;

    //TestUse
    public GameObject DangerZone;
    public GameObject ExplosionZone;

    Animator MubAnimator;

    CapsuleCollider Selfcapsule;

    CharacterController TargetCapsule;

    bool StartExplosive = false;

    bool RoarBool = false;

    bool tracing;

    bool InATKrange;

    float TraceRadius;

    float ATKRadius;
    float GizmoRa;
    public float Speed;

    Vector3 GetTargetNormalize;

    float GetTargetMegnitude;

    float MoveSpeed;

    int FrameCount_Roar;
    private void Start()
    {
        mesh = gameObject.GetComponent<SkinnedMeshRenderer>().sharedMesh;

        mat = gameObject.GetComponent<SkinnedMeshRenderer>().materials[matIndex];

        m_NowState = SpiderState.Idle;

        Selfcapsule = GetComponent<CapsuleCollider>();

        TargetCapsule = Target.GetComponent<CharacterController>();

        MubAnimator = GetComponent<Animator>();

        FrameCount_Roar = 150;

        ATKRadius = 50;//Weapon覆蓋

        GizmoRa = 50+ TargetCapsule.radius*10;

        TraceRadius = ATKRadius;

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
            Debug.LogError("f:" + FrameCount_Roar);

            MubAnimator.SetBool("Roar", false);

            if (m_NowState!= SpiderState.Attack)
            {
                GetTargetNormalize = (Target.transform.position - transform.position).normalized;

                Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, Speed * Time.deltaTime);

                TraceStatus();
            }
            AttackStatus();
        }
        Debug.Log(m_NowState);
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
        tracing = IsInRange_TraceRange(TraceRadius, MySelf, Target);
        if (tracing == true)
        {
            m_NowState = SpiderState.Trace;
            Debug.Log("NowInT");
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
        InATKrange = IsInRange_MeleeBattleRange(ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {           
            MoveSpeed = 0f;
            TraceRadius = ATKRadius * 3f;
            m_NowState = SpiderState.Attack;
            MubAnimator.SetBool("Trace", false);
            Attack();
            
            Debug.Log("NowInA");
        }
    }
    public void Move()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
        Debug.Log("當前速度" + MoveSpeed);        

        Vector3 m = Vector3.MoveTowards(transform.position, Target.transform.position, MoveSpeed);

        transform.position = m;

        Debug.Log(MoveSpeed);

        if (GetTargetMegnitude == ATKRadius+Selfcapsule.radius){MoveSpeed = Speed * 0f;}

        else{ MoveSpeed = Speed * .01f;}
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
            MubAnimator.speed = .3f;
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
        for (int i = 10; i >= 0; i--)
        {
            Debug.Log("changing");
            offset = -3;
            mat.SetFloat("_offset", offset);
            yield return new WaitForSeconds(i*.1f);
            offset = 5;
            yield return new WaitForSeconds(i*.1f);
        }
    }
    public void ZoneOpen()
    {
        ATKRadius *=6;
        Instantiate(DangerZone,MySelf.transform.position,Quaternion.identity,MySelf.transform);
        StartCoroutine(changShader());
    }
    private void Animation_AttackEventTest()
    {

        Instantiate(ExplosionZone, MySelf.transform.position, Quaternion.identity, MySelf.transform);

    }
    private void AnimationSpeed_AttackEnd()
    {
        m_NowState = SpiderState.Dead;
        
        ATKRadius *=1f;       
        Destroy(gameObject);
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color =  Color.cyan;
        Gizmos.DrawWireSphere(transform.position, GizmoRa);
    }
}
