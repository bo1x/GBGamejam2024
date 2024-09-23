using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : MonoBehaviour
{
    [SerializeField] private GameObject EYE1;
    [SerializeField] private GameObject EYE2;
    [SerializeField] private GameObject EYE3;
    [SerializeField] private int HPxEye = 1;
    private int actualEyeHP;
    private int EyeIndex = 0;

    [SerializeField] private float attackCooldown;
    [SerializeField] private GameObject projectile;
    [SerializeField] private float projectileCooldown;
    [SerializeField] private Transform shootTransform;
    private Vector2 playerPos;
    private Vector2 dir;
    private bool canAttack = true;
    private bool dead = false;

    [SerializeField] private GameObject win;
    // Start is called before the first frame update
    void Start()
    {
        actualEyeHP = HPxEye;
        setEyeIdleAnim(GETeye(EyeIndex));
    }

    // Update is called once per frame
    void Update()
    {
        if (dead) { return; }
        if (canAttack)
        {
           GameObject gEP = Instantiate(projectile, shootTransform.position,Quaternion.identity);
           EnemyProjectile eP = gEP.GetComponent<EnemyProjectile>();
           playerPos = Singleton.Instance.GetPlayerPosition();

            float dirX = playerPos.x - shootTransform.position.x;
            float dirY = playerPos.y - shootTransform.position.y;
            dir = new Vector2(dirX, dirY);

            eP.setDir(dir);
           AttackFinish(projectileCooldown);
        }
    }

    IEnumerator AttackCD(float time) 
    {
        yield return new WaitForSeconds(time);
        canAttack = true;
    }

    private void AttackFinish(float time)
    {
        StartCoroutine(AttackCD(time));
        canAttack = false;
    }



    private void setEyeIdleAnim(GameObject go)
    {
        Animator animator = go.GetComponent<Animator>();
        animator.Play("idle");
    }

    private void setEyeDieAnim(GameObject go)
    {
        Animator animator = go.GetComponent<Animator>();
        animator.Play("die");
    }

    public void GetDamaged()
    {
        if (dead) { return; }
        actualEyeHP = actualEyeHP - 1;
        if (actualEyeHP <=0)
        {
            actualEyeHP = HPxEye;
            setEyeDieAnim(GETeye(EyeIndex));
            EyeIndex++;
            if (EyeIndex >= 3)
            {
                win.SetActive(true);
                Debug.Log("WIN");
                dead = true;
                return;
            }
            setEyeIdleAnim(GETeye(EyeIndex));
        }
        
    }

    private GameObject GETeye(int i)
    {
        switch(i)
        {
            case 0:
                return EYE1;
                
            case 1:
                return EYE2;
                
            case 2:
                return EYE3;
            default:
                Debug.LogError("error");
                return null;
        }
    }


}
