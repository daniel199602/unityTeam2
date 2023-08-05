using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FogOfWarPlayer : MonoBehaviour
{

    public Transform FogOfWarPlane;
    [SerializeField] int Number = 1;



    
    void Update()
    {
        
            FogOfWarPlane.GetComponent<Renderer>().material.SetVector("_Player1_Pos", transform.position);
    }
}
