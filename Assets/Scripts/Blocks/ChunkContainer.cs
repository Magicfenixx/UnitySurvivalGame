using Assets.Scripts.Entities;
using Assets.Scripts.Items;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts.Blocks
{
    public class ChunkContainer : MonoBehaviour
    {

        private EntityContainer entityContainer;
        private ResourceStorage resourceStorage;

        private Dictionary<int, Chunk> chunks;
        private Generator generator;
        public int ChunkWidth;
        public int ChunkHeight;
        public string ChunksPath;
        private System.Random random;

        private void Awake()
        {
            entityContainer = GameObject.Find("EntityContainer").GetComponent<EntityContainer>();
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();

            chunks = new Dictionary<int, Chunk>();
            generator = new FlatGenerator(47, 5, 0.1);
            random = new System.Random();
        }

        public void LoadChunk(int chunkX)
        {
            if (IsLoadedChunk(chunkX)) return;

            GameObject chunkObject = Instantiate(resourceStorage.ChunkPrefab, transform);
            Chunk chunk = chunkObject.GetComponent<Chunk>();
            chunk.transform.position = new Vector2(ChunkWidth * chunkX, 0);

            string chunkPath = Application.dataPath + "/" + ChunksPath + "/" + chunkX + ".dat";
            if (File.Exists(chunkPath))
            {
                using (var stream = File.Open(chunkPath, FileMode.Open))
                {
                    using (var reader = new BinaryReader(stream, Encoding.UTF8, false))
                    {
                        chunk.Deserialize(reader);
                    }
                }
            }
            else
            {
                chunk.Generate(generator);
            }
            chunks[chunkX] = chunk;
        }

        public void UnloadChunk(int chunkX)
        {
            if (!chunks.ContainsKey(chunkX)) return;
            Chunk chunk = chunks[chunkX];

            string chunkPath = Application.dataPath + "/" + ChunksPath + "/" + chunkX + ".dat";
            using (var stream = File.Open(chunkPath, FileMode.Create))
            {
                using (var writer = new BinaryWriter(stream, Encoding.UTF8, false))
                {
                    chunk.Serialize(writer);
                }
            }
            Destroy(chunk.gameObject);
            chunks.Remove(chunkX);
        }

        public bool IsLoadedChunk(int x)
        {
            return chunks.ContainsKey(x);
        }

        public int ToChunkX(float x)
        {
            if (x >= 0) return (int)x / ChunkWidth;
            return (int)((x + 1) / ChunkWidth) - 1;
        }

        public void ToBlockPos(float x, float y, out int blockX, out int blockY)
        {
            blockX = (int)Math.Floor(x);
            blockY = (int)Math.Floor(y);
        }

        public Block GetBlock(int x, int y)
        {
            if (y < 0 || y >= ChunkHeight) return null;
            int chunkX = ToChunkX(x);
            if (!chunks.ContainsKey(chunkX)) return null;
            Chunk chunk = chunks[chunkX];
            int relX = x - (int)chunk.transform.position.x;
            return chunk.GetBlock(relX, y);
        }

        public void BreakBlock(int x, int y, EntityPlayer player)
        {
            Block block = GetBlock(x, y);
            if (block == null || block.Material == Material.AIR) return;
            EntityItem item = entityContainer.DropItem(x + 0.5f, y + 0.5f, new ItemStack(block.Material, 1));
            item.rigid.velocity = new Vector2((float)random.NextDouble() - 0.5f, (float)random.NextDouble() / 3);
            block.Material = Material.AIR;
        }

        public void PlaceBlock(int x, int y, EntityPlayer player)
        {
            ItemStack item = player.inventory.GetItem(player.CurrentSlot);
            if (item == null) return;
            Block block = GetBlock(x, y);
            if (block.Material != Material.AIR) return;
            player.inventory.SubstractItem(player.CurrentSlot, 1);
            block.Material = item.Material;
            if(block.Material == Material.BOMB)
            {
                block.Material = Material.AIR;
                entityContainer.CreateEntity(EntityType.BOMB, x + 0.5f, y + 0.5f);
            }
        }

        public void BreakBlock(int x, int y)
        {
            BreakBlock(x, y, null);
        }

        private void OnApplicationQuit()
        {
            HashSet<int> toUnload = new HashSet<int>(chunks.Keys);
            foreach(var chunkX in toUnload)
            {
                UnloadChunk(chunkX);
            }
        }

    }
}
