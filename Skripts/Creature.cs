using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Creature : MonoBehaviour, IDestructable
{

    protected Animator animator;
    protected Rigidbody2D rigidbody;
    [SerializeField] public float speed;
    [SerializeField] public float damage;
    [SerializeField] public float health = 100;
    public float Health
    {
        get
        {
            return health;
        }
        set
        {
            health = value;
        }
    }
    void Awake()
    {
        animator = gameObject.GetComponentInChildren<Animator>();
        rigidbody = gameObject.GetComponent<Rigidbody2D>();
    }

    public virtual void Die()
    {
        GameController.Instance.Killed(this);
    }

    public void RecieveHit(float damage)
    {

        Health -= damage;
        GameController.Instance.Hit(this);
        if (Health <= 0)
        {
            Die();
        }
    }
    protected void DoHit(Vector3 hitPosition, float hitRadius, float hitDamage)
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(hitPosition, hitRadius);

        for (int i = 0; i < hits.Length; i++)
        {
            if (!GameObject.Equals(hits[i].gameObject, gameObject))
            {
                IDestructable destructable = hits[i].gameObject.GetComponent<IDestructable>();
                                     

                if (destructable != null)
                {
                    destructable.RecieveHit(hitDamage);
                }
            }
        }
    }


}

