using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChooseWeaponEvent_ForBorn : MonoBehaviour
{
    UIManager uIManager;
    // Start is called before the first frame update
    void Start()
    {
        
        //uIManager.OpenOneOfThreeAndChooseWeapon();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            uIManager = UIManager.Instance();
            uIManager.OpenOneOfThreeAndChooseWeapon();
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            this.gameObject.SetActive(false);
        }
    }
}
