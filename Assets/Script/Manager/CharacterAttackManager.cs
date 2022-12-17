using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using Unity.VisualScripting;
using System.Collections;

public class CharacterAttackManager : MonoBehaviour
{
    RecoilShake recoilShake;//���Y�_��
    public GameObject hitVFX;  //�Ǫ������S��
    Vector3 up = new Vector3(0, 10, 0);//�հ��Ǫ������S�Ħ�m

    private bool isOpenOnDrawGizmos = false;//�O�_���}OnDrawGizmos
    Animator playerAnimator;
    
    private void Start()
    {
        playerAnimator = GetComponent<Animator>();
        recoilShake = GetComponent<RecoilShake>();
        isOpenOnDrawGizmos = true;
    }

    /// <summary>
    /// �ʵe��t�ƥ�
    /// </summary>
    public void SpeedMinusEvent()
    {
        playerAnimator.speed = 0.8f;
    }
    /// <summary>
    /// �ʵe���`�t�רƥ�
    /// </summary>
    public void SpeedNormalEvent()
    {
        playerAnimator.speed = 1f;
    }
    /// <summary>
    /// �ʵe�[�t�ƥ�
    /// </summary>
    public void SpeedPlusEvent()
    {
        playerAnimator.speed = 2f;
    }

    /// <summary>
    /// �����������ƥ�A�j�b�����������ʵe�W
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
    /// ��������ƥ�A�j�b��������ʵe�W
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
    /// �k������ƥ�A�j�b�k������ʵe�W
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
    /// �k������ƥ�A�j�b�k������ʵe�W
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
    /// ��¥����ɤl�S��
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
    /// �����d��P�w
    /// </summary>
    /// <param name="sectorAngle">����</param>
    /// <param name="sectorRadius">�Z��</param>
    /// <param name="attacker">������</param>
    /// <param name="attacked">�Q������</param>
    /// <returns></returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)//1209�y�L�׾�@�U�ܼƦW��
    {
        Vector3 direction = attacked.position - attacker.position;
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        float mobRadius = attacked.GetComponent<CharacterController>().radius;
        return offsetAngle < sectorAngle * .7f && direction.magnitude - mobRadius < sectorRadius;
    }

    /// <summary>
    /// ���Ǫ���_�u��ߧY�ˮ`
    /// </summary>
    /// <param name="mob">�Ǫ�</param>
    /// <param name="demage_instant">�ߧY�ˮ`</param>
    public void DeductMobHpInstant(GameObject mob, int demage_instant)
    {
        mob.GetComponent<MubHpData>().HpDeduction(demage_instant);
    }


    /// <summary>
    /// ���Ǫ���_�u��Debuff�y������ˮ`
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    public void DeductMobHpDelay(GameObject mob, int demage_delay)
    {
        StartCoroutine(DamageDelay(mob,demage_delay));
        Debug.LogWarning("���򦩦嵲��");
    }
    /// <summary>
    /// ����ˮ`_Debuff���򦩦�
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
    /// �Ǫ��ʧ@��C
    /// </summary>
    public void DeductMobAnimatorDelay(GameObject mob)
    {
        StartCoroutine(AnimatorDelay(mob));
        Debug.LogWarning("��C");
    }
    /// <summary>
    /// �Ǫ��ʧ@��C0.5����_
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
            //�k��Z�������b�|
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