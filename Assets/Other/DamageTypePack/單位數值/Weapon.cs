using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Weapon : MonoBehaviour
{
    public int Weapon_Type;
    [HideInInspector] public int Weapon_Damage_Instant;
    [HideInInspector] public int Weapon_Damamge_Delay;
    [HideInInspector] public float weaPonangle;
    [HideInInspector] public float weaPonRadius;
    private void Start()
    {
        Weapon_Damage_Instant=0;
        Weapon_Damamge_Delay = 0;
        weaPonangle = 0;
        weaPonRadius = 0;
    }
    public int WeaponType(int Type)
    {
        GetWeaponType(Type);
        return Type;   
    }
    public int GetWeaponType(int Type)
    {
        if (Type == 0)
        {
            Debug.Log("Poison");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 40f;
            weaPonRadius = 85f;
        }
        if (Type == 1)
        {
            Debug.Log("Fire");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 40f;
            weaPonRadius = 85f;
        }
        if (Type == 2)
        {
            Debug.Log("Monster_01");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 40f;
            weaPonRadius = 85f;
        }
        if (Type == 3)
        {
            Debug.Log("Monster_02");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 40f;
            weaPonRadius = 85f;
        }
        if (Type == 4)
        {
            Debug.Log("Monster_03");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 40f;
            weaPonRadius = 85f;        
        }
        if (Type == 5)
        {
            Debug.Log("Monster_04");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 40f;
            weaPonRadius = 85f;
        }
        if (Type == 6)
        {
            Debug.Log("Sword_Sheid");
            Weapon_Damage_Instant = 50;
            Weapon_Damamge_Delay = 0;
            weaPonangle = 40f;
            weaPonRadius = 10f;
        }
        if (Type == 7)
        {
            Debug.Log("GreatSword");
            Weapon_Damage_Instant = 90;
            Weapon_Damamge_Delay = 0;
            weaPonangle = 95f;
            weaPonRadius = 15f;
        }
        if (Type == 8)
        {
            Debug.Log("Magic?");
            Weapon_Damage_Instant = 30;
            Weapon_Damamge_Delay = 10;
            weaPonangle = 150f;
            weaPonRadius = 2f;
        }
        if (Type == 9)
        {
            Debug.Log("Torch");
            Weapon_Damage_Instant = 10;
            Weapon_Damamge_Delay = 5;
            weaPonangle = 30f;
            weaPonRadius = 55f;
        }
        return Type;
    }
}

