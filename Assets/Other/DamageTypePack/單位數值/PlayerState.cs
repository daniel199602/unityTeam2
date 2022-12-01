using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] public int Hp=1000;
    [SerializeField] private int currentHp;

    Animator MubAnimator = new Animator();

    HealthBar Health;

    private void Awake()
    {
        Hp = 1000;
    }
    private void Start()
    {
        Hp = 1000;
        currentHp = Hp;
       
        MubAnimator = GetComponent<Animator>();
        
        Health = GetComponent<HealthBar>();                   
    }

    private void Update()
    {
        if (Hp != currentHp)
        {
            currentHp = Hp;
            MubAnimator.SetBool("GetHit", true);
            Health.BarFilter();
        }
        else
        {
            MubAnimator.SetBool("GetHit", false);
        }
        if (currentHp <= 0)
        {
            MubAnimator.SetTrigger("isTriggerDie");
        }
       
    }

    private void FixedUpdate()
    {
        Debug.LogWarning("¦å¶q:" + currentHp);
    }    
}