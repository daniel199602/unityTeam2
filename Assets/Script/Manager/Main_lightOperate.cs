using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Main_lightOperate : MonoBehaviour
{
    [SerializeField] GameObject goDirectionalLight;
    [SerializeField] GameObject goHandTorch;
    [SerializeField] GameObject shadow;
    Light _directionalLight;
    Light _handTorch;
   // MeshRenderer _shadow;
    
    float handTorchRange;
    Color color0 = new Color (229f/255f, 219f/255f, 219f/255f);
    Color color1 = new Color (248f/255f, 22f/255f , 186f/255f);
    
    private float colorTime;
   
   

    bool firetest;
    private void Start()
    {
        _directionalLight = goDirectionalLight.GetComponent<Light>();
        _handTorch = goHandTorch.GetComponent<Light>();
       // _shadow = shadow.GetComponent<MeshRenderer>();
    }

    public void FireFalse()
    {
        _handTorch.intensity = 2;
        _handTorch.range = 35;
       //_shadow.materials[0].SetFloat("Shadow_A", 0.2f);
    }
    public void FireLight()
    {
        _handTorch.intensity = 10;
        _handTorch.range = 100;
        //_shadow.materials[0].SetFloat("Shadow_A", 0.7f);
    }
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            FireLight();
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && WeaponManager.Instance().CurrentWeaponR_weaponR)
        {
            FireFalse();
        }
        
    }
   
}
