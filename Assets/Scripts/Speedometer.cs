using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Speedometer : MonoBehaviour
{
    public CarControllerNew Speed;
    public TextMeshProUGUI KMH;

    private void Start()
    {
        KMH = this.GetComponent<TextMeshProUGUI>();
    }
    void LateUpdate()
    {
        KMH.SetText(Speed.speed.ToString("f0") + " KM/H");
    }
}
