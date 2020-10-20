using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Timeline;

public class Snake : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;
    bool facingRight = false;
    bool noChao = false;
    public Transform groundCheck;

    public Animator anima;
    public float attackRate;
    public float attackDistance;
    private Transform player;
    private float nextAttack;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        groundCheck = transform.Find("SnakeGroundCheck");
        player = GameObject.FindGameObjectWithTag("Player").transform;
        anima = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        speed = 3;
        attackRate = 2;
        attackDistance = 3;
    }

    // Update is called once per frame
    void Update()
    {
        noChao = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (!noChao)
            speed *= -1;

        CheckTarget();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed, rb.velocity.y);

        if (speed > 0 && !facingRight)
        {
            Flip();
        }
        else if (speed < 0 && facingRight)
            Flip();
    }

    void CheckTarget()
    {
        float distance = Vector2.Distance(transform.position, player.position);
        float dir = player.transform.position.x - transform.position.x;

        if(distance < attackDistance)
        {
            if((speed < 0 && dir < 0) || (speed > 0 && dir > 0))
            {
                if(Time.time > nextAttack)
                {
                    nextAttack = Time.time + attackRate;
                    anima.SetBool("playerPerto", true);
                }
            }
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            BoxCollider2D[] boxes = gameObject.GetComponents<BoxCollider2D>();
            foreach (BoxCollider2D box in boxes)
            {
                box.enabled = false;
            }
        }
    }
}
