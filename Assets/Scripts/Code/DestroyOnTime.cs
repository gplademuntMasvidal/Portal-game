using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnTime : MonoBehaviour
{
    public float m_LifeTime;

    private void Start()
    {
        StartCoroutine(DestroyOnTimeCoroutine());
    }

    IEnumerator DestroyOnTimeCoroutine()
    {
        yield return new WaitForSeconds(m_LifeTime);
        GameObject.Destroy(gameObject);
    }
}
