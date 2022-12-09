using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnWeapon : MonoBehaviour
{
    public WeaponItem thisWeaponItem;

    /// <summary>
    /// �Z���W��
    /// </summary>
    public string weaponName { private set; get; }
    /// <summary>
    /// �Z������
    /// </summary>
    public int weaponType { private set; get; }
    /// <summary>
    /// �Z��ID
    /// </summary>
    public int weaponID { private set; get; }
    /// <summary>
    /// �Z���ˮ`_�ߧY
    /// </summary>
    public int weaponDamage_instant { private set; get; }
    /// <summary>
    /// �Z���ˮ`_����
    /// </summary>
    public int weaponDamage_delay { private set; get; }
    /// <summary>
    /// �Z���P�w����
    /// </summary>
    public float weaponAngle { private set; get; }
    /// <summary>
    /// �Z���P�w�b�|
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
