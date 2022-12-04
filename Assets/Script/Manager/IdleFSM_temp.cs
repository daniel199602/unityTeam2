using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdleFSM_temp : MonoBehaviour
{
    Animator MubAnimator;
    
    // Start is called before the first frame update
    private void Start()
    {
        MubAnimator = GetComponent<Animator>();
    }
    public void BeHit()       
    {
        int BH;
        BH = Random.Range(0, 2);
        if (BH ==0)
        {
            MubAnimator.SetBool("BeHit01", true);
        }
        else
        {
            MubAnimator.SetBool("BeHit02", true);
        }
        
    }
    public void Idle()
    {
        MubAnimator.SetBool("BeHit01", false);

        MubAnimator.SetBool("BeHit02", false);
    }

    public void Die()
    {       
        int BH;
        BH = Random.Range(0, 2);
        if (BH == 0)
        {
            MubAnimator.SetTrigger("Die01");
        }
        else
        {
            MubAnimator.SetTrigger("Die02");
        }
    }
}

