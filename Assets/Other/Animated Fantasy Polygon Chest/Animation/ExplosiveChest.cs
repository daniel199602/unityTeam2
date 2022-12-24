using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplosiveChest : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip boxKnock;

    UIManager uIManager;
    PlayerGetHit getHit = new PlayerGetHit();
    public LayerMask HitPlayer;
    ParticleSystem GetPower;
    ParticleSystem Booms;
    Animator ChestAnimator = new Animator();
    GameObject GetPowers;
    GameObject Bomb;
    int enable = 0;


    bool isInZone;
    // Start is called before the first frame update
    void Awake()
    {
        GetPowers = GameObject.Find("GetPower");
        Bomb = GameObject.Find("Explosion");
        GetPower = GetPowers.GetComponent<ParticleSystem>();
        Booms = Bomb.GetComponent<ParticleSystem>();
        GetPower.Stop();
        Booms.Stop();
        ChestAnimator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        uIManager = UIManager.Instance();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 PosF = this.transform.forward;
        Vector3 PosR = this.transform.right;
        Vector3 PosP = this.transform.position + (transform.up * 1.5f);
        Ray rayF = new Ray(PosP, PosF);
        Ray rayR = new Ray(PosP, PosR);
        //RaycastHit raycastHit;
        if (isInZone && Input.GetKeyDown(KeyCode.E))
        {
           ChestAnimator.SetBool("Open", true);
            Debug.Log("OPPN");
           
        }
        else
        {
            ChestAnimator.SetBool("Open", false);
        }
    }

    /// <summary>
    /// 寶箱選武器事件
    /// </summary>
    public void BoxChangeWeaponEvent()
    {
        uIManager.OpenOneOfThreeAndChooseWeapon();
        this.gameObject.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("in");
            isInZone = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            //Debug.Log("out");
            isInZone = false;
            
        }
    }



    private void OnDrawGizmos()
    {
        float fDetectLengh = 5.0f;
        float fDetectLenghs = 7.2f;
        Vector3 vpos = transform.position;
        Vector3 vec = transform.forward;
        Vector3 vecR = transform.right;
        Vector3 vUp = transform.up * 2;
        Vector3 vUpPos = (vpos + vUp);
        Gizmos.DrawLine(vUpPos, vUpPos + vec * fDetectLenghs);
        Gizmos.DrawLine(vUpPos, vUpPos + -vec * fDetectLenghs);
        Gizmos.DrawLine(vUpPos, vUpPos + vecR * fDetectLenghs);
        Gizmos.DrawLine(vUpPos, vUpPos + -vecR * fDetectLenghs);
        Gizmos.DrawLine(vUpPos, vUpPos + (vec + vecR) * fDetectLengh);
        Gizmos.DrawLine(vUpPos, vUpPos + (vec + -vecR) * fDetectLengh);
        Gizmos.DrawLine(vUpPos, vUpPos + (-vec + vecR) * fDetectLengh);
        Gizmos.DrawLine(vUpPos, vUpPos + (-vec + -vecR) * fDetectLengh);
        Gizmos.color = Color.red;
    }
    public void ObsorbPower()
    {
        GetPower.Play();
        //Debug.Log("1");
    }
    public void GetEnoughOfPower()
    {
        GetPower.Stop();
        //Debug.Log("2");
    }
    public void KaBoom()
    {
        Booms.Play();
        //Debug.Log("3");
    }
    public void ReClam()
    {
        Booms.Stop();
        this.gameObject.SetActive(false);
        //Debug.Log("4");
    }
    public void Boom()
    {
        Vector3 Pos = this.transform.forward;
        Vector3 PosU = this.transform.up * 1.5f;
        Vector3 PosP = this.transform.position + PosU;
        Ray ray = new Ray(PosP, Pos);
        RaycastHit raycastHit;
        if (Physics.Raycast(ray, out raycastHit, 5.0f, HitPlayer))
        {
            getHit.GetHitType_Damage(0);
        }
        else
        {
            return;
        }
    }

    /// <summary>
    /// 播放寶相搖晃音效
    /// </summary>
    void PlayBoxKnockEvent()
    {
        audioSource.PlayOneShot(boxKnock, 0.8f);
    }

}
