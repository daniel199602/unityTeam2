using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] public int Hp = 1000;
    [SerializeField] private int currentHp;

    HealthBar Health;

    IdleFSM_temp idleFSM_;

    private void Awake()
    {
        Health = GetComponent<HealthBar>();
        idleFSM_ = GetComponent<IdleFSM_temp>();
        Hp = 1000;
    }
    private void Start()
    {
        currentHp = Hp;
    }

    private void Update()
    {
        HpCheck();
    }
    public void HpCheck()
    {
        if (Hp != currentHp)
        {
            Debug.Log("¦©¦å");
            currentHp = Hp;
            Health.BarFilter();
        }
        if (currentHp < 0)
        {
            currentHp = 0;
            Hp = currentHp;
        }
    }
    private void FixedUpdate()
    {
        Debug.LogWarning("¦å¶q:" + currentHp);
    }
}