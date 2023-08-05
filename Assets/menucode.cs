using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class menucode : MonoBehaviour
{
    Animator men;
    private void Start()
    {
        men = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    private void lookhand()
    {
        men.speed = 0f;
    }
    private void startme() 
    {
        men.speed = 1.2f;
    }
}
