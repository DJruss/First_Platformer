using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpSpeed;
    private Rigidbody2D body;
    private Animator anim;
    private PlayerStats playerStats;
    private bool grounded;
    public bool isImmune;
    
    void Awake(){
        body = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        playerStats = GetComponent<PlayerStats>();
    }

    void Update()
    {
        if(!playerStats.IsDead())
        {
            float HorizontalInput = Input.GetAxis("Horizontal");

            body.velocity = new Vector2(HorizontalInput * speed, body.velocity.y);

            //Flip Player left/right when moving
            if(HorizontalInput >= 0.01) {
                transform.localScale = Vector3.one; 
            }
            else if(HorizontalInput <= -0.01) {
                transform.localScale = new Vector3(-1, 1 , 1);
            }
            
            if(Input.GetKey(KeyCode.LeftShift) && grounded == true) {
                anim.SetBool("IdleBlock", true);
                
                //anim.SetTrigger("Block");
                isImmune = true;
                
                body.velocity = new Vector2(0,0);
            }
            else {
                anim.SetBool("IdleBlock", false);
                isImmune = false;
            }

            //Animation controls
            anim.SetInteger("AnimState", 0);
            if(HorizontalInput != 0 && grounded == true) {
                anim.SetInteger("AnimState", 1);
            }
            anim.SetBool("Grounded", grounded);

            if(Input.GetKey(KeyCode.UpArrow) && grounded == true && !Input.GetKey(KeyCode.LeftShift)) {
                Jump();
            }
            if(Input.GetKey(KeyCode.W) && grounded == true && !Input.GetKey(KeyCode.LeftShift)) {
                Jump();
            }
        }
    }
    private void Jump() {
        body.velocity = new Vector2(body.velocity.x, jumpSpeed);
        anim.SetTrigger("Jump");
        grounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Ground") {
            grounded = true;
        }
    }

    public bool GetImmune() {
        return isImmune;
    }
}
