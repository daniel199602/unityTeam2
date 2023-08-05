using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHit : MonoBehaviour
{
    /*目前無用*/
    ParticleSystem HeadEnable;
    ParticleSystem HeadEnables;
 
    Animator RockAnimator = new Animator();
    GameObject LavaGuardHead;
    GameObject LavaGuardHead02;

    // Start is called before the first frame update
    private void Awake()
    {
        LavaGuardHead = GameObject.Find("Magic fire 2 (1)");
        LavaGuardHead02 = GameObject.Find("Magic fire 2");
    }   
}
