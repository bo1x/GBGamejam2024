using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightOnObject : MonoBehaviour
{
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<Enemy>(out Enemy enemigo))
        {
            Debug.Log("ENEMIGO YES");
        }
    }
}
