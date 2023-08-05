using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_lightOperate : MonoBehaviour
{
    [SerializeField] GameObject goHandTorch;
    Light _handTorch;
   
    
    float handTorchRange;
    Color color0 = new Color (229f/255f, 219f/255f, 219f/255f);
    Color color1 = new Color (248f/255f, 22f/255f , 186f/255f);
    
    private float colorTime;
   
   

    bool firetest;
    private void Start()
    {
       
        _handTorch = goHandTorch.GetComponent<Light>();
      
    }

    public void FireFalse()
    {
        _handTorch.intensity = 2;
        _handTorch.range = 35;
    }
    public void FireLight()
    {
        _handTorch.intensity = 6;
        _handTorch.range = 80;
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FireLight();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponManager.Instance.CurrentWeaponR_weaponR)
        {
            FireFalse();
        }
        
    }
   
}
