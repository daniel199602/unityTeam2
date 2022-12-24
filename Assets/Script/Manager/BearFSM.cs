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
    ItemOnMob ThisItemOnMob_State;

    private GameObject Target;//�s���a
    private GameObject MySelf;//�s�ۤv

    MubHpData State;
    Animator MubAnimator;
    [SerializeField]int hpTemporary;
    int TargetHp;
    CharacterController capsule;
    bool backing;
    bool tracing;
    bool InATKrange;
    bool InATKrange_Close;
    bool OutATKrange;
    bool AwakeBool = false;
    bool Awaken;

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
    private void Awake()
    {
        ThisItemOnMob_State = GetComponent<ItemOnMob>();
    }
    // Start is called before the first frame update
    void Start()
    {        
        Target = GameManager.Instance().PlayerStart;//��X���a
        MySelf = this.transform.gameObject;//��X�ۤv
        

        m_NowState = BearState.Idle;
        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();
        hpTemporary = State.Hp;
        FrameCount_Roar = 340;//����_�l�첾
        LeaveAttackRangeBool = false;
        InAttackRangeBool = false;

        m_NowState = BearState.Idle;

        ATKRadius = ThisItemOnMob_State.mobRadius;//Weapon�л\

        Close_ATKRadius = ATKRadius * 0.4f;        
        LeaveATKRadius = ATKRadius * 1.02f;
        RunRadius = ATKRadius * 2.3f;
        AwakeRadius = ATKRadius * 2f;
        AttackCBool = false;
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        TargetHp = Target.GetComponent<PlayerHpData>().Hp;
        Quaternion c = new Quaternion(0, transform.rotation.y, 0,transform.rotation.w);
        transform.rotation = c;
        AwakeSensor();

        if (AwakeBool == true)
        {
            if (FrameCount_Roar > 0)
            {
                FrameCount_Roar--;
                LookTarget();
            }
            if (RoarBool == false)
            {                
                Roar();
                RoarBool = true;
            }
            if (State.Hp <= 0)//���`���L���A
            {
                m_NowState = BearState.Dead;
                DeadStatus();
                return;
            }
            else if (TargetHp <= 1)
            {
                EatPlayer();
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
                        capsule.SimpleMove(-(transform.forward * (MoveSpeed / 5)));
                    }
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
    }
    public void LookTarget()
    {
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

        Quaternion R = Quaternion.Slerp(transform.rotation, Look, 1.2f * Time.deltaTime);
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
        if (m_NowState == BearState.Dead)
        {
            MubAnimator.SetTrigger("Die");
            MubAnimator.SetBool("Attack01", false);
            MubAnimator.SetBool("Attack02", false);
            MubAnimator.SetBool("Attack03", false);
            MubAnimator.SetBool("Rotate", false);
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("GetHit", false);
            isAttacking = true;
            capsule.radius = 0f;
        }
    }
    //�l�����A
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
    //�������A
    public void AttackStatus()
    {
        InATKrange = IsInRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
           
            MoveSpeed = 0f;
            m_NowState = BearState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            MubAnimator.SetBool("Rotate", false);
            Attack();
        }
    }
    //�b�����J����(�~��)���A
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
        }
    }
    //�b�����J����(����)���A
    public void TooCloseAttackStatus_York()
    {
        if (InAttackRangeBool == true)
        {
            InATKrange_Close = CloseRange_RangedBattleRange(ATKRadius, Close_ATKRadius, MySelf, Target);
            if (InATKrange_Close == true)
            {
                m_NowState = BearState.Attack;
                Attack();
            }
            GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
            if (GetTargetMegnitude < Close_ATKRadius)
            {
                InAttackRangeBool = false;
            }
        }
    }
    //��h���A
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
    //�b���{�����d�� 
    public bool IsInRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude >= RadiusMin;
    }
    //�b���{�����J���Ͻd��(�~��) 
    public bool IsOutRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
    }
    //�b���{�����J���Ͻd��(����) 
    public bool CloseRange_RangedBattleRange(float RadiusMax, float RadiusMin, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= RadiusMax && direction.magnitude >= RadiusMin;
    }
    //�d��P�w_��h
    public bool TooCloseRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude < Radius;
    }

    /*�d��P�w_�l��*/
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
            LeaveAttackRangeBool = true;
            InAttackRangeBool = true;
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
            
        }
    }
    public void Idle()
    {
        MubAnimator.SetBool("Trace", false);
        MubAnimator.SetBool("Rotate", true);
        MubAnimator.SetBool("Attack01", false);
        MubAnimator.SetBool("Attack02", false);
        MubAnimator.SetBool("Attack03", false);
    }
    public void EatPlayer()
    {
        isAttacking = false;
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;        
        MubAnimator.SetBool("Attack01", false);
        MubAnimator.SetBool("Attack02", false);
        MubAnimator.SetBool("Attack03", false);
        MubAnimator.SetBool("Rotate", false);
        MubAnimator.SetBool("Trace", false);
        MubAnimator.SetBool("GetHit", false);
        if (GetTargetMegnitude <= 30)
        {
            MubAnimator.SetBool("GoTo", false);
        }
        else
        {
            capsule.SimpleMove(GetTargetNormalize * MoveSpeed);
            MubAnimator.SetBool("GoTo", true);
        }
    }
    public void Move()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
        isAttacking = false;
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
        isAttacking = false;

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

        isAttacking = true;
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        capsule.SimpleMove(GetTargetNormalize * MoveSpeed*3);
        MubAnimator.speed = 2f;        
    }

    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.02f;
        Close_ATKRadius = ATKRadius * 0.4f;
        MubAnimator.applyRootMotion = false;
        isAttacking = false;
        LeaveAttackRangeBool = false;
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    private void AnimationSpeed_AttackEnd01P()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.02f;
        Close_ATKRadius = ATKRadius * 0.4f;
        MubAnimator.applyRootMotion = false;
    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.02f;
        Close_ATKRadius = ATKRadius * 0.4f;
        MubAnimator.applyRootMotion = false;
        isAttacking = false;
        LeaveAttackRangeBool = false;
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    private void AnimationSpeed_AttackEnd02P()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.02f;
        Close_ATKRadius = ATKRadius * 0.4f;
        MubAnimator.applyRootMotion = false;
    }
    private void AnimationSpeed_AttackEnd03()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.02f;
        Close_ATKRadius = ATKRadius * 0.4f;
        MubAnimator.applyRootMotion = false;
        isAttacking = false;
        LeaveAttackRangeBool = false;
        Count = 3;
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
