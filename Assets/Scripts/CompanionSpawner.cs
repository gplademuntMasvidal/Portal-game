using UnityEngine;

public class CompanionSpawner : MonoBehaviour
{
    [SerializeField] private GameObject m_CompanionPrefab;
    [SerializeField] private Transform m_SpanwerPoint;

    public void Spawn()
    {
        m_CompanionPrefab.transform.localScale = Vector3.one;   
        //m_CompanionPrefab.gameObject.SetActive(true);
        GameObject gameObject = Instantiate(m_CompanionPrefab);
        gameObject.SetActive(true);
        gameObject.transform.position = m_SpanwerPoint.position;
    }
}