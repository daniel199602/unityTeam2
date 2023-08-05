using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemOnMob : MonoBehaviour
{
    public MobItem thisMobItem;

    /// <summary>
    /// 怪物名
    /// </summary>
    public string mobName { private set; get; }
    /// <summary>
    /// 怪物種類
    /// </summary>
    public int mobType { private set; get; }
    /// <summary>
    /// 怪物最大血量
    /// </summary>
    public int mobMaxHp { private set; get; }
    /// <summary>
    /// 怪物傷害_立即
    /// </summary>
    public int mobDamage_instant { private set; get; }
    /// <summary>
    /// 怪物傷害_延遲
    /// </summary>
    public int mobDamage_delay { private set; get; }
    /// <summary>
    /// 怪物判定角度
    /// </summary>
    public float mobAngle { private set; get; }
    /// <summary>
    /// 怪物判定半徑
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
