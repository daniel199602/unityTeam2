using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponItem",menuName ="Inventory/WeaponItem")]
public class WeaponItem : ScriptableObject
{
    public string weaponName;//�Z���W��
    public int weaponType;//�Z������
    public int weaponID;//�Z��ID
    public int weaponDamage_instant;//�Z���ˮ`_�ߧY
    public int weaponDamage_delay;//�Z���ˮ`_����
    public float weaponAngle;//�Z���P�w����
    public float weaponRadius;//�Z���P�w�b�|
}
