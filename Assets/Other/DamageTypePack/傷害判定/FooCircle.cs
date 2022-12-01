using UnityEngine;
using UnityEditor;
using Unity.VisualScripting;

public class FooCircle : MonoBehaviour
{
    //半徑
    private float radius;
    [SerializeField] private float radiusMin;
    [SerializeField] private float radiusMax;
    //物體B
    [SerializeField] private Transform b;
    CapsuleCollider c;
    private bool flag;
    private void Start()
    {
        c = b.GetComponent<CapsuleCollider>();
        radiusMin -= c.radius;
        radiusMax += c.radius;
    }
    private void Update()
    {
        flag = IsInRange(radiusMin,radiusMax, transform, b);
       
        Debug.Log(flag);
    }
    
    /// <summary>
    /// 判斷target是否在扇形區域內
    /// </summary>
    /// <param name="sectorAngle">扇形角度</param>
    /// <param name="sectorRadius">扇形半徑</param>
    /// <param name="attacker">攻擊者的transform信息</param>
    /// <param name="target">目標</param>
    /// <returns>目標target在扇形區域內返回true 否則返回false</returns>
    //public bool IsInRange(float sectorRadius, Transform attacker, Transform attacked)
    //{
    //    //攻擊者位置指向目標位置的向量
    //    Vector3 direction = attacked.position - attacker.position;
    //    Debug.Log(direction.magnitude);
    //    //點乘積結果

    //    return  direction.magnitude - c.radius < sectorRadius;
    //}
    public bool IsInRange(float firstRadius, float sectorRadius, Transform attacker, Transform attacked)
    {
        //攻擊者位置指向目標位置的向量
        Vector3 direction = attacked.position - attacker.position;
        Debug.Log(direction.magnitude);
        //點乘積結果

        return firstRadius < direction.magnitude + c.radius && direction.magnitude - c.radius < sectorRadius;
    }

    private void OnDrawGizmos()
    {
        if (flag == true)
        {
            Gizmos.color = Color.cyan;
        }
        else
        {
            Gizmos.color = Color.red;
        }
        Gizmos.DrawWireSphere(transform.position, radiusMin);
        Gizmos.DrawWireSphere(transform.position, radiusMax);        
    }
}