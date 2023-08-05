using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FogControl : MonoBehaviour
{
    [SerializeField] Transform LookPoint;

    public Material M1, M2;
    float t = 0.0001f;
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
            // this.transform.GetComponent<Renderer>().material = M2;
            t += Time.deltaTime;
            this.transform.GetComponent<MeshRenderer>().materials[0].SetFloat("_FogRadius", Mathf.Lerp(80, 60, t));
        }
        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponManager.Instance().CurrentWeaponR_weaponR)
        {
            //this.transform.GetComponent<Renderer>().material = M1;
            t += Time.deltaTime;
            this.transform.GetComponent<MeshRenderer>().materials[0].SetFloat("_FogRadius", Mathf.Lerp(60, 80, t));
        }
    }
}
