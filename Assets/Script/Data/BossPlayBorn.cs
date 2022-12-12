using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossPlayBorn : MonoBehaviour
{
     public GameObject Player;
     private float duration;
    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player");
        duration = 1;
        Debug.Log(Player.transform.position);
        Invoke(nameof(PlayerBorn), duration);
        Player.transform.position = this.gameObject.transform.position;
        
    }
    void PlayerBorn()
    {
        Player.transform.position = this.gameObject.transform.position;
        Debug.Log(Player.transform.position);
    }

    
    // Update is called once per frame
    void Update()
    {
        
    }
}
