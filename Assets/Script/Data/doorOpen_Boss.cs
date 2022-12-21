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
    bool outDoorC;
    public GameObject fog;

    public AnimationCurve fadeOut;

    float t = 0.001f;
    // Start is called before the first frame update
    void Start()
    {             
        myAnim = GetComponent<Animator>();
        fog = GameObject.Find("FogOfWarPlane");
        //frameTransform.gameObject.SetActive(false);
        Debug.Log("123");

        outDoorC = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(isInZone&&Input.GetKeyDown(KeyCode.E))
        {                       
            myAnim.SetBool("isOpen", true);
        }
        if(outDoorC ==true)
        {
            t += Time.deltaTime;
            fog.GetComponent<MeshRenderer>().materials[0].SetFloat("_FogRadius", Mathf.Lerp(80, 200, t));
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
            outDoorC = true;
            //fog.GetComponent<MeshRenderer>().materials[0].SetFloat("_FogRadius", 200);
            frameTransform.gameObject.SetActive(true);
        }
    }
}
