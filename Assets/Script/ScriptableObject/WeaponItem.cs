using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponItem",menuName ="Inventory/WeaponItem")]
public class WeaponItem : ScriptableObject
{
    public string weaponName;//武器名稱
    public int weaponType;//武器種類
    public int weaponID;//武器ID
    public int weaponDamage_instant;//武器傷害_立即
    public int weaponDamage_delay;//武器傷害_延遲
    public float weaponAngle;//武器判定角度
    public float weaponRadius;//武器判定半徑
}
