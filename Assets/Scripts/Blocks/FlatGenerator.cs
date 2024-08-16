using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Blocks
{
    public class FlatGenerator : Generator
    {

        public readonly int Height;
        public readonly double GoldChance;
        private readonly System.Random random;

        public FlatGenerator(int seed, int height, double goldChance)
        {
            Height = height;
            GoldChance = goldChance;
            random = new System.Random(seed);
        }

        public Material GetMaterial(int chunkX, int x, int y)
        {
            if (y > Height) return Material.AIR;
            if (y == Height) return Material.GRASS;
            double value = random.NextDouble();
            if (value <= GoldChance) return Material.GOLD;
            return Material.STONE;
        }

    }
}