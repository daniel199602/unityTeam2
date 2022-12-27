using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFade_bigS : MonoBehaviour
{
    public GameObject fadeColor_ForBoss;
    Animation fadeBoss;
    // Start is called before the first frame update
    void Start()
    {
        fadeBoss = fadeColor_ForBoss.GetComponent<Animation>();
    }

    void BossFade_Big()
    {
        fadeBoss.Play("FadeBoss");
    }
}
