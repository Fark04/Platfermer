using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : Entity
{
    [SerializeField] private float speed = 3f; //Скорость переджижения
    [SerializeField] private int lives = 5; //Ково жизней
    [SerializeField] private float jumpForce = 15f; //Сила прыжка
    private bool isGrounded = false;


    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sprite;

    public static Hero Instance { get; set; }

    private States State
    {
        get { return (States)anim.GetInteger("state"); }
        set { anim.SetInteger("state", (int)value); }
    }

    private void Awake()
    {
        Instance = this;
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sprite = GetComponentInChildren<SpriteRenderer>();
    }

    private void Run()
    {
        if (isGrounded) State = States.Run;

        Vector3 dir = transform.right * Input.GetAxis("Horizontal");

        transform.position = Vector3.MoveTowards(transform.position, transform.position + dir, speed * Time.deltaTime);

        sprite.flipX = dir.x < 0.0f;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    

    private void Update()
    {
        if (isGrounded) State = States.Idle;

        if (Input.GetButton("Horizontal"))
            Run();
        if (isGrounded && Input.GetButtonDown("Jump"))
            Jump();
    }

    private void Jump()
    {
        rb.AddForce(transform.up * jumpForce, ForceMode2D.Impulse);
    }

    private void CheckGround()
    {
        Collider2D[] collider = Physics2D.OverlapCircleAll(transform.position, 0.3f);
        isGrounded = collider.Length > 1;

        if (!isGrounded) State = States.Jump;
    }

    public override void GetDamage()
    {
        lives -= 1;
        Debug.Log(lives);
    }

}

public enum States
{
    Idle,
    Run,
    Jump
}