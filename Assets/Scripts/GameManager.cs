using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

    public Transform ruutu;
    public GameObject laitteet;
    public Light KopinValo;
    public Transform VasenOvi;
    public Transform OikeaOvi;

    public GameObject napit;
    public GameObject feikki;
    public AudioSource radio;

    float lowAngle = 0;
    public float highAngle = -180;
    float rotateSeconds = 0.5f;

    CameraManager cameraManager;
    PowerManager powerUse;
    PlayerCamera playerCamera;
    TimeManager timeManager;
    ChicaAI chicaAI;
    BonnieAI bonnieAI;
    FoxyAI foxyAI;

    int kamerojenvirrankulutus = 1;

    public float currentAngle = 0;

    public bool screenOn = false;

    public bool jokuKopissa = false;//Hyökkäykseen liittyviä tietotyyppejä.
    int attackDelay;
    float timeToAttack = 0f;
    public Transform attackLocation;
    public Transform player;
    float hyökkäysVauhti = 0.1f;
    public AudioClip WashingMachineAttack;
    string attacker;

    float cameraRotationTime;
    public float cameraRotationEnd;
    Quaternion initialRotation;
    public Transform foxyFinalRotation;
    public Transform otherFinalRotations;
    public AnimationCurve attackCam;

    bool avaaOvet = false;
    float sulkuVauhti = 0.05f;

    public AudioSource source;//Äänet joita GameManager soittaa!
    bool pHubPlayed = false;
    public AudioClip pHub;
    public AudioClip poweringDown;
    public AudioClip vacuumAttack;
    public AudioClip washingMachineAttack;

    bool alreadyEndedIt;
    string EndEventId;
    public GameObject BonnieScreen;
    public GameObject ChicaScreen;
    public GameObject FoxyScreen;
    public GameObject VictoryScreen;
    
    private void Awake() {

        timeManager = FindObjectOfType<TimeManager>();
        cameraManager = FindObjectOfType<CameraManager>();
        powerUse = FindObjectOfType<PowerManager>();
        playerCamera = FindObjectOfType<PlayerCamera>();
        bonnieAI = FindObjectOfType<BonnieAI>();
        chicaAI = FindObjectOfType<ChicaAI>();
        foxyAI = FindObjectOfType<FoxyAI>();

    }

    public void MonstersMove() {

        var hd = timeManager.hourData;
        
        chicaAI.MoveChica(hd.ChicasAIlevel);
        bonnieAI.MoveBonnie(hd.BonniesAIlevel);
        foxyAI.MoveFoxy(hd.FoxysAIlevel);

    }

    public void MonsterEnters(string monster) {//monsterEnters(enum "hirviön nimi") ??

        initialRotation = player.rotation;

        attacker = monster;
        
        jokuKopissa = true;
        
        attackDelay = Random.Range(7, 20);
    }

    public void PowerOutage() {//Sulkee sähköt (UI-laitteet, avaa ovet, sulkee valot...

        screenOn = false; //Poistavat UI-elementit ja turvakameroiden käyttömahdollisuuden.
        cameraManager.ShutCamera();
        laitteet.SetActive(false);
        
        KopinValo.enabled = false;//Poistaa kaikki valot.

        napit.SetActive(false);//Poistavat käytöstä napit ja niihin liittyvät toiminnot!
        feikki.SetActive(true);
        radio.enabled = false;//Sulkee radion kun sähköt katkeavat

        avaaOvet = true;//Pakottaa kopin ovet auki sähkökatkoksessa!

        source.PlayOneShot(poweringDown);
        source.Play();
    }


    private void Update() {

        Vector3 ylös = Vector3.up * sulkuVauhti;      //Avaa ovet sähkökatkoksessa.

        if (avaaOvet == true && VasenOvi.position.y <= 12f) {
            VasenOvi.position += ylös;
        }
        if (avaaOvet == true && OikeaOvi.position.y <= 12f) {
            OikeaOvi.position += ylös;
        }

        //Alhaalla on toiminnallisuuksia ruudun toiminnalle.

        float targetAngle = screenOn ? highAngle : lowAngle;
        float diff = Mathf.Abs(highAngle - lowAngle);
        float speed = diff / rotateSeconds;

        if (!Mathf.Approximately(currentAngle, targetAngle)) {
            currentAngle = Mathf.MoveTowards(currentAngle, targetAngle, speed * Time.deltaTime);
            ruutu.localRotation = Quaternion.Euler(currentAngle, 0, 0);
            if (Mathf.Approximately(currentAngle, targetAngle) && screenOn == true) {
                cameraManager.OpenCamera(1);
            }
        }
    }

    public void EndGame(string tapahtuma) {

        EndEventId = tapahtuma;

        rotateSeconds = 0.1f;
        screenOn = false; //Poistavat UI-elementit ja turvakameroiden käyttömahdollisuuden.

        cameraManager.ShutCamera();
        laitteet.SetActive(false);

        napit.SetActive(false);//Poistavat käytöstä napit ja niihin liittyvät toiminnot!
        feikki.SetActive(true);
        radio.enabled = false;//Sulkee radion

        if (EndEventId == "Victory" && alreadyEndedIt == false) {

            alreadyEndedIt = true;
            Invoke("EndScreenInvoke", 0f);
        }

        if (EndEventId == "Foxy" && alreadyEndedIt == false) {

            alreadyEndedIt = true;
            Invoke("EndScreenInvoke", 4f);
        }

        if (EndEventId == "Bonnie" && alreadyEndedIt == false) {

            alreadyEndedIt = true;
            source.PlayOneShot(washingMachineAttack);
            Invoke("EndScreenInvoke", 4f);
        }

        if (EndEventId == "Chica" && alreadyEndedIt == false) {

            alreadyEndedIt = true;
            source.PlayOneShot(vacuumAttack);
            Invoke("EndScreenInvoke", 4f);
        }
    }

    public void EndScreenInvoke() {

        source.Stop();
        Time.timeScale = 0;

        if (EndEventId == "Victory") {
            VictoryScreen.SetActive(true);
        }

        if (EndEventId == "Foxy") {
            FoxyScreen.SetActive(true);
        }

        if (EndEventId == "Bonnie") {
            BonnieScreen.SetActive(true);
        }

        if (EndEventId == "Chica") {
            ChicaScreen.SetActive(true);
        }
    }

    private void FixedUpdate() {//Vihollisille, jotta voivat pysähtyä 0 timeScalessa

        if (jokuKopissa != true)
            return;

        if (attacker == "Foxy") {

            playerCamera.enabled = false;
            cameraRotationTime += Time.deltaTime;
            var t = cameraRotationTime / cameraRotationEnd;
            t = attackCam.Evaluate(t);
            var rot = Quaternion.Slerp(initialRotation, foxyFinalRotation.rotation, t);
            player.rotation = rot;

            EndGame("Foxy");
        }

        Vector3 hyökkäys = Vector3.up * hyökkäysVauhti;

        if (attacker == "Bonnie") {

            if (timeToAttack < attackDelay) {

                timeToAttack += Time.deltaTime;

            } else {

                if (attackLocation.position.y <= 2.3f) {
                    attackLocation.position += hyökkäys;
                }

                playerCamera.enabled = false;
                cameraRotationTime += Time.deltaTime;
                var t = cameraRotationTime / cameraRotationEnd;
                t = attackCam.Evaluate(t);
                var rot = Quaternion.Slerp(initialRotation, otherFinalRotations.rotation, t);
                player.rotation = rot;

                EndGame("Bonnie");
            }
        }

        if (attacker == "Chica") {

            if (timeToAttack < attackDelay) {

                timeToAttack += Time.deltaTime;

            } else {

                if (attackLocation.position.y <= 2.3f) {
                    attackLocation.position += hyökkäys;
                }

                playerCamera.enabled = false;
                cameraRotationTime += Time.deltaTime;
                var t = cameraRotationTime / cameraRotationEnd;
                t = attackCam.Evaluate(t);
                var rot = Quaternion.Slerp(initialRotation, otherFinalRotations.rotation, t);
                player.rotation = rot;

                EndGame("Chica");
            }
        }
    }

    public void Screen() {//Antaa laittaa päälle turvakameratilan jos on virtaa
        
        if (screenOn == false) {
            screenOn = true;
            
            powerUse.AdjustUsage(kamerojenvirrankulutus);
            
            if (pHubPlayed == false) {
                source.PlayOneShot(pHub);
                pHubPlayed = true;
            }

        } else if (screenOn == true) {
            
            screenOn = false;
            cameraManager.ShutCamera();
            powerUse.AdjustUsage(-kamerojenvirrankulutus);
        }
    }
}

