using Mirror;
using UnityEngine;
using UnityEngine.SceneManagement;
using MirrorBasics;
using System.Collections.Generic;

[RequireComponent(typeof(CapsuleCollider))]
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(NetworkTransform))]
    [RequireComponent(typeof(NetworkAnimator))]
    [RequireComponent(typeof(Rigidbody))]
    public class PlayerController : NetworkBehaviour
    {
        public PlayerController localGamePlayer;
        public CharacterController characterController;
        public List<GameObject> characterModel = new List<GameObject>();

        // [SyncVar] public int teamID = 1;
        [SyncVar] public int playerIndex;

        [Header("Movement Settings")]
        [SyncVar]public float moveSpeed = 5f;
        public float turnSensitivity = 70f;
        public float maxTurnSpeed = 110f;
        public float jumpSpeed = 0f;
        public float jumpPower = 4.5f;
        public bool isGrounded = true;
        public bool isFalling = false;
        public float dashCooldown = 5f;
        public bool isDashing = false; 
        [SyncVar] public string modelName;      

        [Header("References")]
        [SerializeField]  private Animator characterAnimator;
        [SerializeField] public NetworkAnimator networkAnimator;
        private UIGameplay uiGameplay;
        public PlayerScore pScore;
        private PlayerCamera pCamera;

        [Header("Diagnostics")]
        public float horizontal;
        public float vertical;
        public float turn;

        public Vector3 velocity;

        [SyncVar (hook=nameof(SetReadyState)) ]
        public bool isReady = false;

        // private UIScore uiScore;
        // private UILobby uiLobby;
        // private Player localPlayer;

        private LevelController levelManager;

    
        public override void OnStartLocalPlayer()
        {
            levelManager = FindObjectOfType<LevelController>();
            if (characterController == null)
                characterController = GetComponent<CharacterController>();

            if (characterAnimator == null)
                characterAnimator = GetComponentInChildren<Animator>();

            if (!NetworkClient.ready) {NetworkClient.ready = true;}

            localGamePlayer = this;
            levelManager.pController = localGamePlayer;
            characterController.enabled = false;
            GetComponent<Rigidbody>().isKinematic = true;
            uiGameplay = GameObject.FindObjectOfType<UIGameplay>();
            uiGameplay.player = this;
            uiGameplay.ChangeUIState(1);

            pScore = gameObject.GetComponentInParent<PlayerScore>();
            pCamera = this.GetComponent<PlayerCamera>();
            networkAnimator = GetComponent<NetworkAnimator>();
        }

        public override void OnStartServer()
        {
            levelManager = FindObjectOfType<LevelController>();
            pScore = gameObject.GetComponentInParent<PlayerScore>();
            levelManager.gamePlayers.Add(this);
            
        }
        public override void OnStartClient()
        {
            SetModel();
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
            Debug.Log("Finalize setting playercontroller ready for gamePlayer of id: " + this.netId );
            characterController.enabled = newValue; 
            characterAnimator.enabled = newValue;
            this.pCamera.SetupPlayerCamera();
            uiGameplay.ChangeUIState(1);    
        }

        void Update()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
               { return;}

            horizontal = Input.GetAxis("Horizontal");
            vertical = Input.GetAxis("Vertical");

            if (dashCooldown >0) {dashCooldown -= Time.deltaTime;}

            if (isGrounded) isFalling = false;

            if (!isDashing && !isFalling && Input.GetKey(KeyCode.Space))
            {
                jumpSpeed = Mathf.Lerp(.3f, jumpPower, .6f);
            }
            else if (!isGrounded) 
            {
                isFalling = true;
                jumpSpeed = 0;
            }
            if (isGrounded && !isFalling && !isDashing && Input.GetKeyDown(KeyCode.LeftShift))
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



        void FixedUpdate()
        {
            if (!isLocalPlayer || characterController == null || !characterController.enabled)
                return;

            if (dashCooldown <= 4) { isDashing = false; }
            Vector3 rotation = new Vector3 (0, horizontal, 0);

            characterController.transform.Rotate(rotation * turnSensitivity * Time.deltaTime);

            Vector3 direction = new Vector3(0f, jumpSpeed * 1.2f, vertical);
            direction = Vector3.ClampMagnitude(direction, 1.2f);
            direction = transform.TransformDirection(direction);
            direction *= moveSpeed;

            if (jumpSpeed > 0 ) characterController.Move(direction * 1.2f * Time.deltaTime );
            if (isDashing == true) characterController.Move(direction * 2 * Time.deltaTime);
            else characterController.SimpleMove(direction);

            isGrounded = characterController.isGrounded;
            velocity = characterController.velocity;            
        }

        private void Animate()
        {
            if (!isLocalPlayer || characterAnimator == null || !characterAnimator.enabled)
            return;
            characterAnimator.SetBool("Walking", velocity != Vector3.zero);
            characterAnimator.SetBool("Idle", velocity == Vector3.zero);
            characterAnimator.SetBool("Rolling", isDashing);
        }
       
        [ClientRpc]
        public void SetModel()
        {
//          (!isLocalPlayer) {return;}
            RpcSetModel(modelName);
        }

        [ClientRpc]
        public void RpcSetModel(string modelName)
        {
            foreach (GameObject charModel in characterModel) 
            {

                if (charModel.name == modelName)
                {
                    charModel.SetActive(true);
                    characterAnimator = charModel.GetComponent<Animator>();
                    networkAnimator.animator = charModel.GetComponent<Animator>();
                    Debug.Log("Changed model to: " + charModel.name);
                }
                if (charModel.name != modelName)
                {
                    charModel.SetActive(false);
                    Debug.Log("Disabling not needed models");
                }

                
            }

        }

   


        [ServerCallback]
        void OnTriggerEnter(Collider other)
        {
            // Stealing ability 
            if (other.gameObject.CompareTag("Player") && pScore.teamID != other.GetComponent<PlayerScore>().teamID)
            {
                if (!this.pScore.canSteal){return;}
                if (other.gameObject.GetComponent<PlayerScore>().hasItem && !this.pScore.hasItem) 
                {                
                    other.gameObject.GetComponent<PlayerScore>().hasItem = false;
                    pScore.hasItem = true;
                    pScore.canSteal = false;
                }
                else return;
            }           
        }

    }

