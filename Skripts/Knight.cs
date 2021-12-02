using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Knight : Creature, IDestructable
{
    public bool onStair;

    [SerializeField] private float stairSpeed;

    [SerializeField] private Transform groundCheck;

    [SerializeField] private float jumpForce;

    [SerializeField] private Transform AttackPoint;

    [SerializeField] private float attackRange;

    [SerializeField] private float hitDelay;

   
    private bool onGround = true;

    public bool OnStair
    {
        get => onStair;
        set
        {
            if (value == true) rigidbody.gravityScale = 0;
            else rigidbody.gravityScale = 3;

            onStair = value;            
        }
    }

    private void Start()
    {
        GameController.Instance.OnUpdateHeroParameters += HandleOnUpdateHeroParameters;

        // health = GameController.Instance.maxHealth;
        GameController.Instance.Knight = this;
    }

    private void OnDisable()
    {
        GameController.Instance.OnUpdateHeroParameters -= HandleOnUpdateHeroParameters;
    }


    private void Update()
    {
        onGround = CheckGround();

        animator.SetFloat("Speed", Mathf.Abs(Input.GetAxis("Horizontal")));

        Vector2 velocity     = rigidbody.velocity;
                velocity.x   = Input.GetAxis("Horizontal") * speed; 
        rigidbody.velocity = velocity;

        animator.SetBool("Jump", !onGround);

        if (Input.GetButtonDown("Fire1")) {animator.SetTrigger("Attack"); Attack(); Invoke("Attack", hitDelay);}

        if (transform.localScale.x < 0)
        {
            if (Input.GetAxis("Horizontal") > 0)
                transform.localScale = Vector3.one;
        }
        else
        {
            if (Input.GetAxis("Horizontal") < 0)
                transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Input.GetButtonDown("Jump") && onGround)
        {
            rigidbody.AddForce(Vector2.up * jumpForce);
        }
        if (OnStair)
        {
            velocity = rigidbody.velocity;
            velocity.y = Input.GetAxis("Vertical") * stairSpeed;
            rigidbody.velocity = velocity;
        }
    }
    private bool CheckGround()
    {
        RaycastHit2D[] hits = Physics2D.LinecastAll(transform.position, groundCheck.position);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!Equals(hits[i].collider.gameObject, gameObject))
            {
                return true;
            }
        }
        return false;
    }
   
    public void Attack()
    {
        DoHit(AttackPoint.position, attackRange, damage);
    }


    private void HandleOnUpdateHeroParameters(HeroParameters parameters)
    {
        Health = parameters.maxHealth;
        damage = parameters.damage;
        speed = parameters.speed;

    }


}