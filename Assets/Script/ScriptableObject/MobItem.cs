using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobItem", menuName = "Inventory/MobItem")]
public class MobItem : ScriptableObject
{
    public string mobName;//怪物名稱
    public int mobType;//怪物種類
    public int mobMaxHp;//最大血量
    public int mobDamage_instant;//怪物傷害_立即
    public int mobDamage_delay;//怪物傷害_延遲
    public float mobAngle;//怪物判定角度
    public float mobRadius;//怪物判定半徑
}
