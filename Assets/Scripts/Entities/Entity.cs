using Assets.Scripts.Blocks;
using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;
using UnityEditor;
using UnityEngine;
using UnityEngine.PlayerLoop;

namespace Assets.Scripts.Entities
{
    public abstract class Entity : MonoBehaviour
    {

        [HideInInspector] public SpriteRenderer spriteRenderer;
        [HideInInspector] public CapsuleCollider2D boxCollider;
        [HideInInspector] public Rigidbody2D rigid;

        [HideInInspector] public GameManager gameManager;
        [HideInInspector] public ChunkContainer chunkContainer;
        [HideInInspector] public EntityContainer entityContainer;
        [HideInInspector] public ResourceStorage resourceStorage;
        [HideInInspector] public PlayerInterface playerInterface;

        [HideInInspector] public LayerMask groundLayerMask;

        public EntityType Type;
        public float HorizontalSpeed;
        public float JumpSpeed;

        private float _horizontalAxis;
        private bool _jump;


        protected virtual void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            boxCollider = GetComponent<CapsuleCollider2D>();
            rigid = GetComponent<Rigidbody2D>();

            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            chunkContainer = GameObject.Find("ChunkContainer").GetComponent<ChunkContainer>();
            entityContainer = GameObject.Find("EntityContainer").GetComponent<EntityContainer>();
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();
            playerInterface = GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>();

            groundLayerMask = LayerMask.GetMask("Block");
        }

        public void Remove()
        {
            entityContainer.RemoveEntity(this);
        }

        public bool IsGrounded()
        {
            float extraHeight = 0.05f;
            RaycastHit2D raycastHit = Physics2D.BoxCast(boxCollider.bounds.center, new Vector3(boxCollider.bounds.size.x - extraHeight, boxCollider.bounds.size.y, 0 - extraHeight), 0f, Vector2.down, extraHeight, groundLayerMask);
            return raycastHit.collider != null;
        }

        public float HorizontalAxis
        {
            get
            {
                return _horizontalAxis;
            }
            set
            {
                _horizontalAxis = value;
            }
        }

        public bool Jump
        {
            get
            {
                return _jump;
            }
            set
            {
                _jump = value;
            }
        }

        protected virtual void FixedUpdate()
        {
            if(transform.position.y < -10)
            {
                Remove();
            }
            ProcessMovement();
        }

        private void ProcessMovement()
        {
            Vector2 current = rigid.velocity;
            if (HorizontalAxis != 0)
            {
                current.x = HorizontalAxis * HorizontalSpeed;
                HorizontalAxis = 0;
            }
            if (Jump)
            {
                if (IsGrounded())
                {
                    current.y = JumpSpeed;
                }
            }
            rigid.velocity = current;
        }

        protected virtual void Update()
        {

        }

    }
}