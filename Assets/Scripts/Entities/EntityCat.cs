using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityCat : EntityLiving
    {

        [HideInInspector] public Animator animator;
        private EntityLiving _target;
        public float DamagePower;

        protected override void Awake()
        {
            base.Awake();
            animator = GetComponent<Animator>();
            MaxHealth = 150;
            Health = 150;
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
        private float shootCooldown = 2f;

        protected override void Update()
        {
            base.Update();
            if (shootCooldown <= 0)
            {
                shootCooldown = 2f;
                Shoot();
            } else
            {
                shootCooldown -= Time.deltaTime;
            }
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

        public void Shoot()
        {
            if (_target != null)
            {
                GameObject obj = Instantiate(resourceStorage.CatBulletPrefab, transform.position, Quaternion.identity);
                CatBullet bullet = obj.GetComponent<CatBullet>();
                Vector2 dir = (_target.transform.position - transform.position).normalized;
                bullet.direction = dir;
                bullet.start = true;
            }
        }

        protected override void Death()
        {
            base.Death();
            entityContainer.DropItem(transform.position.x, transform.position.y, new Items.ItemStack(Material.GOLD, 8));
        }

    }
}
