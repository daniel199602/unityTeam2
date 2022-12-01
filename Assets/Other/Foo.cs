using UnityEngine;
using UnityEditor;
public class Foo : MonoBehaviour
{
    //���Ψ���
    [SerializeField] private float angle = 80f;
    //���Υb�|
    [SerializeField] private float radius = 3.5f;
    //����B
    [SerializeField] private Transform b;
    private bool flag;
    private void Update()
    {
        flag = IsInRange(angle, radius, transform, b);
    }
    /// <summary>
    /// �P�_target�O�_�b���ΰϰ줺
    /// </summary>
    /// <param name="sectorAngle">���Ψ���</param>
    /// <param name="sectorRadius">���Υb�|</param>
    /// <param name="attacker">�����̪�transform�H��</param>
    /// <param name="target">�ؼ�</param>
    /// <returns>�ؼ�target�b���ΰϰ줺��^true �_�h��^false</returns>
    public bool IsInRange(float sectorAngle, float sectorRadius, Transform attacker, Transform attacked)
    {
        //�����̦�m���V�ؼЦ�m���V�q
        Vector3 direction = attacked.position - attacker.position;
        //�I���n���G
        float dot = Vector3.Dot(direction.normalized, transform.forward);
        //�ϧE���p�⨤��
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