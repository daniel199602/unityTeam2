using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthPlayerBar : MonoBehaviour
{
    public Image Healthbar;
    public Image HealthbarLate;
    public float Health, Maxhealth=10;
    private float _lerpspeed = 8f;
    PlayerHpData PlayerHp;
    int CountTime;
    public RectTransform hpBar;

    // Start is called before the first frame update
    private void Awake()
    {
        PlayerHp = GetComponent<PlayerHpData>();       
    }
    void Start()
    {
        Health = PlayerHp.Hp;
        //hpBar.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        Health = PlayerHp.Hp;
        StartCoroutine(HealthBarDelay());
        if(Maxhealth != Health&& Health>0)
        {
            hpBar.gameObject.SetActive(true);
        }
        else if(Health<=0)
        {
            hpBar.gameObject.SetActive(false);
        }
    }

    public void BarFilter()
    {
        Healthbar.fillAmount = Health/ Maxhealth;
        //Debug.Log("´î¤Ö¤¤"+Health / Maxhealth);
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
