using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Vector2 _dir;
    private Rigidbody2D _rigidbody;
    [SerializeField]private float _speed;

    void Start()
    {
        Destroy(this.gameObject, 5f);
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _rigidbody.velocity = _dir * _speed;
        Debug.Log(_rigidbody.velocity);
    }

    public void setDir(Vector2 dir)
    {
        _dir = dir;
    }

    public void setSpeed(float speed)
    {
        _speed = speed;
    }



    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("HIT SOMETHING");
        Destroy(this.gameObject);
    }
}
