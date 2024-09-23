using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeGetDamaged : MonoBehaviour
{
    [SerializeField] private GameObject m_gameObject;

    public void damaged()
    {
        BossAI bAI = m_gameObject.GetComponent<BossAI>();
        bAI.GetDamaged();
    }

    public void DestroyDEAD()
    {
        Destroy(this.gameObject);
    }
}
