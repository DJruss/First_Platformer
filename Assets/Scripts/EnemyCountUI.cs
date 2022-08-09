using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyCountUI : MonoBehaviour
{
    
    public static int numOfEnemies;
    private Text enemies;

    void Start()
    {
        enemies = GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        enemies.text = "Enemies left: " + numOfEnemies;
    }
}
