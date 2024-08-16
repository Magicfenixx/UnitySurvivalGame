using Assets.Scripts.Blocks;
using Assets.Scripts.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

namespace Assets.Scripts
{
    public class ChunkLoader : MonoBehaviour
    {

        private GameManager gameManager;
        private ChunkContainer chunkContainer;

        private HashSet<int> loadedChunks;
        public int RenderDistance;

        private void Awake()
        {
            gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
            chunkContainer = GameObject.Find("ChunkContainer").GetComponent<ChunkContainer>();

            loadedChunks = new HashSet<int>();
        }

        private void FixedUpdate()
        {
            if (gameManager.MainPlayer == null) return;

            float focusX = gameManager.MainPlayer.transform.position.x;
            int blockDistance = RenderDistance * chunkContainer.ChunkWidth;
            int minX = (int)focusX - blockDistance;
            int maxX = (int)focusX + blockDistance;
            int minChunk = chunkContainer.ToChunkX(minX);
            int maxChunk = chunkContainer.ToChunkX(maxX);
            HashSet<int> capturedChunks = new HashSet<int>();
            for (int i = minChunk; i <= maxChunk; i++)
            {
                capturedChunks.Add(i);
            }
            HashSet<int> chunksToLoad = new HashSet<int>(capturedChunks.Except(loadedChunks));
            HashSet<int> chunksToUnload = new HashSet<int>(loadedChunks.Except(capturedChunks));
            loadedChunks.AddRange(chunksToLoad);
            loadedChunks.ExceptWith(chunksToUnload);
            foreach (int chunkX in chunksToLoad)
            {
                chunkContainer.LoadChunk(chunkX);
            }
            foreach (int chunkX in chunksToUnload)
            {
                chunkContainer.UnloadChunk(chunkX);
            }
        }

    }
}
