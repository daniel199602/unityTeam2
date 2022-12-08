using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="WeaponItem",menuName ="Inventory/WeaponItem")]
public class WeaponItem : ScriptableObject
{
    public string weaponName;//�Z���W��
    public int weaponType;//�Z������
    public int weaponID;//�Z��ID
    public int weaponDamage_instant;//�ߧY�ˮ`
    public int weaponDamage_delay;//����ˮ`
    public float weaponAngle;//�Z���@�νd�򨤫�
    public float weaponRadius;//�Z���@�νd��b�|
}
