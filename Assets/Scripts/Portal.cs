using System.Collections.Generic;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform m_OtherPortalTransform;
    [SerializeField] private Camera m_Camera;
    public Portal m_MirrorPortal;
    [SerializeField] private float m_OffsetCamera = 0.9f;

    //public LineRenderer m_Laser;
    public List<Transform> m_ValidPoints;
    public LayerMask m_ValidityLayerMask;
    public float m_ValidityOffset = 0.02f;
    public float m_ValidityAngleInDegrees = 3.0f;

    private LineRenderer m_LaserRenderer;
    public LayerMask m_LayerMask;
    public float m_MaxDistanceLaser = 50.0f;
    public float m_MaxAngleLaserAlife = 10.0f;
    private PlayerController playerController;
    public float m_MaxDistance = 50.0f;

        bool m_CreateRefraction;

    private void Awake()
    {
        if (m_MirrorPortal != null)
        {
            m_LaserRenderer = m_MirrorPortal.gameObject.AddComponent<LineRenderer>();
            m_LaserRenderer.enabled = false;
            m_LaserRenderer.startWidth = 0.05f;
            m_LaserRenderer.endWidth = 0.05f;
            m_LaserRenderer.material = new Material(Shader.Find("Unlit/Color")) { color = Color.red };
        }
    }

    private void Update()
    {
       //s Camera l_CameraPlayerController = GameManager.GetGameManager().GetPlayer().m_Camera;
        Vector3 l_Position = PlayerController.instance.m_Camera.transform.position;
        Vector3 l_Forward = PlayerController.instance.m_Camera.transform.forward;
        Vector3 l_LocalPosition = m_OtherPortalTransform.InverseTransformPoint(l_Position);
        Vector3 l_LocalForward = m_OtherPortalTransform.InverseTransformDirection(l_Forward);

        Vector3 l_WorldPosition = m_MirrorPortal.transform.TransformPoint(l_LocalPosition);
        Vector3 l_WorldForward = m_MirrorPortal.transform.TransformDirection(l_LocalForward);

        m_MirrorPortal.m_Camera.transform.position = l_WorldPosition;
        m_MirrorPortal.m_Camera.transform.forward = l_WorldForward;

        float l_DistanceToPortal = Vector3.Distance(l_WorldPosition, m_MirrorPortal.transform.position);
        float l_DistanceNearClipPlane = m_OffsetCamera + l_DistanceToPortal;
        m_MirrorPortal.m_Camera.nearClipPlane = l_DistanceNearClipPlane;

        //m_Laser.gameObject.SetActive(m_CreateRefraction);

        m_CreateRefraction = false;
    }

    public bool IsValidPosition(Vector3 Position, Vector3 Normal)
    {
        transform.position = Position;
        transform.rotation = Quaternion.LookRotation(Normal);
        Camera l_PlayerCamera = GameManager.GetGameManager().GetPlayer().m_Camera;
        float l_Angle = Mathf.Cos(m_ValidityAngleInDegrees * Mathf.Deg2Rad);
        bool l_IsValid = true;

        for (int i = 0; i < m_ValidPoints.Count; i++)
        {
            Vector3 l_Direction = m_ValidPoints[i].position - l_PlayerCamera.transform.position;
            float l_Distance = l_Direction.magnitude;
            l_Direction /= l_Distance;
            Ray l_Ray = new Ray(l_PlayerCamera.transform.position, l_Direction);
            Color l_Color = Color.green;

            if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, l_Distance + m_ValidityOffset, m_ValidityLayerMask.value))
            {
                if (l_RaycastHit.collider.CompareTag("Drawable"))
                {
                    if (Vector3.Distance(m_ValidPoints[i].position, l_RaycastHit.point) < m_ValidityOffset)
                    {
                        float l_DotAngle = Vector3.Dot(l_RaycastHit.normal, m_ValidPoints[i].forward);
                        if (l_DotAngle < l_Angle)
                        {
                            l_IsValid = false;
                            l_Color = Color.magenta;
                        }
                    }
                    else
                    {
                        l_IsValid = false;
                        l_Color = Color.cyan;
                    }
                }
                else
                {
                    l_IsValid = false;
                    l_Color = Color.blue;
                }
            }
            else
            {
                l_IsValid = false;
                l_Color = Color.red;
            }
            Debug.DrawLine(l_PlayerCamera.transform.position, m_ValidPoints[i].position, l_Color, 2.0f);
        }
        return l_IsValid;
    }
   /* public void CreateRefraction()
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
            else if (l_Raycasthit.collider.CompareTag("Player"))
            {
                playerController.Die();
            }
            else if (l_Raycasthit.collider.CompareTag("Turret"))
            {
                l_Raycasthit.collider.GetComponent<Turret>().DestroyTurret();
            }
            /*else if (l_Raycasthit.collider.CompareTag("Portal"))
            {
                l_Raycasthit.collider.GetComponent<Portal>().ActivateLaser(l_Raycasthit.point, transform.forward);
            }
        }
        else
        {
            m_Laser.gameObject.SetActive(false);
        }
    }*/

}