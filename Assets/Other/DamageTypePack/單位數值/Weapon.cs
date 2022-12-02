using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    public int Weapon_Type;
    [HideInInspector] public int Weapon_Damage_Instant;
    [HideInInspector] public int Weapon_Damamge_Delay;
    [HideInInspector] public float weaPonangle;
    [HideInInspector] public float weaPonRadius;
    CharacterAttackManager CharacterAttackManager_w;
    private void Start()
    {
        CharacterAttackManager_w = GetComponent<CharacterAttackManager>();
        Weapon_Damage_Instant = CharacterAttackManager_w.Weapondamage_Instant;
        Weapon_Damamge_Delay = CharacterAttackManager_w.Weapondamamge_Delay;
        weaPonangle = CharacterAttackManager_w.angle;
        weaPonRadius = CharacterAttackManager_w.radius;
    }
    public int WeaponType(int Type)
    {
        GetWeaponType(Type);
        Debug.Log(Type);
        return Type;
    }
    public int GetWeaponType(int Type)
    {
        switch (Type)
        {
            case 0:
                TorchData();
                break;
            case 1:
                SowrdData();
                break;
            case 2:
                GreatSowrdData();
                break;
            case 3:
                MagicData();
                break;
            case 4:
                SketletonMonsterData();
                break;
            case 5:
                ExplosionBugData();
                break;
            case 6:
                MagicCasterData();
                break;
            case 7:
                FrogTrapData();
                break;
            case 8:
                BoxTrapData();
                break;
            case 9:
                FloorTrapData();
                break;
            case 10:
                BossData();
                break;
        }
        return Type;
    }
    public void GreatSowrdData()
    {
        Debug.Log("GreatSword");
        Weapon_Type = 7;
        Weapon_Damage_Instant = 90;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 95f;
        weaPonRadius = 15f;
    }
    public void SowrdData()
    {
        Weapon_Type = 6;
        Debug.Log("Sword_Sheid");
        Weapon_Damage_Instant = 50;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 40f;
        weaPonRadius = 10f;
    }
    public void TorchData()
    {
        Debug.Log("Torch");
        Weapon_Type = 9;
        Weapon_Damage_Instant = 10;
        Weapon_Damamge_Delay = 5;
        weaPonangle = 30f;
        weaPonRadius = 55f;
    }
    public void MagicData()
    {
        Debug.Log("Magic?");
        Weapon_Type = 8;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 150f;
        weaPonRadius = 2f;
    }
    public void BoxTrapData()
    {
        Weapon_Type = 2;
        Debug.Log("Monster_01");
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
    public void FloorTrapData()
    {
        Weapon_Type = 2;
        Debug.Log("Monster_01");
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
    public void FrogTrapData()
    {
        Debug.Log("Poison");
        Weapon_Type = 0;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
    public void SketletonMonsterData()
    {
        Debug.Log("Monster_03");
        Weapon_Type = 4;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
    public void ExplosionBugData()
    {
        Debug.Log("Monster_02");
        Weapon_Type = 3;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
    public void MagicCasterData()
    {
        Debug.Log("MagicCaster");
        Weapon_Type = 5;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
    public void BossData()
    {
        Debug.Log("Monster_04");
        Weapon_Type = 5;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 40f;
        weaPonRadius = 85f;
    }
}

