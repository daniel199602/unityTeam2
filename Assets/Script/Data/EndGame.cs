using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndGame : MonoBehaviour
{
    bool isInZone;
    bool EndVFXing;
    public GameObject EndVFX;
    ParticleSystem ps;
    // Start is called before the first frame update
    void Start()
    {
        isInZone = false;
        EndVFXing = false;
        ps = EndVFX.GetComponent<ParticleSystem>();
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E)&& isInZone)
        {
            if (EndVFXing == false)
            {
                ps.Play();
                EndVFXing = true;
            }
            Invoke(nameof(EndToGameMenu), 3);
            

        }
    }

    void EndToGameMenu()
    {
        SceneManager.LoadScene("GameMenu");
        UIManager.Instance().GameMenuPanelOpen();
        UIManager.Instance().QuitGameUIClose();
        GameManager.Instance().MobPoolClear();
        GameManager.Instance().PlayerSetActiveSwitch(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        isInZone = true;
    }

    private void OnTriggerExit(Collider other)
    {
        isInZone = false;
    }
}
