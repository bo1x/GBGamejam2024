using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{

    [SerializeField] private int MaxHP;
    [SerializeField] private int actualHP;

    [SerializeField] private float AttackRange;
    [SerializeField] private float AttackCD;
    [SerializeField] private bool AlreadyAttacked;
    [SerializeField] private int Damage;
    [SerializeField] private float enemySpeed;

    private bool agroo;
    private Vector2 playerPos;

    private Rigidbody2D rb2d;

    private void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        actualHP = MaxHP;
    }
    private void Update()
    {
        if (agroo && !AlreadyAttacked)
        {
            Debug.Log(AlreadyAttacked);
            MoveToPlayer();
        }
        else
        {
            rb2d.velocity = new Vector2(0,rb2d.velocity.y);
        }
        
    }

    private void MoveToPlayer()
    {
        
        playerPos = Singleton.Instance.GetPlayerPosition();
       // if (Vector2.Distance(playerPos, transform.position) > 10f) { agroo = false;return; }
        if (playerPos != null) 
        {
            Debug.Log("MOVING");
            Vector2 dir = new Vector2(playerPos.x-transform.position.x, playerPos.y-transform.position.y);
            rb2d.velocity = dir.normalized * enemySpeed;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Luz"))
        {
            Debug.Log("AGRO ONLINE");
            agroo = true;
        }
    }


    private void OnCollisionStay2D(Collision2D collision)
    {
        if (AlreadyAttacked)
        {
            return;
        }
        if (collision.gameObject.TryGetComponent<MovementController>(out MovementController PlayerScript))
        {
            Debug.Log("playeratttacked");
            PlayerScript.LoseHP(Damage);
            AlreadyAttacked = true;
            StartCoroutine(AttackCOOLDOWNRESET());

        }
    }


    public void LoseHP(int damage)
    {
        int tempactualHP = actualHP - damage;
        if (tempactualHP - damage <= 0)
        {
            actualHP = 0;
            dead();
        }
        else
        {
            actualHP = tempactualHP;
        }
        Debug.Log(actualHP);

    }
    public void Heal(int heal)
    {
        actualHP = actualHP + heal;
        if (actualHP > MaxHP)
        {
            actualHP = MaxHP;
        }
    }

    private void dead()
    {
        Destroy(this.gameObject);
    }

    IEnumerator AttackCOOLDOWNRESET()
    {
        
        yield return new WaitForSeconds(AttackCD);
        AlreadyAttacked = false;
    }
}
