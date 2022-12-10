using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Weapon 已棄用(之後刪除)
/// </summary>
public class Weapon : MonoBehaviour
{
    public int Weapon_Type;
    [HideInInspector] public int Weapon_Damage_Instant;
    [HideInInspector] public int Weapon_Damamge_Delay;
    [HideInInspector] public float weaPonangle;
    [HideInInspector] public float weaPonRadius;

    private void Start()
    {
    }

    public int WeaponType(int Type)
    {
        GetWeaponType(Type);
        //Debug.Log(Type);
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
                ExplosionBugData_Explosion();
                break;
            case 7:
                MagicCasterData();
                break;
            case 8:
                BoxTrapData();
                break;
            case 9:
                FloorTrapData();
                break;
            case 10:
                FrogTrapData();
                break;
            case 11:
                BossData_Melee();
                break;
            case 12:
                BossData_Ranged();
                break;
            case 13:
                Cystalsv_IncreaseBlood();
                break;
        }
        return Type;
    }
    public void TorchData()
    {
        //Debug.Log("Torch");
        Weapon_Type = 0;
        Weapon_Damage_Instant = 10;
        Weapon_Damamge_Delay = 5;
        weaPonangle = 30f;
        weaPonRadius = 5f;
    }
   public void SowrdData()
    {
        Weapon_Type = 1;
        Debug.Log("Sword_Sheid");
        Weapon_Damage_Instant = 50;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 60f;
        weaPonRadius = 10f;
    }

    public void GreatSowrdData()
    {
        Debug.Log("GreatSword");
        Weapon_Type = 2;
        Weapon_Damage_Instant = 90;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 95f;
        weaPonRadius = 15f;
    }
    
    public void MagicData()
    {
        Debug.Log("Magic?");
        Weapon_Type = 3;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 10f;
        weaPonRadius = 40f;
    }

    public void SketletonMonsterData()
    {
        Debug.Log("SketletonMonster");
        Weapon_Type = 4;
        Weapon_Damage_Instant = 70;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 40f;
        weaPonRadius = 10f;
    }

    public void ExplosionBugData()
    {
        Debug.Log(" ExplosionBugData");
        Weapon_Type = 5;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 40f;
        weaPonRadius = 5f;
    }

    public void ExplosionBugData_Explosion()
    {
        Debug.Log("ExplosionBugData_Explosing");
        Weapon_Type = 6;
        Weapon_Damage_Instant = 250;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 360;
        weaPonRadius = 5;
    }

    public void MagicCasterData()
    {
        Debug.Log("MagicCaster");
        Weapon_Type = 7;
        Weapon_Damage_Instant = 50;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 20f;
        weaPonRadius = 55f;
    }
    public void BoxTrapData()
    {
        Weapon_Type = 8;
        Debug.Log("BoxTrap");
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 360f;
        weaPonRadius = 20f;
    }

    public void FloorTrapData()
    {
        Weapon_Type = 9;
        Debug.Log("FloorTrap");
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 360f;
        weaPonRadius = 5f;
    }

    public void FrogTrapData()
    {
        Debug.Log("FrogTrap");
        Weapon_Type = 10;
        Weapon_Damage_Instant = 30;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 30f;
        weaPonRadius = 25f;
    }


    public void BossData_Melee()
    {
        Debug.Log("Boss_Melee Attack");
        Weapon_Type = 11;
        Weapon_Damage_Instant = 90;
        Weapon_Damamge_Delay = 10;
        weaPonangle = 120f;
        weaPonRadius = 25f;
    }

    public void BossData_Ranged()
    {
        Debug.Log("Boss_Ranged Attack");
        Weapon_Type = 12;
        Weapon_Damage_Instant = 40;
        Weapon_Damamge_Delay = 20;
        weaPonangle = 25f;
        weaPonRadius = 55f;
    }

    public void BossData_Melee_StageTwo()
    {
        Debug.Log("Boss_Melee Attack");
        Weapon_Type = 13;
        Weapon_Damage_Instant = 100;
        Weapon_Damamge_Delay = 20;
        weaPonangle = 150f;
        weaPonRadius = 30f;
    }

    public void BossData_Ranged_StageTwo()
    {
        Debug.Log("Boss_Ranged Attack");
        Weapon_Type = 14;
        Weapon_Damage_Instant = 60;
        Weapon_Damamge_Delay = 50;
        weaPonangle = 40f;
        weaPonRadius = 60f;
    }

    public void Cystalsv_IncreaseBlood()
    {
        Weapon_Damage_Instant = -200;
        Debug.Log("-200---------------------");
    }
}

