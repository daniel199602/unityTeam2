using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogOfFire2 : MonoBehaviour
{
    public Transform FogOfWarPlane;
    // Start is called before the first frame update
    void Start()
    {
        FogOfWarPlane = GameObject.Find("FogOfWarPlane").transform;
    }

    // Update is called once per frame
    void Update()
    {

        FogOfWarPlane.GetComponent<Renderer>().material.SetVector("_Player3_Pos", transform.position);

    }
}
