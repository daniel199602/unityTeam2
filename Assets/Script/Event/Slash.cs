using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject bigSwoadSlash_Right;
    public GameObject bigSwoadSlash_1;
    public GameObject bigSwoadSlash_2;
    public GameObject bigSwoadSlash_3;

    void bigSwoadSlashRight_Event()
    {
        ParticleSystem ps = bigSwoadSlash_Right.GetComponent<ParticleSystem>();
        ps.Play();
    }

    void bigSwoadSlash1_Event()
    {
        ParticleSystem ps = bigSwoadSlash_1.GetComponent<ParticleSystem>();
        ps.Play();
    }

    void bigSwoadSlash2_Event()
    {
        ParticleSystem ps = bigSwoadSlash_2.GetComponent<ParticleSystem>();
        ps.Play();
    }

    void bigSwoadSlash3_Event()
    {
        ParticleSystem ps = bigSwoadSlash_3.GetComponent<ParticleSystem>();
        ps.Play();
    }

}
