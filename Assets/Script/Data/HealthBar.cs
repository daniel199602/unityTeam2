using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image Healthbar;
    public Image HealthbarLate;//不知為何抓不到1208
    public float Health, Maxhealth=10;
    private float _lerpspeed = 8f;
    PlayerState PlayerHp;
    int CountTime;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerHp = GetComponent<PlayerState>();       
    }
    void Start()
    {
        Maxhealth = PlayerHp.MaxHp;
        Health = PlayerHp.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        Health = PlayerHp.Hp;
        StartCoroutine(HealthBarDelay());
    }

    public void BarFilter()
    {
        Healthbar.fillAmount = Health/ Maxhealth;
        Debug.Log("減少中"+Health / Maxhealth);
        //Debug.Log(Healthbar.fillAmount);
        
    }

    IEnumerator HealthBarDelay()
    {
        CountTime = 1;
        while (CountTime >= 0)
        {
            yield return new WaitForSeconds(Time.deltaTime);
            if (CountTime < 0)
            {
                if (HealthbarLate.fillAmount > Healthbar.fillAmount)
                {
                    HealthbarLate.fillAmount -= 0.01f;
                }
                else
                {
                    HealthbarLate.fillAmount = Healthbar.fillAmount;
                }
            }
            CountTime--;
        }
    }


}
