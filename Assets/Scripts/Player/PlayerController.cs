using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MirrorBasics
{
    [RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(NetworkAnimator))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        public CharacterController characterController;

        // [SyncVar] public int teamID = 1;
        [SyncVar] public int playerIndex;

        [Header("Movement Settings")]
        public float moveSpeed = 5f;
        public float turnSensitivity = 70f;
        public float maxTurnSpeed = 110f;
        public float jumpSpeed = 5f;

        [Header("Diagnostics")]
        public float horizontal;
        public float vertical;
        public float turn;
        public bool isGrounded = true;
        public bool isFalling = false;
        public Vector3 velocity;
        public float dashCooldown = 5f;
        public bool isDashing = false;

        [SyncVar (hook=nameof(SetReadyState)) ]
        public bool isReady = false;

        [Header("Animation")]
        [SerializeField]
        private Animator characterAnimator;
        private UILobby uiLobby;
        private UIGameplay uiGameplay;
        public PlayerCamera pCamera;

        private Player localPlayer;
        public static PlayerController localGamePlayer;
        private LevelController levelManager;

    
        public override void OnStartLocalPlayer()
        {
            
            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (characterAnimator == null)
                characterAnimator = GetComponent<Animator>();

            NetworkClient.ready = true;
            characterController.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            uiLobby = GameObject.FindObjectOfType<UILobby>();
            uiGameplay = GameObject.FindObjectOfType<UIGameplay>();
            uiGameplay.player = this;
 
            levelManager = uiGameplay.levelController;
            uiGameplay.ChangeUIState(1);
            pCamera = this.GetComponent<PlayerCamera>();
        }


[Command (requiresAuthority = false)]
    public void SetReadyState(bool oldValue,bool newValue)
    {
        this.isReady = newValue;
        Debug.Log(" Setting up ready state for gameplayer to syncvar for server");
    }


[TargetRpc]
        public void SetPlayerReady (bool oldValue, bool newValue)
        {
            Debug.Log("Finalize setting playercontroller ready for gamePlayer of id: " +this.netId );
            characterController.enabled = newValue; 
            characterAnimator.enabled = newValue;
            this.pCamera.SetupPlayerCamera();
            uiGameplay.ChangeUIState(2);    
        }

        void Update()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (dashCooldown >0) {dashCooldown -= Time.deltaTime;}

            
            if (isGrounded)
                isFalling = false;

            if ((isGrounded || !isFalling) && jumpSpeed < 1f && Input.GetKey(KeyCode.Space))
            {
                jumpSpeed = Mathf.Lerp(jumpSpeed, 1f, 0.5f);
            }
            else if (!isGrounded)
            {
                isFalling = true;
                jumpSpeed = 0;
            }
            if (isGrounded && Input.GetKey(KeyCode.LeftShift))
            {
                if (dashCooldown >0 || isDashing == true) {return;} 
                if (dashCooldown <= 0 && isDashing == false)
                {
                    isDashing = true;
                    dashCooldown = 5;
                }
            }

            Animate();
        }

        private void Animate()
        {
            if (!isLocalPlayer || characterAnimator == null || !characterAnimator.enabled)
            return;
            characterAnimator.SetBool("Walking", velocity != Vector3.zero);
            characterAnimator.SetBool("Idle", velocity == Vector3.zero);
            characterAnimator.SetBool("Rolling", isDashing);
        }

        void FixedUpdate()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            if (dashCooldown <= 4) { isDashing = false; }

            Vector3 rotation = new Vector3 (0, horizontal, 0);
            characterController.transform.Rotate(rotation * turnSensitivity * Time.deltaTime);

            Vector3 direction = new Vector3(0f, jumpSpeed*1.3f, vertical*1.0f);
            direction = Vector3.ClampMagnitude(direction, 1.3f);
            direction = transform.TransformDirection(direction);
            direction *= moveSpeed;


            if (jumpSpeed > 0 )
                characterController.Move(direction * Time.fixedDeltaTime);

            if (isDashing == true)
                characterController.Move(direction*1.4f * Time.fixedDeltaTime);
            else
                characterController.SimpleMove(direction);

            isGrounded = characterController.isGrounded;
            velocity = characterController.velocity;
        }
    }
}
