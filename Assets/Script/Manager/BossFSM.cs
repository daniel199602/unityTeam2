using System.Collections;
using UnityEngine;

public enum BossState
{
    Idle, Trace, Attack, RangeAttack, GetHit, Dead,
}
public class BossFSM : MonoBehaviour
{
    private BossState m_NowState;

    [SerializeField] private GameObject Target;//存玩家
    private GameObject MySelf;//存自己

    [SerializeField] private GameObject TeleportPoint01;
    [SerializeField] private GameObject TeleportPoint02;

    public GameObject AxeOnHand;
    public GameObject BigSwordOnHand;
    public GameObject AxeOnBack;
    public GameObject BigSwordOnBack;
    public GameObject temp;

    MubHpData State;
    Animator MubAnimator;
    int TargetHp;
    int hpTemporary;
    int hpTemporaryMax;
    CharacterController capsule;

    bool StartBattle = false;
    bool LookBool;
    bool inRrangeBool;
    bool InATKrangeSwitch;
    bool backing;
    bool tracing;
    bool InRangeATKrange;
    bool RangeCDtodo;
    bool InATKrange;
    bool GetHit;
    bool StageTwo = false;
    bool Ulting = false;
    bool IK = false;//2階段單次判定，不會重複進入2階段

    float TraceRadius;
    float ATKRadius;
    float ATKRadius_norm;
    float ATKRadius_Atk;
    float RangedRadius;
    float RangedRadius_norm;

    float mobAngle;

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

    //噴火
    GameObject LeftArm;
    ParticleSystem Spilt_Fire;
    public GameObject HitFlame_R;
    //傳送
    GameObject Teleport;
    ParticleSystem magic_circle;
    public GameObject Teleport02;
    ParticleSystem magic_circle02;
    //大招
    GameObject UltimateFire;
    ParticleSystem Fire;
    public GameObject HitFlame;
    ParticleSystem FlameONground;
    //2階段
    GameObject Flame;
    ParticleSystem Roar;
    //刀光
    //public GameObject BLight;
    //ParticleSystem Bladelight;
    //頭
    public GameObject Head01;
    ParticleSystem Head01P;
    public GameObject Head02;
    ParticleSystem Head02P;
    // Start is called before the first frame update
    private void Awake()
    {
        ThisItemOnMob_State = GetComponent<ItemOnMob>();
        TeleportPoint01 = GameObject.Find("TeleportPoint01");
        TeleportPoint02 = GameObject.Find("TeleportPoint02");
        LeftArm = GameObject.Find("FlameThrower");
        Teleport = GameObject.Find("MagicRune3");
        UltimateFire = GameObject.Find("FlameEmission");
        Flame = GameObject.Find("Candle3");
    }
    void Start()
    {
        StartBattle = false;
        Target = GameManager.Instance().PlayerStart;//抓出玩家
        MySelf = this.transform.gameObject;//抓出自己
        TargetHp = Target.GetComponent<PlayerHpData>().Hp;
        capsule = GetComponent<CharacterController>();
        MubAnimator = GetComponent<Animator>();
        State = GetComponent<MubHpData>();

        //武器初始狀態
        BigSwordOnBack.SetActive(true);
        BigSwordOnHand.SetActive(false);
        AxeOnBack.SetActive(true);
        AxeOnHand.SetActive(false);

        //特效
        Spilt_Fire = LeftArm.GetComponent<ParticleSystem>();
        magic_circle = Teleport.GetComponent<ParticleSystem>();
        magic_circle02 = Teleport02.GetComponent<ParticleSystem>();
        Fire = UltimateFire.GetComponent<ParticleSystem>();
        Roar = Flame.GetComponent<ParticleSystem>();
        //Bladelight = BLight.GetComponent<ParticleSystem>();
        FlameONground = HitFlame.GetComponentInChildren<ParticleSystem>();
        Head01P = Head01.GetComponentInChildren<ParticleSystem>();
        Head02P = Head02.GetComponentInChildren<ParticleSystem>();

        //血量區
        hpTemporary = State.Hp;
        hpTemporaryMax = State.Hp;

        LookBool = true;
        RotateSpeed = 7f;

        //初始狀態
        m_NowState = BossState.Idle;

        //範圍設定
        ATKRadius = ThisItemOnMob_State.mobRadius;
        ATKRadius_norm = ThisItemOnMob_State.mobRadius;//Weapon覆蓋
        ATKRadius_Atk = ThisItemOnMob_State.mobRadius * 1.5f;
        RangedRadius = ATKRadius * 3f;
        RangedRadius_norm = ATKRadius * 3f;
        TraceRadius = ATKRadius * 4f;
        mobAngle = ThisItemOnMob_State.mobAngle;

        GetHit = false;
        inRrangeBool = false;
        InATKrangeSwitch = false;//近距離&遠距離切換判定

        //初始冷卻設定
        Count = 0;
        RCount = 0;
        UCount = 60;
    }

    // Update is called once per frame
    void Update()
    {
        TargetHp = Target.GetComponent<PlayerHpData>().Hp;
        if (StartBattle == true)
        {
            Vector3 pv = new Vector3(transform.position.x, 0f, transform.position.z);
            transform.position = pv;

            if (State.Hp <= 0)//死亡→無狀態
            {
                m_NowState = BossState.Dead;
                DeadStatus();
                return;
            }
            else if (State.Hp > 0)
            {
                if (TargetHp <= 1)
                {
                    CheckPlayerState();
                }
                else if ((float)hpTemporary / (float)hpTemporaryMax <= 0.5f && IK == false)
                {
                    StageTwo = true;
                    IK = true;
                    MubAnimator.SetBool("Trace", false);
                    MubAnimator.SetBool("Attack01", false);
                    MubAnimator.SetBool("Attack02", false);
                    MubAnimator.SetBool("Attack03", false);
                    MubAnimator.SetBool("R_Attack", false);
                    MubAnimator.SetBool("TAttack01", false);
                    MubAnimator.SetBool("TAttack02", false);
                    MubAnimator.SetBool("TAttack03", false);
                    StageTwoCost();
                }
                else if (UCount == 0)
                {
                    MubAnimator.SetBool("GetHit01", false);
                    MubAnimator.SetBool("GetHit02", false);
                    Ultimate();
                    Ulting = true;
                }
                else if (State.Hp != hpTemporary)
                {
                    hpTemporary = State.Hp;
                    if (Ulting == false)
                    {
                        if (hpTemporary - State.Hp < 50)
                        {
                            hpTemporary = State.Hp;
                        }
                        else if (hpTemporary - State.Hp >= 50)
                        {
                            if (GetHit == false && Count != 0 && RCount != 0)
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
                    }
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
            }

        }
    }
    public void DoorOpen()
    {
        MubAnimator.SetBool("DrawWeapon", true);
        StartBattle = true;
    }
    public void CheckPlayerState()
    {
        MubAnimator.SetBool("Trace", false);
        MubAnimator.SetBool("Attack01", false);
        MubAnimator.SetBool("Attack02", false);
        MubAnimator.SetBool("Attack03", false);
        MubAnimator.SetBool("R_Attack", false);
        MubAnimator.SetBool("TAttack01", false);
        MubAnimator.SetBool("TAttack02", false);
        MubAnimator.SetBool("TAttack03", false);
        MubAnimator.SetTrigger("Idle");
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
        if (LookBool == true)
        {
            GetTargetNormalize = (Target.transform.position - transform.position).normalized;

            Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);

            transform.rotation = Quaternion.Slerp(this.transform.rotation, Look, RotateSpeed * Time.deltaTime);

            Debug.Log("Looking");
        }
    }
    public void DeadStatus()
    {
        if (m_NowState == BossState.Dead)
        {
            MySelf.isStatic = true;
            MubAnimator.SetTrigger("Die");
            CheckPlayerState();
            magic_circle.Stop();
            Spilt_Fire.Stop();
            Fire.Stop();
            //Bladelight.Stop();
            Roar.Stop();
            Head01P.Stop();
            Head02P.Stop();
            LookBool = false;
            capsule.radius = 0f;
        }
    }
    //追擊狀態
    public void TraceStatus()
    {
        if (inRrangeBool == false)
        {
            tracing = IsInRange_TraceRange(RangedRadius, MySelf, Target);
            if (tracing == true)
            {
                LookBool = true;
                m_NowState = BossState.Trace;
                LookPoint();
                Debug.Log("ttttttttttttttttttttttttttttt");
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
                    MubAnimator.SetBool("TurnL", false);
                    MubAnimator.SetBool("TurnR", false);
                    MubAnimator.SetBool("TurnB", false);
                }
            }
        }
    }
    public void RangedAttackStatus()
    {
        if (InATKrangeSwitch == false && Ulting == false)
        {
            InRangeATKrange = IsInRange_RangedBattleRange(RangedRadius, MySelf, Target);
            if (InRangeATKrange == true)
            {
                RangedRadius = TraceRadius;
                m_NowState = BossState.RangeAttack;
                MubAnimator.SetBool("Trace", false);
                inRrangeBool = true;
                RangeAttack();
                Debug.Log("rrrrrr");
            }
            else
            {
                inRrangeBool = false;
                RangedRadius = RangedRadius_norm;
            }
        }
    }
    //攻擊狀態
    public void AttackStatus()
    {
        if (Ulting == false)
        {
            InATKrange = IsInRange_BattleRange(ATKRadius, MySelf, Target);
            if (InATKrange == true)
            {
                InATKrangeSwitch = true;
                ATKRadius = ATKRadius_Atk;
                m_NowState = BossState.Attack;
                MubAnimator.SetBool("Trace", false);
                MubAnimator.SetBool("Back", false);
                MubAnimator.SetBool("R_Attack", false);
                Attack();
                Debug.Log("aaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaaa");
            }
            else
            {
                ATKRadius = ATKRadius_norm;
                InATKrangeSwitch = false;
            }
        }
    }

    public void RangeAttack()
    {
        if (m_NowState == BossState.RangeAttack)
        {
            if (RCount == 0)
            {
                LookBool = true;
                LookPoint();
                MubAnimator.SetBool("R_Attack", true);
                RCount = 12;
                StartCoroutine(RangerCooldown());
            }
            else if (RCount != 0)
            {
                RangeCDtodo = IsInRange_TraceRange(ATKRadius, MySelf, Target);
                if (RangeCDtodo == true)
                {
                    LookBool = true;
                    LookPoint();
                    Debug.Log("tttttttttttttttttttttttttttttrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrrr");
                    MubAnimator.SetBool("Trace", true);
                    MubAnimator.SetBool("R_Attack", false);
                    MubAnimator.SetBool("TurnL", false);
                    MubAnimator.SetBool("TurnR", false);
                    MubAnimator.SetBool("TurnB", false);
                }
            }
        }
    }

    public void Attack()
    {
        if (m_NowState == BossState.Attack)
        {
            if (Count == 0)
            {
                Vector3 direction = Target.transform.position - transform.position;

                float dot = Vector3.Dot(direction.normalized, transform.forward);

                float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (offsetAngle > (mobAngle * .7f)/3*2)
                {
                    MubAnimator.speed = 1.5f;
                    Vector3 MyF = transform.forward;
                    Vector3 MyR = transform.right;
                    Vector3 MT = Target.transform.position - transform.position;
                    Vector3 MTN = (Target.transform.position - transform.position).normalized;
                    Vector3 MXTFB = Vector3.Cross(MyR, MTN);
                    Vector3 MXTLR = Vector3.Cross(MyF, MT);
                    if (MXTFB.y > 0)//不會向後轉
                    {
                        MubAnimator.SetBool("TurnB", true);
                        Debug.Log("TurnBack");
                    }
                    else if (MXTLR.y < 0)//左轉
                    {
                        MubAnimator.SetBool("TurnL", true);
                        Debug.Log("TurnLeft");
                    }
                    else if (MXTLR.y > 0)//右轉
                    {
                        MubAnimator.SetBool("TurnR", true);
                        Debug.Log("TurnRight");
                    }
                }
                else if(offsetAngle <= (mobAngle * .7f) / 3 * 2)
                {
                    MubAnimator.SetBool("TurnL", false);
                    MubAnimator.SetBool("TurnR", false);
                    MubAnimator.SetBool("TurnB", false);
                    MubAnimator.speed = 1f;
                    LookPoint();
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
                            Count = 3;
                            StartCoroutine(AttackCooldown());
                        }
                        else if (RandomChoose == 2)
                        {
                            MubAnimator.SetBool("TAttack02", true);
                            Count = 3;
                            StartCoroutine(AttackCooldown());
                        }
                        else if (RandomChoose == 3)
                        {
                            MubAnimator.SetBool("TAttack03", true);
                            Count = 3;
                            StartCoroutine(AttackCooldown());
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
                            Count = 2;
                            StartCoroutine(AttackCooldown());
                        }
                        else if (RandomChoose == 2)
                        {
                            MubAnimator.SetBool("Attack02", true);
                            Count = 3;
                            StartCoroutine(AttackCooldown());
                        }
                        else if (RandomChoose == 3)
                        {
                            MubAnimator.SetBool("Attack03", true);
                            Count = 3;
                            StartCoroutine(AttackCooldown());
                        }
                    }
                }
            }
            else if (Count != 0)
            {
                MubAnimator.SetBool("Attack01", false);
                MubAnimator.SetBool("Attack02", false);
                MubAnimator.SetBool("Attack03", false);
                MubAnimator.SetBool("TAttack01", false);
                MubAnimator.SetBool("TAttack02", false);
                MubAnimator.SetBool("TAttack03", false);
                Vector3 direction = Target.transform.position - transform.position;

                float dot = Vector3.Dot(direction.normalized, transform.forward);

                float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
                if (offsetAngle > mobAngle)
                {
                    Vector3 MyF = transform.forward;
                    Vector3 MyR = transform.right;
                    Vector3 MT = Target.transform.position - transform.position;
                    Vector3 MTN = (Target.transform.position - transform.position).normalized;
                    Vector3 MXTFB = Vector3.Cross(MyR, MTN);
                    Vector3 MXTLR = Vector3.Cross(MyF, MT);
                    if (MXTFB.y > 180)//不會向後轉
                    {
                        MubAnimator.SetBool("TurnB", true);
                        Debug.Log("TurnBack");
                    }
                    else if (MXTLR.y < 0)//左轉
                    {
                        MubAnimator.SetBool("TurnL", true);
                        Debug.Log("TurnLeft");
                    }
                    else if (MXTLR.y > 0)//右轉
                    {
                        MubAnimator.SetBool("TurnR", true);
                        Debug.Log("TurnRight");
                    }                   
                }
                else//平行 
                {
                    MubAnimator.SetBool("TurnL", false);
                    MubAnimator.SetBool("TurnR", false);
                    MubAnimator.SetBool("TurnB", false);
                }
            }
        }
    }
    public void Ultimate()
    {
        Debug.LogWarning("大招");
        MubAnimator.SetTrigger("Ulti");
        MubAnimator.SetBool("Attack01", false);
        MubAnimator.SetBool("Attack02", false);
        MubAnimator.SetBool("Attack03", false);
        MubAnimator.SetBool("TAttack01", false);
        MubAnimator.SetBool("TAttack02", false);
        MubAnimator.SetBool("TAttack03", false);
        MubAnimator.SetBool("R_Attack", false);
        LookPoint();
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

        Gizmos.color = Color.white;
        Gizmos.DrawLine(MySelf.transform.position, MySelf.transform.forward);

        Gizmos.color = Color.green;
        Vector3 direction = Target.transform.position - transform.position;
        Gizmos.DrawLine(transform.position, direction);
    }
    private void Animation_Attack()
    {
        MubAnimator.speed = 2f;
        //BLight.transform.rotation = AxeOnHand.transform.rotation;
        //Bladelight.Play();
    }

    private void Animation_RangedAttack()
    {
        MubAnimator.speed = 0.4f;
        RotateSpeed /= 5;
        Spilt_Fire.Play();
        Quaternion QQ = new Quaternion(transform.rotation.x, transform.rotation.y - 45, transform.rotation.z, transform.rotation.w);
        Debug.LogWarning(Quaternion.Euler(MySelf.transform.rotation.x, MySelf.transform.rotation.y - 45, MySelf.transform.rotation.z));
        Debug.LogWarning(QQ);
        
    }
    private void AnimationR_s()
    {
        Spilt_Fire.Stop();
        Instantiate(HitFlame_R, MySelf.transform.position, MySelf.transform.rotation);
    }
    private void AnimationSpeed_R_AttackEnd()
    {
        MubAnimator.speed = 1f;
        RotateSpeed *= 5;
    }

    private void AnimationSpeed_AttackEnd()
    {
        MubAnimator.speed = 1f;

    }
    private void StartAim()
    {
        LookBool = true;
        LookPoint();
    }
    private void StageTwoEventSwitch()
    {
        Roar.Play();
        MubAnimator.ResetTrigger("T2");
    }
    private void StageTwoEventSwitch_s()
    {
        Roar.Stop();
    }
    private void TeleportEvent()
    {
        float TeleoortChoose01 = (TeleportPoint01.transform.position - transform.position).magnitude;
        float TeleoortChoose02 = (TeleportPoint02.transform.position - transform.position).magnitude;
        if (TeleoortChoose01 > TeleoortChoose02)
        {
            MubAnimator.ResetTrigger("Ulti");
            MubAnimator.applyRootMotion = false;
            Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);
            transform.rotation = Look;
            magic_circle02.Stop();
            transform.position = TeleportPoint01.transform.position;
            magic_circle.Stop();
            MubAnimator.SetBool("ChangeWeapon", true);
        }
        else
        {
            MubAnimator.ResetTrigger("Ulti");
            MubAnimator.applyRootMotion = false;
            Quaternion Look = Quaternion.LookRotation(GetTargetNormalize);
            transform.rotation = Look;
            magic_circle02.Stop();
            transform.position = TeleportPoint02.transform.position;
            magic_circle.Stop();
            MubAnimator.SetBool("ChangeWeapon", true);
        }
    }
    private void OpenAnimationMove()
    {
        MubAnimator.applyRootMotion = true;
    }
    private void ChangeWeaponEvent()
    {
        MubAnimator.SetBool("ChargeUp", true);
    }
    private void ChargeUpEvent_GetPower()
    {
        Fire.Play();
        MubAnimator.speed = 0.2f;
    }
    private void Animation_UltimateAttack()
    {
        MubAnimator.speed = 2f;
        LookBool = false;
    }
    private void ChargeUpEvent_Attack()
    {
        Instantiate(HitFlame, MySelf.transform.position, MySelf.transform.rotation);
    }
    private void Animation_UltimateCoolDown()
    {
        MubAnimator.speed = 1f;
        MubAnimator.SetBool("ChargeUp", false);
        MubAnimator.ResetTrigger("Ulti");
        LookBool = true;
        Ulting = false;
        UCount = 80;
        StartCoroutine(UltimateCooldown());
    }
    private void UltiPatricle_End()
    {
        Fire.Stop();
    }
    private void GetHitReset()
    {
        GetHit = false;
    }
    private void ReadyBattle()
    {
        BigSwordOnBack.SetActive(true);
        BigSwordOnHand.SetActive(false);
        AxeOnBack.SetActive(false);
        AxeOnHand.SetActive(true);
    }
    private void ChangeWeapon()
    {
        MubAnimator.SetBool("ChangeWeapon", false);
        LookBool = false;
        if (AxeOnHand.activeInHierarchy == true)
        {
            AxeOnBack.SetActive(true);
            AxeOnHand.SetActive(false);
        }
        else
        {
            BigSwordOnBack.SetActive(false);
            BigSwordOnHand.SetActive(true);
            AxeOnBack.SetActive(false);
            AxeOnHand.SetActive(true);
        }
        if (BigSwordOnHand.activeInHierarchy == true)
        {
            BigSwordOnBack.SetActive(true);
            BigSwordOnHand.SetActive(false);
        }
        else
        {
            BigSwordOnBack.SetActive(false);
            BigSwordOnHand.SetActive(true);
        }
    }
    private void WeaponOnHand()
    {
        MubAnimator.SetBool("DrawWeapon", false);
    }
    private void WeaponOnBack_E()
    {
        BigSwordOnBack.SetActive(true);
        BigSwordOnHand.SetActive(false);
        AxeOnBack.SetActive(true);
        AxeOnHand.SetActive(false);
    }
    private void Teleport_Start()
    {
        float TeleoortChoose01 = (TeleportPoint01.transform.position - transform.position).magnitude;
        float TeleoortChoose02 = (TeleportPoint02.transform.position - transform.position).magnitude;
        if (TeleoortChoose01 > TeleoortChoose02)
        {
            magic_circle.Play();
            Vector3 tt = new Vector3(TeleportPoint01.transform.position.x, TeleportPoint01.transform.position.y + 1, TeleportPoint01.transform.position.z);
            Instantiate(Teleport02, tt, Quaternion.identity);
            magic_circle02.Play();
        }
        else
        {
            magic_circle.Play();
            Vector3 tt = new Vector3(TeleportPoint02.transform.position.x, TeleportPoint02.transform.position.y + 1, TeleportPoint02.transform.position.z);
            Instantiate(Teleport02, tt, Quaternion.identity);
            magic_circle02.Play();
        }

    }
    IEnumerator AttackCooldown()
    {
        while (Count > 0)
        {
            yield return new WaitForSeconds(1);
            Count--;
            Debug.Log("攻擊CD:" + Count);
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
            Debug.Log("遠程CD:" + RCount);
        }
    }
    IEnumerator UltimateCooldown()
    {
        while (UCount > 0)
        {
            yield return new WaitForSeconds(1);
            UCount--;
            Debug.Log("大招CD:" + UCount);
        }
    }
}
