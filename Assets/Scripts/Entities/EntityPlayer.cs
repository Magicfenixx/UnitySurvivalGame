using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityPlayer : EntityLiving
    {

        [HideInInspector] public Animator animator;
        [HideInInspector] public Inventory inventory;

        public int InventorySize;

        private int _currentSlot;

        private bool _gunHUD;
        public GameObject gunObject;
        private SpriteRenderer gunRenderer;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            inventory = new Inventory(this);
        }

        public int CurrentSlot
        {
            get
            {
                return _currentSlot;
            }
            set
            {
                if (value < 0) return;
                if (value >= InventorySize) return;
                _currentSlot = value;
                ItemStack item = inventory.GetItem(value);
                if (item != null && item.Material == Material.GUN)
                {
                    GunHud = true;
                } else
                {
                    GunHud = false;
                }
            }
        }

        public bool GunHud {
            get
            {
                return _gunHUD;
            }
            set
            {
                _gunHUD = value;
                if(value)
                {
                    gunObject = new GameObject("gun_hud");
                    gunObject.transform.parent = transform;
                    gunRenderer = gunObject.AddComponent<SpriteRenderer>();
                    gunRenderer.sprite = resourceStorage.GunSprite;
                    gunRenderer.sortingOrder = 1;
                    gunObject.transform.localPosition = Vector3.right;
                    gunObject.transform.localRotation = Quaternion.Euler(0, transform.localRotation.y - 180f, 0);
                } else {
                    Destroy(gunObject);
                    gunObject = null;
                    gunRenderer = null;
                }
            }
        }

        protected override void Death()
        {
            base.Death();
            gameManager.OpenMenuPanel();
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector2 current = rigid.velocity;
            animator.SetFloat("Horizontal_Speed", Math.Abs(current.x));
            animator.SetFloat("Jump", current.y);
            transform.localRotation = Quaternion.Euler(0, current.x > 0 ? 0 : 180, 0);
            if(Jump && IsGrounded())
            {
                AudioSource.PlayClipAtPoint(resourceStorage.JumpAudio, transform.position, 0.1f);
            }
        }

    }
}