using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager mInstance;
    public static UIManager Instance() { return mInstance; }

    public RectTransform weaponOneOfThreePanel;//�T��@���O
    public RectTransform weaponFramePanel;//�Z���歱�O

    public Dictionary<int, Sprite> dicIDWeaponImage;//ID �� �Z���� �r��

    bool isOpenOneOfThreeFrame = false;

    private void Awake()
    {
        if (mInstance != null)
        {
            Debug.LogErrorFormat(gameObject, "Multiple instances of {0} is not allow", GetType().Name);
            DestroyImmediate(gameObject);
            return;
        }
        mInstance = this;
        DontDestroyOnLoad(gameObject);


        dicIDWeaponImage = new Dictionary<int, Sprite>();
    }

    private void Start()
    {
        dicIDWeaponImage.Add(10, Resources.Load<Sprite>("w_10"));
        dicIDWeaponImage.Add(11, Resources.Load<Sprite>("w_11"));
        dicIDWeaponImage.Add(12, Resources.Load<Sprite>("w_12"));
        dicIDWeaponImage.Add(13, Resources.Load<Sprite>("w_13"));
        dicIDWeaponImage.Add(20, Resources.Load<Sprite>("w_20"));
        dicIDWeaponImage.Add(21, Resources.Load<Sprite>("w_21"));
        dicIDWeaponImage.Add(22, Resources.Load<Sprite>("w_22"));
        dicIDWeaponImage.Add(23, Resources.Load<Sprite>("w_23"));
        dicIDWeaponImage.Add(30, Resources.Load<Sprite>("w_30"));
        dicIDWeaponImage.Add(31, Resources.Load<Sprite>("w_31"));
        dicIDWeaponImage.Add(32, Resources.Load<Sprite>("w_32"));
        dicIDWeaponImage.Add(33, Resources.Load<Sprite>("w_33"));
        dicIDWeaponImage.Add(-1, Resources.Load<Sprite>("w_none"));
        dicIDWeaponImage.Add(-2, Resources.Load<Sprite>("w_close"));
    }

    private void Update()
    {
        //���եζ}����E
        if (Input.GetKeyDown(KeyCode.E))
        {
            isOpenOneOfThreeFrame = !isOpenOneOfThreeFrame;

            if (isOpenOneOfThreeFrame)
            {
                OneOfThreeUIOpen();
                weaponOneOfThreePanel.GetComponent<WeaponOneOfThreeUI>().SetRandomThreeWeaponR();//�]�m�k��Z���T��@
            }
            else
            {
                OneOfThreeUIClose();
            }
        }
        
    }

    /// <summary>
    /// �}�ҤT��@����
    /// </summary>
    public void OneOfThreeUIOpen()
    {
        weaponOneOfThreePanel.gameObject.SetActive(true);
        Time.timeScale = 0.001f;
    }

    /// <summary>
    /// �����T��@����
    /// </summary>
    public void OneOfThreeUIClose()
    {
        weaponOneOfThreePanel.gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

}
