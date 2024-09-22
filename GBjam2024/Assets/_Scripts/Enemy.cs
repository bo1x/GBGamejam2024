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
    private bool islookingRight;
    [SerializeField] private Animator animator;
    [SerializeField] private GameObject AttackColliderGO;
    [SerializeField] private GameObject hitlight;
    [SerializeField] private float timeLight;
    private bool agroo;
    private Vector2 playerPos;
    private bool Attacking;
    private Rigidbody2D rb2d;
    private bool hitisplaying = false;

    private SpriteRenderer spriteRenderer;

    private void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
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
        if (Attacking)
        {
            return;
        }

        animator.Play("MonsterWalk");
        playerPos = Singleton.Instance.GetPlayerPosition();
       // if (Vector2.Distance(playerPos, transform.position) > 10f) { agroo = false;return; }
        if (playerPos != null) 
        {
            Debug.Log("MOVING");
            Vector2 dir = new Vector2(playerPos.x-transform.position.x, playerPos.y-transform.position.y);
            if (dir.x>0)
            {
                spriteRenderer.flipX = true;
            }
            else if(dir.x<0){
                spriteRenderer.flipX = false;

            }
            rb2d.velocity = dir.normalized * enemySpeed;

            if (Vector2.Distance(transform.position,playerPos)<1.7f)
            {
                Attacking = true;
                animator.Play("MonsterAttack");
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Luz"))
        {
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
        hitlight.SetActive(true);
        
        int tempactualHP = actualHP - damage;
        if (tempactualHP - damage <= 0)
        {
            actualHP = 0;

            dead();
        }
        else
        {
            actualHP = tempactualHP;
            if (hitisplaying) { return; }
            hitisplaying = true;
            animator.Play("MonsterHit");
            StartCoroutine(LightHit(timeLight));

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
        Attacking = true;
        rb2d.velocity = Vector3.zero;
        animator.Play("MonsterDead");
        AnimatorClipInfo[] clipInfo = animator.GetCurrentAnimatorClipInfo(0);
        StartCoroutine(DieIn(clipInfo[0].clip.length));
    }

    IEnumerator AttackCOOLDOWNRESET()
    {
        
        yield return new WaitForSeconds(AttackCD);
        AlreadyAttacked = false;
    }

    IEnumerator DieIn(float time)
    {

        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

    public void ActivateAttackCollider()
    {
        AttackColliderGO.SetActive(true);
    }

    public void DesactivateAttackCollider()
    {
        animator.Play("MonsterStop");
        Attacking = false;
        AttackColliderGO.SetActive(false);
    }

    IEnumerator LightHit(float time)
    {
        yield return new WaitForSeconds(time);
        hitlight.SetActive(false);
    }

    public void setHitFalse()
    {
        hitisplaying = false;
    }
}
