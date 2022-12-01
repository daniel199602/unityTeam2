using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FogControl : MonoBehaviour
{
    [SerializeField] Transform LookPoint;

    public Material M1, M2;

    [SerializeField] float Y = 100f;
    // Start is called before the first frame update
    void Start()
    {
       
    }


    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(LookPoint.position.x,Y,LookPoint.position.z) ;
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            this.transform.GetComponent<Renderer>().material = M2;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            this.transform.GetComponent<Renderer>().material = M1;
        }
    }
}
