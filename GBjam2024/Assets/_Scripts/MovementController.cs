using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor.Rendering;
using UnityEngine;
using TMPro;

public class MovementController : MonoBehaviour
{
    private Rigidbody2D rb;
    private float movX = 0;
    private float movY = 0;
    public float speed = 1f;
    public float JumpSpeed = 3f;

    [SerializeField] private Transform groundTransform;
    private bool groundCheck;
    private float groundCheckRayLenght = 0.2f;
    //We do 3 cast to determine if characters touches ground, left= groundtransform-variation middle= groundtransform and right = groundtransform+variation
    [SerializeField] private float groundCheckVariations = 0.2f;
    [SerializeField] private LayerMask groundLayer;

    [SerializeField] private GameObject PlayerLIGHT;
    [SerializeField] private GameObject FlashLight;
    [SerializeField] private GameObject ShootLight;
    [SerializeField] private float ShootLightTime;

    private bool attack;
    private bool canAttack = true;

    [SerializeField] private float attackCD;
    [SerializeField] private int numberOfPellets;
    [SerializeField] private GameObject projectile;
    [SerializeField] private Transform shootingPoint;
    [SerializeField] private GameObject particles;
    private bool islookingRight;
    private charState _charState = charState.Idle;
    private usingItem _usingItem = usingItem.flashlight;

    [SerializeField] private int actualHP;
    [SerializeField] private int maxHP;

    [SerializeField] private TextMeshProUGUI textHP;
    [SerializeField] private TextMeshProUGUI textItem;
    [SerializeField] private SpriteRenderer spriteRenderer;

    [SerializeField] private Animator animator;
    private bool animBlock = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        textHP.text = actualHP.ToString();
        textItem.text = "Flashlight";
    }

    // Update is called once per frame
    void Update()
    {
        GroundCheck();
        WallCheck();
        GetInputs();
        if (animBlock) { return; }
        CheckCharDir();
        Walk(movX);
        Jump(movY);
        Attack();


        if (actualHP <=0)
        {
            Debug.Log("DEAD");
        }

    }

    public void blockAnimOnDamage()
    {
        PlayerLIGHT.SetActive(true);
        animBlock = true;
    }

    public void unblockAnimOnDamage()
    {
        PlayerLIGHT.SetActive(true);
        animBlock = false;
    }
    public void LoseHP(int damage)
    {
        blockAnimOnDamage();
        animator.Play("PlayerHurt");
        AudioManager.instance.PlaySFX("playerHurt");
        int tempactualHP = actualHP-damage;
        if (tempactualHP-damage<=0)
        {
            actualHP = 0;
            dead();
        }
        else
        {
            actualHP = tempactualHP;
        }
        textHP.text = actualHP.ToString();
    }
    public void Heal(int heal)
    {
        actualHP = actualHP + heal;
        if (actualHP>maxHP)
        {
            actualHP = maxHP;
        }
        textHP.text = actualHP.ToString();
    }

    private void dead()
    {

    }

    private void WallCheck()
    {
        
    }

    private void GroundCheck()
    {
        //this does three small raycasts at the specified positions to see if the player is grounded.
        if (Physics2D.Raycast(groundTransform.position, Vector2.down, groundCheckRayLenght, groundLayer) || Physics2D.Raycast(groundTransform.position + new Vector3(-groundCheckVariations, 0), Vector2.down, groundCheckRayLenght, groundLayer) || Physics2D.Raycast(groundTransform.position + new Vector3(groundCheckVariations, 0), Vector2.down, groundCheckRayLenght, groundLayer))
        {
            groundCheck = true;
        }
        else
        {
            groundCheck = false;
        }
        Debug.Log(groundCheck);
    }

    private void Jump(float movY)
    {
        if (groundCheck == false) { return;}
        if (movY == 0) { return; }
        AudioManager.instance.PlaySFX("Jump");
        rb.velocity = new Vector2(rb.velocity.x, movY * JumpSpeed);
    }

    private void Walk(float movX)
    {
        rb.velocity = new Vector2(movX * speed, rb.velocity.y);
        if (animBlock) { return; }
        if (groundCheck == false)
        {
            if (_usingItem == usingItem.flashlight)
            {
                animator.Play("IdleFlashlight");
            }
            if (_usingItem == usingItem.gun)
            {
                animator.Play("IdleShotgun");
            }
        }
        else if(movX != 0) 
        {
            if (_usingItem == usingItem.flashlight)
            {
                animator.Play("PlayerRunFlashlight");
            }
            else if(_usingItem == usingItem.gun)
            {
                animator.Play("PlayerRunShotgun");

            }
        }
        else if(movX == 0)
        {
            if (_usingItem == usingItem.flashlight)
            {
                animator.Play("IdleFlashlight");
            }
            else if (_usingItem == usingItem.gun)
            {
                animator.Play("IdleShotgun");

            }
        }
       
    }

    private void GetInputs()
    {
        movX = 0;
        movY = 0;
        attack = false;
        if (Input.GetKey(KeyCode.A))
        {
            islookingRight = false;
            movX = -1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            islookingRight = true;
            movX = 1;

        }
        if (Input.GetKey(KeyCode.Space))
        {
            movY = 1;

        }
        if (Input.GetKeyUp(KeyCode.K))
        {
            attack = true;
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            AudioManager.instance.PlaySFX("switchItem");
            if (_usingItem == usingItem.gun)
            {
                textItem.text = "Flashlight";
                _usingItem = usingItem.flashlight;
                FlashLight.SetActive(true);
            }
            else
            {
                textItem.text = "Shotgun";
                _usingItem = usingItem.gun;
                FlashLight.SetActive(false);

            }
        }

        if (movX==0 && movY == 0)
        {
            _charState = charState.Idle;
        }

        if (movX != 0 && movY == 0)
        {
            _charState = charState.Walking;
        }

        if (movY != 0)
        {
            _charState = charState.Jumping;
        }
    }

    private void CheckCharDir()
    {
        if (islookingRight == false)
        {
            spriteRenderer.flipX = true;
            FlashLight.transform.rotation = Quaternion.Euler(new Vector3(FlashLight.transform.rotation.x, 180, FlashLight.transform.rotation.z));
            ShootLight.transform.rotation = Quaternion.Euler(new Vector3(FlashLight.transform.rotation.x, 180, FlashLight.transform.rotation.z));
        }
        else
        {
            spriteRenderer.flipX = false;
            FlashLight.transform.rotation = Quaternion.Euler(new Vector3(FlashLight.transform.rotation.x, 0, FlashLight.transform.rotation.z));
            ShootLight.transform.rotation = Quaternion.Euler(new Vector3(FlashLight.transform.rotation.x, 0, FlashLight.transform.rotation.z));
        }
    }

    private void Attack()
    {
        if (!attack) { return; }
        if (_usingItem == usingItem.flashlight)
        {
            AudioManager.instance.PlaySFX("lanternswitch");

            if (FlashLight.gameObject.activeSelf == true)
            {
                FlashLight.gameObject.SetActive(false);
            }
            else
            {
                FlashLight.gameObject.SetActive(true);
            }
        }


        if (_usingItem == usingItem.gun)
        {
            
            if (canAttack)
            {
                AudioManager.instance.PlaySFX("playerShoot");
                canAttack = false;
                StartCoroutine(startAttackCD());
                StartCoroutine(startShotgunLight());
                for (int i = 0; i < numberOfPellets; i++)
                {
                    GameObject tempGameObject = Instantiate(projectile, shootingPoint.position, transform.rotation);
                    Projectile pro = tempGameObject.GetComponent<Projectile>();
                        if (islookingRight)
                        {
                            pro.setDir(new Vector2(1, UnityEngine.Random.Range(-0.15f,0.15f)));
                            pro.setSpeed(UnityEngine.Random.Range(9f, 15f));
                        }
                        else
                        {
                            pro.setDir(new Vector2(-1, UnityEngine.Random.Range(-0.15f, 0.15f)));
                            pro.setSpeed(UnityEngine.Random.Range(9f, 15f));
                        }
                }
            }
            else
            {
                AudioManager.instance.PlaySFX("Reloading");
            }


        }
    }

    private void ChangeAnimation()
    {
        if (_charState == charState.Walking)
        {

        }

        if (_charState == charState.Jumping)
        {

        }

        if (_charState == charState.Dead)
        {

        }
        if (_charState == charState.Idle)
        {

        }
    }


    public enum charState
    {
        Idle = 0,
        Walking = 1,
        Jumping = 2,
        Dead = 3,
    }

    public enum usingItem
    {
        gun = 0,
        flashlight = 1
    }

    IEnumerator startAttackCD()
    {
        yield return new WaitForSeconds(attackCD);
        canAttack = true;
    }

    IEnumerator startShotgunLight()
    {
        ShootLight.SetActive(true);
        yield return new WaitForSeconds(ShootLightTime);
        ShootLight.SetActive(false);

    }
}
