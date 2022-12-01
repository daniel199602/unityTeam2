using UnityEngine;
using UnityEditor;
public class Foo : MonoBehaviour
{
    //扇形角度
    [SerializeField] private float angle = 80f;
    //扇形半徑
    [SerializeField] private float radius = 3.5f;
    //物體B
    [SerializeField] private Transform b;
    private bool flag;
    private void Update()
    {
        flag = IsInRange(angle, radius, transform, b);
    }
    /// <summary>
    /// 判斷target是否在扇形區域內
    /// </summary>
    /// <param name="sectorAngle">扇形角度</param>
    /// <param name="sectorRadius">扇形半徑</param>
    /// <param name="attacker">攻擊者的transform信息</param>
    /// <param name="target">目標</param>
    /// <returns>目標target在扇形區域內返回true 否則返回false</returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)
    {
        //攻擊者位置指向目標位置的向量
        Vector3 direction = attacked.position - attacker.position;
        //點乘積結果
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        //反余弦計算角度
        float offsetAngle = Mathf.Acos(dot) * Mathf.Rad2Deg;
        return offsetAngle < sectorAngle * .5f && direction.magnitude < sectorRadius;
    }
    //private void OnDrawGizmos()
    //{
    //    Handles.color = flag ? Color.cyan : Color.red;
    //    float x = radius * Mathf.Sin(angle / 2f * Mathf.Deg2Rad);
    //    float y = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //    Vector3 a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + y);
    //    Vector3 b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);
    //    Handles.DrawLine(transform.position, a);
    //    Handles.DrawLine(transform.position, b);
    //    float half = angle / 2;
    //    for (int i = 0; i < half; i++)
    //    {
    //        x = radius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
    //        y = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        a = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + y);
    //        x = radius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
    //        y = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        b = new Vector3(transform.position.x - x, transform.position.y, transform.position.z + y);
    //        Handles.DrawLine(a, b);
    //    }
    //    for (int i = 0; i < half; i++)
    //    {
    //        x = radius * Mathf.Sin((half - i) * Mathf.Deg2Rad);
    //        y = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        a = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);
    //        x = radius * Mathf.Sin((half - i - 1) * Mathf.Deg2Rad);
    //        y = Mathf.Sqrt(Mathf.Pow(radius, 2f) - Mathf.Pow(x, 2f));
    //        b = new Vector3(transform.position.x + x, transform.position.y, transform.position.z + y);
    //        Handles.DrawLine(a, b);
    //    }
    //}
}