using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum pFSMState
{
    NONE = -1,
    MoveTree,
    Roll,
    Attack,
    Dead,
    GetHit,
}

public class PlayerController : MonoBehaviour
{
    //攝影機
    Camera cameraMain;
    public LayerMask hitMask;

    //人物操控
    CharacterController cc;
    Animator charaterAnimator;
    [SerializeField] private float rotateSpeed = 14f;

    //操作開關，會根據玩家當前狀態開啟關閉
    bool isUseFire1 = true;
    bool isUseJump = true;
    private float attackMoveSpeed = 0;//攻擊位移速度
    bool isOpenAttackMove = false;//攻擊位移開關
    bool isUseMouseChangeForward = true;//滑鼠人物轉向開關
    public bool isInvincible = false;//翻滾無敵
    bool isInvincibleModeSwitch = false;
    [HideInInspector] public int currentLayerNum = 0;//當前Layer預設第0層
    //各層Layer的當前狀態
    AnimatorStateInfo animStateInfo;

    //手綁物品方塊
    public GameObject torchL;//綁火把左手
    public GameObject weaponL;//左手
    public GameObject weaponR;//右手

    //玩家數值(挖洞)
    PlayerHpData State;
    [SerializeField]int hpTemporary;
    int hpTemporaryMax;
    int RandomNum;
    bool isOpenBeHit = true;
    bool Reviving = false;

    public enum pAnimLayerState
    {
        Layer0,
        Layer1,
        Layer2
    }
    private pAnimLayerState m_pCurrentAnimLayer;

    
    private pFSMState m_pCurrentState;

    private void Awake()
    {
        m_pCurrentState = pFSMState.MoveTree;//預設狀態
        cameraMain = Camera.main;//攝影機要綁tag:MainCamera
        charaterAnimator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        animStateInfo = charaterAnimator.GetCurrentAnimatorStateInfo(currentLayerNum);//預設當前Layer(0)的當前動畫狀態
        charaterAnimator.SetLayerWeight(1, currentLayerNum);//Layer(0),BaseLayer
        charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);//動作權限預設啟用Layer(0)
        charaterAnimator.SetFloat("animSpeed", 1.5f);//預設速度1.5
        torchL.SetActive(true);
        weaponL.SetActive(false);
        weaponR.SetActive(false);

        State = GetComponent<PlayerHpData>();
        hpTemporary = State.Hp;
        
    }

    public void InvincibleMode()
    {
        if (Input.GetKey(KeyCode.F11))
        {
            isInvincible = true;
            isInvincibleModeSwitch = true;
            hpTemporaryMax = State.Hp;
        }
        if (Input.GetKey(KeyCode.F12))
        {
            isInvincible = false;
            isInvincibleModeSwitch = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        animStateInfo = charaterAnimator.GetCurrentAnimatorStateInfo(currentLayerNum);//當前Layer第Num層的當前動畫狀態
        InvincibleMode();
        //玩家Animator Layer狀態機
        if (m_pCurrentAnimLayer == pAnimLayerState.Layer0)
        {
            if (animStateInfo.IsName("BlendMove"))
            {
                m_pCurrentState = pFSMState.MoveTree;
            }
            charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);
            charaterAnimator.SetLayerWeight(1, 0);
            charaterAnimator.SetLayerWeight(2, 0);
        }
        if (m_pCurrentAnimLayer == pAnimLayerState.Layer1)
        {
            if (animStateInfo.IsName("BlendMoveSingleHand"))
            {
                m_pCurrentState = pFSMState.MoveTree;
            }
            charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);
            charaterAnimator.SetLayerWeight(1, 1);
            charaterAnimator.SetLayerWeight(2, 0);
        }
        if (m_pCurrentAnimLayer == pAnimLayerState.Layer2)
        {
            if (animStateInfo.IsName("BlendMove2Hands"))
            {
                m_pCurrentState = pFSMState.MoveTree;
            }
            charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);
            charaterAnimator.SetLayerWeight(1, 0);
            charaterAnimator.SetLayerWeight(2, 1);
        }
        if (isInvincibleModeSwitch == true)
        {
            State.Hp = hpTemporary;
            if (isInvincibleModeSwitch == true)
            {
                if (hpTemporary != hpTemporaryMax)
                {
                    hpTemporary++;
                    Reviving = true;
                }
                if (hpTemporary == hpTemporaryMax&&Reviving == true)
                {
                    charaterAnimator.SetTrigger("Revive");
                    Reviving = false; 
                    if (Reviving == false)
                    {
                        charaterAnimator.ResetTrigger("Revive");
                    }
                }
            }            
            if (m_pCurrentState == pFSMState.MoveTree)
            {
                isUseFire1 = true;
                isUseJump = true;
                isOpenAttackMove = false;//攻擊位移關閉，防呆
                charaterAnimator.SetFloat("animSpeed", 1.5f);
            }
            else if (m_pCurrentState == pFSMState.Roll)
            {
                isUseFire1 = false;//禁用滑鼠左鍵，禁止攻擊
            }
            else if (m_pCurrentState == pFSMState.Attack)
            {
                isUseJump = false;//禁用空白鍵，禁止翻滾
            }

            SwitchLayer(currentLayerNum);
            ControlSwitchWeapon();
            MousePosChangeForward(isUseMouseChangeForward);
            ControlMove(isUseJump);
            ControlAttack(isUseFire1);
        }
        else
        {
            //玩家狀態機
            if (State.Hp <= 0f)
            {
                m_pCurrentState = pFSMState.Dead;
                DeadStatus();
                return;
            }
            else if (State.Hp != hpTemporary && isInvincible == false)
            {
                if (hpTemporary - State.Hp < 50)
                {
                    hpTemporary = State.Hp;
                }
                else if (hpTemporary - State.Hp >= 50)
                {
                    if (isOpenBeHit == true)
                    {
                        RandomNum = Random.Range(1, 2);
                        isOpenBeHit = false;
                    }
                    hpTemporary = State.Hp;
                    if (RandomNum == 1)
                    {
                        charaterAnimator.SetBool("GetHit01", true);
                        //cc.SimpleMove(-(transform.forward * 500));
                    }
                    else if (RandomNum == 2)
                    {
                        charaterAnimator.SetBool("GetHit02", true);
                        //cc.SimpleMove(-(transform.forward * 500));
                    }
                }                               
            }
            else
            {
                charaterAnimator.SetBool("GetHit01", false);
                charaterAnimator.SetBool("GetHit02", false);
                if (m_pCurrentState == pFSMState.MoveTree)
                {
                    isUseFire1 = true;
                    isUseJump = true;
                    isOpenAttackMove = false;//攻擊位移關閉，防呆
                    charaterAnimator.SetFloat("animSpeed", 1.5f);
                }
                else if (m_pCurrentState == pFSMState.Roll)
                {
                    isUseFire1 = false;//禁用滑鼠左鍵，禁止攻擊
                }
                else if (m_pCurrentState == pFSMState.Attack)
                {
                    isUseJump = false;//禁用空白鍵，禁止翻滾
                }

                SwitchLayer(currentLayerNum);
                ControlSwitchWeapon();
                MousePosChangeForward(isUseMouseChangeForward);
                ControlMove(isUseJump);
                ControlAttack(isUseFire1);
            }
        }
        
    }


    /// <summary>
    /// 玩家角色的攻擊操控，目前暫定雙手武器Layer(1)
    /// </summary>
    /// <param name="isUseFire1">是否使用滑鼠輸入</param>
    private void ControlAttack(bool isUseFire1)
    {
        //滑鼠Input
        bool isMouseClickDown = Input.GetButtonDown("Fire1");
        bool isMouseClickDownR = Input.GetButtonDown("Fire2");

        //*******這裡可能會造成不同層Layer的矛盾，可能要分開或合併
        //Layer(0)的攻擊
        if (currentLayerNum == 0 &&!animStateInfo.IsName("BlendMove") && animStateInfo.normalizedTime > 0.8f)
        {
            // 每次設置完參數之後,都應該在下一幀開始時將參數設置清空,避免連續切換  
            charaterAnimator.SetInteger("intAttackID", 0);
            isOpenAttackMove = false;
        }
        //Layer(1)的攻擊
        if (currentLayerNum == 1&& !animStateInfo.IsName("BlendMoveSingleHand") && animStateInfo.normalizedTime > 0.8f)
        {
            // 每次設置完參數之後,都應該在下一幀開始時將參數設置清空,避免連續切換  
            charaterAnimator.SetInteger("intAttackID", 0);
            isOpenAttackMove = false;
        }
        //Layer(2)的攻擊
        if (currentLayerNum==2 && !animStateInfo.IsName("BlendMove2Hands") && animStateInfo.normalizedTime > 0.95f)
        {
            // 每次設置完參數之後,都應該在下一幀開始時將參數設置清空,避免連續切換  
            charaterAnimator.SetInteger("intAttackID", 0);
            isOpenAttackMove = false;
        }




        if (isMouseClickDown && isUseFire1)
        {
            m_pCurrentState = pFSMState.Attack;//進入攻擊狀態***
            charaterAnimator.SetInteger("intAttackID", 1);

            if (!charaterAnimator.IsInTransition(1))
            {
                charaterAnimator.SetTrigger("isTriggerAttack");
            }
        }
        else if (isMouseClickDownR && currentLayerNum == 1 && animStateInfo.IsName("BlendMoveSingleHand"))//滑鼠右鍵
        {
            m_pCurrentState = pFSMState.Attack;
            if (!charaterAnimator.IsInTransition(1))
            {
                charaterAnimator.SetTrigger("isTriggerAttack2");
            }
        }
        if(isMouseClickDownR && currentLayerNum == 2 && animStateInfo.IsName("BlendMove2Hands"))//滑鼠右鍵
        {
            m_pCurrentState = pFSMState.Attack;
            if (!charaterAnimator.IsInTransition(2))
            {
                charaterAnimator.SetTrigger("isTriggerAttack2");
            }
        }

        if (isOpenAttackMove)
        {
            cc.Move(transform.forward * attackMoveSpeed * Time.deltaTime);//攻擊位移
        }
    }

    /// <summary>
    /// 切換 m_pCurrentAnimLayer當前層級狀態
    /// </summary>
    /// <param name="layerNum">Layer層級數</param>
    private void SwitchLayer(int layerNum)
    {
        if (layerNum == 0)
        {
            m_pCurrentAnimLayer = pAnimLayerState.Layer0;
        }
        else if (layerNum == 1)
        {
            m_pCurrentAnimLayer = pAnimLayerState.Layer1;
        }
        else if (layerNum == 2)
        {
            m_pCurrentAnimLayer = pAnimLayerState.Layer2;
        }
    }

    /// <summary>
    /// 給WeaponManage用
    /// 判斷當前的右手武器類型
    /// 若玩家當前 layer==1 or layer==2，才去切換玩家Animator Layer狀態
    /// </summary>
    public void AutoSwitchWeaponR(GameObject currentWeaponR)
    {
        if (currentLayerNum == 1 || currentLayerNum == 2)
        {
            if (currentWeaponR.GetComponent<ItemOnWeapon>().weaponType == 2)
            {
                currentLayerNum = 1;
                charaterAnimator.SetTrigger("isTriggerLayerChange");
                StartCoroutine(ShowWeaponLWeaponR());
            }
            else if (currentWeaponR.GetComponent<ItemOnWeapon>().weaponType == 3)
            {
                currentLayerNum = 2;
                charaterAnimator.SetTrigger("isTriggerLayerChange");
                StartCoroutine(ShowWeaponR());
            }
        }
    }



    /// <summary>
    /// 切換武器模式Key1,2
    /// 同時切換currentLayerNum數值
    /// </summary>
    private void ControlSwitchWeapon()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentLayerNum = 0;
            charaterAnimator.SetTrigger("isTriggerLayerChange");

            StartCoroutine(ShowTorchL());
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            if (WeaponManager.Instance().CurrentWeaponR_weaponR)
            {
                if (WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponType == 2)
                {
                    currentLayerNum = 1;
                    charaterAnimator.SetTrigger("isTriggerLayerChange");
                    StartCoroutine(ShowWeaponLWeaponR());
                }
                else if (WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponType == 3)
                {
                    currentLayerNum = 2;
                    charaterAnimator.SetTrigger("isTriggerLayerChange");
                    StartCoroutine(ShowWeaponR());
                }
            }
            
        }
    }

    /// <summary>
    /// 顯示左手torchL
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowTorchL()
    {
        weaponR.SetActive(false);
        weaponL.SetActive(false);
        yield return new WaitForSeconds(0.2f);
        torchL.SetActive(true);
    }
    /// <summary>
    /// 顯示雙手weaponL,weaponR
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowWeaponLWeaponR()
    {
        torchL.SetActive(false);
        yield return new WaitForSeconds(0.1f);
        weaponR.SetActive(true);
        weaponL.SetActive(true);
    }
    /// <summary>
    /// 顯示右手weaponR
    /// </summary>
    /// <returns></returns>
    IEnumerator ShowWeaponR()
    {
        torchL.SetActive(false);
        weaponL.SetActive(false);
        yield return new WaitForSeconds(0.3f);
        weaponR.SetActive(true);
    }


    /// <summary>
    /// 玩家角色的移動操控
    /// 校正攝影機方向
    /// </summary>
    /// <param name="isUseJump">是否使用空白建輸入</param>
    private void ControlMove(bool isUseJump)
    {
        //鍵盤Input
        float fH = Input.GetAxis("Horizontal");
        float fV = Input.GetAxis("Vertical");
        bool isJump = Input.GetButtonDown("Jump");

        //上下、左右變量*Camera單位向量，得出修正的向量
        Vector3 cameraForward = cameraMain.transform.forward;
        Vector3 cameraRight = cameraMain.transform.right;
        cameraForward.y = 0;
        cameraRight.y = 0;
        cameraForward = cameraForward.normalized;
        cameraRight = cameraRight.normalized;
        Vector3 rightRelativeHorizontalInput = fH * cameraRight;
        Vector3 forwardRelativeVerticalInput = fV * cameraForward;
        Vector3 cameraRelativeMovement = rightRelativeHorizontalInput + forwardRelativeVerticalInput;

        if (fH != 0 || fV != 0)
        {
            //BlendTree動作計算
            float cos = Vector3.Dot(transform.forward, cameraRelativeMovement.normalized);
            float sin = Mathf.Sqrt(1 - cos * cos);
            Vector3 cross = Vector3.Cross(transform.forward, cameraRelativeMovement.normalized);
            if (cross.y < 0) sin *= -1;
            charaterAnimator.SetFloat("velocityV", cos);
            charaterAnimator.SetFloat("velocityH", sin);
            var aSI = charaterAnimator.GetCurrentAnimatorStateInfo(0);

            if (isJump && isUseJump)
            {
                m_pCurrentState = pFSMState.Roll;//進入翻滾狀態***
                isUseMouseChangeForward = false;
                transform.forward = cameraRelativeMovement;
                charaterAnimator.SetTrigger("isTriggerJump");
            }
        }
        else
        {
            charaterAnimator.SetFloat("velocityV", 0.0f);
            charaterAnimator.SetFloat("velocityH", 0.0f);
            if (isJump && isUseJump)
            {
                m_pCurrentState = pFSMState.Roll;//進入翻滾狀態***
                charaterAnimator.SetTrigger("isTriggerJump");
            }
        }
    }

    /// <summary>
    /// 滑鼠座標改變玩家forward方向
    /// </summary>
    /// <param name="isUseMouseChangeForward">是否開啟人物轉向</param>
    public void MousePosChangeForward(bool isUseMouseChangeForward)
    {
        if (isUseMouseChangeForward)
        {
            Vector3 mousePos = Input.mousePosition;
            Ray ray = cameraMain.ScreenPointToRay(mousePos);
            RaycastHit rh;

            if (Physics.Raycast(ray, out rh, 1000.0f, hitMask))
            {
                Vector3 vec = rh.point - transform.position;
                vec.y = 0.0f;
                Quaternion charaterRotaion = Quaternion.LookRotation(vec);
                transform.rotation = Quaternion.Slerp(transform.rotation, charaterRotaion, rotateSpeed * Time.deltaTime);
            }
        }
    }

    /// <summary>
    /// 翻滾完成後，開啟人物轉向
    /// </summary>
    private void AfterRollEvent()
    {
        isUseMouseChangeForward = true;
    }

    /// <summary>
    /// 開始攻擊位移事件
    /// </summary>
    private void AtkMoveEventStart()
    {
        isOpenAttackMove = true;
        attackMoveSpeed = 50f;
    }

    /// <summary>
    /// 結束攻擊位移事件
    /// </summary>
    private void AtkMoveEventEnd()
    {
        isOpenAttackMove = false;
        attackMoveSpeed = 0f;
    }

    private void AnimSpeedPlus()
    {
        charaterAnimator.SetFloat("animSpeed", 5f);
    }
    private void invincibleRoll_Start()
    {
        isInvincible = true;
    }
    private void invincibleRoll_End()
    {
        isInvincible = false;
    }
    public void DeadStatus()
    {
        if (m_pCurrentState == pFSMState.Dead)
        {
            charaterAnimator.SetTrigger("isTriggerDie");
            cc.radius = 0f;
        }
    }

    /// <summary>
    /// 開啟觸發人物受擊事件
    /// </summary>
    private void BeHitResetEvent()
    {
        isOpenBeHit = true;
    }
}
