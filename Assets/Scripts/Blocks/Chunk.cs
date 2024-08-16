using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Blocks
{
    public class Chunk : MonoBehaviour
    {
        private ResourceStorage resourceStorage;
        private ChunkContainer chunkContainer;

        private Block[,] Blocks;

        private void Awake()
        {
            resourceStorage = GameObject.Find("ResourceStorage").GetComponent<ResourceStorage>();
            chunkContainer = GameObject.Find("ChunkContainer").GetComponent<ChunkContainer>();

            Blocks = new Block[chunkContainer.ChunkWidth, chunkContainer.ChunkHeight];
            for (int x = 0; x < chunkContainer.ChunkWidth; x++)
            {
                for (int y = 0; y < chunkContainer.ChunkHeight; y++)
                {
                    GameObject blockObject = Instantiate(resourceStorage.BlockPrefab, transform);
                    Block block = blockObject.GetComponent<Block>();
                    block.transform.localPosition = new Vector3(x + 0.5f, y + 0.5f, 0);
                    Blocks[x, y] = block;
                }
            }
        }

        public void Generate(Generator generator)
        {
            for (int x = 0; x < chunkContainer.ChunkWidth; x++)
            {
                for (int y = 0; y < chunkContainer.ChunkHeight; y++)
                {
                    //!TODO fix chunk
                    Blocks[x, y].Material = generator.GetMaterial(0, x, y);
                }
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            for (int x = 0; x < chunkContainer.ChunkWidth; x++)
            {
                for (int y = 0; y < chunkContainer.ChunkHeight; y++)
                {
                    Blocks[x, y].Material = (Material)reader.ReadInt32();
                }
            }
        }

        public void Serialize(BinaryWriter writer)
        {
            for (int x = 0; x < chunkContainer.ChunkWidth; x++)
            {
                for (int y = 0; y < chunkContainer.ChunkHeight; y++)
                {
                    writer.Write((int)Blocks[x, y].Material);
                }
            }
        }

        public void SetBlock(int x, int y, Material material)
        {
            Blocks[x, y].Material = material;
        }

        public Block GetBlock(int x, int y)
        {
            return Blocks[x, y];
        }


    }
}