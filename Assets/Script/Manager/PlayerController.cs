using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    [HideInInspector] public int currentLayerNum = 0;//當前Layer預設第0層
    //各層Layer的當前狀態
    AnimatorStateInfo animStateInfo;

    //手綁物品方塊
    public GameObject torchL;//綁火把左手
    public GameObject weaponL;//左手
    public GameObject weaponR;//右手

    /*武器，先暫時直接用，之後改抓武器Data*/
    //盾牌
    public GameObject Shield_Basic;
    //單手武器
    public GameObject w_Sword4_Yellow;
    //雙手劍
    public GameObject w_Sword_4;
    

    //keyCode.Alpha2 暫時切換 劍盾 與 雙手劍 bool變數，之後改再刪除
    bool alpha2Switch = false;


    public enum pAnimLayerState
    {
        Layer0,
        Layer1,
        Layer2
    }
    private pAnimLayerState m_pCurrentAnimLayer;

    public enum pFSMState
    {
        NONE=-1,
        MoveTree,
        Roll,
        Attack,
        Dead
    }
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
    }

    

    // Update is called once per frame
    void Update()
    {
        animStateInfo = charaterAnimator.GetCurrentAnimatorStateInfo(currentLayerNum);//當前Layer第Num層的當前動畫狀態

        //玩家Animator Layer狀態機
        if (m_pCurrentAnimLayer==pAnimLayerState.Layer0)
        {
            if (animStateInfo.IsName("BlendMove"))
            {
                m_pCurrentState = pFSMState.MoveTree;
            }
            charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);
            charaterAnimator.SetLayerWeight(1, 0);
            charaterAnimator.SetLayerWeight(2, 0);
        }
        if(m_pCurrentAnimLayer==pAnimLayerState.Layer1)
        {
            if (animStateInfo.IsName("BlendMoveSingleHand"))
            {
                m_pCurrentState = pFSMState.MoveTree;
            }
            charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);
            charaterAnimator.SetLayerWeight(1, 1);
            charaterAnimator.SetLayerWeight(2, 0);
        }
        if(m_pCurrentAnimLayer==pAnimLayerState.Layer2)
        {
            if (animStateInfo.IsName("BlendMove2Hands"))
            {
                m_pCurrentState = pFSMState.MoveTree;
            }
            charaterAnimator.SetInteger("intCurrentLayer", currentLayerNum);
            charaterAnimator.SetLayerWeight(1, 0);
            charaterAnimator.SetLayerWeight(2, 1);
        }

        //玩家狀態機
        if (m_pCurrentState==pFSMState.MoveTree)
        {
            isUseFire1 = true;
            isUseJump = true;
            isOpenAttackMove = false;//攻擊位移關閉，防呆
            charaterAnimator.SetFloat("animSpeed", 1.5f);
        }
        else if(m_pCurrentState==pFSMState.Roll)
        {
            isUseFire1 = false;//禁用滑鼠左鍵，禁止攻擊
        }
        else if(m_pCurrentState==pFSMState.Attack)
        {
            isUseJump = false;//禁用空白鍵，禁止翻滾
        }
        else if(m_pCurrentState==pFSMState.Dead)
        {
            //死亡
        }

        SwitchLayer(currentLayerNum);
        ControlSwitchWeapon();
        MousePosChangeForward(isUseMouseChangeForward);
        ControlMove(isUseJump);
        ControlAttack(isUseFire1);
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
        if (!animStateInfo.IsName("BlendMove") && animStateInfo.normalizedTime > 0.8f)
        {
            // 每次設置完參數之後,都應該在下一幀開始時將參數設置清空,避免連續切換  
            charaterAnimator.SetInteger("intAttackID", 0);
            isOpenAttackMove = false;
        }
        //Layer(1)的攻擊
        if (!animStateInfo.IsName("BlendMoveSingleHand") && animStateInfo.normalizedTime > 1f)
        {
            // 每次設置完參數之後,都應該在下一幀開始時將參數設置清空,避免連續切換  
            charaterAnimator.SetInteger("intAttackID", 0);
            isOpenAttackMove = false;
        }
        //Layer(2)的攻擊
        if (!animStateInfo.IsName("BlendMove2Hands") && animStateInfo.normalizedTime > 1f)
        {
            // 每次設置完參數之後,都應該在下一幀開始時將參數設置清空,避免連續切換  
            charaterAnimator.SetInteger("intAttackID", 0);
            isOpenAttackMove = false;
        }




        if (isMouseClickDown&& isUseFire1)
        {
            m_pCurrentState = pFSMState.Attack;//進入攻擊狀態***
            charaterAnimator.SetInteger("intAttackID", 1);

            if (!charaterAnimator.IsInTransition(1))
            {
                charaterAnimator.SetTrigger("isTriggerAttack");
            }
        }
        else if (isMouseClickDownR && currentLayerNum==1&& animStateInfo.IsName("BlendMoveSingleHand"))//滑鼠右鍵
        {
            m_pCurrentState = pFSMState.Attack;
            if (!charaterAnimator.IsInTransition(1))
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
    /// 切換武器模式Key1,2,3,Tab
    /// 同時切換currentLayerNum數值
    /// </summary>
    private void ControlSwitchWeapon()
    {
        //if(Input.GetKeyDown(KeyCode.Tab))
        //{
        //    currentLayerNum++;
        //    if(currentLayerNum>2)currentLayerNum = 0;
        //    charaterAnimator.SetTrigger("isTriggerLayerChange");

        //    JudegShowWeapon();//暫時使用之後武器儲存方式再改
        //}

        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            currentLayerNum = 0;
            charaterAnimator.SetTrigger("isTriggerLayerChange");

            StartCoroutine(ShowTorchL());
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //keyCode.Alpha2 暫時切換 劍盾 與 雙手劍 bool變數
            alpha2Switch = !alpha2Switch;

            if (/*之後依據武器的type切換成 劍、盾*/alpha2Switch)
            {
                currentLayerNum = 1;
                charaterAnimator.SetTrigger("isTriggerLayerChange");

                //武器之後要想要怎麼存，這裡先硬寫
                w_Sword4_Yellow.SetActive(true);
                w_Sword_4.SetActive(false);
                Shield_Basic.SetActive(true);//換盾牌

                StartCoroutine(ShowWeaponLWeaponR());
            }
            else /*之後依據武器的type切換成 雙手劍*/
            {
                currentLayerNum = 2;
                charaterAnimator.SetTrigger("isTriggerLayerChange");
                //武器之後要想要怎麼存，這裡先硬寫
                w_Sword4_Yellow.SetActive(false);
                w_Sword_4.SetActive(true);

                StartCoroutine(ShowWeaponR());
            }
        }

    }


    /// <summary>
    /// 暫時使用，之後武器儲存方式再改
    /// </summary>
    //void JudegShowWeapon()
    //{
    //    if (currentLayerNum == 0)
    //    {
    //        StartCoroutine(ShowTorchL());
    //    }
    //    else if(currentLayerNum == 1)
    //    {
    //        StartCoroutine(ShowWeaponLWeaponR());
    //    }
    //    else if(currentLayerNum == 2)
    //    {
    //        StartCoroutine(ShowWeaponR());
    //    }
    //}

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

            if (isJump&& isUseJump)
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
            if (isJump&& isUseJump)
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
}
