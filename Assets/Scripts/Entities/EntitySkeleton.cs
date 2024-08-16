using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntitySkeleton : EntityLiving
    {

        [HideInInspector] public Animator animator;
        private EntityLiving _target;
        public float DamagePower;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            MaxHealth = 45;
            Health = 45;
        }

        public EntityLiving Target
        {
            get
            {
                return _target;
            }
            set
            {
                _target = value;
            }
        }

        private float cooldown;

        protected override void Update()
        {
            base.Update();
            if (_target != null)
            {
                Vector2 targetPosition = _target.transform.position;
                Vector2 position = transform.position;
                float axis;
                if (targetPosition.x > position.x)
                {
                    axis = 1;
                }
                else
                {
                    axis = -1;
                }
                HorizontalAxis = axis;
                RaycastHit2D hit = Physics2D.BoxCast(boxCollider.bounds.center, boxCollider.bounds.size / 2, 0, new Vector2(axis, 0), 1f, groundLayerMask);
                if (hit.collider != null)
                {
                    Jump = true;
                }
                else
                {
                    Jump = false;
                }
                if (cooldown <= 0)
                {
                    cooldown = 1;
                    float distance = Vector2.Distance(transform.position, Target.transform.position);
                    if (distance <= 1)
                    {
                        Target.Damage(DamagePower);
                    }
                }
                else
                {
                    cooldown -= Time.deltaTime;
                }
            }
            else
            {
                HorizontalAxis = 0;
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            Vector2 current = rigid.velocity;
            animator.SetFloat("Horizontal", Math.Abs(current.x));
            animator.SetFloat("Jump", current.y);
            transform.localRotation = Quaternion.Euler(0, current.x > 0 ? 0 : 180, 0);

        }

        protected override void Death()
        {
            base.Death();
            entityContainer.DropItem(transform.position.x, transform.position.y, new Items.ItemStack(Material.GOLD, 3));
        }

    }
}
