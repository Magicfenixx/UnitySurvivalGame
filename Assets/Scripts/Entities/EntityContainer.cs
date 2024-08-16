using Assets.Scripts.Blocks;
using Assets.Scripts.Items;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Entities
{
    public class EntityContainer : MonoBehaviour
    {
        private ResourceStorage resourceStorage;
        private GameManager gameManager;
        private PlayerInterface playerInterface;
        private ChunkContainer chunkContainer;

        private HashSet<Entity> entities;

        private void Awake()
        {
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            playerInterface = GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>();
            chunkContainer = GameObject.Find("ChunkContainer").GetComponent<ChunkContainer>();

            entities = new HashSet<Entity>();
        }

        public Entity CreateEntity(EntityType type, float x, float y)
        {
            GameObject prefab = null;
            switch(type)
            {
                case EntityType.ITEM:
                    {
                        prefab = resourceStorage.ItemPrefab;
                        break;
                    }
                case EntityType.PLAYER:
                    {
                        prefab = resourceStorage.PlayerPrefab;
                        break;
                    }
                case EntityType.ZOMBIE:
                    {
                        prefab = resourceStorage.ZombiePrefab;
                        break;
                    }
                case EntityType.SKELETON:
                    {
                        prefab = resourceStorage.SkeletonPrefab;
                        break;
                    }
                case EntityType.CAT:
                    {
                        prefab = resourceStorage.CatPrefab;
                        break;
                    }
                case EntityType.BOMB:
                    {
                        prefab = resourceStorage.BombPrefab;
                        break;
                    }
            }
            GameObject gameObject = Instantiate(prefab, transform);
            Entity entity = gameObject.GetComponent<Entity>();
            entity.transform.position = new Vector3(x, y, 0);
            entities.Add(entity);
            return entity;
        }

        public void RemoveEntity(Entity entity)
        {
            entities.Remove(entity);
            Destroy(entity.gameObject);
        }

        public EntityItem DropItem(float x, float y, ItemStack itemStack)
        {
            EntityItem entityItem = (EntityItem)CreateEntity(EntityType.ITEM, x, y);
            entityItem.ItemStack = itemStack;
            return entityItem;
        }

        public void PickupItem(EntityPlayer player, EntityItem item)
        {
            bool result = player.inventory.AddItem(item.ItemStack.Material, item.ItemStack.Amount);
            if (result) RemoveEntity(item);
        }

        public void MakeExplosion(Vector2 position, float radius)
        {
            Instantiate(resourceStorage.ExplosionParticlesPrefab, new Vector3(position.x, position.y, -2), Quaternion.Euler(-90, 0, 0));
            AudioSource.PlayClipAtPoint(resourceStorage.ExplosionAudio, position);
            RaycastHit2D[] hits = Physics2D.CircleCastAll(position, radius, Vector2.zero);
            foreach(RaycastHit2D hit in hits)
            {
                Debug.Log("Boom");
                Block block = hit.collider.GetComponent<Block>();
                if(block)
                {
                    Debug.Log("Boom1");
                    chunkContainer.BreakBlock((int)block.transform.position.x, (int)block.transform.position.y, null);
                } else
                {
                    Entity entity = hit.collider.GetComponent<Entity>();
                    if(entity)
                    {
                        if(entity is EntityLiving)
                        {
                            EntityLiving living = (EntityLiving)entity;
                            living.Damage(18f);
                            ExplosionForce(living.rigid, position, radius);
                        } else
                        {
                            entity.Remove();
                        }
                    }
                }
            }


        }

        public void ExplosionForce(Rigidbody2D rb, Vector2 explosionPosition, float explosionRadius)
        {
            var explosionDir = rb.position - explosionPosition;
            var explosionDistance = explosionDir.magnitude;

            rb.velocity = explosionDir * 10 / explosionDistance;
        }

    }
}
