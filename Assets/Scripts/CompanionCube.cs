using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCube : MonoBehaviour
{
    bool m_Teleportable = true;
    Rigidbody m_Rigidbody;
    public float m_TeleportOffset = 1.5f;
    //public List<DoorController> m_DoorControllers; // Lista de controladores de puertas
   // public DoorController m_DoorController;
   // public int m_ButtonCounter;

    public void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

     void Start()
    {
        //m_ButtonCounter = 1;

    }

    private void Update()
    {
        // L�gica adicional, si es necesaria.
    }

    public bool IsTeleportable()
    {
        return m_Teleportable;
    }

    public void SetTeleportable(bool Teleportable)
    {
        
        m_Teleportable = Teleportable;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (IsTeleportable() && other.CompareTag("Portal"))
        {
            Teleport(other.GetComponent<Portal>());
        }
        else if (other.CompareTag("DestroyingArea"))
        {
            Debug.Log(other + "destroyed"); 
            gameObject.SetActive(false);
           // Destroy(gameObject);
        }
    }

 
    void Teleport(Portal _Portal)
    {
        Vector3 l_MovementDirection = m_Rigidbody.velocity;
        l_MovementDirection.Normalize();

        Vector3 l_Position = transform.position + l_MovementDirection * m_TeleportOffset;
        Vector3 l_LocalPosition = _Portal.m_OtherPortalTransform.InverseTransformPoint(l_Position);
        Vector3 l_WorldPosition = _Portal.m_MirrorPortal.transform.TransformPoint(l_LocalPosition);

        Vector3 l_Forward = transform.forward;
        Vector3 l_LocalForward = _Portal.m_OtherPortalTransform.InverseTransformDirection(l_Forward);
        Vector3 l_WorldForward = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalForward);

        Vector3 l_LocalVelocity = _Portal.m_OtherPortalTransform.InverseTransformDirection(m_Rigidbody.velocity);
        Vector3 l_WorldVelocity = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalVelocity);

        // Aqu� obtenim l'escala original del cub abans de teletransportar-se
        float l_OriginalScale = transform.localScale.x;
        float l_Scale;

        // Calculem l'escala a la sortida en funci� de la relaci� d'escala entre els portals
        if (l_OriginalScale != _Portal.m_MirrorPortal.transform.localScale.x)
        {
            l_Scale = _Portal.m_MirrorPortal.transform.localScale.x / _Portal.transform.localScale.x * l_OriginalScale;

        }
        else
        {
            l_Scale = l_OriginalScale;
        }

        // Debug per veure qu� est� passant amb l'escala
        Debug.Log($"Escala original: {l_OriginalScale}, Escala calculada per sortir: {l_Scale}");

        // Apliquem la nova escala
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.transform.position = l_WorldPosition;
        m_Rigidbody.transform.rotation = Quaternion.LookRotation(l_WorldForward);
        m_Rigidbody.transform.localScale = Vector3.one * l_Scale;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.velocity = l_WorldVelocity;
        l_Scale = 0;
    }
   
}