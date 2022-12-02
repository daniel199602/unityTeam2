using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    [SerializeField] public int Hp = 1000;
    [SerializeField] private int currentHp;

    HealthBar Health;

    private void Awake()
    {
        Health = GetComponent<HealthBar>();
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
            currentHp = Hp;
            Health.BarFilter();
        }
        if (currentHp < 0)
        {
            currentHp = 0;
            currentHp = Hp;
        }
        Health.BarFilter();
    }
    private void FixedUpdate()
    {
        Debug.LogWarning("¦å¶q:" + currentHp);
    }
}