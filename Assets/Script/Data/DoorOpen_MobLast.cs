using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen_MobLast : MonoBehaviour
{
    public int mobLast = 8;

    public GameObject door_VFX;
    bool door_VFX_isPlaying;

    AudioEvent_electircDoor mobBornRoom;//���ĸ}��_�p�q��

    private void Start()
    {
        mobBornRoom = this.gameObject.GetComponentInParent<AudioEvent_electircDoor>();//����˪��}��

        door_VFX_isPlaying = false;
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
                ParticleSystem ps = door_VFX.GetComponent<ParticleSystem>();
                ps.Play();

                mobBornRoom.PlayElectricityEvent();//������

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


