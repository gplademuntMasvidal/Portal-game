using UnityEngine;

public class Turret : MonoBehaviour
{
    public LineRenderer m_Laser;
    public LayerMask m_LayerMask;
    public float m_MaxDistanceLaser = 50.0f;
    public float m_MaxAngleLaserAlife = 10.0f;
    bool m_Teleportable = true;
    Rigidbody m_Rigidbody;
    //PlayerController m_PlayerController;
    public float m_TeleportOffset = 1.5f;

    //private PlayerController m_PlayerController;

    private void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        //m_PlayerController = GetComponent<PlayerController>();
        // m_PlayerController = FindObjectOfType<PlayerController>(); // Encuentra al jugador en la escena
    }

    private void Update()
    {
        if (m_Laser == null) return; // Verifica que el láser esté configurado

        if (IsLaserAlife())
        {
            Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);
            if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxDistanceLaser, m_LayerMask.value))
            {
                m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_RaycastHit.distance));
                m_Laser.gameObject.SetActive(true);

                if (l_RaycastHit.collider.CompareTag("Player"))
                {
                    GameManager.GetGameManager().GetPlayer().Die();
                }
                else if (l_RaycastHit.collider.CompareTag("Turret"))
                {
                    if (GameManager.GetGameManager().GetPlayer().m_TurretIsPicked == false)
                    {

                        l_RaycastHit.collider.GetComponent<Turret>().DestroyTurret();
                    }
                }
                else if (l_RaycastHit.collider.CompareTag("RefractionCube"))
                {
                    l_RaycastHit.collider.GetComponent<RefractionCube>().CreateRefraction();
                }
                else if (l_RaycastHit.collider.CompareTag("Portal"))
                {
                    // Notificar al portal para crear un rayo en el portal de salida
                    Portal portal = l_RaycastHit.collider.GetComponent<Portal>();
                    if (portal != null)
                    {
                        // portal.TeleportLaser(l_RaycastHit.point, l_Ray.direction);
                    }
                }
            }
            else
            {
                m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, m_MaxDistanceLaser));
                m_Laser.gameObject.SetActive(true);
            }
        }
        else
        {
            m_Laser.gameObject.SetActive(false);
        }
    }

    bool IsLaserAlife()
    {
        return Vector3.Dot(transform.up, Vector3.up) > Mathf.Cos(m_MaxAngleLaserAlife * Mathf.Deg2Rad);
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

        float l_Scale = _Portal.m_MirrorPortal.transform.localScale.x / _Portal.transform.localScale.x;
        m_Rigidbody.isKinematic = true;
        m_Rigidbody.transform.position = l_WorldPosition;
        m_Rigidbody.transform.rotation = Quaternion.LookRotation(l_WorldForward);
        m_Rigidbody.transform.localScale = Vector3.one * l_Scale;
        m_Rigidbody.isKinematic = false;
        m_Rigidbody.velocity = l_WorldVelocity;
    }

    public void DestroyTurret()
    {
        Destroy(gameObject); // Destruye el objeto de la torreta
        Debug.Log("Torreta Destruida");
    }


}
