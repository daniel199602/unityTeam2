using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebuffFSM : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject Player;
    public int Health;
    public int curseStage;
    public bool poiSons;
    public int poiSonsDamage;
    public void Poison() => Health -= poiSonsDamage * curseStage;
    public float Timer(float Timecount)
    {
        Timecount = 0 + Timecount;
        return Timecount;
    }
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (poiSons == true)
        {
            
        }
    }
}
