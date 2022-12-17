using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum doorOPen
{
    Close,Open
}
public class doorOpen_Boss : MonoBehaviour
{

    public RectTransform frameTransform;
    GameObject BossTrigger;
    Animator myAnim;
    bool isInZone;
    public GameObject fog;
    // Start is called before the first frame update
    void Start()
    {             
        myAnim = GetComponent<Animator>();
        fog = GameObject.Find("FogOfWarPlane");
        //frameTransform.gameObject.SetActive(false);
        Debug.Log("123");
         
    }

    // Update is called once per frame
    void Update()
    {
        if(isInZone&&Input.GetKeyDown(KeyCode.E))
        {                       
            myAnim.SetBool("isOpen", true);
            
            
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag=="Player")
        {
            BossTrigger = GameManager.Instance().mobPool[0];
            isInZone = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Debug.Log(BossTrigger.name);
            Debug.Log(BossTrigger.GetComponent<BossFSM>());
            BossTrigger.GetComponent<BossFSM>().DoorOpen();
            
            myAnim.SetBool("isOpen", false);
            isInZone = false;
            fog.SetActive(false);

            frameTransform.gameObject.SetActive(true);
        }
    }
}
