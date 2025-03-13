using System.Collections.Generic;
using UnityEngine;

public class CPoolElement
{
    List<GameObject> m_Pool;

    int m_CurrentElementId = 0;
    public CPoolElement(int ElementsCount, GameObject PrefabElement)
    {
        m_Pool = new List<GameObject>();
        for (int i = 0; i < ElementsCount; i++)
        {
            GameObject l_GameObject = GameObject.Instantiate(PrefabElement);
            l_GameObject.SetActive(false);
            m_Pool.Add(l_GameObject);
        }
    }
    public GameObject GetNextElement()
    {
        GameObject l_GameObject = m_Pool[m_CurrentElementId];
        ++m_CurrentElementId;
        if (m_CurrentElementId >= m_Pool.Count)
        {
            m_CurrentElementId = 0;
        }
        return l_GameObject;
    }
}
