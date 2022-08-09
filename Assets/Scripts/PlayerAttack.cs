using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{

    private float timeBtwAttack;
    public float startTimeBtwAttack;

    private PlayerStats playerStats;

    public LayerMask whatIsEnemy;
    public Transform attackPos;
    public Animator playerAnim;
    public float attackRange;
    public int damage;

    void Update()
    {
        if(!playerStats.IsDead())
        {
            if(timeBtwAttack <= 0 && !Input.GetKey(KeyCode.LeftShift)) {
                if(Input.GetKey(KeyCode.Space) && playerAnim.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.2 && !playerAnim.IsInTransition(0)) {
                    playerAnim.SetTrigger("Attack1");
                    Collider2D[] enemiesToDmg = Physics2D.OverlapCircleAll(attackPos.position, attackRange, whatIsEnemy);
                    for (int i = 0; i < enemiesToDmg.Length; i++)
                    {
                        enemiesToDmg[i].GetComponent<Enemy>().TakeDamage(damage);
                    }

                    timeBtwAttack = startTimeBtwAttack;
                }
            } else {
                timeBtwAttack -= Time.deltaTime;
            }
        }
    }

    void OnDrawGizmosSelected() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

    void Awake() {
        playerStats = GetComponent<PlayerStats>();
    }
}
