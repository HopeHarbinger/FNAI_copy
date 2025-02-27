using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OikeaValoNappi : MonoBehaviour
{
    [SerializeField]
    int valonvirrankulutus = 1;

    VasenValoNappi vasenValo;
    PowerManager powerUse;

    public Light valo;
    public GameObject pimeys;

    public bool oikeaValoP‰‰ll‰;//toista valokatkaisijaa varten.

    public bool lightOn = false;//mˆrkˆj‰ varten.

    public AudioSource sourceOikeaNappi;
    public AudioClip nappi;

    private void Awake() {
        vasenValo = FindObjectOfType<VasenValoNappi>();
        powerUse = FindObjectOfType<PowerManager>();
    }

    void OnMouseUp() {

        if (lightOn == false && vasenValo.vasenValoP‰‰ll‰ == false) {
            
            lightOn = true;
            oikeaValoP‰‰ll‰ = true;

            powerUse.AdjustUsage(valonvirrankulutus);

            valo.enabled = true;
            pimeys.SetActive(false);

            sourceOikeaNappi.Stop();//Est‰‰ overlapin
            sourceOikeaNappi.PlayOneShot(nappi);

        } else if (lightOn == true) {
            
            lightOn = false;
            oikeaValoP‰‰ll‰ = false;

            powerUse.AdjustUsage(-valonvirrankulutus);

            valo.enabled = false;
            pimeys.SetActive(true);

            sourceOikeaNappi.Stop();//Est‰‰ overlapin
            sourceOikeaNappi.PlayOneShot(nappi);
        }
    }
}
