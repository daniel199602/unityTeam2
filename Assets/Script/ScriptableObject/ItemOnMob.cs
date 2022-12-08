using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnMob : MonoBehaviour
{
    public MobItem thisMobItem;

    /// <summary>
    /// �Ǫ��W
    /// </summary>
    public string mobName { private set; get; }
    /// <summary>
    /// �Ǫ�����
    /// </summary>
    public int mobType { private set; get; }
    /// <summary>
    /// �Ǫ��̤j��q
    /// </summary>
    public int mobMaxHp { private set; get; }
    /// <summary>
    /// �Ǫ��ˮ`_�ߧY
    /// </summary>
    public int mobDamage_instant { private set; get; }
    /// <summary>
    /// �Ǫ��ˮ`_����
    /// </summary>
    public int mobDamage_delay { private set; get; }
    /// <summary>
    /// �Ǫ��P�w����
    /// </summary>
    public float mobAngle { private set; get; }
    /// <summary>
    /// �Ǫ��P�w�b�|
    /// </summary>
    public float mobRadius { private set; get; }

    void Awake()
    {
        this.mobName = thisMobItem.mobName;
        this.mobType = thisMobItem.mobType;
        this.mobMaxHp = thisMobItem.mobMaxHp;
        this.mobDamage_instant = thisMobItem.mobDamage_instant;
        this.mobDamage_delay = thisMobItem.mobDamage_delay;
        this.mobAngle = thisMobItem.mobAngle;
        this.mobRadius = thisMobItem.mobRadius;
    }
}
