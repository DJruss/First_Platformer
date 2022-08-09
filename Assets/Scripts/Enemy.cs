using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private int enemyMaxHealth = 100;
    public int enemyCurrentHealth;

    public HealthBar healthBar;
    public Animator anim;

    // Start is called before the first frame update
    void Start()
    {
        enemyCurrentHealth = enemyMaxHealth;
        healthBar.SetMaxHealth(enemyMaxHealth);
        anim = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if(IsDead()) {
            Death();
        }
    }

    public void TakeDamage(int damage) {
        if(enemyCurrentHealth > 0) {
            enemyCurrentHealth -= damage;
            anim.SetTrigger("Hurt");
            healthBar.SetHealth(enemyCurrentHealth);
            if (enemyCurrentHealth <= 0) {
                EnemyCountUI.numOfEnemies += -1;
            }
        }
    }

    private void Death() {
        anim.SetTrigger("Death");
        
        Destroy(gameObject, 5);
    }

    public bool IsDead() {
        if (enemyCurrentHealth <= 0) {
            return true;
        }
        else {
            return false;
        }
    }
}
