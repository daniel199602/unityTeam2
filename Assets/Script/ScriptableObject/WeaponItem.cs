using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponItem",menuName ="Inventory/WeaponItem")]
public class WeaponItem : ScriptableObject
{
    public string weaponName;//武器名稱
    public int weaponType;//武器種類
    public int weaponID;//武器ID
    public int weaponDamage_instant;//立即傷害
    public int weaponDamage_delay;//延遲傷害
    public float weaponAngle;//武器作用範圍角度
    public float weaponRadius;//武器作用範圍半徑
}
