using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class World : MonoBehaviour
{
    public Material material;

    //Data
    private Chunk[,] chunks;
    private int chunkCount;

    private void Start() {
        this.chunks = new Chunk[WorldData.ChunkCount, WorldData.ChunkCount];
        this.chunkCount = 0;

        //Some logging to measure how long it takes to generate a chunk
        Stopwatch sw = new Stopwatch();
        sw.Start();

        //Generate chunks
        for (int x = 0; x < WorldData.ChunkCount; x++)
            for (int z = 0; z < WorldData.ChunkCount; z++)
                GenerateChunk((x,z));

        UnityEngine.Debug.Log($"{chunkCount} chunks generated in {sw.ElapsedMilliseconds} ms. Avg per chunk: {sw.ElapsedMilliseconds/chunkCount} ms.");
        sw.Restart();

        //Render Chunks
        for (int x = 0; x < WorldData.ChunkCount; x++)
            for(int z = 0; z < WorldData.ChunkCount; z++)
                chunks[x,z].Render(this);

        UnityEngine.Debug.Log($"{chunkCount} chunks rendered in {sw.ElapsedMilliseconds} ms. Avg per chunk: {sw.ElapsedMilliseconds/chunkCount} ms.");
        sw.Stop();
    }

    private void GenerateChunk((int x, int z) location) {
        chunks[location.x, location.z] = new Chunk(location, this);
        chunkCount++;
    }

    public Block GetBlock((int x, int y, int z) pos) {
        var cPos = pos.ToChunkCoordinates();
        return chunks[cPos.x, cPos.z].GetBlockFromGlobal(pos);
    }

    public bool IsInWorld((int x, int y, int z) pos)
        => (pos.x >= 0 && pos.x < WorldData.BlockCount && pos.y >= 0 && pos.y < WorldData.ChunkHeight && pos.z >= 0 && pos.z < WorldData.BlockCount);

    public bool IsChunkInWorld((int x, int z) pos)
        => ((pos.x >= 0 && pos.x < WorldData.ChunkCount && pos.z >= 0 && pos.z < WorldData.ChunkCount) && chunks[pos.x, pos.z] != null);


}