using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Image Healthbar;
    public Image HealthbarLate;
    public float Health, Maxhealth;
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
        Health = PlayerHp.Hp;
        Maxhealth = PlayerHp.Hp;
    }

    // Update is called once per frame
    void Update()
    {
        Health = PlayerHp.Hp;
        Debug.Log(Health / Maxhealth);    
    }
    public void BarFilter()
    {
        Healthbar.fillAmount = Mathf.Lerp(Healthbar.fillAmount, Health / Maxhealth, _lerpspeed);
        Debug.Log(Healthbar.fillAmount);
        //StartCoroutine(HealthBarDelay());                
    }
    //IEnumerator HealthBarDelay()
    //{
    //    CountTime = 2;
    //    while (CountTime >= 0)
    //    {
    //        yield return new WaitForSeconds(1);
    //        if (CountTime < 0)
    //        {
    //            HealthbarLate.fillAmount = Mathf.Lerp(HealthbarLate.fillAmount, Health / Maxhealth, _lerpspeed);
    //        }            
    //        CountTime--;
    //    }
    //}


}
