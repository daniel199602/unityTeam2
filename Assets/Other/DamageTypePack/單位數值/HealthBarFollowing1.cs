using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarFollowing1 : MonoBehaviour
{   
    public RectTransform bloodTransform;
    public RectTransform frameTransform;
    public Transform Head;
    HealthBar M_HealthBar;
    private void Start()
    {
        M_HealthBar = GetComponent<HealthBar>();
    }
    void Update()
    {
        Vector2 player2DPosition = Camera.main.WorldToScreenPoint(transform.position);
        Vector2 HeadPosition = Camera.main.WorldToScreenPoint(Head.position);
       

        bloodTransform.position = HeadPosition;
        frameTransform.position = HeadPosition;
        //Debug.Log("螢幕長度" + Screen.width+"螢幕寬度" + Screen.height);
        //Debug.Log(bloodTransform.localScale);
        
        //Vector2 ScaleValue = new Vector2((Screen.width / Screen.height), (Screen.width / Screen.height));
        //Debug.Log(ScaleValue);
        //bloodTransform.localScale = ScaleValue;
        //frameTransform.localScale = ScaleValue;

        //血條超出螢幕        
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            bloodTransform.gameObject.SetActive(false);
            frameTransform.gameObject.SetActive(false);
        }
        else
        {
            bloodTransform.gameObject.SetActive(true);
            frameTransform.gameObject.SetActive(true);
        }
        if (M_HealthBar.Healthbar.fillAmount <= 0)
        {
            bloodTransform.gameObject.SetActive(false);
            frameTransform.gameObject.SetActive(false);
        }
    }
}
