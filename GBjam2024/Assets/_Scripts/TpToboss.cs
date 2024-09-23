using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TpToboss : MonoBehaviour
{

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<MovementController>(out MovementController SCRIPT))
        {
            SceneManager.LoadScene("BossBatlle");
        }
    }
}
