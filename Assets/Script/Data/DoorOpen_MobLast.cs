using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen_MobLast : MonoBehaviour
{
    public int mobLast = 8;

    public GameObject door_VFX;
    bool door_VFX_isPlaying;
    ParticleSystem ps;
    private void Start()
    {
        door_VFX_isPlaying = false;
        ps = door_VFX.GetComponent<ParticleSystem>();
    }
    public void killMob()
    {
        mobLast -= 1;
    }
    private void Update()
    {
        if (mobLast == 0)
        {
            if(door_VFX_isPlaying==false)
            {
                ps.Play();
                door_VFX_isPlaying = true;
            }
            Invoke(nameof(door_Open), 2);
        }
    }
    
    void door_Open()
    {
        this.gameObject.SetActive(false);
    }
}


