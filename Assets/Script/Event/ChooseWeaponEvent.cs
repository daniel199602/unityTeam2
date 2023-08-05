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
        
    }

    private void Start()
    {
        uIManager = UIManager.Instance();
        Debug.LogWarning(uIManager);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInZone && Input.GetKeyDown(KeyCode.E))
        {
            uIManager.OpenOneOfThreeAndChooseWeapon();
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
