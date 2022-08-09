using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{

    [SerializeField] private int playerMaxHealth = 100;
    public int playerCurrentHealth;

    [Header ("IFrames")]
    [SerializeField] private float iFramesDuration;
    [SerializeField] private int numberOfFlashes;

    private bool coRoutRunning;

    private SpriteRenderer spriteRend;
    public HealthBar healthBar;
    private Animator anim;
    private PlayerMovement playMoveScpt;

    void Start()
    {
        playerCurrentHealth = playerMaxHealth;
        healthBar.SetMaxHealth(playerMaxHealth);
        anim = GetComponent<Animator>();
        playMoveScpt = this.GetComponent<PlayerMovement>();
        spriteRend = GetComponent<SpriteRenderer>();
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) {
            DmgTaken (50, playMoveScpt.isImmune);
        }

        if (IsDead()) {
            Death();
        }
    }

    public void DmgTaken(int dmg, bool isImmune) {

        if (!isImmune && !coRoutRunning) {
            playerCurrentHealth -= dmg;
            anim.SetTrigger("Hurt");
            StartCoroutine(Invulnerability());
        }

        healthBar.SetHealth(playerCurrentHealth);
    }

    private void Death() {
        anim.SetTrigger("Death");
        GameManager.maxWaves = FindObjectOfType<WaveSpawner>().GetWaveSize();
        GameManager.waves = FindObjectOfType<WaveSpawner>().GetCurrentWave();
        FindObjectOfType<GameManager>().LoseGame();
        FindObjectOfType<GameManager>().CompleteLevelUI();
    }

    public bool IsDead() {
        if (playerCurrentHealth <= 0) {
            return true;
        }
        else {
            return false;
        }
    }

    private IEnumerator Invulnerability() {
        coRoutRunning = true;
        for (int i = 0; i < numberOfFlashes; i++)
        {
            spriteRend.color = new Color(1, 0, 0, 0.5f);
            yield return new WaitForSeconds(iFramesDuration/ (numberOfFlashes * 2));
            spriteRend.color = Color.white;
            yield return new WaitForSeconds(iFramesDuration/ (numberOfFlashes * 2));
        }
        coRoutRunning = false;
    }
}
