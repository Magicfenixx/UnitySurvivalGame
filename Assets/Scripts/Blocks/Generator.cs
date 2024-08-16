using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Blocks
{
    public interface Generator
    {
        Material GetMaterial(int chunkX, int x, int y);

    }
}