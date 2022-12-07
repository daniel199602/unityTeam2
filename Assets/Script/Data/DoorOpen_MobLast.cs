using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorOpen_MobLast : MonoBehaviour
{
    public int mobLast = 8;

    public void killMob()
    {
        mobLast -= 1;
    }
    private void Update()
    {
        if (mobLast == 0)
        {
            this.gameObject.SetActive(false);
        }
    }

}
