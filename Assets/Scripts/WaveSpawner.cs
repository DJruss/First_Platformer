using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public enum SpawnState {Spawning, Waiting, Counting};

    [System.Serializable]
    public class Wave {
        public string name;
        public Transform enemy;
        public int count;
        public float rate;
    }

    public Wave[] waves;
    public Transform[] spawnPoints;
    private int nextWave = 0;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    private SpawnState state = SpawnState.Counting;
    private float searchCountdown = 1f;
    private int currentWaves;

    void Start() {
        if (spawnPoints.Length == 0) {
            Debug.LogError ("No Spawn points references");
        }

        waveCountdown = timeBetweenWaves;
        WaveCounterUI.maxWaves = waves.Length;
    }

    void Update() {
        if (state == SpawnState.Waiting) {
            if (!EnemyIsAlive()) {
                WaveCompleted();

            }else {
                return;
            }
        }

        if (waveCountdown <= 0) {
            if (state != SpawnState.Spawning) {
                StartCoroutine( SpawnWave (waves[nextWave]));
                currentWaves += 1;
                WaveCounterUI.numOfWaves = currentWaves;
            }
        }else {
            waveCountdown -= Time.deltaTime; 
        }
    }

    void WaveCompleted() {
        state = SpawnState.Counting;
        waveCountdown = timeBetweenWaves;

        if ((nextWave + 1) > (waves.Length - 1)) {

            FindObjectOfType<GameManager>().WonGame();
            FindObjectOfType<GameManager>().CompleteLevelUI();

            nextWave = 0;
            Debug.Log("All Waves Complete");
        }else {
            nextWave++;
        }
    }

    bool EnemyIsAlive() {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f) {
            searchCountdown = 1f;

            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave (Wave _wave) {
        Debug.Log("Spawning Wave" + _wave.name);
        state = SpawnState.Spawning;

        for (int i = 0; i < _wave.count; i++)
        {
            EnemyCountUI.numOfEnemies += 1;
            SpawnEnemy(_wave.enemy);
            yield return new WaitForSeconds(1/_wave.rate);
        }

        state = SpawnState.Waiting;

        yield break;
    }

    void SpawnEnemy (Transform _enemy) {

        //Instantiate at 0.0.0
        Transform _sp = spawnPoints[ Random.Range(0, spawnPoints.Length)];

        
        Instantiate(_enemy, _sp.position, _sp.rotation);
        Debug.Log("Spawning Enemy = " + _enemy.name);
    }

    public int GetWaveSize() {
        return waves.Length;
    }
    public int GetCurrentWave() {
        return currentWaves;
    }
}
