using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BearState
{
    Idle, Trace, Attack, GetHit, Dead,
}
public class BearFSM : MonoBehaviour
{
    private BearState m_NowState;
    ItemOnMob ThisItemOnMob_State;

    private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    MubHpData State;
    Animator MubAnimator;
    [SerializeField] int hpTemporary;
    int TargetHp;
    CharacterController capsule;
    bool tracing;
    bool InATKrange;
    bool AwakeBool = false;
    bool Awaken;

    float RunRadius;
    float ATKRadius;
    float LeaveATKRadius;
    float Close_ATKRadius;
    float AwakeRadius;
    float mobAngle;

    public float Speed = 20f;
    [SerializeField] float MoveSpeed;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
    int Count;

    bool AttackCBool = false;

    bool RoarBool = false;

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
        Target = GameManager.Instance().PlayerStart;//抓出玩家

        MySelf = this.transform.gameObject;//抓出自己

        MoveSpeed = Speed;

        capsule = GetComponent<CharacterController>();

        MubAnimator = GetComponent<Animator>();

        State = GetComponent<MubHpData>();

        hpTemporary = State.Hp;

        FrameCount_Roar = 200;//鎖住起始位移

        m_NowState = BearState.Idle;

        ATKRadius = ThisItemOnMob_State.mobRadius;//Weapon覆蓋

        mobAngle = ThisItemOnMob_State.mobAngle;

        Close_ATKRadius = ATKRadius;

        LeaveATKRadius = ATKRadius * 1.5f;

        RunRadius = ATKRadius * 2.3f;

        AwakeRadius = ATKRadius * 2f;

        AttackCBool = false;

        Count = 0;
    }
    //追擊狀態
    public void TraceStatus()
    {
        if (AttackCBool == false)
        {
            tracing = IsInRange_TraceRange(ATKRadius, MySelf, Target);
            if (tracing == true)
            {
                m_NowState = BearState.Trace;
            }
        }
    }
    //攻擊狀態
    public void AttackStatus()
    {
        InATKrange = IsInRange_RangedBattleRange(ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            m_NowState = BearState.Attack;
        }
    }

    // Update is called once per frame
    void Update()
    {
        TargetHp = Target.GetComponent<PlayerHpData>().Hp;
        Quaternion c = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
        transform.rotation = c;
        AwakeSensor();
        Debug.LogError("BearState:" + m_NowState);
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
            if (State.Hp <= 0)//死亡→無狀態
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
                    }
                }
                else if (FrameCount_Roar <= 0)
                {
                    MubAnimator.SetBool("GetHit", false);

                    MubAnimator.SetBool("Warn", false);

                    TraceStatus();

                    AttackStatus();

                    Move();

                    Attack();
                }
            }
        }
    }
    public void LookTarget()
    {
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

        Quaternion R = Quaternion.Slerp(transform.rotation, Look, 1.2f * Time.deltaTime);

        transform.rotation = R;

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
            capsule.radius = 0f;
        }
    }

    public void Roar()
    {
        MubAnimator.SetBool("Warn", true);
    }
    public void Attack()
    {
        if (m_NowState == BearState.Attack)
        {
            if (Count == 0)
            {
                Debug.Log("c=0");
                GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
                if (GetTargetMegnitude>= ATKRadius)
                {
                    AttackCBool = false;
                    return;
                }
                else if(GetTargetMegnitude < ATKRadius)
                {
                    MubAnimator.SetBool("Trace", false);
                    AttackCBool = true;
                    ATKRadius = LeaveATKRadius;
                    Vector3 direction = Target.transform.position - transform.position;

                    float dot = Vector3.Dot(direction.normalized, transform.forward);

                    float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

                    if (offsetAngle > (mobAngle * .7f) / 3)
                    {
                        LookTarget();
                        MubAnimator.SetBool("Rotate", true);
                    }
                    else if (offsetAngle <= (mobAngle * .7f) / 3)
                    {
                        MubAnimator.SetBool("Rotate", false);
                        if (RandomChooseCoolDown == false)
                        {
                            RandomChoose = UnityEngine.Random.Range(1, 4);
                            RandomChooseCoolDown = true;
                            if (RandomChoose == 1)
                            {
                                MubAnimator.SetBool("Attack01", true);

                            }
                            else if (RandomChoose == 2)
                            {
                                MubAnimator.SetBool("Attack02", true);

                            }
                            else if (RandomChoose == 3)
                            {
                                MubAnimator.SetBool("Attack03", true);

                            }
                        }
                       
                    }
                }
               
            }
            else if (Count != 0)
            {
                Debug.Log("c!=0");
                tracing = IsInRange_TraceRange(Close_ATKRadius, MySelf, Target);
                if (tracing == true)
                {
                    Move_A();
                }
                else
                {
                    Debug.Log("ttttt");
                    Vector3 direction = Target.transform.position - transform.position;

                    float dot = Vector3.Dot(direction.normalized, transform.forward);

                    float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                    if (offsetAngle > (mobAngle * .7f) / 3)
                    {
                        LookTarget();
                        MubAnimator.SetBool("Rotate", true);
                    }
                    else if (offsetAngle <= (mobAngle * .7f) / 3)
                    {
                        MubAnimator.SetBool("Rotate", false);
                    }
                }
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
            }
        }
    }
    public void EatPlayer()
    {
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
            LookTarget();
            capsule.Move(GetTargetNormalize * MoveSpeed * Time.deltaTime);
            MubAnimator.SetBool("GoTo", true);
        }
    }
    public void Move()
    {
        if (m_NowState == BearState.Trace)
        {
            Debug.Log("tt");
            ATKRadius = Close_ATKRadius;
            MubAnimator.SetBool("Trace", true);
            MubAnimator.SetBool("Attack01", false);
            MubAnimator.SetBool("Attack02", false);
            MubAnimator.SetBool("Attack03", false);
            MubAnimator.SetBool("Rotate", false);

            GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
            GetTargetNormalize = (Target.transform.position - transform.position).normalized;
            capsule.Move(GetTargetNormalize * MoveSpeed*Time.deltaTime);

            LookTarget();
            if (GetTargetMegnitude > RunRadius)
            {
                MoveSpeed = Speed * 1.5f;
                MubAnimator.SetBool("Run", true);
            }
            else
            {
                MubAnimator.SetBool("Run", false);
                if (GetTargetMegnitude <= ATKRadius)
                {
                    MoveSpeed = Speed * 0f;
                }
                else
                {
                    MoveSpeed = Speed * 1f;                   
                }
            }
        }
    }
    public void Move_A()
    {
        Debug.Log("TTA");
        ATKRadius = Close_ATKRadius;
        MubAnimator.SetBool("Trace", true);
        MubAnimator.SetBool("Attack01", false);
        MubAnimator.SetBool("Attack02", false);
        MubAnimator.SetBool("Attack03", false);
        MubAnimator.SetBool("Rotate", false);

        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;
        capsule.Move(GetTargetNormalize * MoveSpeed * Time.deltaTime);
        LookTarget();

        if (GetTargetMegnitude <= Close_ATKRadius)
        {
            MoveSpeed = Speed * 0f;
        }
        else
        {
            MoveSpeed = Speed * 1f;            
        }

    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, AwakeRadius);
    }
    private void Animation_Attack()
    {
        MubAnimator.speed = 2f;
    }

    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    private void AnimationSpeed_AttackEnd01P()
    {
        MubAnimator.speed = 1f;
    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
        Count = 3;
        StartCoroutine(SummonCooldown());
    }
    private void AnimationSpeed_AttackEnd02P()
    {
        MubAnimator.speed = 1f;
    }
    private void AnimationSpeed_AttackEnd03()
    {
        MubAnimator.speed = 1f;
        Count = 3;
        StartCoroutine(SummonCooldown());
    }
    IEnumerator SummonCooldown()
    {
        while (Count > 0)
        {
            Debug.LogWarning("CD:"+ Count);
            yield return new WaitForSeconds(1);
            Count--;
            if (Count == 1)
            {
                RandomChooseCoolDown = false;
            }
        }
    }
    public bool IsInRange_AwakeRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude <= Radius;
    }
    //在攻擊範圍內 
    public bool IsInRange_RangedBattleRange(float Radius, GameObject attacker, GameObject attacked)
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
