using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldData
{
    public static readonly int ChunkHeight = 20;
    public static readonly int ChunkWidth = 10;
    
    //World size in chunks
    public static readonly int ChunkCount = 5;
    public static readonly int BlockCount = ChunkCount * ChunkWidth;

    public static BlockType[] BlockTypes = {
        /* 0 */ new BlockType("Air", null, false),
        /* 1 */ new BlockType("Grass", new FaceTextures(2, 2, 7, 1, 2, 2)),
        /* 2 */ new BlockType("Stone", new FaceTextures(3)),
        /* 3 */ new BlockType("Dirt", new FaceTextures(1)),
        /* 4 */ new BlockType("Cobblestone", new FaceTextures(8)),
        /* 5 */ new BlockType("Bedrock", new FaceTextures(9)),
        /* 6 */ new BlockType("Sand", new FaceTextures(10)),
        /* 7 */ new BlockType("Bricks", new FaceTextures(11)),
        /* 8 */ new BlockType("Furnace", new FaceTextures(12, 13, 15, 15, 13))
    };
}
