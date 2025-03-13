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
    float m_StartPitch;
    float m_StartYaw;
    public float m_YawSpeed;
    public float m_PitchSpeed;
    public float m_MinPitch;
    public float m_MaxPitch;
    public float m_Speed;
    public float m_VerticalSpeed = 0.0f;
    CharacterController m_CharacterController;
    public float m_FastSpeedMultiplier = 1.2f;
    public float m_JumpSpeed;
    public float m_ScaleSpeed = 0.1f;
    private int m_ScrollingCounter = 0;
    public DoorController m_DoorController;
    private bool m_IsDead = false;
    //private bool m_ObjectIsAttached;

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
    public bool m_BluePortalIsActive;
    public bool m_OrangePortalIsActive;
    public GameObject m_BluePortalPreview;
    public GameObject m_OrangePortalPreview;

    [Header("Shoot")]
    public float m_MaxShootDistance;
    public LayerMask m_ShootLayerMask;
    WeaponSoundManager m_Shoot;

    [Header("AttachObjects")]
    public Transform m_AttachTransformCube;
    public Transform m_AttachTransformTurret;
    public Transform m_AttachTransformRefraction;
    public bool m_AttachingObject;
    public bool m_AttachedObject;
    Rigidbody m_AttachedObjectRigidbody;
    public float m_AttachObjectSpeed = 8.0f;
    public float m_StartDistanceToRotateAttachObject = 2.5f;
    public float m_DetachObjectForce = 20.0f;
    Transform m_AttachedObjectPreviousParent;
    public float m_MinDistanceToAttach = 1.0f;
    bool m_IsCube;
    bool m_IsTurret;
    bool m_IsCubeRefraction;
    public bool m_TurretIsPicked;

    [Header("Animations")]
    public Animation m_Animation;
    public AnimationClip m_DyingAnimationClip;
    //public AnimationClip m_DyingPanelAnimationClip;
    public DeadPanelAnimation m_DeadPanelAnimation;

    [Header("Checkpoints")]
    private Vector3 m_LastCheckpointPosition;
    private Quaternion m_LastCheckpointRotation;


    //private Animator m_Animator;


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
        m_Shoot = GetComponent<WeaponSoundManager>();
    }

    void Start()
    {
        m_LastCheckpointPosition = transform.position;
        m_LastCheckpointRotation = transform.rotation;
        m_StartPosition = transform.position;
        m_StartRotation = transform.rotation;
        m_CanMove = true;
        GameManager.GetGameManager().SetPlayer(this);
        GameManager l_GameManager = GameManager.GetGameManager();
        //m_Animator = GetComponent<Animator>();  
        if (instance == null)
        {
            instance = this;
        }

        m_Yaw = transform.eulerAngles.y;
        m_Pitch = m_PitchController.eulerAngles.x;
        m_StartPitch = m_Pitch;
        m_StartYaw = m_Yaw;
        m_TurretIsPicked = false;

        // m_DyingAnimationClip.wrapMode = WrapMode.Once;


    }


    void Update()
    {
        Debug.Log("m_IsPLaying" + GameManager.GetGameManager().m_IsPlaying);
        if (GameManager.GetGameManager().m_IsPlaying)
        {


            if (Input.GetKeyDown(KeyCode.Q))
            {
                GameManager.GetGameManager().PauseGame();
            }
            if (m_CanMove)
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
            }

            Debug.Log("Esta agafat l'objecte?" + m_AttachedObject);

            if (Input.GetKeyDown(KeyCode.E))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                // Verificar si el rayo impacta un objeto que tenga un tag específico, por ejemplo "ObjetoInteractivo"
                if (Physics.Raycast(ray, out hit, 100f)) // Ajusta la distancia (2f) según sea necesario
                {
                    if (hit.collider.CompareTag("CompanionCube"))
                    {
                        m_IsCube = true;
                        AttachObject(hit.rigidbody);
                    }
                    else if (hit.collider.CompareTag("Turret"))
                    {
                        m_IsTurret = true;
                        AttachObject(hit.rigidbody);
                        //l_RaycastHit.transform.SetParent(transform);
                        m_TurretIsPicked = true;

                    }
                    else if (hit.collider.CompareTag("RefractionCube"))
                    {
                        m_IsCubeRefraction = true;
                        AttachObject(hit.rigidbody);
                    }
                }
            }
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

                if (Input.GetMouseButton(0))
                {
                    UpdatePreviewPortalPosition(m_BluePortalPreview);
                }
                else if (Input.GetMouseButton(1))
                {
                    UpdatePreviewPortalPosition(m_OrangePortalPreview);
                }

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

            if (m_BluePortalIsActive && m_OrangePortalIsActive)
            {
                m_GreenTick.gameObject.SetActive(false);
                m_RedCross.gameObject.SetActive(false);
                m_BluePortalPreview.SetActive(false);
                Shoot(m_BluePortal);
                m_Shoot.PlayShootSound();
            }
            else if (Input.GetMouseButtonUp(1))
            {
                m_GreenTick.gameObject.SetActive(false);
                m_RedCross.gameObject.SetActive(false);
                m_OrangePortalPreview.SetActive(false);
                Shoot(m_OrangePortal);
                m_Shoot.PlayShootSound();
            }

            if (m_AttachingObject && m_AttachedObjectRigidbody != null)
            {
                UpdateAttachingObject();
            }
        }

    }


    private void UpdatePreviewPortalPosition(GameObject _PreviewPortal)
    {
        Ray l_Ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(l_Ray, out RaycastHit l_RaycastHit, m_MaxShootDistance, m_ShootLayerMask.value))
        {
            if (m_DummyPortal.IsValidPosition(l_RaycastHit.point, l_RaycastHit.normal))
            {
                m_RedCross.gameObject.SetActive(false);
                m_GreenTick.gameObject.SetActive(true);


                _PreviewPortal.SetActive(true);
                _PreviewPortal.transform.position = l_RaycastHit.point;
                _PreviewPortal.transform.rotation = Quaternion.LookRotation(l_RaycastHit.normal);
                Vector3 l_OriginalPortalScale = new Vector3(1, 1, 1);

                float l_Scroll = Input.GetAxis("Mouse ScrollWheel");


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

                if (_PreviewPortal == m_BluePortalPreview)
                {
                    m_BluePortalPreview.transform.localScale = _PreviewPortal.transform.localScale;
                }
                else if (_PreviewPortal == m_OrangePortalPreview)
                {
                    m_OrangePortalPreview.transform.localScale = _PreviewPortal.transform.localScale;

                }


            }
            else
            {
                _PreviewPortal.SetActive(false); // Si el rayo no toca nada, desactiva la previsualización
                m_GreenTick.gameObject.SetActive(false);
                m_RedCross.gameObject.SetActive(true);

            }
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

            if ((l_RaycastHit.collider.CompareTag("PortalButton")))
            {
                l_RaycastHit.collider.GetComponent<CompanionSpawner>().Spawn();
            }

            if (l_LayerHit != l_NoValidLayer && l_LayerHit != l_OrangePortalLayer && l_LayerHit != l_BluePortalLayer)
            {
                if (m_DummyPortal.IsValidPosition(l_RaycastHit.point, l_RaycastHit.normal))
                {
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
            }
            else if (l_LayerHit == l_OrangePortalLayer)
            {
                if (_Portal == m_OrangePortal)
                {
                    _Portal.gameObject.SetActive(false);
                    m_OrangePortalCreated.gameObject.SetActive(false);
                }
            }
            else if (l_LayerHit == l_BluePortalLayer)
            {

                if (_Portal == m_BluePortal)
                {
                    _Portal.gameObject.SetActive(false);
                    m_BluePortalCreated.gameObject.SetActive(false);

                }
            }
            else
            {
                m_BluePortalCreated.gameObject.SetActive(false);
                m_OrangePortalCreated.gameObject.SetActive(false);
            }

        }
        m_DummyPortal.gameObject.SetActive(false);
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
        else if (other.CompareTag("DeathZones"))
        {
            Die();
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

            Vector3 l_Forward = transform.forward;
            Vector3 l_LocalForward = _Portal.m_OtherPortalTransform.InverseTransformDirection(m_MovementDirection); // //converteix la direcció del player en les cordenades del altre portal
            Vector3 l_WorldForward = _Portal.m_MirrorPortal.transform.TransformDirection(l_LocalForward);//converteix la direcció local respecte el mon i cap a on sortira el player

            m_CharacterController.enabled = false;
            transform.position = l_WorldPosition;
            transform.forward = l_WorldForward;
            m_Yaw = transform.eulerAngles.y;
            m_CharacterController.enabled = true;
        }

    }

   

    void AttachObject(Rigidbody AttachObjectRigidBody)
    {
        if (AttachObjectRigidBody == null) return;

        m_AttachedObjectRigidbody = AttachObjectRigidBody;
        m_AttachedObjectRigidbody.isKinematic = true;
        m_AttachedObjectPreviousParent = m_AttachedObjectRigidbody.transform.parent;
        m_AttachingObject = true;
        m_AttachedObject = true;
        GetComponent<AttachSoundManager>()?.PlayAttachSound();
        

        if (m_IsCube)
        {
            var cubeComponent = m_AttachedObjectRigidbody.GetComponent<CompanionCube>();
            if (cubeComponent != null)
            {
                cubeComponent.SetTeleportable(false);
            }

        }
        else if (m_IsCubeRefraction)
        {
            var refractionComponent = m_AttachedObjectRigidbody.GetComponent<RefractionCube>();
            if (refractionComponent != null)
            {
                refractionComponent.SetTeleportable(false);
            }

        }
        else if (m_IsTurret)
        {
            var turretComponent = m_AttachedObjectRigidbody.GetComponent<Turret>();
            if (turretComponent != null)
            {
                turretComponent.SetTeleportable(false);
            }

        }
    }

    void DetachObject(float Force)
    {
        // m_ObjectIsAttached = false;
        if (m_AttachedObjectRigidbody == null) return;

        m_AttachedObjectRigidbody.isKinematic = false;
        m_AttachedObjectRigidbody.transform.SetParent(m_AttachedObjectPreviousParent);
        m_AttachedObjectRigidbody.velocity = m_AttachTransformCube.forward * Force;
        m_AttachedObjectRigidbody.velocity = m_AttachTransformCube.forward * Force;

        if (m_IsCube)
        {
            m_AttachedObjectRigidbody.GetComponent<CompanionCube>()?.SetTeleportable(true);
            m_IsCube = false;

        }
        else if (m_IsCubeRefraction)
        {
            m_AttachedObjectRigidbody.GetComponent<RefractionCube>()?.SetTeleportable(true);
            m_IsCubeRefraction = false;

        }
        else if (m_IsTurret)
        {

            m_AttachedObjectRigidbody.GetComponent<Turret>()?.SetTeleportable(true);
            m_TurretIsPicked = false;
            m_IsTurret = false;
        }
        StartCoroutine(WaitForOneSeconds());


    }
    IEnumerator WaitForOneSeconds()
    {
        yield return new WaitForSeconds(0.5f);
        m_AttachingObject = false;
        m_AttachedObject = false;
    }
    void UpdateAttachingObject()
    {
        if (m_AttachedObjectRigidbody == null)
        {
            m_AttachedObject = false;
            return;
        }

        if (m_IsCube == true && m_AttachingObject)
        {

            Vector3 l_Direction = m_AttachTransformCube.position - m_AttachedObjectRigidbody.position;
            float l_Distance = l_Direction.magnitude;
            l_Direction /= l_Distance;
            float l_Movement = m_AttachObjectSpeed * Time.deltaTime;

            if (l_Movement >= l_Distance || l_Distance < m_MinDistanceToAttach)
            {
                m_AttachedObject = true;
                m_AttachingObject = false;
                m_AttachedObjectRigidbody.transform.SetParent(m_AttachTransformCube);
                m_AttachedObjectRigidbody.transform.localPosition = Vector3.zero;
                m_AttachedObjectRigidbody.transform.localRotation = Quaternion.identity;
            }
            else
            {
                m_AttachedObjectRigidbody.transform.position += l_Movement * l_Direction;
                float l_Pct = Mathf.Min(1.0f, l_Distance / m_StartDistanceToRotateAttachObject);
                m_AttachedObjectRigidbody.transform.rotation = Quaternion.Lerp(m_AttachTransformCube.rotation, m_AttachedObjectRigidbody.transform.rotation, l_Pct);
            }


        }
        else if (m_IsCubeRefraction == true && m_AttachingObject)
        {

            Vector3 l_Direction = m_AttachTransformRefraction.position - m_AttachedObjectRigidbody.position;
            float l_Distance = l_Direction.magnitude;
            l_Direction /= l_Distance;
            float l_Movement = m_AttachObjectSpeed * Time.deltaTime;

            if (l_Movement >= l_Distance || l_Distance < m_MinDistanceToAttach)
            {
                m_AttachedObject = true;
                m_AttachingObject = false;
                m_AttachedObjectRigidbody.transform.SetParent(m_AttachTransformRefraction);
                m_AttachedObjectRigidbody.transform.localPosition = Vector3.zero;
                m_AttachedObjectRigidbody.transform.localRotation = Quaternion.identity;

            }
            else
            {
                m_AttachedObjectRigidbody.transform.position += l_Movement * l_Direction;
                float l_Pct = Mathf.Min(1.0f, l_Distance / m_StartDistanceToRotateAttachObject);
                m_AttachedObjectRigidbody.transform.rotation = Quaternion.Lerp(m_AttachTransformRefraction.rotation, m_AttachedObjectRigidbody.transform.rotation, l_Pct);

            }


        }

        else if (m_IsTurret == true && m_AttachingObject)
        {
            Vector3 l_Direction = m_AttachTransformTurret.position - m_AttachedObjectRigidbody.position;
            float l_Distance = l_Direction.magnitude;
            l_Direction /= l_Distance;
            float l_Movement = m_AttachObjectSpeed * Time.deltaTime;

            if (l_Movement >= l_Distance || l_Distance < m_MinDistanceToAttach)
            {
                m_AttachedObject = true;
                m_AttachingObject = false;
                m_AttachedObjectRigidbody.transform.SetParent(m_AttachTransformTurret);
                m_AttachedObjectRigidbody.transform.localPosition = Vector3.zero;
                m_AttachedObjectRigidbody.transform.localRotation = Quaternion.identity;

            }
            else
            {
                m_AttachedObjectRigidbody.transform.position += l_Movement * l_Direction;
                float l_Pct = Mathf.Min(1.0f, l_Distance / m_StartDistanceToRotateAttachObject);
                m_AttachedObjectRigidbody.transform.rotation = Quaternion.Lerp(m_AttachTransformTurret.rotation, m_AttachedObjectRigidbody.transform.rotation, l_Pct);

            }


        }
    }

    public void SetCheckpoint(Vector3 position, Quaternion rotation)
    {
        m_LastCheckpointPosition = position;
        m_LastCheckpointRotation = rotation;
        Debug.Log("Checkpoint position set to: " + position);
    }

    public void Respawn()
    {
        m_IsDead = false;
        m_CharacterController.enabled = false;
        transform.position = m_LastCheckpointPosition;
        transform.rotation = m_LastCheckpointRotation;
        m_Pitch = m_StartPitch;
        m_Yaw = m_StartYaw;
        m_CharacterController.enabled = true;
        m_CanMove = true;
    }
    public void Die()
    {
        if (m_IsDead) return;

        m_IsDead = true;
        m_CanMove = false;
        Debug.Log("Player has died.");


      //  m_Animation.CrossFade(m_DyingAnimationClip.name);

        // Inicia una corutina que espera a que termine la animación del jugador
        StartCoroutine(DieSequence());

    }

    private IEnumerator DieSequence()
    {
        //yield return new WaitForSeconds(m_DyingAnimationClip.length);
        m_DeadPanelAnimation.gameObject.SetActive(true);
        //m_DeadPanelAnimation.PlayPanelAnimation();
        yield return StartCoroutine(m_DeadPanelAnimation.PlayPanelAnimationAndWait());
        GameManager.GetGameManager().YouAreDead();
        //RestartGame();
    }

    public void RestartGame()
    {
        m_IsDead = false;
        m_CharacterController.enabled = false;
        transform.position = m_StartPosition;
        transform.rotation = m_StartRotation;
        m_Pitch = m_StartPitch;
        m_Yaw = m_StartYaw;
        m_CharacterController.enabled = true;
        m_CanMove = true;


    }
}