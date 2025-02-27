using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxyAI : MonoBehaviour
{
    int aiLevel = 0;//Jos AI-level on nolla, Foxy ei tee mit‰‰n. AI-level myˆs v‰hennet‰‰n attack buildupista.

    int pointOfAttack;//Hyˆkk‰ysajastinta varten;
    float attackBuildup = 0; //VAIHE 1: attackBuildupin t‰ytyy kasvaa pointOfAttackin yli!
    
    int pointOfRunning = 10;
    float runningDelay; //VAIHE 2: Foxy odottaa, ett‰ runningDelay on t‰ynn‰ tai ett‰ pelaaja katsoo valvomon vasenta k‰yt‰v‰‰!

    bool nytJuoksen = false;//VAIHE 3: Foxy l‰htee liikkumaan location1 --> location2 ja kun paikka on location2 siirryt‰‰n vaiheeseen 4

    bool yrit‰nKurkata = false; //VAIHE 4: Foxy testaa onko ovi auki. Jos ei, Foxy siirtyy resettiin kun waitAtDoor = 5.

    float waitAtDoor = 0f;//VAIHE 5: waitAtDoor kasvaa ja jos on = reset, Foxy asetetaan alkutilaan.
    int reset = 5;

    EditValues editValues;
    GameManager gameManager;
    CameraManager cameraManager;

    public AudioSource ovi; //Koputuksen suuntaa varten.
    public AudioSource source;//Player-AudioSource, johon syˆtet‰‰n FoxyRunning-‰‰ni.
    bool FoxyRunningPlayed = false;//Jotta screen Glitch ei sp‰mm‰‰ pelaajan katsoessa Foxyn juoksukameraan.
    public AudioClip FoxyRunning;
    public AudioClip screenGlitch;
    public AudioClip KnockKnockKnock;
    public AudioClip DunDunDUN;

    public Transform location0;//"Foxyn" ennalta m‰‰r‰tyt sijainnit.
    public Transform location1;
    public Transform location2;

    public Transform vasenOvi;

    private void Awake() {

        editValues = FindObjectOfType<EditValues>();
        gameManager = FindObjectOfType<GameManager>();
        cameraManager = FindObjectOfType<CameraManager>();
    }
    
    public void MoveFoxy(int level) {
        
        aiLevel = level;
        pointOfAttack = 31 - aiLevel;
    }

    private void Update() {

        if (gameManager.jokuKopissa == true)
            return;

        if (aiLevel > 0 && attackBuildup < pointOfAttack) {

            if (cameraManager.cameras[0].enabled == false) {

                attackBuildup += Time.deltaTime;

            } else return;

        } 
        
        if (attackBuildup > pointOfAttack && nytJuoksen == false) {

            if (runningDelay > pointOfRunning && nytJuoksen == false || cameraManager.cameras[2].enabled && nytJuoksen == false) {

                transform.position = location1.position;
                nytJuoksen = true;

            } else runningDelay += Time.deltaTime;

            //TƒSTƒ ALKAA LAUSEKE VISUAALISELLE EFEKTILLE JA VAROITUSƒƒNELLE!

            if (cameraManager.cameras[2].enabled && FoxyRunningPlayed == false && nytJuoksen == true) {
                
                FoxyRunningPlayed = true;                              
                editValues.EffectsMax();                                
                source.PlayOneShot(FoxyRunning);
            }
        }

        //AI:n seuraavat vaiheet!

        if (nytJuoksen == true && yrit‰nKurkata == false) {

            transform.position = Vector3.MoveTowards(transform.position, location2.position, 20f * Time.deltaTime);
        }

        if (transform.position == location2.position) {

            yrit‰nKurkata = true;
            if (yrit‰nKurkata == true) {
                waitAtDoor += Time.deltaTime;
            }

            if (vasenOvi.position.y >= 12f) {

                gameManager.MonsterEnters("Foxy");

                transform.rotation = location2.rotation;

                source.PlayOneShot(DunDunDUN);

            } else if (waitAtDoor > reset) {

                ResetFoxy();
            }
        }
    }

    private void ResetFoxy() {
        if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen" 
            editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
            source.PlayOneShot(screenGlitch);
        }

        ovi.PlayOneShot(KnockKnockKnock);

        transform.position = location0.position;
        FoxyRunningPlayed = false;

        attackBuildup = 0; // VAIHE 1: NOLLATTU
        runningDelay = 0; // VAIHE 2: NOLLATTU
        waitAtDoor = 0;// VAIHE 3: NOLLATTU
        nytJuoksen = false;//VAIHE 4: NOLLATTU
        yrit‰nKurkata = false; //VAIHE 5: NOLLATTU
    }
}
