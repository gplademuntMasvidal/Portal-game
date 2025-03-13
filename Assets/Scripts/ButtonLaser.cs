using UnityEngine;

public class ButtonLaser : MonoBehaviour
{
    public LineRenderer m_Laser;
    private PlayerController m_PlayerController;
    public LayerMask m_LayerMask;
    public float m_MaxDistanceLaser = 50.0f;
    public float m_MaxAngleLaserAlife = 10.0f;


    private void Awake()
    {
        
    }

    private void Update()
    {
        m_PlayerController = FindAnyObjectByType<PlayerController>();

        if (m_Laser == null) return; // Verifica que el láser esté configurado

        
            Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);
            if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxDistanceLaser, m_LayerMask.value))
            {
                m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_RaycastHit.distance));
                m_Laser.gameObject.SetActive(true);

                if (l_RaycastHit.collider.CompareTag("Player"))
                {
                     m_PlayerController.Die();
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

    
}