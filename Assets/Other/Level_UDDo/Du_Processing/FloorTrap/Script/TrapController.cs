using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapController : MonoBehaviour
{
    public Animator TrapAnimator;
    private float Timer=1f;
    // Start is called before the first frame update
    public void TrapRelease()
    {
        TrapAnimator.SetBool("Release", true);
    }
    public void TrapRevoke()
    {
        TrapAnimator.SetBool("Release", false);
    }
    void Start()
    {       
        TrapAnimator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider Player)
    {
        if (Player.tag=="Player")
        {            
            //Debug.Log("Enter");
            TrapRelease();
        }
    }
    private void OnTriggerStay(Collider Player)
    {
        if (Player.tag == "Player")
        {
            //Debug.Log("Stay");
            this.Invoke("TrapRelease", Timer);
        }
    }
    private void OnTriggerExit(Collider Player)
    {
        if (Player.tag == "Player")
        {
            //Debug.Log("Exit");
            CancelInvoke("TrapRelease");
            TrapRevoke();
        }
    }
    // Update is called once per frame
}
