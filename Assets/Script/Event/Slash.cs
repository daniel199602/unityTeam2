using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slash : MonoBehaviour
{
    public GameObject bigSwoadSlash_Right;

    void bigSwoadSlashRight_Event()
    {
        ParticleSystem ps = bigSwoadSlash_Right.GetComponent<ParticleSystem>();
        ps.Play();
    }

}
