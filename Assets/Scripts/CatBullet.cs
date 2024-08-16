using Assets.Scripts.Blocks;
using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class CatBullet : MonoBehaviour
    {

        private Rigidbody2D rb;
        private BoxCollider2D coll;

        public bool start;
        public Vector2 direction;
        public EntityCat owner;

        private void Awake()
        {
            coll = GetComponent<BoxCollider2D>();
            rb = GetComponent<Rigidbody2D>();
        }

        private void FixedUpdate()
        {
            if (!start) return;
            rb.velocity = direction * 5;
            RaycastHit2D[] hits = Physics2D.BoxCastAll(coll.bounds.center, coll.bounds.size, 0, Vector2.zero);
            foreach(var hit in hits)
            {
                Collider2D collider = hit.collider;
                GameObject obj = collider.gameObject;
                Entity entity = obj.GetComponent<Entity>();
                if (entity)
                {
                    if (entity is EntityPlayer)
                    {
                        EntityPlayer player = entity as EntityPlayer;
                        player.Damage(5f);
                        start = false;
                        Destroy(gameObject);
                        return;
                    }
                }
                Block block = obj.GetComponent<Block>();
                if (block)
                {
                    block.Material = Material.AIR;
                    start = false;
                    Destroy(gameObject);
                    return;
                }
            }
        }

    }
}
