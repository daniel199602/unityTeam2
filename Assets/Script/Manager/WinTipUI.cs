using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;


public class WinTipUI : MonoBehaviour
{
    public GameObject bossHp;
    MubHpData mubHpData;
    public Image vitory;
    bool isDoItOnce = true;

    // Start is called before the first frame update
    void Start()
    {
        mubHpData = bossHp.GetComponent<MubHpData>();
        vitory.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
    }

    // Update is called once per frame
    void Update()
    {
        if (mubHpData.Hp <= 0 && isDoItOnce)
        {
            vitory.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
            StartCoroutine(WinTipDelayClose());
            isDoItOnce = false;
        }
    }

    IEnumerator  WinTipDelayClose()
    {
        yield return new WaitForSeconds(5);
        vitory.GetComponent<Image>().color = new Color32(255, 255, 255, 0);
    }
}
