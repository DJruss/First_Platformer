using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class EnemyAI : MonoBehaviour
{

    private enum State {
        Chasing,
        Attacking,
    }

    [Header ("Pathfinding")]
    public Transform target;
    public float speed = 400f;
    public float nextWaypointDistance = 1f;

    [Header ("Attack")] 
    [SerializeField] private BoxCollider2D enemyAtkPos;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float attackCooldown;
    [SerializeField] private int damage;
    [SerializeField] private float range;
    [SerializeField] private float colliderDistance;
    private float coolDownTimer = Mathf.Infinity;

    //References
    private Animator anim;
    private PlayerStats playerHealth;

    private State state;

    Path path;
    int currentWaypoint = 0;

    private PlayerMovement playMoveScpt;
    private Enemy enemyScript;

    Seeker seeker;
    Rigidbody2D rb;

    private void Awake() {
        anim = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        seeker = GetComponent<Seeker>();
        rb = GetComponent<Rigidbody2D>();
        enemyScript = GetComponent<Enemy>();

        InvokeRepeating("UpdatePath", 0f, 0.5f);
        state = State.Chasing;
        anim.SetInteger("AnimState", 1);
    }

    void OnEnable () {
        target = GameObject.Find("Player").transform;
    }

    void UpdatePath() {
        if(seeker.IsDone()){
            seeker.StartPath(rb.position, target.position, OnPathComplete);
        }
    }

    void OnPathComplete(Path p) {
        if(!p.error){
            
            path = p;
            currentWaypoint = 0;
        }
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (state == State.Chasing) {
            anim.SetInteger("AnimState", 2);
            if(path == null) {
                return;
            }

            //Determining direction and force
            Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
            Vector2 force = direction * speed * rb.mass * rb.drag;

            //adding force to the object.
            force.y = 0f;
            if(enemyScript.enemyCurrentHealth > 0) {
                rb.AddForce(force);
            }
            else {
                force.x = 0;
            }

            float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

            if(distance < nextWaypointDistance) {
                currentWaypoint++;
            }

            //flipping the model based on the direction
            if(force.x >= 0.01f) {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
            else if(force.x <= -0.01f) {
                transform.localScale = new Vector3(1f, 1f, 1f);
            }

            if (PlayerInSight()) {
                state = State.Attacking;
                //Debug.Log("State Attacking");
            }
        }
    }
    void Update() {
        if (state == State.Attacking) {
            anim.SetInteger("AnimState", 1);
            coolDownTimer += Time.deltaTime;

            //Add Iframs here too
            if(PlayerInSight() && enemyScript.enemyCurrentHealth > 0 && playerHealth.playerCurrentHealth > 0) {
                Attack();
                //Debug.Log("Enemy Attacks");
            }

            if (!PlayerInSight()) {
                state = State.Chasing;
                anim.SetInteger("AnimState", 2);
                //Debug.Log("State Chasing");
            }
        }
    }

    private bool PlayerInSight() {

        RaycastHit2D hit = Physics2D.BoxCast(enemyAtkPos.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
         new Vector3(enemyAtkPos.bounds.size.x * range, enemyAtkPos.bounds.size.y, enemyAtkPos.bounds.size.z), 0, Vector2.left, 0, playerLayer);

        if(hit.collider != null) {
            playerHealth = hit.transform.GetComponent<PlayerStats>();
            playMoveScpt = hit.transform.GetComponent<PlayerMovement>();
        }
        return hit.collider != null;
    }

    private void OnDrawGizmos() {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(enemyAtkPos.bounds.center + transform.right * range * transform.localScale.x * colliderDistance,
        new Vector3(enemyAtkPos.bounds.size.x * range, enemyAtkPos.bounds.size.y, enemyAtkPos.bounds.size.z));
    }

    void Attack() {
        if(coolDownTimer >= attackCooldown && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0)) {
            coolDownTimer = 0;
            anim.SetTrigger("Attack");
            DamagePlayer();
        }
    }

    private void DamagePlayer() {
        if(PlayerInSight()) {
            playerHealth.DmgTaken(damage, playMoveScpt.isImmune);
        }
    }
}
