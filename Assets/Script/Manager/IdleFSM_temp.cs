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
        MubAnimator.SetBool("BeHit", true);
    }
    public void Idle()
    {
        MubAnimator.SetBool("BeHit", false);
    }

    public void Die()
    {
        MubAnimator.SetTrigger("Die");
    }
}
