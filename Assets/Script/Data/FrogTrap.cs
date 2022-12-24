using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTrap : MonoBehaviour
{
    /// <summary>
    /// �����쪱�a
    /// </summary>
    public LayerMask HitPlayer;
    /// <summary>
    /// �C�쪺�r
    /// </summary>
    public GameObject trap;
    public GameObject prepare;
    bool Spilt =false;

    [HideInInspector] public int mobDamage_instant;
    [HideInInspector] public int mobDamamge_delay;
    [HideInInspector] public float radius;
    [HideInInspector] public float angle;
    [SerializeField] private GameObject Target;
    PlayerHpData PlayerData;
    int TimeLine;

    AudioEvent_frogTrap frogTrap;

    void Start()
    {
        frogTrap = this.gameObject.GetComponentInParent<AudioEvent_frogTrap>();//����˪��}��
        mobDamage_instant = GetComponent<ItemOnMob>().mobDamage_instant;
        mobDamamge_delay = GetComponent<ItemOnMob>().mobDamage_delay;
        radius = GetComponent<ItemOnMob>().mobRadius;
        angle = GetComponent<ItemOnMob>().mobAngle;
        Target = GameManager.Instance().PlayerStart;
        PlayerData = Target.GetComponent<PlayerHpData>();
    }

    void FixedUpdate()
    {
        //�C��e100���Z���Y�����쪱�a����C�쪺�r
        Vector3 mePos =this.transform.right;
        Ray r = new Ray(this.transform.position, mePos);
        RaycastHit rh;
        if (Physics.Raycast(r, out rh, radius, HitPlayer))
        {
            ParticleSystem ps = prepare.GetComponent<ParticleSystem>();
            ps.Play();

            frogTrap.PlayFrogSpitFireEvent();//����C��Q������

            TimeLine = 3;
            if (Spilt == false)
            {
                StartCoroutine(FrogAttack());
                StartCoroutine(cc());
                Spilt = true;
            }
            
        }
        else
        {
            Spilt = false;
            CancelInvoke("AttackEvent_Normal");
        }
    }

    IEnumerator cc()
    {
        while (TimeLine > 0)
        {
            yield return new WaitForSeconds(1);
            TimeLine--;
        }
        if (TimeLine <= 0)
        {
            ParticleSystem ps = trap.GetComponent<ParticleSystem>();
            ps.Stop();
            ParticleSystem pc = prepare.GetComponent<ParticleSystem>();
            pc.Stop();
            CancelInvoke("AttackEvent_Normal");
        }
    }

    IEnumerator FrogAttack()
    {
        yield return new WaitForSeconds(1);
        ParticleSystem ps = trap.GetComponent<ParticleSystem>();
        ps.Play();
        InvokeRepeating("AttackEvent_Normal",0,1);
    }

    private void AttackEvent_Normal()
    {
        bool flag;
        flag = IsInRange(angle, radius, gameObject.transform, Target.transform);

        if (flag)
        {
            DeductMobHpInstant(Target, mobDamage_instant);
            DeductMobHpDelay(Target, mobDamamge_delay);
        }
    }
    /// <summary>
    /// ���Ǫ���_�u��ߧY�ˮ`
    /// </summary>
    /// <param name="mob">�Ǫ�</param>
    /// <param name="demage_instant">�ߧY�ˮ`</param>
    public void DeductMobHpInstant(GameObject mob, int demage_instant)
    {
        mob.GetComponent<PlayerHpData>().HpDeduction(demage_instant);
    }

    /// <summary>
    /// ���Ǫ���_�u��Debuff�y������ˮ`
    /// </summary>
    /// <param name="mob"></param>
    /// <param name="demage_delay"></param>
    public void DeductMobHpDelay(GameObject mob, int demage_delay)
    {
        StartCoroutine(DamageDelay(mob, demage_delay));
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
            mob.GetComponent<PlayerHpData>().HpDeduction(demage_delay);
            Count--;
        }
    }

    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)
    {
        Vector3 direction = attacked.position - attacker.position;

        float dot = Vector3.Dot(direction.normalized, transform.right);

        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;

        return offsetAngle < sectorAngle * .7f && direction.magnitude < sectorRadius;
    }

    /// <summary>
    /// ���սu(�C��Z��)
    /// </summary>
    private void OnDrawGizmos()
    {
        float fDetectLengh = radius;
        Vector3 vec = transform.right;
        Vector3 vpos = transform.position;
        Gizmos.DrawLine(vpos, vpos + vec * fDetectLengh);
        Gizmos.color = Color.blue;

        Gizmos.color = Color.red;
        float angleR = angle;
        float radiusR = radius;
        int segments = 100;
        float deltaAngle = angleR / segments;
        Vector3 forward = transform.right;


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
