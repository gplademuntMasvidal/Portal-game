using UnityEngine;
using UnityEngine.Pool;

public class RefractionCube : MonoBehaviour
{
    public LineRenderer m_Laser;
    private Rigidbody m_RigidBody;
    private PlayerController m_PlayerController;
    private LaserReceiver m_LaserReceiver;
    bool m_Teleportable = true;
    public float m_TeleportOffset = 1.5f;

    public LayerMask m_LayerMask;

    public float m_MaxDistance = 50.0f;

    bool m_CreateRefraction;

    private void Awake()
    {
        m_RigidBody = GetComponent<Rigidbody>();
        m_PlayerController = FindObjectOfType<PlayerController>();
    }

    void Update()
    {
        m_Laser.gameObject.SetActive(m_CreateRefraction);
        m_CreateRefraction = false;
    }

    public void CreateRefraction()
    {
        if (m_CreateRefraction) return;

        m_CreateRefraction = true;
        Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);

        if (Physics.Raycast(l_Ray, out RaycastHit l_Raycasthit, m_MaxDistance, m_LayerMask.value))
        {
            m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_Raycasthit.distance));
            m_Laser.gameObject.SetActive(true);

            if (l_Raycasthit.collider.CompareTag("RefractionCube"))
            {
                l_Raycasthit.collider.GetComponent<RefractionCube>().CreateRefraction();
            }
            if (l_Raycasthit.collider.CompareTag("Player"))
            {
                m_PlayerController.Die();
            }
            if (l_Raycasthit.collider.CompareTag("Turret"))
            {
                l_Raycasthit.collider.GetComponent<Turret>().DestroyTurret();
            }
            if (l_Raycasthit.collider.CompareTag("RefractionButton"))
            {
                m_LaserReceiver = FindAnyObjectByType<LaserReceiver>();
                if (m_LaserReceiver.linkedDoor != null) // Verifica que haya una puerta asignada
                {
                    m_LaserReceiver.linkedDoor.OpenDoor(); // Abre la puerta asignada
                    m_LaserReceiver.isActivated = true; // Marca como activado
                }
                else
                {
                    Debug.LogWarning("No se ha asignado una puerta al botón.");
                }
            }

        }
        else
        {
            m_Laser.gameObject.SetActive(false);
        }
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
        Vector3 l_MovementDirection = m_RigidBody.velocity;
        l_MovementDirection.Normalize();

        Vector3 l_Position = transform.position + l_MovementDirection * m_TeleportOffset; // Calcula la nueva posición del objeto con un offset para evitar borde del portal
        Vector3 l_LocalPosition = _Portal.m_OtherPortalTransform.InverseTransformPoint(l_Position); // Convierte la posición a coordenadas locales del otro portal
        Vector3 l_WorldPosition = _Portal.m_MirrorPortal.transform.TransformPoint(l_LocalPosition); // Convierte la posición local a coordenadas globales

        Vector3 l_Forward = transform.forward;
        Vector3 l_LocalForward = _Portal.m_OtherPortalTransform.InverseTransformDirection(l_Forward); // Convierte la dirección a coordenadas locales del otro portal
        Vector3 l_WorldForward = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalForward); // Convierte la dirección local a coordenadas globales

        Vector3 l_LocalVelocity = _Portal.m_OtherPortalTransform.InverseTransformDirection(m_RigidBody.velocity);
        Vector3 l_WorldVelocity = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalVelocity);

        float l_Scale = _Portal.m_MirrorPortal.transform.localScale.x / _Portal.transform.localScale.x;
        m_RigidBody.isKinematic = true;
        m_RigidBody.transform.position = l_WorldPosition;
        m_RigidBody.transform.rotation = Quaternion.LookRotation(l_WorldForward);
        m_RigidBody.transform.localScale = Vector3.one * l_Scale;
        m_RigidBody.isKinematic = false;
        m_RigidBody.velocity = l_WorldVelocity;
    }
}