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

    public GameObject TempPoint;//後退用，目前沒有用到

    MubHpData State;
    Animator MubAnimator;
    int hpTemporary;
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

    float CDs;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;
    int Count;
    bool AttackCBool=false;

    bool LeaveAttackRangeBool;
    bool InAttackRangeBool;
    bool RoarBool = false;
    int FrameCount_Roar;

    int RandomChoose;
    int i = 0;
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

        Close_ATKRadius = ATKRadius * 0.5f;        
        LeaveATKRadius = ATKRadius * 1.3f;
        RunRadius = ATKRadius * 2.3f;
        AwakeRadius = ATKRadius * 2f;
        AttackCBool = false;
        Count = 0;
    }

    // Update is called once per frame
    void Update()
    {
        AwakeSensor();
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
            }
            else if (FrameCount_Roar <= 0)
            {
                MubAnimator.SetBool("GetHit", false);

                MubAnimator.SetBool("Warn", false);

                GetTargetNormalize = (Target.transform.position - transform.position).normalized;

                Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

                transform.rotation = Quaternion.Slerp(transform.rotation, Look, 7f * Time.deltaTime);


                Debug.Log("CDs:" + Count);

                TraceStatus();

                AttackStatus();

                LeaveAttackStatus();

                TooCloseAttackStatus_York();

                BackStatus();

            }
        }
        
        //Debug.Log(m_NowState);
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
                Debug.Log("NowInT");
                if (m_NowState == BearState.Trace)
                {
                    Move();
                    MubAnimator.SetBool("Trace", true);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                }
            }
            else
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
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
            Attack();
            Debug.Log("NowInA");
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
                Debug.LogError("L is active");
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
                Debug.LogError("LI is active");
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
                Debug.Log("NowInB");
                if (m_NowState == BearState.Back)
                {
                    Back();
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                }
            }
            else
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false); 
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

        return direction.magnitude <= RadiusMax && direction.magnitude > RadiusMin;
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
            if (i==0)
            {
                RandomChoose = UnityEngine.Random.Range(1, 3);
                i++;
            }                       
            Debug.Log("隨機數:"+RandomChoose);
            if (RandomChoose ==1)
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
    }
    public void Move()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;

        Debug.Log("當前速度" + MoveSpeed);

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

        Vector3 m = Vector3.MoveTowards(transform.position, Target.transform.position, MoveSpeed);

        transform.position = m;

        Debug.Log(MoveSpeed);

        if (GetTargetMegnitude == ATKRadius)
        {
            MoveSpeed = Speed * 0f;
        }
        else
        {
            MoveSpeed = Speed * .01f;
        }
    }
    public void Back()
    {
        GetTargetMegnitude = (Target.transform.position - transform.position).magnitude;

        Debug.Log("當前速度" + MoveSpeed);
      
        Vector3 m = Vector3.MoveTowards(transform.position, TempPoint.transform.position, BackSpeed);

        transform.position = m;

        Debug.Log(MoveSpeed);

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
        MoveSpeed = Speed * .01f;
        Vector3 m = Vector3.MoveTowards(transform.position, Target.transform.position, MoveSpeed*5);
        transform.position = m;
        MubAnimator.speed = 2f;
        
    }

    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    private void AnimationSpeed_AttackEnd03()
    {
        MubAnimator.speed = 1f;
        LeaveATKRadius = ATKRadius * 1.3f;
        Close_ATKRadius = ATKRadius * .5f;
    }
    public void ZoneOpen()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 2;
        StartCoroutine(SummonCooldown());
    }
    public void ZoneOpen01()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 4;
        Instantiate(TempPoint, transform.position, Quaternion.identity, MySelf.transform);
    }

    public void ZoneOpen02()
    {
        LeaveATKRadius = ATKRadius * 6;
        Close_ATKRadius = ATKRadius * .1f;
        Count = 4;
        Instantiate(TempPoint, transform.position, Quaternion.identity, MySelf.transform);
    }
    IEnumerator SummonCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;
            if (Count == 1)
            {
                i = 0;
            }
        }
    }
}
