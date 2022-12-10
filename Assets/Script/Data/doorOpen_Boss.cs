using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class doorOpen_Boss : MonoBehaviour
{
    public RectTransform bloodTransform;
    public RectTransform bloodDelayTransform;
    public RectTransform frameTransform;

    Animator myAnim;
    bool isInZone;
    public GameObject fog;
    // Start is called before the first frame update
    void Start()
    {
        myAnim = GetComponent<Animator>();
        bloodTransform.gameObject.SetActive(false);
        bloodDelayTransform.gameObject.SetActive(false);
        frameTransform.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(isInZone&&Input.GetKeyDown(KeyCode.E))
        {
            
            bool isOpen = myAnim.GetBool("isOpen");
            myAnim.SetBool("isOpen", !isOpen);
            
            bloodTransform.gameObject.SetActive(true);
            bloodDelayTransform.gameObject.SetActive(true);
            frameTransform.gameObject.SetActive(true);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            
            isInZone = false;
            fog.SetActive(false);
        }
    }
}
