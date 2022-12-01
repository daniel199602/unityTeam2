using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen_Boss : MonoBehaviour
{
    Animator myAnim;
    bool isInZone;
    public GameObject fog;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(isInZone&&Input.GetKeyDown(KeyCode.E))
        {
            //Debug.Log("e");
            bool isOpen = myAnim.GetBool("isOpen");
            myAnim.SetBool("isOpen", !isOpen);
            //Debug.Log(isOpen);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            //Debug.Log("in");
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("out");
            isInZone = false;
            fog.SetActive(false);
        }
    }
}
