using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnterHit : MonoBehaviour
{
    PlayerGetHit getHit;
    public LayerMask HitPlayer;
    ParticleSystem HeadEnable;
    ParticleSystem HeadEnables;
    ParticleSystem Booms;
    ParticleSystem Boomes;
    ParticleSystem Boomess;
    Animator RockAnimator = new Animator();
    GameObject LavaGuardHead;
    GameObject LavaGuardHead02;
    GameObject LavaGuardHit;
    GameObject LavaGuardHit02;
    GameObject LavaGuardGetPower;

    int state;

    // Start is called before the first frame update
    private void Awake()
    {
        LavaGuardHead = GameObject.Find("Magic fire 2 (1)");
        LavaGuardHead02 = GameObject.Find("Magic fire 2");
        LavaGuardHit = GameObject.Find("particle glow");
       // LavaGuardHit02 = GameObject.Find("particle sphere");
        LavaGuardGetPower = GameObject.Find("Explosioloop fallback");
        HeadEnable = LavaGuardHead.GetComponent<ParticleSystem>();
        HeadEnables = LavaGuardHead02.GetComponent<ParticleSystem>();
        Booms = LavaGuardGetPower.GetComponent<ParticleSystem>();
        Boomes = LavaGuardHit.GetComponent<ParticleSystem>();
        //Boomess = LavaGuardHit02.GetComponent<ParticleSystem>();
        Booms.Stop();
        Boomes.Stop();
        //HeadEnable.Stop();
        //HeadEnables.Stop();
        //Boomess.Stop();
        state = 0;
    }
    void Start()
    {
        RockAnimator = GetComponent<Animator>();
    }
    //void Update()
    //{
    //    Vector3 Pos = this.transform.forward;
    //    Vector3 PosU = this.transform.up * 1.5f;
    //    Vector3 PosP = this.transform.position + PosU;
    //    Ray ray = new Ray(PosP, Pos);
    //    RaycastHit raycastHit;        
    //    if (Physics.Raycast(ray,out raycastHit ,5.0f , HitPlayer))
    //    {
    //        state = 1;
    //        HeadEnable.Play();
    //        HeadEnables.Play();
    //        RockAnimator.SetBool("Enter",true);
    //    }
    //    else
    //    {
    //        state = 0;
    //        RockAnimator.SetBool("Enter", false);
    //    }
    //}
    //private void OnDrawGizmos()
    //{
    //    float fDetectLengh = 5.0f;
    //    Vector3 vec = transform.forward;
    //    Vector3 vpos = transform.position;
    //    Vector3 vUp = transform.up*1.5f;
    //    Gizmos.DrawLine(vpos+ vUp, (vpos + vUp) + vec* fDetectLengh);        
    //    Gizmos.color = Color.red;
    //}
    //public void GetPower()
    //{
    //    Booms.Play();
    //}
    //public void Boom()
    //{
    //    Boomes.Play();
    //    //Boomess.Play();
    //}
    //public void EndAttack() {
    //    HeadEnable.Stop();
    //    HeadEnables.Stop();
    //}
    //public void Attack()
    //{
    //    Vector3 Pos = this.transform.forward;
    //    Vector3 PosU = this.transform.up*1.5f;
    //    Vector3 PosP = this.transform.position + PosU;
    //    Ray ray = new Ray(PosP, Pos);
    //    RaycastHit raycastHit;       
    //    if (Physics.Raycast(ray, out raycastHit, 5.0f, HitPlayer))
    //    {
    //        getHit.GetHitType_Damage(0);
    //    }
    //    else
    //    {
    //        return;
    //    }
    //}
    
}
