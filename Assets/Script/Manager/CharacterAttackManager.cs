using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class CharacterAttackManager : MonoBehaviour
{
    RecoilShake recoilShake;//鏡頭震動
    public GameObject hitVFX;  //怪物受擊特效
    Vector3 up = new Vector3(0, 10, 0);//調高怪物受擊特效位置

    private bool isOpenOnDrawGizmos = false;//是否打開OnDrawGizmos
    Animator playerAnimator;
    
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        recoilShake = GetComponent<RecoilShake>();
        isOpenOnDrawGizmos = true;
    }

    /// <summary>
    /// 動畫減速事件
    /// </summary>
    public void SpeedMinusEvent()
    {
        playerAnimator.speed = 0.8f;
    }
    /// <summary>
    /// 動畫正常速度事件
    /// </summary>
    public void SpeedNormalEvent()
    {
        playerAnimator.speed = 1f;
    }
    /// <summary>
    /// 動畫加速事件
    /// </summary>
    public void SpeedPlusEvent()
    {
        playerAnimator.speed = 2f;
    }

    /// <summary>
    /// 左手火把攻擊事件，綁在左手火把攻擊動畫上
    /// </summary>
    public bool AttackEvent_left_torch()
    {
        int weaponDamage_instant = WeaponManager.Instance().CurrentTorchL_torch.GetComponent<ItemOnWeapon>().weaponDamage_instant;
        int weaponDamamge_delay = WeaponManager.Instance().CurrentTorchL_torch.GetComponent<ItemOnWeapon>().weaponDamage_delay;
        float angle = WeaponManager.Instance().CurrentTorchL_torch.GetComponent<ItemOnWeapon>().weaponAngle;
        float radius = WeaponManager.Instance().CurrentTorchL_torch.GetComponent<ItemOnWeapon>().weaponRadius;

        foreach (GameObject mob in GameManager.Instance().mobPool)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, mob.transform))
            {
                DeductMobHpInstant(mob, weaponDamage_instant);
                DeductMobHpDelay(mob, weaponDamamge_delay);
                Debug.LogWarning("Hit : " + mob.GetComponent<ItemOnMob>().mobName + "Hp : " + mob.GetComponent<MubHpData>().Hp);
            }
        }
        return true;
    }

    /// <summary>
    /// 左手攻擊事件，綁在左手攻擊動畫上
    /// </summary>
    private void AttackEvent_left()
    {
        int weaponDamage_instant = WeaponManager.Instance().CurrentWeaponL_weaponL.GetComponent<ItemOnWeapon>().weaponDamage_instant;
        int weaponDamamge_delay = WeaponManager.Instance().CurrentWeaponL_weaponL.GetComponent<ItemOnWeapon>().weaponDamage_delay;
        float angle = WeaponManager.Instance().CurrentWeaponL_weaponL.GetComponent<ItemOnWeapon>().weaponAngle;
        float radius = WeaponManager.Instance().CurrentWeaponL_weaponL.GetComponent<ItemOnWeapon>().weaponRadius;

        foreach (GameObject mob in GameManager.Instance().mobPool)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, mob.transform))
            {
                DeductMobHpInstant(mob, weaponDamage_instant);
                DeductMobHpDelay(mob, weaponDamamge_delay);
                DeductMobAnimatorDelay(mob);
                ParticleSystem ps = hitVFX.GetComponent<ParticleSystem>();
                Instantiate(ps, mob.transform.position+up, mob.transform.rotation);
                ps.Play();
                recoilShake.camraPlayerSake();
                Debug.LogWarning("Hit : " + mob.GetComponent<ItemOnMob>().mobName + "Hp : " + mob.GetComponent<MubHpData>().Hp);
            }
        }
    }

    /// <summary>
    /// 右手攻擊事件，綁在右手攻擊動畫上
    /// </summary>
    private void AttackEvent()
    {
        int weaponDamage_instant = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
        int weaponDamamge_delay = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
        float angle = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
        float radius = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponRadius;

        foreach (GameObject mob in GameManager.Instance().mobPool)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, mob.transform))
            {
                DeductMobHpInstant(mob, weaponDamage_instant);
                DeductMobHpDelay(mob, weaponDamamge_delay);
                ParticleSystem ps = hitVFX.GetComponent<ParticleSystem>();
                Instantiate(ps, mob.transform.position+up, mob.transform.rotation);
                ps.Play();
                Debug.LogWarning("Hit : " + mob.GetComponent<ItemOnMob>().mobName + "Hp : " + mob.GetComponent<MubHpData>().Hp);
            }
        }
    }

    /// <summary>
    /// 右手攻擊事件，綁在右手攻擊動畫上
    /// </summary>
    private void AttackEvent_bigRight()
    {
        int weaponDamage_instant = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponDamage_instant;
        int weaponDamamge_delay = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponDamage_delay;
        float angle = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
        float radius = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponRadius;

        foreach (GameObject mob in GameManager.Instance().mobPool)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, mob.transform))
            {
                DeductMobHpInstant(mob, weaponDamage_instant);
                DeductMobHpDelay(mob, weaponDamamge_delay);
                DeductMobAnimatorDelay(mob);
                ParticleSystem ps = hitVFX.GetComponent<ParticleSystem>();
                Instantiate(ps, mob.transform.position+up, mob.transform.rotation);
                ps.Play();
                recoilShake.camraPlayerSake();
                Debug.LogWarning("Hit : " + mob.GetComponent<ItemOnMob>().mobName + "Hp : " + mob.GetComponent<MubHpData>().Hp);
            }
        }
    }

    /// <summary>
    /// 單純打擊粒子特效
    /// </summary>
    private void AttackParticleEvent()
    {
        float angle = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
        float radius = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponRadius;

        foreach (GameObject mob in GameManager.Instance().mobPool)
        {
            Debug.Log("attack");
            if (IsInRange(angle, radius, gameObject.transform, mob.transform))
            {
                ParticleSystem ps = hitVFX.GetComponent<ParticleSystem>();
                Instantiate(ps, mob.transform.position+up, mob.transform.rotation);
                ps.Play();
            }
        }
    }


    /// <summary>
    /// 攻擊範圍判定
    /// </summary>
    /// <param name="sectorAngle">角度</param>
    /// <param name="sectorRadius">距離</param>
    /// <param name="attacker">攻擊者</param>
    /// <param name="attacked">被攻擊者</param>
    /// <returns></returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)//1209稍微修整一下變數名稱
    {
        Vector3 direction = attacked.position - attacker.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float mobRadius = attacked.GetComponent<CharacterController>().radius;
        return offsetAngle < sectorAngle * .7f && direction.magnitude - mobRadius < sectorRadius;
    }

    /// <summary>
    /// 扣怪物血_只算立即傷害
    /// </summary>
    /// <param name="mob">怪物</param>
    /// <param name="demage_instant">立即傷害</param>
    public void DeductMobHpInstant(GameObject mob, int demage_instant)
    {
        mob.GetComponent<MubHpData>().HpDeduction(demage_instant);
    }


    /// <summary>
    /// 扣怪物血_只算Debuff造成延遲傷害
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    public void DeductMobHpDelay(GameObject mob, int demage_delay)
    {
        StartCoroutine(DamageDelay(mob,demage_delay));
        Debug.LogWarning("持續扣血結束");
    }
    /// <summary>
    /// 延遲傷害_Debuff持續扣血
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    /// <returns></returns>
    IEnumerator DamageDelay(GameObject mob, int demage_delay)
    {
        int Count = 5;
        while (Count >= 0)
        {
            yield return new WaitForSeconds(1);
            mob.GetComponent<MubHpData>().HpDeduction(demage_delay);
            Count--;
        }
    }
    /// <summary>
    /// 怪物動作放慢
    /// </summary>
    public void DeductMobAnimatorDelay(GameObject mob)
    {
        StartCoroutine(AnimatorDelay(mob));
        Debug.LogWarning("放慢");
    }
    /// <summary>
    /// 怪物動作放慢0.5秒後恢復
    /// </summary>
    IEnumerator AnimatorDelay(GameObject mob)
    {
        this.gameObject.GetComponent<Animator>().speed = 0.1f;
        mob.GetComponent<Animator>().speed = 0.1f;
        yield return new WaitForSeconds(0.3f);
        mob.GetComponent<Animator>().speed = 1.0f;
        this.gameObject.GetComponent<Animator>().speed = 1.0f;
    }



    private void OnDrawGizmos()
    {
        if(isOpenOnDrawGizmos && WeaponManager.Instance().CurrentWeaponR_weaponR)
        {
            //右手武器攻擊半徑
            Gizmos.color = Color.red;
            float angleR = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponAngle;
            float radiusR = WeaponManager.Instance().CurrentWeaponR_weaponR.GetComponent<ItemOnWeapon>().weaponRadius;
            int segments = 100;
            float deltaAngle = angleR / segments;
            Vector3 forward = transform.forward;


            Vector3[] vertices = new Vector3[segments + 2];
            vertices[0] = transform.position;
            for (int i = 1; i < vertices.Length; i++)
            {
                Vector3 pos = Quaternion.Euler(0f, -angleR / 2 + deltaAngle * (i - 1), 0f) * forward * radiusR + transform.position;
                vertices[i] = pos;
            }
            for (int i = 1; i < vertices.Length - 1; i++)
            {
                Gizmos.DrawLine(vertices[i], vertices[i + 1]);
            }
            Gizmos.DrawLine(vertices[0], vertices[vertices.Length - 1]);
            Gizmos.DrawLine(vertices[0], vertices[1]);
        }

    }
}