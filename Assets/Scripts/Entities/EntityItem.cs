using Assets.Scripts;
using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    public class EntityItem : Entity
    {

        private ItemStack _itemStack;

        public ItemStack ItemStack
        {
            get {
                return _itemStack;
            }
            set
            {
                _itemStack = value;
                spriteRenderer.sprite = resourceStorage.ByMaterial(_itemStack.Material);
            }
        }

        protected override void FixedUpdate()
        {
            base.FixedUpdate();
            RaycastHit2D[] hits = Physics2D.BoxCastAll(boxCollider.bounds.center, boxCollider.bounds.size, 0, Vector2.zero);
            foreach(var hit in hits)
            {
                GameObject obj = hit.collider.gameObject;
                Entity entity = obj.GetComponent<Entity>();
                if (entity)
                {
                    EntityType type = entity.Type;
                    if (type == EntityType.PLAYER)
                    {
                        EntityPlayer player = (EntityPlayer)entity;
                        entityContainer.PickupItem(player, this);
                    }
                }
            }
        }

    }
}