using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealingCrystal : MonoBehaviour
{
    public Animator CrystalAnimator;
    // Start is called before the first frame update
    private void Heal()
    {

    } 
    void Start()
    {
        CrystalAnimator = GetComponent<Animator>();
    }
    private void OnTriggerEnter(Collider Player)
    {       
            if (Player.tag == "Player")
            {
                CrystalAnimator.SetBool("CrystalApproach", true);
            }             
    }
    private void OnTriggerStay(Collider Player)
    {
        if (Input.GetKeyDown("f"))
        {
            if (Player.tag == "Player")
            {
                CrystalAnimator.SetBool("CrystalBreak", true);
            }
        }
    }
    private void OnTriggerExit(Collider Player)
    {
        if (Player.tag == "Player")
        {
            CrystalAnimator.SetBool("CrystalApproach", false);
        }
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void End()
    {
        Destroy(gameObject);
    }
}
