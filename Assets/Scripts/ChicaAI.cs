using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChicaAI : MonoBehaviour {

    [SerializeField]
    int aiLevel = 0;//Jos AI-level on suurempi kuin nopanheitto, AI liikkuu.

    [SerializeField]
    int nopanheitto;//Jos nopanheitto on suurempi kuin AI:n level, AI ei liiku.

    VasenOviNappi vasenOviNappi;
    EditValues editValues;
    GameManager gameManager;
    VasenValoNappi vasenValoNappi;

    int nextLocation = 0;
    int shouldIstay;

    public AudioSource Chica;//Chican oma AudioSource.
    public AudioSource source;//Player-AudioSource, johon syˆtet‰‰n screenGlitch-‰‰ni.
    public AudioClip screenGlitch;

    bool ChicaKopissa = false;//ƒ‰nten logiikkaa varten luotu.
    bool loopActivated = false;

    Transform[] locations;

    public Transform location0;//"Chican" ennalta m‰‰r‰tyt sijainnit.
    public Transform location1;
    public Transform location2;
    public Transform location3;
    public Transform location4;
    public Transform location5;
    public Transform location6;
    public Transform attackLocation;

    private void Awake() {

        vasenOviNappi = FindObjectOfType<VasenOviNappi>();
        editValues = FindObjectOfType<EditValues>();
        gameManager = FindObjectOfType<GameManager>();
        vasenValoNappi = FindObjectOfType<VasenValoNappi>();

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

    private void Update() {

        if (vasenValoNappi.vasenValoP‰‰ll‰ == true && loopActivated == false && ChicaKopissa != true) {

            loopActivated = true;
            Chica.Play();

        } else if (vasenValoNappi.vasenValoP‰‰ll‰ != true && loopActivated == true && ChicaKopissa != true) {

            loopActivated = false;
            Chica.Stop();

        } else return;
    }
    public void MoveChica(int level) {

        aiLevel = level;//T‰m‰n avulla toisesta scriptist‰ voidaan kasvattaa Chican AI-leveli‰.

        nopanheitto = Random.Range(0, 21);//Heitto m‰‰ritt‰‰ saako Chica liikkua.

        if (gameManager.jokuKopissa == true)//Pit‰isi est‰‰ bugit, myˆs menem‰st‰ pois kopista.
            return;

        if (aiLevel >= nopanheitto && nextLocation != 7) {

            if (nextLocation != 6) {

                transform.position = locations[nextLocation].position;
                transform.rotation = locations[nextLocation].rotation;
                nextLocation++;

                if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen"
                    editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
                    source.PlayOneShot(screenGlitch);
                }

            } else if (nextLocation == 6 && vasenValoNappi.lightOn != true || nextLocation == 6 && gameManager.screenOn == true) {

                transform.position = locations[nextLocation].position;
                transform.rotation = locations[nextLocation].rotation;
                nextLocation++;

                if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen" 
                    editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
                    source.PlayOneShot(screenGlitch);
                }
            }

        } else if (aiLevel > nopanheitto && nextLocation == 7 && vasenValoNappi.lightOn != true || aiLevel > nopanheitto && nextLocation == 7 && gameManager.screenOn == true) {

            if (vasenOviNappi.leftDoorClosed == false) {

                ChicaKopissa = true;//Stoppaa pesurummun ‰‰nen,
                Chica.Stop();//jotta pelaaja ei voi arvata Chican olevan kopissa.

                if (gameManager.highAngle == gameManager.currentAngle) {     //Luo visuaalisen "h‰m‰yksen" 
                    editValues.EffectsMax();                                 //kun pelaaja katsoo kameraa.
                    source.PlayOneShot(screenGlitch);
                }

                transform.position = locations[nextLocation].position;
                transform.rotation = locations[nextLocation].rotation;
                transform.parent = attackLocation.transform;
                gameManager.MonsterEnters("Chica");


            } else {
                
                shouldIstay = Random.Range(0, 1);
                if (shouldIstay == 1) {
                    print("I am dirty camper t: Chica :*");
                } else nextLocation = Random.Range(0, 6);
                
                //Jos hirviˆ ei onnistu p‰‰sem‰‰n koppiin, se palaa paikkaan x.
            }

        } else return;//Est‰‰ hirviˆt‰ jatkamasta liikett‰, jos hirviˆ on p‰‰ssyt p‰‰m‰‰r‰‰n.
    }
}
