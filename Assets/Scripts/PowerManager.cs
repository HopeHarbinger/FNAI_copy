using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PowerManager : MonoBehaviour {
    public Text textDisplay;

    float powerLeft = 100;

    public Image BatteryUsage;

    [SerializeField]
    int deviceUsage;
    [SerializeField]
    int baseUsage;

    float UsageAmount;

    [SerializeField]
    float powerFactor;

    public AudioSource source;
    public AudioClip alarm;

    GameManager gameManager;
    bool powerOutageHappening = false;

    List<DayData> days;

    private void Awake() {
        textDisplay.text = powerLeft + "%";
        gameManager = FindObjectOfType<GameManager>();
    }

    public void PowerUsageFactor(float plus) {
        powerFactor = plus;
    }

    public void AdjustUsage(int usage) {
        deviceUsage += usage;
    }

    private void FixedUpdate() {

        if (powerOutageHappening)
            return;

        if (powerLeft > 0) {

            int floor = (int)powerLeft;
            powerLeft -= Time.deltaTime * (baseUsage + deviceUsage) * powerFactor;

            if (floor != (int)powerLeft && floor <= 11) {
                source.PlayOneShot(alarm);//Toistaa myös audioefektin!
            }

        } else {

            powerOutageHappening = true;
            gameManager.PowerOutage();

        }

        //if (powerLeft > 0 && powerOutageHappening == false) {

        //    int floor = (int)powerLeft;
        //    powerLeft -= Time.deltaTime * (baseUsage + deviceUsage) * powerFactor;

        //    if (floor != (int)powerLeft && floor <= 10) {
        //        source.PlayOneShot(alarm);//Toistaa myös audioefektin!
        //    }

        //} else if (powerLeft <= 0 && powerOutageHappening == false) {

        //    powerOutageHappening = true;
        //    gameManager.PowerOutage();

        //} else return;

        UpdatePowerUI();

        textDisplay.text = ((int)powerLeft) + "%";

        UpdateUsageUI();
    }

    private void UpdatePowerUI() {

        if (powerLeft < 61 && powerLeft > 31) {

            textDisplay.color = new Color(1, 0.92f, 0.016f, 1);

        } else if (powerLeft < 31 && powerLeft > 11) {

            textDisplay.color = new Color(1, 0.64f, 0);

        } else if (powerLeft < 11) {

            textDisplay.color = new Color(1, 0, 0, 1);
        }
    }

    private void UpdateUsageUI() {  //Tämä osa visualisoi kuinka vaarallisella tasolla virran kulutus on!

        if (deviceUsage <= 0) {
            deviceUsage = 0;
            UsageAmount = 0f;
        } else if (deviceUsage == 1) {
            UsageAmount = 0.17f;
        } else if (deviceUsage == 2) {
            UsageAmount = 0.34f;
        } else if (deviceUsage == 3) {
            UsageAmount = 0.50f;
        } else if (deviceUsage == 4) {
            UsageAmount = 0.67f;
        } else if (deviceUsage == 5) {
            UsageAmount = 0.84f;
        } else {
            UsageAmount = 1f;
        }
        BatteryUsage.fillAmount = UsageAmount;
    }
}
