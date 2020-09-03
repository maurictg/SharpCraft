using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;

    //Data
    private Chunk[,] chunks = new Chunk[WorldData.ChunkCount, WorldData.ChunkCount];
    private int chunkCount = 0;

    private void Start() {
        //Some logging to measure how long it takes to generate a chunk
        Stopwatch sw = new Stopwatch();
        sw.Start();

        //Generate chunks
        for (int x = 0; x < WorldData.ChunkCount; x++)
            for (int z = 0; z < WorldData.ChunkCount; z++)
            {
                chunks[x, z] = new Chunk((x, z), this);
                chunkCount++;
            }

        UnityEngine.Debug.Log($"{chunkCount} chunks generated in {sw.ElapsedMilliseconds} ms. Avg per chunk: {sw.ElapsedMilliseconds/chunkCount} ms.");
        sw.Restart();

        //Render Chunks
        for (int x = 0; x < WorldData.ChunkCount; x++)
            for(int z = 0; z < WorldData.ChunkCount; z++)
                chunks[x,z].Render(this);

        UnityEngine.Debug.Log($"{chunkCount} chunks rendered in {sw.ElapsedMilliseconds} ms. Avg per chunk: {sw.ElapsedMilliseconds/chunkCount} ms.");
        sw.Stop();
    }

    public Block GetBlock((int x, int y, int z) pos) {
        var cPos = pos.ToChunkCoordinates();
        return chunks[cPos.x, cPos.z].GetBlockFromGlobal(pos);
    }

    public bool IsInWorld((int x, int y, int z) pos)
        => (pos.x >= 0 && pos.x < WorldData.BlockCount && pos.y >= 0 && pos.y < WorldData.ChunkHeight && pos.z >= 0 && pos.z < WorldData.BlockCount);

    public bool IsChunkInWorld((int x, int z) pos)
        => ((pos.x >= 0 && pos.x < WorldData.ChunkCount && pos.z >= 0 && pos.z < WorldData.ChunkCount) && chunks[pos.x, pos.z] != null);

    public Block GenerateBlock((int x, int y, int z) pos) {
        /* IMMUTABLE PASS */

        Biome biome = WorldData.Biomes[0]; //Default

        // If outside world, return air.
        if (!IsInWorld(pos))
            return new Block();

        // If bottom block of chunk, return bedrock.
        if (pos.y == 0)
            return new Block(5);

        /* BASIC TERRAIN PASS */

        int terrainHeight = Mathf.FloorToInt(biome.terrainHeight * PerlinNoise.Get2D(new Vector2(pos.x, pos.y), 0, biome.terrainScale)) + biome.groundHeight;
        int blockId = 0;

        if (pos.y == terrainHeight)
            blockId = 1;
        else if (pos.y < terrainHeight && pos.y > terrainHeight - 4)
            blockId = 3;
        else if (pos.y > terrainHeight)
            blockId = 0;
        else
            blockId = 2;

        /* SECOND PASS */
        if(blockId == 2) {
            foreach (Lode l in biome.lodes)
            {
                if (pos.y > l.minHeight && pos.y < l.maxHeight)
                    if (PerlinNoise.Get3D(pos.ToVector3(), l.noiseOffset, l.scale, l.threshold))
                        blockId = (int)l.block.BlockType;
            }
        }

        return new Block((byte)blockId);
    }
        
}