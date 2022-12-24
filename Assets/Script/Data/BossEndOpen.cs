using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossEndOpen : MonoBehaviour
{
    public GameObject bossHp;
    public GameObject bossEndDoor;
    public GameObject door_VFX_Boss;
    MubHpData mubHpData;

    ParticleSystem ps;
    bool door_VFX_isPlaying_Boss;


    void Start()
    {
        mubHpData = bossHp.GetComponent<MubHpData>();
        door_VFX_isPlaying_Boss = false;
        ps = door_VFX_Boss.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if (mubHpData.Hp > 0)
        {
            bossEndDoor.SetActive(true);
        }
        else
        {
            if (door_VFX_isPlaying_Boss == false)
            {
                ps.Play();
                GameManager.Instance().audioSource.clip = GameManager.Instance().audios[3];
                GameManager.Instance().audioSource.Play();
                door_VFX_isPlaying_Boss = true;
            }
            Invoke(nameof(BossEndDoorOpen), 2);
        }
    }

    void BossEndDoorOpen()
    {
        bossEndDoor.SetActive(false);
    }
}
