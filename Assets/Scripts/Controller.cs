using Assets.Scripts.Blocks;
using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using UnityEngine;

namespace Assets.Scripts
{
    public class Controller : MonoBehaviour
    {

        private GameManager gameManager;
        private PlayerInterface playerInterface;
        private ChunkContainer chunkContainer;
        private ResourceStorage resourceStorage;

        private bool playContext = true;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            playerInterface = GameObject.Find("PlayerInterface").GetComponent<PlayerInterface>();
            chunkContainer = GameObject.Find("ChunkContainer").GetComponent<ChunkContainer>();
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();
        }

        private void Update()
        {
            if (gameManager.MainPlayer == null) return;
            if (Input.GetKeyDown(KeyCode.E))
            {
                playContext = !playContext;
                playerInterface.OpenedShop = !playerInterface.OpenedShop;
                playerInterface.SetGoldCount(gameManager.MainPlayer.inventory.AmountOf(Material.GOLD));
            }
            if (!playContext) return;
            Vector3 mousePosition;
            Vector3 worldPosition;
            if (Input.GetMouseButtonDown(0))
            {
                mousePosition = Input.mousePosition;
                worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                int blockX;
                int blockY;
                chunkContainer.ToBlockPos(worldPosition.x, worldPosition.y, out blockX, out blockY);
                if (blockY >= 0 && blockY <= chunkContainer.ChunkHeight)
                {
                    chunkContainer.BreakBlock(blockX, blockY, gameManager.MainPlayer);
                }
            }
            if (Input.GetMouseButtonDown(1))
            {
                mousePosition = Input.mousePosition;
                worldPosition = Camera.main.ScreenToWorldPoint(mousePosition);
                int blockX;
                int blockY;
                chunkContainer.ToBlockPos(worldPosition.x, worldPosition.y, out blockX, out blockY);
                ItemStack item = gameManager.MainPlayer.inventory.GetItem(gameManager.MainPlayer.CurrentSlot);
                if(item != null && item.Material == Material.GUN) {
                    if(gameManager.MainPlayer.inventory.AmountOf(Material.AMMO) == 0)
                    {
                        AudioSource.PlayClipAtPoint(resourceStorage.EmptyShot, gameManager.MainPlayer.transform.position);
                        return;
                    }
                    gameManager.MainPlayer.inventory.TakeItems(Material.AMMO, 1);
                    Shot(gameManager.MainPlayer);
                    return;
                }
                if(item != null && item.Material == Material.AMMO)
                {
                    return;
                }
                if (blockY >= 0 && blockY <= chunkContainer.ChunkHeight)
                {
                    chunkContainer.PlaceBlock(blockX, blockY, gameManager.MainPlayer);
                }
            }
            if (Input.GetKeyDown(KeyCode.Alpha1))
            {
                gameManager.MainPlayer.CurrentSlot = 0;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha2))
            {
                gameManager.MainPlayer.CurrentSlot = 1;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha3))
            {
                gameManager.MainPlayer.CurrentSlot = 2;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha4))
            {
                gameManager.MainPlayer.CurrentSlot = 3;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha5))
            {
                gameManager.MainPlayer.CurrentSlot = 4;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                gameManager.MainPlayer.CurrentSlot = 5;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha7))
            {
                gameManager.MainPlayer.CurrentSlot = 6;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha8))
            {
                gameManager.MainPlayer.CurrentSlot = 7;
                playerInterface.UpdateSelection();
            }
            if (Input.GetKeyDown(KeyCode.Alpha9))
            {
                gameManager.MainPlayer.CurrentSlot = 8;
                playerInterface.UpdateSelection();
            }
            if (Input.mouseScrollDelta.y > 0)
            {
                gameManager.MainPlayer.CurrentSlot += 1;
                playerInterface.UpdateSelection();
            }
            if (Input.mouseScrollDelta.y < 0)
            {
                gameManager.MainPlayer.CurrentSlot -= 1;
                playerInterface.UpdateSelection();
            }
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameManager.ToggleMenuPanel();
            }
            gameManager.MainPlayer.HorizontalAxis = Input.GetAxis("Horizontal");
            gameManager.MainPlayer.Jump = Input.GetKey(KeyCode.Space);
        }

        public void Shot(EntityPlayer shooter)
        {
            AudioSource.PlayClipAtPoint(resourceStorage.ShootAudio, shooter.transform.position);
            RaycastHit2D hit = Physics2D.Raycast(shooter.gunObject.transform.position, new Vector2(shooter.transform.localRotation.y == 0 ? 1f : -1f, 0), 15);
            if(hit.collider != null)
            {
                Entity entity = hit.collider.gameObject.GetComponent<Entity>();
                if (entity) {
                    if(entity is EntityLiving)
                    {
                        EntityLiving living = (EntityLiving)entity;
                        living.Damage(9f);
                    }
                }
            }
        }

    }
}
