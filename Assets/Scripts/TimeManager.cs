using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeManager : MonoBehaviour
{
    public TextMesh timeText;

    int currentHour = 0;

    GameManager gameManager;
    PowerManager powerManager;
    DataStorage dataStorage;

    [SerializeField] List<DayData> days;
    public HourData hourData;

    [SerializeField]
    float timer;

    float moveTimer = 0f;//Hirviöiden liiketttä varten
    int timeToMove = 4;

    bool call;
    public AudioSource source;
    public AudioClip PhoneCall;

    void StartNewHour() {
        
        hourData = days[dataStorage.currentDay].hours[currentHour];
        timeText.text = currentHour + " : 00";
    }

    void PlayPhoneCall() {

        call = days[dataStorage.currentDay].playPhoneCall;
        if (call == true) {
            source.PlayOneShot(PhoneCall);
            call = false;
        }
    }

    DataStorage InitializeStorage() {
        
        DataStorage ds = FindObjectOfType<DataStorage>();
        if (ds == null) {
            var go = new GameObject("DataStorage", typeof(DataStorage));
            DontDestroyOnLoad(go);
            ds = go.GetComponent<DataStorage>();

        }
        return ds;
    }
    private void Awake() {

        dataStorage = InitializeStorage();
        powerManager = FindObjectOfType<PowerManager>();
        gameManager = FindObjectOfType<GameManager>();
        StartNewHour();
        PlayPhoneCall();
        powerManager.PowerUsageFactor(days[dataStorage.currentDay].powerUsageFactor);
    }

    private void Update() {

        if ( moveTimer < timeToMove ) {

            moveTimer += Time.deltaTime;

        } else if (moveTimer > timeToMove) {

            gameManager.MonstersMove();
            moveTimer -= timeToMove;
        }

        //Alhaalla tunnit kuluu!

        if (timer <= days[dataStorage.currentDay].hourLenghts[currentHour]) 
        {
            timer += Time.deltaTime;
        } 
        else 
        {
            if (currentHour == days[dataStorage.currentDay].hourLenghts.Count - 1) {

                gameManager.EndGame("Victory");
                dataStorage.currentDay++;
                return; //TODO: Tee pelin voittamiselle toiminnot.
            }

            timer = 0;
            currentHour++;
            StartNewHour();
        } 
    }
}
