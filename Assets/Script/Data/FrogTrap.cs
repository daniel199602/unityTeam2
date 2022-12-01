using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTrap : MonoBehaviour
{
    /// <summary>
    /// �����쪱�a
    /// </summary>
    public LayerMask HitPlayer;
    /// <summary>
    /// �C�쪺�r
    /// </summary>
    public GameObject trap;
    



    void FixedUpdate()
    {
        //�C��e100���Z���Y�����쪱�a����C�쪺�r
        Vector3 mePos =this.transform.right;
        Ray r = new Ray(this.transform.position, mePos);
        RaycastHit rh;
        if (Physics.Raycast(r, out rh, 100.0f, HitPlayer))
        {
            ParticleSystem ps = trap.GetComponent<ParticleSystem>();
            ps.Play();
        }
    }
   
    

    /// <summary>
    /// ���սu(�C��Z��)
    /// </summary>
    private void OnDrawGizmos()
    {
        float fDetectLengh = 100.0f;
        Vector3 vec = transform.right;
        Vector3 vpos = transform.position;
        Gizmos.DrawLine(vpos, vpos + vec * fDetectLengh);
        Gizmos.color = Color.blue;
    }
}
