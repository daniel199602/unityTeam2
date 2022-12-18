using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ChooseWeaponEvent : MonoBehaviour
{
    bool isInZone;
    UIManager uIManager;

    private void Awake()
    {
        uIManager = UIManager.Instance();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInZone && Input.GetKeyDown(KeyCode.E))
        {
            uIManager.weaponOneOfThreePanel.GetComponent<WeaponOneOfThreeUI>().SetRandomThreeWeaponR();//�]�m�k��Z���T��@
            uIManager.OneOfThreeUIOpen();//�T��@UI�������}
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            isInZone = false;
        }
    }

}
