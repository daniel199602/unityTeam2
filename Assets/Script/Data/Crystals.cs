using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crystals : MonoBehaviour
{
    public GameObject player;
    CharacterAttackManager characterM;
    
    private void Start()
    {
        characterM = player.GetComponent<CharacterAttackManager>();
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            characterM.fHp = -200;


            this.gameObject.SetActive(false);
        }

    }
}
