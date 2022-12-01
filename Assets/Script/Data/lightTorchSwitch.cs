using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightTorchSwitch : MonoBehaviour
{
    public GameObject fire;
    bool isInZone;
    bool haveFire;
    // Start is called before the first frame update
    void Start()
    {
        haveFire = true;
        //測試需開關用
        //fire.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isInZone && Input.GetMouseButtonDown(0)&& haveFire)
        {
            //測試需開關用
            //fire.SetActive(!fire.activeSelf);
            
            fire.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag =="Player")
        {
            isInZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInZone = false;
        }
    }
}
