using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BonnieAI : MonoBehaviour
{   
    [SerializeField]
    int aiLevel = 0;//Jos AI-level on suurempi kuin nopanheitto, AI liikkuu.
    
    [SerializeField]
    int nopanheitto;//Jos nopanheitto on suurempi kuin AI:n level, AI ei liiku.

    OikeaOviNappi oikeaOviNappi;
    EditValues editValues;
    GameManager gameManager;
    OikeaValoNappi oikeaValoNappi;

    int nextLocation = 0;
    int shouldIstay;

    public AudioSource Bonnie;//Bonnien oma AudioSource.
    public AudioSource source;//Player-AudioSource, johon syˆtet‰‰n screenGlitch-‰‰ni.
    public AudioClip screenGlitch;

    bool BonnieKopissa = false;//ƒ‰nten logiikkaa varten luotu.
    bool loopActivated = false;

    Transform[] locations;

    public Transform location0;//"Bonnien" ennalta m‰‰r‰tyt sijainnit.
    public Transform location1;
    public Transform location2;
    public Transform location3;
    public Transform location4;
    public Transform location5;
    public Transform location6;
    public Transform attackLocation;

    private void Awake() {

        oikeaOviNappi = FindObjectOfType<OikeaOviNappi>();
        editValues = FindObjectOfType<EditValues>();
        gameManager = FindObjectOfType<GameManager>();
        oikeaValoNappi = FindObjectOfType<OikeaValoNappi>();

        locations = new Transform[9];

        locations[0] = location0;
        locations[1] = location1;
        locations[2] = location2;
        locations[3] = location3;
        locations[4] = location4;
        locations[5] = location5;
        locations[6] = location6;
        locations[7] = attackLocation;
    }

    private void FixedUpdate() {

        if (oikeaValoNappi.oikeaValoP‰‰ll‰ == true && loopActivated == false && BonnieKopissa != true) {

            loopActivated = true;
            Bonnie.Play();

        } else if (oikeaValoNappi.oikeaValoP‰‰ll‰ != true && loopActivated == true && BonnieKopissa != true) {

            loopActivated = false;
            Bonnie.Stop();

        } else return;
    }
    public void MoveBonnie(int level) {
        
        aiLevel = level;//T‰m‰n avulla toisesta scriptist‰ voidaan kasvattaa Bonnien AI-leveli‰.

        nopanheitto = Random.Range(0, 21);//Heitto m‰‰ritt‰‰ saako Bonnie liikkua.
        
        if (gameManager.jokuKopissa == true)//Pit‰isi est‰‰ bugit, myˆs menem‰st‰ pois kopista.
            return;

        if (aiLevel >= nopanheitto && nextLocation != 7) {

            if (nextLocation !=6) {

                transform.position = locations[nextLocation].position;
                transform.rotation = locations[nextLocation].rotation;
                nextLocation++;

                if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen"
                    editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
                    source.PlayOneShot(screenGlitch);
                }

            } else if (nextLocation == 6 && oikeaValoNappi.lightOn != true || nextLocation == 6 && gameManager.screenOn == true) {

                transform.position = locations[nextLocation].position;
                transform.rotation = locations[nextLocation].rotation;
                nextLocation++;

                if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen" 
                    editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
                    source.PlayOneShot(screenGlitch);
                }
            }

        } else if (aiLevel > nopanheitto && nextLocation == 7 && oikeaValoNappi.lightOn != true || aiLevel > nopanheitto && nextLocation == 7 && gameManager.screenOn == true) {

            if (oikeaOviNappi.rightDoorClosed == false) {

                BonnieKopissa = true;//Stoppaa pesurummun ‰‰nen,
                Bonnie.Stop();//jotta pelaaja ei voi arvata Bonnien olevan kopissa.
                
                if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen" 
                    editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
                    source.PlayOneShot(screenGlitch);
                }

                transform.position = locations[nextLocation].position;
                transform.rotation = locations[nextLocation].rotation;
                transform.parent = attackLocation.transform;
                gameManager.MonsterEnters("Bonnie");
                
            } else {
                
                shouldIstay = Random.Range(0, 2);
                if (shouldIstay == 1) {
                    print("I am dirty camper t: Bonnie :P");
                } else nextLocation = Random.Range(0, 6);

                //Jos hirviˆ ei j‰‰ tai onnistu p‰‰sem‰‰n koppiin, se palaa paikkaan x.
            }

        } else return;//Est‰‰ hirviˆt‰ jatkamasta liikett‰, jos hirviˆ on p‰‰ssyt p‰‰m‰‰r‰‰n.
    }
}
