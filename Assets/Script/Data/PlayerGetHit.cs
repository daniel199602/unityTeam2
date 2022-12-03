using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGetHit : MonoBehaviour
{
    int Weapondamage_Instant = -70;
    int Weapondamamge_Delay = 0;
    int Count;
    CharacterAttackManager fooHp;
    private void Start()
    {
        fooHp = GetComponent<CharacterAttackManager>();
        //Weapondamage_Instant = fooHp.Weapondamage_Instant;
        //Weapondamamge_Delay = fooHp.Weapondamamge_Delay;
        Count = 0;
    }
    public int GetHitByOther(int i)
    {
        GetHitType_Damage(i);
        Debug.Log("�ǤJ��" + i);
        return i;
    }
    public int GetHitType_Damage(int Type)
    {
        Debug.Log("�ˮ`����" + Type);
        switch (Type)
        {
            case 1:
                AttackWithDebuff();
                break;
            case 0:
                AttackWithoutDebuff();
                break;
        }
        return Type;
    }
    public void AttackWithDebuff()
    {
        fooHp.fHp -= Weapondamage_Instant;
        StartCoroutine(DamageDelay());
        Debug.Log("HP��ֶq" + fooHp.fHp);
        Debug.Log("�ˮ`�q" + Weapondamage_Instant);
    }

    IEnumerator DamageDelay()
    {
        Count = 5;
        while (Count >= 0)
        {
            yield return new WaitForSeconds(1);
            fooHp.fHp -= Weapondamamge_Delay;
            Count--;
        }
    }
    public void AttackWithoutDebuff()
    {
        fooHp.fHp -= Weapondamage_Instant;
        Debug.Log("HP��ֶq" + fooHp.fHp);
        Debug.Log("�ˮ`�q" + Weapondamage_Instant);
    }
}
