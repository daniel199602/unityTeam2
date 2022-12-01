using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrogTrap : MonoBehaviour
{
    /// <summary>
    /// °»´ú¨ìª±®a
    /// </summary>
    public LayerMask HitPlayer;
    /// <summary>
    /// «Cµìªº¬r
    /// </summary>
    public GameObject trap;
    



    void FixedUpdate()
    {
        //«Cµì«e100ªº¶ZÂ÷­Y°»´ú¨ìª±®a¼·©ñ«Cµìªº¬r
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
    /// ´ú¸Õ½u(«Cµì¶ZÂ÷)
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
