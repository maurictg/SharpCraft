using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class WorldData
{
    public static readonly int ChunkHeight = 128; //20
    public static readonly int ChunkWidth = 16; //10
    
    //World size in chunks
    public static readonly int ChunkCount = 4; //4
    public static readonly int BlockCount = ChunkCount * ChunkWidth;

    //FaceTexture is index in Textures/Blocks.png file
    public static BlockType[] BlockTypes = {
        /* 0 */ new BlockType("Air", null, false),
        /* 1 */ new BlockType("Grass", new FaceTextures(2, 2, 7, 1, 2, 2)),
        /* 2 */ new BlockType("Stone", new FaceTextures(3)),
        /* 3 */ new BlockType("Dirt", new FaceTextures(1)),
        /* 4 */ new BlockType("Cobblestone", new FaceTextures(8)),
        /* 5 */ new BlockType("Bedrock", new FaceTextures(9)),
        /* 6 */ new BlockType("Sand", new FaceTextures(10)),
        /* 7 */ new BlockType("Bricks", new FaceTextures(11)),
        /* 8 */ new BlockType("Furnace", new FaceTextures(12, 13, 15, 15, 13)),
        /* 9 */ new BlockType("Unknown", new FaceTextures(0)),
    };

    public static Biome[] Biomes = {
        /* 0 */ new Biome() { 
            name = "Default",
            groundHeight = 42,
            terrainHeight = 42,
            terrainScale = 0.25f,
            lodes = new Lode[] {
                new Lode() {
                    block = new Block(3), //dirt
                    minHeight = 1,
                    maxHeight = 255,
                    scale = 0.1f,
                    threshold = 0.5f,
                    noiseOffset = 0,
                },
                new Lode() {
                    block = new Block(6), //sand
                    minHeight = 30,
                    maxHeight = 60,
                    scale = 0.2f,
                    threshold = 0.6f,
                    noiseOffset = 500
                }
            }
        }
    };
}
