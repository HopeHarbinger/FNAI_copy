using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VasenValoNappi : MonoBehaviour
{   
    [SerializeField]
    int valonvirrankulutus = 1;

    OikeaValoNappi oikeaValo;
    PowerManager powerUse;

    public Light valo;
    public GameObject pimeys;

    public bool vasenValoPäällä = false;//toista valokatkaisijaa varten.

    public bool lightOn = false;//mörköjä varten.

    public AudioSource sourceVasenNappi;
    public AudioClip nappi;

    private void Awake() {
        oikeaValo = FindObjectOfType<OikeaValoNappi>();
        powerUse = FindObjectOfType<PowerManager>();
    }

    void OnMouseUp() {

        if (lightOn == false && oikeaValo.oikeaValoPäällä == false ) {
            
            lightOn = true;
            vasenValoPäällä = true;

            powerUse.AdjustUsage(valonvirrankulutus);

            valo.enabled = true;
            pimeys.SetActive(false);

            sourceVasenNappi.Stop();
            sourceVasenNappi.PlayOneShot(nappi);

        } else if (lightOn == true) {
            
            lightOn = false;
            vasenValoPäällä = false;

            powerUse.AdjustUsage(-valonvirrankulutus);

            valo.enabled = false;
            pimeys.SetActive(true);

            sourceVasenNappi.Stop();
            sourceVasenNappi.PlayOneShot(nappi);
        }
    }
}
