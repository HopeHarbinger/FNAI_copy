using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "DayData")]
public class DayData : ScriptableObject {
    
    public List<HourData> hours;
    public List<int> hourLenghts;
    public float powerUsageFactor;
    public float excessPower;
    public bool playPhoneCall;
}
