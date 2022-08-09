using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveCounterUI : MonoBehaviour
{
    public static int numOfWaves;
    public static int maxWaves;
    private Text waves;

    void Start()
    {
        waves = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        waves.text = "Wave: " + numOfWaves + " / " + maxWaves;
    }
}
