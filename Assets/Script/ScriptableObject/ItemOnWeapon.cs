using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWeapon : MonoBehaviour
{
    public WeaponItem thisWeaponItem;
    /// <summary>
    /// 武器名稱
    /// </summary>
    public string weaponName { private set; get; }
    /// <summary>
    /// 武器種類
    /// </summary>
    public int weaponType { private set; get; }
    /// <summary>
    /// 武器ID
    /// </summary>
    public int weaponID { private set; get; }
    /// <summary>
    /// 立即傷害
    /// </summary>
    public int weaponDamage_instant { private set; get; }
    /// <summary>
    /// 延遲傷害
    /// </summary>
    public int weaponDamage_delay { private set; get; }
    /// <summary>
    /// 武器作用範圍角度
    /// </summary>
    public float weaponAngle { private set; get; }
    /// <summary>
    /// 武器作用範圍半徑
    /// </summary>
    public float weaponRadius { private set; get; }

    void Awake()
    {
        this.weaponName = thisWeaponItem.weaponName;
        this.weaponType = thisWeaponItem.weaponType;
        this.weaponID = thisWeaponItem.weaponID;
        this.weaponDamage_instant = thisWeaponItem.weaponDamage_instant;
        this.weaponDamage_delay = thisWeaponItem.weaponDamage_delay;
        this.weaponAngle = thisWeaponItem.weaponAngle;
        this.weaponRadius = thisWeaponItem.weaponRadius;
    }
}
