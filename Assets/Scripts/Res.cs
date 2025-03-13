


/*
 *//////////////////////////////////////////////////////////////////////
//PORTAL GUILLEM
/*
using System.Collections.Generic;
using UnityEditor.Build.Content;
using UnityEngine;

public class Portal : MonoBehaviour
{
    public Transform m_OtherPortalTransform;
    [SerializeField] private Camera m_Camera;
    public Portal m_MirrorPortal;
    [SerializeField] private float m_OffsetCamera = 0.9f;

    public List<Transform> m_ValidPoints;
    public LayerMask m_ValidityLayerMask;
    public float m_ValidityOffset = 0.02f;
    public float m_ValidityAngleInDegrees = 3.0f;


    private void Update()
    {
        Camera l_CameraPlayerController = GameManager.GetGameManager().GetPlayer().m_Camera;
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
}
*/
/*
 /////////////////////////////////////////////////////////////////////////
//COMPANION CUBE Guillem 
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CompanionCube : MonoBehaviour
{
    bool m_Teleportable = true;
    Rigidbody m_Rigidbody;
    public float m_TeleportOffset = 1.5f;
    //public List<DoorController> m_DoorControllers; // Lista de controladores de puertas
    public DoorController m_DoorController;
    public int m_ButtonCounter;

    public void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_ButtonCounter = 1;
    }

    private void Update()
    {
        // Lógica adicional, si es necesaria.
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
    }

    private void OnCollisionEnter(Collision collision)
    {
        //string buttonTag = "Button " + m_ButtonCounter; // Crea el tag basado en el contador.
        /*if (collision.collider.CompareTag("Button 1"))
        {
            foreach (var doorController in m_DoorControllers)
            {
                // Imprimir el botón actual que está siendo procesado
                Debug.Log("Contactando con botón " + m_ButtonCounter);

                // Si el botón tiene un tag correspondiente al contador m_ButtonCounter, abre la puerta.

                if (collision.collider.CompareTag(buttonTag)) // Compara el tag generado con el del objeto en la colisión.
                {
                    doorController.OpenDoor(); // Abre la puerta asociada.
                }
            

        }
        // Aquí, busca en la lista `m_DoorControllers` el controlador que deba ser activado.

        // Incrementa el contador después de revisar todos los botones.
        m_ButtonCounter++; // Esto solo debe hacerse después de revisar todos los botones
        */
/*
using UnityEngine;

if (collision.collider.CompareTag("Button 1"))
{
    Debug.Log("Contactando");
    m_DoorController.OpenDoor();

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
}

*///////////////////////////////////////////////////////////////////////////
  // TURRET Guillem
/*
using UnityEngine;
public class Turret : MonoBehaviour
{
    public LineRenderer m_Laser;
    public LayerMask m_LayerMak;
    public float m_MaxDistanceLaser = 50.0f;
    public float m_MaxAngleLaserAlife = 10.0f;

    // Update is called once per frame
    private void Update()
    {
        if (IsLaserAlife())
        {
            Ray l_Ray = new Ray(m_Laser.transform.position, m_Laser.transform.forward);
            if (Physics.Raycast(l_Ray, out RaycastHit l_Raycashit, m_MaxDistanceLaser, m_LayerMak.value))
            {
                m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_Raycashit.distance));
                m_Laser.gameObject.SetActive(true);
                if (l_Raycashit.collider.CompareTag("RefractionCube"))
                    l_Raycashit.collider.GetComponent<RefractionCube>().CreateRefraction();
            }
            else
            {
                m_Laser.gameObject.SetActive(false);
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
}
*/
///////////////////////////////////////////////////////////////////////////////////
//REFRACTION CUBE Guillem 

/*
 * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    public LineRenderer m_Laser;
    private Rigidbody m_Rigidbody;
    private PlayerController m_PlayerController;
    bool m_Teleportable = true;
    public float m_TeleportOffset
    public LayerMask m_LayerMak;
    public float m_MaxDistance = 50.0f;
    bool m_CreateRefraction;

    // Update is called once per frame
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
        if (Physics.Raycast(l_Ray, out RaycastHit l_Raycashit, m_MaxDistance, m_LayerMak.value))
        {
            m_Laser.SetPosition(1, new Vector3(0.0f, 0.0f, l_Raycashit.distance));
            m_Laser.gameObject.SetActive(true);
            if (l_Raycashit.collider.CompareTag("RefractionCube"))
            {
                l_Raycashit.collider.GetComponent<RefractionCube>().CreateRefraction();
            }
        }
        else
        {
            m_Laser.gameObject.SetActive(false);
        }


    }

}
*/
/////////////////////////////////////////////////////////////////////////////////////////////

// PLAYER CONTROLLER GUILLEM
/*
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;
//using UnityEngine.UIElements;

public class PlayerController : MonoBehaviour, IRestartGameElement
{
    public static PlayerController instance;




    //Rotation
    public Camera m_Camera;
    public Transform m_PitchController;
    float m_Yaw;
    float m_Pitch;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public float m_MinPitch;
    public float m_MaxPitch;
    public float m_Speed;
    float m_VerticalSpeed = 0.0f;
    CharacterController m_CharacterController;
    public float m_FastSpeedMultiplier = 1.2f;
    public float m_JumpSpeed;
    public float m_ScaleSpeed = 0.1f;
    private int m_ScrollingCounter = 0;
    public DoorController m_DoorController;

    Vector3 m_StartPosition;
    Quaternion m_StartRotation;

    [Header("Teleport")]
    Vector3 m_MovementDirection;
    public float m_TeleportOffset;
    public float m_MaxAngleToTeleport = 45.0f;

    [Header("Canvas")]
    public Image m_BluePortalCreated;
    public Image m_OrangePortalCreated;
    public Image m_NoPortalIsCreated;
    public Image m_BothPortalCreated;
    public Image m_RedCross;
    public Image m_GreenTick;

    [Header("Portals")]
    public Portal m_BluePortal;
    public Portal m_OrangePortal;
    public Portal m_DummyPortal;
    public Sprite m_BluePortalSprite;
    public Image m_BluePortalImage;
    public bool m_BluePortalIsActive;
    public bool m_OrangePortalIsActive;
    public GameObject m_BluePortalPreview;
    public GameObject m_OrangePortalPreview;

    [Header("Shoot")]
    public float m_MaxShootDistance;
    public LayerMask m_ShootLayerMask;
    private float m_BulletCounter = 100;

    [Header("AttachObjects")]
    public Transform m_AttachTransform;
    public Transform m_AttachTransformTurret;
    bool m_AttachingObject;
    bool m_AttachedObject;
    Rigidbody m_AttachedObjectRigidbody;
    public float m_AttachObjectSpeed = 8.0f;
    public float m_StartDistanceToRotateAttachObject = 2.5f;
    public float m_DetachObjectForce = 20.0f;
    Transform m_AttachedObjectPreviousParent;
    public float m_MinDistanceToAttach = 1.0f;

    [Header("Keys")]
    public KeyCode m_LeftKeyCode = KeyCode.A;
    public KeyCode m_RightKeyCode = KeyCode.D;
    public KeyCode m_UpKeyCode = KeyCode.W;
    public KeyCode m_DownKeyCode = KeyCode.S;
    public KeyCode m_JumpKeyCode = KeyCode.Space;
    public KeyCode m_RunKeyCode = KeyCode.LeftShift;

    public KeyCode m_DebugLockAngleKeyCode = KeyCode.I;
    public KeyCode m_DebugLockKeyCode = KeyCode.O;

    private bool m_AngleLocked = false;
    private bool m_CanMove = true;

    private void Awake()
    {
        m_CharacterController = GetComponent<CharacterController>();
    }

    void Start()
    {
        GameManager.GetGameManager().SetPlayer(this);
        GameManager l_GameManager = GameManager.GetGameManager();

        if (instance == null)
        {
            instance = this;
        }

        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        m_Yaw = transform.eulerAngles.y;
        m_Pitch = m_PitchController.eulerAngles.x;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

    }


    void Update()
    {
        // Rotation
        float l_HorizontalValue = Input.GetAxis("Mouse X");
        float l_VerticalValue = -Input.GetAxis("Mouse Y");

        if (!m_AngleLocked)
        {
            m_Yaw = m_Yaw + l_HorizontalValue * m_YawSpeed * Time.deltaTime;
            m_Pitch = m_Pitch + l_VerticalValue * m_PitchSpeed * Time.deltaTime;
            m_Pitch = Mathf.Clamp(m_Pitch, m_MinPitch, m_MaxPitch);
        }

        transform.rotation = Quaternion.Euler(0.0f, m_Yaw, 0.0f);
        m_PitchController.localRotation = Quaternion.Euler(m_Pitch, 0.0f, 0.0f);



        // Movement + Diagonal Movement Normalization
        float l_ForwardAngleRadians = m_Yaw * Mathf.Deg2Rad;
        float l_RightAngleRadians = (m_Yaw + 90.0f) * Mathf.Deg2Rad;

        Vector3 l_Forward = new Vector3(Mathf.Sin(l_ForwardAngleRadians), 0.0f, Mathf.Cos(l_ForwardAngleRadians));
        Vector3 l_Right = new Vector3(Mathf.Sin(l_RightAngleRadians), 0.0f, Mathf.Cos(l_RightAngleRadians));

        m_MovementDirection = Vector3.zero;

        // Diagonal Movement (Right has priority over left)
        if (Input.GetKey(m_RightKeyCode))
            m_MovementDirection = l_Right;
        else if (Input.GetKey(m_LeftKeyCode))
            m_MovementDirection = -l_Right;

        // Diagonal Movement (Front has priority over back)
        if (Input.GetKey(m_UpKeyCode))
            m_MovementDirection += l_Forward;
        else if (Input.GetKey(m_DownKeyCode))
            m_MovementDirection -= l_Forward;

        m_MovementDirection.Normalize();

        // Jump
        if (m_CharacterController.isGrounded && Input.GetKeyDown(m_JumpKeyCode))
            m_VerticalSpeed = m_JumpSpeed;

        m_VerticalSpeed += Physics.gravity.y * Time.deltaTime;

        float l_SpeedMultiplier = 1.0f;
        if (Input.GetKey(m_RunKeyCode))
            l_SpeedMultiplier = m_FastSpeedMultiplier;


        Vector3 l_MovementDirection = m_MovementDirection * m_Speed * l_SpeedMultiplier * Time.deltaTime;
        l_MovementDirection.y = m_VerticalSpeed * Time.deltaTime;

        // Gravity
        CollisionFlags l_CollisionFlags = m_CharacterController.Move(l_MovementDirection);
        if ((l_CollisionFlags & CollisionFlags.Below) != 0) // On the ground
            m_VerticalSpeed = 0.0f;
        else if ((l_CollisionFlags & CollisionFlags.Above) != 0 && m_VerticalSpeed > 0.0f) // Hit the ceiling
            m_VerticalSpeed = 0.0f;


        if (m_AttachedObject || m_AttachingObject)
        {
            if (Input.GetMouseButtonDown(0))
            {
                DetachObject(m_DetachObjectForce);
            }
            else if (Input.GetMouseButtonDown(1))
            {
                DetachObject(0.0f);
            }
        }
        else
        {
            if (CanShoot())
            {
                // Al mantener presionado el botón izquierdo o derecho del ratón
                if (Input.GetMouseButton(0))
                {
                    UpdatePreviewPortalPosition(m_BluePortalPreview);
                }
                else if (Input.GetMouseButton(1))
                {
                    UpdatePreviewPortalPosition(m_OrangePortalPreview);
                }
                // Al soltar el botón, establecer el portal en su posición final
                if (Input.GetMouseButtonUp(0))
                {
                    m_GreenTick.gameObject.SetActive(false);
                    m_RedCross.gameObject.SetActive(false);
                    m_BluePortalPreview.SetActive(false);
                    Shoot(m_BluePortal);
                }
                else if (Input.GetMouseButtonUp(1))
                {
                    m_GreenTick.gameObject.SetActive(false);
                    m_RedCross.gameObject.SetActive(false);
                    m_OrangePortalPreview.SetActive(false);
                    Shoot(m_OrangePortal);
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                AttachObject();
            }

        }
        if (m_BluePortalIsActive && m_OrangePortalIsActive)
        {
            m_BothPortalCreated.gameObject.SetActive(true);
            m_BluePortalCreated.gameObject.SetActive(false);
            m_OrangePortalCreated.gameObject.SetActive(false);
        }


        if (m_AttachingObject && m_AttachedObjectRigidbody != null)
        {
            UpdateAttachingObject();
        }




    }

    bool CanShoot()
    {
        if (m_BulletCounter > 0)
        {
            return true;
        }
        else
        {
            return false;
        }


    }
    private void UpdatePreviewPortalPosition(GameObject _PreviewPortal)
    {
        Ray l_Ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Obtén un rayo hacia el cursor

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxShootDistance, m_ShootLayerMask.value))
        {
            if (m_DummyPortal.IsValidPosition(l_RaycastHit.point, l_RaycastHit.normal))
            {
                m_RedCross.gameObject.SetActive(false);
                m_GreenTick.gameObject.SetActive(true);


                _PreviewPortal.SetActive(true); // Asegúrate de que el objeto esté activo
                _PreviewPortal.transform.position = l_RaycastHit.point; // Mueve el objeto de previsualización al punto de impacto
                _PreviewPortal.transform.rotation = Quaternion.LookRotation(l_RaycastHit.normal); // Ajusta la rotación al plano
                Vector3 l_OriginalPortalScale = new Vector3(1, 1, 1);

                float l_Scroll = Input.GetAxis("Mouse ScrollWheel");
                //previewPortal.transform.localScale = Mathf.Clamp(-0.1f, 0.1f);


                if (l_Scroll > 0 && m_ScrollingCounter < 1)
                {
                    _PreviewPortal.transform.localScale = _PreviewPortal.transform.localScale * 2;
                    m_ScrollingCounter = 1;
                }
                else if (l_Scroll < 0 && m_ScrollingCounter > -1)
                {
                    _PreviewPortal.transform.localScale = _PreviewPortal.transform.localScale / 2;
                    m_ScrollingCounter = -1;

                }
                else if (l_Scroll == 0 && _PreviewPortal.transform.localScale == l_OriginalPortalScale)
                {
                    _PreviewPortal.transform.localScale = l_OriginalPortalScale;
                    m_ScrollingCounter = 0;
                }
                //Debug.Log(m_ScrollingCounter);

                //Debug.Log(previewPortal.transform.localScale);
                if (_PreviewPortal == m_BluePortalPreview)
                {
                    m_BluePortalPreview.transform.localScale = _PreviewPortal.transform.localScale;
                }
                else if (_PreviewPortal == m_OrangePortalPreview)
                {
                    m_OrangePortalPreview.transform.localScale = _PreviewPortal.transform.localScale;

                }


            }
        }
        else
        {
            _PreviewPortal.SetActive(false); // Si el rayo no toca nada, desactiva la previsualización
            m_GreenTick.gameObject.SetActive(false);
            m_RedCross.gameObject.SetActive(true);

        }
    }

    private void Shoot(Portal _Portal)
    {

        Ray l_Ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxShootDistance, m_ShootLayerMask.value))
        {
            // Verificar si el raycast tocó un layer válido para los portales
            int l_LayerHit = l_RaycastHit.collider.gameObject.layer;
            int l_NoValidLayer = LayerMask.NameToLayer("CantShoot");
            int l_OrangePortalLayer = LayerMask.NameToLayer("OrangePortal");
            int l_BluePortalLayer = LayerMask.NameToLayer("BluePortal");

            //Debug.Log("Número de capa del collider impactado: " + l_RaycastHit.collider.gameObject.layer);

            if ((l_RaycastHit.transform.CompareTag("PortalButton")))
            {
                l_RaycastHit.collider.GetComponent<CompanionSpawner>().Spawn();
            }

            if (l_LayerHit != l_NoValidLayer && l_LayerHit != l_OrangePortalLayer && l_LayerHit != l_BluePortalLayer)
            {

                // Si el raycast impactó un layer válido, entonces puedes actualizar las imágenes en el canvas
                if (m_DummyPortal.IsValidPosition(l_RaycastHit.point, l_RaycastHit.normal))
                {
                    // m_GreenTick.gameObject.SetActive(true);
                    // m_RedCross.gameObject.SetActive(false);

                    if (_Portal == m_BluePortal)
                    {
                        m_BluePortal.transform.localScale = m_BluePortalPreview.transform.localScale;
                    }
                    else if (_Portal == m_OrangePortal)
                    {
                        m_OrangePortal.transform.localScale = m_OrangePortalPreview.transform.localScale;

                    }

                    _Portal.gameObject.SetActive(true);
                    _Portal.transform.position = l_RaycastHit.point;
                    _Portal.transform.rotation = Quaternion.LookRotation(l_RaycastHit.normal);

                    if (_Portal == m_BluePortal)
                    {
                        m_BluePortalCreated.gameObject.SetActive(true);
                    }
                    else if (_Portal == m_OrangePortal)
                    {
                        m_OrangePortalCreated.gameObject.SetActive(true);
                    }
                }
                else
                {
                    // m_GreenTick.gameObject.SetActive(false);
                    // m_RedCross.gameObject.SetActive(true);
                }
            }
            else if (l_LayerHit == l_OrangePortalLayer)
            {
                if (_Portal == m_OrangePortal)
                {
                    //Debug.Log("Eliminant");
                    _Portal.gameObject.SetActive(false);
                    m_OrangePortalCreated.gameObject.SetActive(false);
                }
            }
            else if (l_LayerHit == l_BluePortalLayer)
            {

                if (_Portal == m_BluePortal)
                {
                    // Debug.Log("Eliminant");
                    _Portal.gameObject.SetActive(false);
                    m_BluePortalCreated.gameObject.SetActive(false);

                }
            }
            else
            {
                // Si no es un layer válido, no actualices las imágenes
                m_BluePortalCreated.gameObject.SetActive(false);
                m_OrangePortalCreated.gameObject.SetActive(false);
            }
        }
        m_DummyPortal.gameObject.SetActive(false);
        m_BulletCounter -= 1;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Portal"))
        {
            Teleport(other.GetComponent<Portal>());
        }
        else if (other.CompareTag("DoorClosers"))
        {
            m_DoorController.CloseDoor();
            Debug.Log("Closing");
        }

    }
    void Teleport(Portal _Portal)
    {
        float l_DotAngle = Vector3.Dot(m_MovementDirection, _Portal.m_OtherPortalTransform.forward);
        if (l_DotAngle >= Mathf.Cos(m_MaxAngleToTeleport * Mathf.Deg2Rad))
        {
            Vector3 l_Position = transform.position + m_MovementDirection * m_TeleportOffset; // calcula la nova posició del player i el m_TeleportOffset es xk no es teletransporti just al borde del portal
            Vector3 l_LocalPosition = _Portal.m_OtherPortalTransform.InverseTransformPoint(l_Position); //converteix la posició del player en les cordenades del altre portal
            Vector3 l_WorldPosition = _Portal.m_MirrorPortal.transform.TransformPoint(l_LocalPosition); //converteix la posició local respecte el mon i cap a on sortira el player 

            Vector3 l_Forward = m_MovementDirection;
            Vector3 l_LocalForward = _Portal.m_OtherPortalTransform.InverseTransformDirection(m_MovementDirection); // //converteix la direcció del player en les cordenades del altre portal
            Vector3 l_WorldForward = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalForward);//converteix la direcció local respecte el mon i cap a on sortira el player

            m_CharacterController.enabled = false;
            transform.position = l_WorldPosition;
            transform.forward = l_WorldForward;
            m_Yaw = transform.eulerAngles.y;
            m_CharacterController.enabled = true;
        }

    }

    void AttachObject()
    {
        Ray l_Ray = m_Camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxShootDistance, m_ShootLayerMask.value))
        {
            if (l_RaycastHit.collider.CompareTag("CompanionCube"))
            {
                AttachObject(l_RaycastHit.rigidbody);
            }


        }
    }

    void AttachObject(Rigidbody AttachObjectRigidBody)
    {
        m_AttachedObjectRigidbody = AttachObjectRigidBody;
        m_AttachedObjectRigidbody.isKinematic = true;
        m_AttachingObject = true;
        m_AttachedObject = false;
        m_AttachedObjectPreviousParent = m_AttachedObjectRigidbody.transform.parent;
        m_AttachedObjectRigidbody.GetComponent<CompanionCube>().SetTeleportable(false);
    }

    void DetachObject(float Force)
    {
        m_AttachedObjectRigidbody.isKinematic = false;
        m_AttachedObjectRigidbody.transform.SetParent(m_AttachedObjectPreviousParent);
        m_AttachedObjectRigidbody.velocity = m_AttachTransform.forward * Force;
        m_AttachingObject = false;
        m_AttachedObject = false;
        m_AttachedObjectRigidbody.GetComponent<CompanionCube>().SetTeleportable(true);


    }
    void UpdateAttachingObject()
    {
        if (m_AttachingObject)
        {
            Vector3 l_Direction = m_AttachTransform.position - m_AttachedObjectRigidbody.position;
            float l_Distance = l_Direction.magnitude;
            l_Direction /= l_Distance;
            float l_Movement = m_AttachObjectSpeed * Time.deltaTime;

            if (l_Movement >= l_Distance || l_Distance < m_MinDistanceToAttach)
            {
                m_AttachedObject = true;
                m_AttachingObject = false;
                m_AttachedObjectRigidbody.transform.SetParent(m_AttachTransform);
                m_AttachedObjectRigidbody.transform.localPosition = Vector3.zero;
                m_AttachedObjectRigidbody.transform.localRotation = Quaternion.identity;

            }
            else
            {
                m_AttachedObjectRigidbody.transform.position += l_Movement * l_Direction;
                float l_Pct = Mathf.Min(1.0f, l_Distance / m_StartDistanceToRotateAttachObject);
                m_AttachedObjectRigidbody.transform.rotation = Quaternion.Lerp(m_AttachTransform.rotation, m_AttachedObjectRigidbody.transform.rotation, l_Pct);

            }
        }
    }

    void Die()
    {
        m_CanMove = false;
        Debug.Log("Player has died.");
        //GameManager.GetGameManager().RestartGame();
    }

    public void RestartGame()
    {

        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_CharacterController.enabled = true;
        m_CanMove = true;
    }
}
*/

////////////////////////////////////////////////////////////////////////////////////////

/*
using UnityEngine;

public class RefractionCube : MonoBehaviour
{
    public LineRenderer m_Laser;
    private Rigidbody m_RigidBody;
    private PlayerController m_PlayerController;
    bool m_Teleportable = true;
    public float m_TeleportOffset = 1.5f;

    public LayerMask m_LayerMak;

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


        if (Physics.Raycast(l_Ray, out RaycastHit l_Raycasthit, m_MaxDistance, m_LayerMak.value))
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
            else if (l_Raycasthit.collider.CompareTag("Turret"))
            {
                l_Raycasthit.collider.GetComponent<Turret>().DestroyTurret();
            }
            /*else if (l_Raycashit.collider.CompareTag("Portal"))
            {
               l_Raycashit.collider.GetComponent<Portal>().ActivateLaser(l_Raycashit.point, transform.forward);
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
    }
    void Teleport(Portal _Portal)
    {
        Vector3 l_MovementDirection = m_RigidBody.velocity;
        l_MovementDirection.Normalize();

        Vector3 l_Position = transform.position + l_MovementDirection * m_TeleportOffset; // calcula la nova posició del player i el m_TeleportOffset es xk no es teletransporti just al borde del portal
        Vector3 l_LocalPosition = _Portal.m_OtherPortalTransform.InverseTransformPoint(l_Position); //converteix la posició del player en les cordenades del altre portal
        Vector3 l_WorldPosition = _Portal.m_MirrorPortal.transform.TransformPoint(l_LocalPosition); //converteix la posició local respecte el mon i cap a on sortira el player 

        Vector3 l_Forward = transform.forward;
        Vector3 l_LocalForward = _Portal.m_OtherPortalTransform.InverseTransformDirection(l_Forward); // //converteix la direcció del player en les cordenades del altre portal
        Vector3 l_WorldForward = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalForward);//converteix la direcció local respecte el mon i cap a on sortira el player

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
*/