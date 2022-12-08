using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MobItem", menuName = "Inventory/MobItem")]
public class MobItem : ScriptableObject
{
    public string mobName;//�Ǫ��W��
    public int mobType;//�Ǫ�����
    public int mobMaxHp;//�̤j��q
    public int mobDamage_instant;//�Ǫ��ˮ`_�ߧY
    public int mobDamage_delay;//�Ǫ��ˮ`_����
    public float mobAngle;//�Ǫ��P�w����
    public float mobRadius;//�Ǫ��P�w�b�|
}
