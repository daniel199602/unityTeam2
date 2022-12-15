using System.Collections;
using UnityEngine;

public enum BossState
{
    Idle, Trace, Back, Attack, RangeAttack, GetHit, Dead,
}
public class BossFSM : MonoBehaviour
{
    private BossState m_NowState;

    private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    private GameObject TeleportPoint01;
    private GameObject TeleportPoint02;

    public GameObject AxeOnHand;
    public GameObject BigSwordOnHand;
    public GameObject AxeOnBack;
    public GameObject BigSwordOnBack;

    MubHpData State;
    Animator MubAnimator;
    int hpTemporary;
    int hpTemporaryMax;
    CharacterController capsule;

    bool LookBool;
    bool InATKrangeSwitch;
    bool backing;
    bool tracing;
    bool InRangeATKrange;
    bool RangeCDtodo;
    bool InATKrange;
    bool GetHit;
    bool StageTwo = false;
    bool IK = false;//2階段單次判定，不會重複進入2階段

    float TraceRadius;
    float ATKRadius;
    float RangedRadius;
    float R_ATKangle;
    public float RotateSpeed;

    int RCount;
    int Count;
    int UCount;

    Vector3 GetTargetNormalize;
    float GetTargetMegnitude;

    int RandomChoose;
    int RandomChooseHit;
    bool RandomChooseCd = false;

    ItemOnMob ThisItemOnMob_State;

    // Start is called before the first frame update
    private void Awake()
    {
        ThisItemOnMob_State = GetComponent<ItemOnMob>();
        TeleportPoint01 = GameObject.Find("TeleportPoint01");
        TeleportPoint02 = GameObject.Find("TeleportPoint02");
    }
    void Start()
    {
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己

        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();

        //武器初始狀態
        BigSwordOnBack.SetActive(true);
        BigSwordOnHand.SetActive(false);
        AxeOnBack.SetActive(false);
        AxeOnHand.SetActive(true);

        //血量區
        hpTemporary = State.Hp;
        hpTemporaryMax = State.Hp;

        LookBool = true;
        RotateSpeed = 7f;

        //初始狀態
        m_NowState = BossState.Idle;

        //範圍設定
        ATKRadius = ThisItemOnMob_State.mobRadius;//Weapon覆蓋        
        RangedRadius = ATKRadius * 1.8f;
        TraceRadius = ATKRadius * 2f;

        GetHit = false;
        InATKrangeSwitch = false;//近距離&遠距離切換判定

        //初始冷卻設定
        Count = 0;
        RCount = 0;
        UCount = 40;        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 pv = new Vector3(transform.position.x, 0f, transform.position.z);
        transform.position = pv;
        if (LookBool== true)
        {
            LookPoint();
        }
        if (State.Hp <= 0)//死亡→無狀態
        {
            m_NowState = BossState.Dead;
            DeadStatus();
            return;
        }
        else if ((float)hpTemporary / (float)hpTemporaryMax <= 0.5f && IK == false)
        {
            StageTwo = true;
            IK = true;
            StageTwoCost();
        }
        else if (State.Hp != hpTemporary)
        {
            hpTemporary = State.Hp;
            if (GetHit == false)
            {
                RandomChooseHit = UnityEngine.Random.Range(1, 2);
                GetHit = true;
                if (RandomChooseHit == 1)
                {
                    MubAnimator.SetBool("GetHit01", true);                    
                    return;
                }
                if (RandomChooseHit == 2)
                {
                    MubAnimator.SetBool("GetHit02", true);
                    return;
                }
            }
        }
        else if (UCount <= 0)
        {
            MubAnimator.SetBool("GetHit01", false);
            MubAnimator.SetBool("GetHit02", false);
            Ultimate();            
        }
        else
        {
            MubAnimator.SetBool("GetHit01", false);
            MubAnimator.SetBool("GetHit02", false);
            GetHit = false;            

            TraceStatus();

            RangedAttackStatus();

            AttackStatus();           
        }
        Debug.Log(m_NowState);
    }
    public void StageTwoCost()
    {
        if (StageTwo == true)
        {
            MubAnimator.SetTrigger("T2");
            UCount = 0;
            LookBool = false;
        }
    }
    public void LookPoint()
    {
        GetTargetNormalize = (Target.transform.position - transform.position).normalized;

        Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

        transform.rotation = Quaternion.Slerp(this.transform.rotation, Look, RotateSpeed * Time.deltaTime);
    }
    public void DeadStatus()
    {
        if (m_NowState == BossState.Dead)
        {
            MubAnimator.SetTrigger("Die");
            capsule.radius = 0f;
        }
    }
    //追擊狀態
    public void TraceStatus()
    {
        tracing = IsInRange_TraceRange(RangedRadius, MySelf, Target);
        if (tracing == true)
        {
            m_NowState = BossState.Trace;
            Debug.Log("NowInT");
            if (m_NowState == BossState.Trace)
            {
                MubAnimator.SetBool("Trace", true);
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
                MubAnimator.SetBool("R_Attack", false);
                MubAnimator.SetBool("TAttack01", false);
                MubAnimator.SetBool("TAttack02", false);
                MubAnimator.SetBool("TAttack03", false);
            }
        }
    }
    public void RangedAttackStatus()
    {
        if (InATKrangeSwitch == false)
        {
            InRangeATKrange = IsInRange_RangedBattleRange(RangedRadius, MySelf, Target);
            if (InRangeATKrange == true)
            {
                m_NowState = BossState.RangeAttack;
                MubAnimator.SetBool("Trace", false);
                RangeAttack();
            }
        }
    }
    //攻擊狀態
    public void AttackStatus()
    {
        InATKrange = IsInRange_BattleRange(ATKRadius, MySelf, Target);
        if (InATKrange == true)
        {
            InATKrangeSwitch = true;
            m_NowState = BossState.Attack;
            MubAnimator.SetBool("Trace", false);
            MubAnimator.SetBool("Back", false);
            Attack();
        }
        else
        {
            InATKrangeSwitch = false;
        }
    }

    public void RangeAttack()
    {
        if (m_NowState == BossState.RangeAttack && RCount == 0)
        {
            MubAnimator.SetBool("R_Attack", true);
        }
        else if (RCount != 0)
        {
            RangeCDtodo = IsInRange_RangedBattleRange(ATKRadius, MySelf, Target);
            if (RangeCDtodo == true)
            {
                m_NowState = BossState.Trace;
                if (m_NowState == BossState.Trace)
                {
                    MubAnimator.SetBool("Trace", true);
                    MubAnimator.SetBool("R_Attack", false);
                }
            }
        }
    }

    public void Attack()
    {
        if (m_NowState == BossState.Attack && Count == 0)
        {
            if (StageTwo == true)
            {
                if (RandomChooseCd == false)
                {
                    RandomChoose = UnityEngine.Random.Range(1, 4);
                    Debug.Log("隨機數:" + RandomChoose);
                    RandomChooseCd = true;
                }                
                if (RandomChoose == 1)
                {
                    MubAnimator.SetBool("TAttack01", true);
                    LookBool = false;
                }
                else if (RandomChoose == 2)
                {
                    MubAnimator.SetBool("TAttack02", true);
                    LookBool = false;
                }
                else if (RandomChoose == 3)
                {
                    MubAnimator.SetBool("TAttack03", true);
                    LookBool = false;
                }
            }
            else
            {
                if (RandomChooseCd == false)
                {
                    RandomChoose = UnityEngine.Random.Range(1, 4);
                    Debug.Log("隨機數:" + RandomChoose);
                    RandomChooseCd = true;
                }
                
                if (RandomChoose == 1)
                {
                    MubAnimator.SetBool("Attack01", true);
                    LookBool = false;
                }
                else if (RandomChoose == 2)
                {
                    MubAnimator.SetBool("Attack02", true);
                    LookBool = false;
                }
                else if (RandomChoose == 3)
                {
                    MubAnimator.SetBool("Attack03", true);
                    LookBool = false;
                }
            }
        }
        else if (Count != 0)
        {
            backing = TooCloseRange_RangedBattleRange(ATKRadius, MySelf, Target);
            if (backing == true)
            {
                m_NowState = BossState.Back;
                if (m_NowState == BossState.Back)
                {
                    MubAnimator.SetBool("Back", true);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                    MubAnimator.SetBool("TAttack01", false);
                    MubAnimator.SetBool("TAttack02", false);
                    MubAnimator.SetBool("TAttack03", false);
                }
            }
        }
    }
    public void Ultimate()
    {
        if (UCount <= 0)
        {
            MubAnimator.SetTrigger("Ulti");
            LookBool = false;
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
    //在攻擊範圍內 
    public bool IsInRange_BattleRange(float Radius, GameObject attacker, GameObject attacked)
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

    //範圍判定_追蹤
    public bool IsInRange_TraceRange(float Radius, GameObject attacker, GameObject attacked)
    {
        Vector3 direction = attacked.transform.position - attacker.transform.position;

        return direction.magnitude > Radius;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = InATKrange ? Color.red : Color.yellow;
        Gizmos.DrawWireSphere(transform.position, ATKRadius);

        Gizmos.color = InRangeATKrange ? Color.gray : Color.blue;
        Gizmos.DrawWireSphere(transform.position, RangedRadius);
    }
    private void Animation_Attack()
    {
        MubAnimator.speed = 2f;
        LookBool = false;
    }

    private void Animation_RangedAttack()
    {
        MubAnimator.speed = 0.1f;
        RotateSpeed /= 5;
    }
    private void AnimationSpeed_R_AttackEnd()
    {
        MubAnimator.speed = 1f;
        LookBool = true;
        RCount = 10;
        RotateSpeed *= 5;
        StartCoroutine(RangerCooldown());
    }
    private void AnimationSpeed_AttackEnd01R()
    {
        MubAnimator.speed = 1f;
        LookBool = true;
    }
    private void AnimationSpeed_AttackEnd01()
    {
        MubAnimator.speed = 1f;
        LookBool = true;
        Count = 1;
        StartCoroutine(AttackCooldown());
    }
    private void AnimationSpeed_AttackEnd02()
    {
        MubAnimator.speed = 1f;
        LookBool = true;
        Count = 3;
        StartCoroutine(AttackCooldown());
    }
    private void AnimationSpeed_TAttackEnd()
    {
        Count = 2;
        LookBool = true;
        StartCoroutine(AttackCooldown());
        MubAnimator.speed = 1f;
    }
    private void StartAim()
    {
        LookBool = true;
    }
    private void StageTwoEventSwitch()
    {
        //階段二特效
        MubAnimator.ResetTrigger("T2");
    }
    private void TeleportEvent()
    {
        float TeleoortChoose01 = (TeleportPoint01.transform.position - transform.position).magnitude;
        float TeleoortChoose02 = (TeleportPoint02.transform.position - transform.position).magnitude;
        if (TeleoortChoose01 > TeleoortChoose02)
        {
            MubAnimator.ResetTrigger("Ulti");
            transform.position = TeleportPoint01.transform.position;
            MubAnimator.SetBool("ChangeWeapon", true);
        }
        else
        {
            MubAnimator.ResetTrigger("Ulti");
            transform.position = TeleportPoint02.transform.position;
            MubAnimator.SetBool("ChangeWeapon", true);
        }
    }
    private void ChangeWeaponEvent()
    {       
        MubAnimator.SetBool("ChargeUp", true);
    }
    private void ChargeUpEvent_GetPower()
    {
        //續力特效
        MubAnimator.speed = 0.2f;
    }
    private void Animation_UltimateAttack()
    {
        MubAnimator.speed = 2f;
        LookBool = false;
    }
    private void ChargeUpEvent_Attack()
    {
        //攻擊特效
    }
    private void Animation_UltimateCoolDown()
    {
        MubAnimator.speed = 1f;
        MubAnimator.SetBool("ChargeUp", false);
        UCount = 80;
        StartCoroutine(UltimateCooldown());
    }
    private void GetHitReset()
    {
        GetHit = false;        
    }
    private void ChangeWeapon()
    {
        MubAnimator.SetBool("ChangeWeapon", false);
        if (BigSwordOnHand.activeInHierarchy == true)
        {
            BigSwordOnBack.SetActive(false);
        }
        else
        {
            BigSwordOnBack.SetActive(true);
            BigSwordOnHand.SetActive(false);            
        }
        if (AxeOnHand.activeInHierarchy == true)
        {
            AxeOnBack.SetActive(false);
        }
        else
        {
            AxeOnBack.SetActive(true);
            AxeOnHand.SetActive(false);
        }
    }
    IEnumerator AttackCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;
            Debug.Log(Count);
            if (Count == 1)
            {
                RandomChooseCd = false;
            }
        }
    }
    IEnumerator RangerCooldown()
    {
        while (RCount > 0)
        {
            yield return new WaitForSeconds(1);
            RCount--;
            Debug.Log(RCount);
        }
    }
    IEnumerator UltimateCooldown()
    {
        while (UCount > 0)
        {
            yield return new WaitForSeconds(1);
            UCount--;
            Debug.Log(UCount);
        }
    }
}
