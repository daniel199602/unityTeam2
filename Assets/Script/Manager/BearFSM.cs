using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BearState
{
    Idle, Trace, Back, Attack, GetHit, Dead,
}
public class BearFSM : MonoBehaviour
{
    private BearState m_NowState;

    private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    MubHpData State;
    Animator MubAnimator;
    [SerializeField]int hpTemporary;
    CharacterController capsule;
    bool backing;
    bool tracing;
    bool InATKrange;
    bool InATKrange_Close;
    bool OutATKrange;
    bool AwakeBool = false;
    bool Awake;

    float RunRadius;
    float ATKRadius;
    float LeaveATKRadius;
    float Close_ATKRadius;
    float AwakeRadius;

    public float Speed=20f;
    float MoveSpeed;
    float BackSpeed;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
    int Count;
    bool AttackCBool=false;

    bool LeaveAttackRangeBool;
    bool InAttackRangeBool;
    bool RoarBool = false;
    bool isAttacking = false;

    int FrameCount_Roar;

    int RandomChoose;
    bool RandomChooseCoolDown = false;
    // Start is called before the first frame update
    void Start()
    {        
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己

        m_NowState = BearState.Idle;
        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();
        hpTemporary = State.Hp;
        FrameCount_Roar = 400;
        LeaveAttackRangeBool = false;
        InAttackRangeBool = false;

        m_NowState = BearState.Idle;

        ATKRadius = 40;//Weapon覆蓋

        Close_ATKRadius = ATKRadius * 0.6f;        
        LeaveATKRadius = ATKRadius * 1.07f;
        RunRadius = ATKRadius * 2.3f;
        AwakeRadius = ATKRadius * 2f;
        AttackCBool = false;
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        //玩家死亡TODO()還沒寫
        Quaternion c = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
        transform.rotation = c;
        AwakeSensor();

        if (AwakeBool == true)
        {
            if (FrameCount_Roar > 0)
            {
                FrameCount_Roar--;
            }
            if (RoarBool == false)
            {
                LookTarget();
                Roar();
                RoarBool = true;
            }
            if (State.Hp <= 0)//死亡→無狀態
            {
                m_NowState = BearState.Dead;
                DeadStatus();
                return;
            }
            else if (State.Hp != hpTemporary)
            {
                hpTemporary = State.Hp;
                MubAnimator.SetBool("GetHit", true);
                capsule.SimpleMove(-(transform.forward*(MoveSpeed/5)));
            }
            else if (FrameCount_Roar <= 0)
            {
                MubAnimator.SetBool("GetHit", false);

                MubAnimator.SetBool("Warn", false);

                LookTarget();

                TraceStatus();

                AttackStatus();

                LeaveAttackStatus();

                TooCloseAttackStatus_York();

                BackStatus();

            }
        }
    }
    public void LookTarget()
    {
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

        Quaternion R = Quaternion.Slerp(transform.rotation, Look, 7f * Time.deltaTime);
        if (isAttacking == false)
        {
            if (GetTargetNormalize != transform.forward)
            {
                MubAnimator.SetBool("Rotate", true);
                transform.rotation = R;
            }
            else if (GetTargetNormalize == transform.forward)
            {
                MubAnimator.SetBool("Rotate", false);
            }
        }       
    }
    public void AwakeSensor()
    {
        {
            if (AwakeBool == false)
            {
                Awake = IsInRange_AwakeRange(AwakeRadius, MySelf, Target);
                if (Awake == true)
                {
                    AwakeBool = true;
                }
            }
        }
    }
    public void DeadStatus()
    {
        if (m_NowState == BearState.Dead)
        {
            MubAnimator.SetTrigger("Die");
            capsule.radius = 0f;
        }
    }
    //追擊狀態
    public void TraceStatus()
    {
        if (LeaveAttackRangeBool == false)
        {
            tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = BearState.Trace;
                if (m_NowState == BearState.Trace)
                {
                    Move();
                    MubAnimator.SetBool("Trace", true);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                    MubAnimator.SetBool("Rotate", false);
                }
            }
            else
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
                MubAnimator.SetBool("Rotate", false);
            }
        }
    }
    //攻擊狀態
    public void AttackStatus()
    {
        InATKrange = IsInRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            LeaveAttackRangeBool = true;
            InAttackRangeBool = true;
            MoveSpeed = 0f;
            m_NowState = BearState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            MubAnimator.SetBool("Rotate", false);
            Attack();
        }
    }
    //在攻擊蛋黃區(外圈)狀態
    public void LeaveAttackStatus()
    {
        if (LeaveAttackRangeBool == true)
        {
            OutATKrange = IsOutRange_RangedBattleRange(LeaveATKRadius, ATKRadius, MySelf, Target);
            if (OutATKrange == true)
            {
                m_NowState = BearState.Idle;
                Idle();
            }
            GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
            if (GetTargetMegnitude > LeaveATKRadius)
            {
                LeaveAttackRangeBool = false;
            }
        }
    }
    //在攻擊蛋黃區(內圈)狀態
    public void TooCloseAttackStatus_York()
    {
        if (InAttackRangeBool == true)
        {
            InATKrange_Close = CloseRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
            if (InATKrange_Close == true)
            {
                m_NowState = BearState.Attack;                
            }
            GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
            if (GetTargetMegnitude < Close_ATKRadius)
            {
                InAttackRangeBool = false;
            }
        }
    }
    //後退狀態
    public void BackStatus()
    {
        if (InAttackRangeBool == false)
        {
            if (AttackCBool ==false)
            {
                backing = TooCloseRange_RangedBattleRange(Close_ATKRadius, MySelf, Target);
                AttackCBool = true;
            }                        
            if (backing == true)
            {
                m_NowState = BearState.Back;
                if (m_NowState == BearState.Back)
                {
                    Back();
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                    MubAnimator.SetBool("Rotate", false);
                }
            }
            else
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
                MubAnimator.SetBool("Rotate", false);
            }
        }
    }
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //在遠程攻擊範圍內 
    public bool IsInRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude >= RadiusMin;
    }
    //在遠程攻擊蛋黃區範圍(外圈) 
    public bool IsOutRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //在遠程攻擊蛋黃區範圍(內圈) 
    public bool CloseRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude >= RadiusMin;
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
        if (m_NowState == BearState.Attack&& Count == 0)
        {
            isAttacking = true;
            if (RandomChooseCoolDown==false)
            {
                RandomChoose = UnityEngine.Random.Range(1, 4);
                RandomChooseCoolDown= true;
            }                       
            if (RandomChoose ==1)
            {
                MubAnimator.SetBool("Attack01", true);
                MubAnimator.applyRootMotion = true;
            }
            else if (RandomChoose == 2)
            {
                MubAnimator.SetBool("Attack02", true);
                MubAnimator.applyRootMotion = true;
            }
            else if (RandomChoose == 3)
            {
                MubAnimator.SetBool("Attack03", true);
                MubAnimator.applyRootMotion = true;
            }
        }
        else if (Count!=0)
        {
            MubAnimator.SetBool("Attack01", false);
            MubAnimator.SetBool("Attack02", false);
            MubAnimator.SetBool("Attack03", false);            
            isAttacking = false;
        }
    }
    public void Idle()
    {
        MubAnimator.SetBool("Trace", false);
    }
    public void Move()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;

        if (GetTargetMegnitude > RunRadius)
        {
            MoveSpeed *= 1.5f;
            MubAnimator.SetBool("Run",true);
        }
        else
        {
            MoveSpeed *= 1f;
            MubAnimator.SetBool("Run", false);
            MubAnimator.SetBool("Trace", true);
        }

        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        capsule.SimpleMove(GetTargetNormalize * MoveSpeed);

        if (GetTargetMegnitude == ATKRadius)
        {
            MoveSpeed = Speed * 0f;
        }
        else
        {
            MoveSpeed = Speed * 1f;
        }
    }
    public void Back()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;

        capsule.SimpleMove(-(transform.forward*5));

        if (GetTargetMegnitude == ATKRadius)
        {
            BackSpeed = Speed * 0f;
        }
        else
        {
            BackSpeed = Speed * .003f;
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color = OutATKrange ? Color.blue : Color.black;
        Gizmos.DrawWireSphere(transform.position, LeaveATKRadius);

        Gizmos.color = InATKrange_Close ? Color.green : Color.cyan;
        Gizmos.DrawWireSphere(transform.position, Close_ATKRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AwakeRadius);
    }
    private void Animation_Attack()
    {
        MoveSpeed = Speed;
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        capsule.SimpleMove(GetTargetNormalize * MoveSpeed*2);
        MubAnimator.speed = 2f;
        
    }

    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.07f;
        Close_ATKRadius = ATKRadius * .5f;
        MubAnimator.applyRootMotion = false;
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    private void AnimationSpeed_AttackEnd01P()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.07f;
        Close_ATKRadius = ATKRadius * .5f;
        MubAnimator.applyRootMotion = false;

    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.07f;
        Close_ATKRadius = ATKRadius * .5f;
        MubAnimator.applyRootMotion = false;
        Count = 4;
        StartCoroutine(SummonCooldown());
    }
    private void AnimationSpeed_AttackEnd02P()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.07f;
        Close_ATKRadius = ATKRadius * .5f;
        MubAnimator.applyRootMotion = false;

    }
    private void AnimationSpeed_AttackEnd03()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.07f;
        Close_ATKRadius = ATKRadius * .5f;
        MubAnimator.applyRootMotion = false;
        Count = 4;
        StartCoroutine(SummonCooldown());
    }
    public void ZoneOpen()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;       
    }
    public void ZoneOpen01()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;      
    }

    public void ZoneOpen02()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;      
    }
    IEnumerator SummonCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;
            if (Count == 1)
            {
                RandomChooseCoolDown = false;
            }
        }
    }
}
