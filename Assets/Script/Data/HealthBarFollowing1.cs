using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HealthBarFollowing1 : MonoBehaviour
{   
    public RectTransform bloodTransform;
    public RectTransform bloodDelayTransform;
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
       

        //bloodTransform.position = HeadPosition;
        //bloodDelayTransform.position = bloodTransform.position;
        frameTransform.position = HeadPosition;

        //血條超出螢幕        
        if (player2DPosition.x > Screen.width || player2DPosition.x < 0 || player2DPosition.y > Screen.height || player2DPosition.y < 0)
        {
            bloodTransform.gameObject.SetActive(false);
            bloodDelayTransform.gameObject.SetActive(false);
            frameTransform.gameObject.SetActive(false);
        }
        else
        {
            bloodTransform.gameObject.SetActive(true);
            bloodDelayTransform.gameObject.SetActive(true);
            frameTransform.gameObject.SetActive(true);
        }
        if (M_HealthBar.Healthbar.fillAmount <= 0)
        {
            bloodTransform.gameObject.SetActive(false);
            bloodDelayTransform.gameObject.SetActive(false);
            frameTransform.gameObject.SetActive(false);
        }
    }
}
