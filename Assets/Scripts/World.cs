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

    private void Start() {
        int cnt = 0;
        long total = 0;

        //Some logging to measure how long it takes to generate a chunk
        Stopwatch sw = new Stopwatch();
        sw.Start();

        for (int x = 0; x < WorldData.ChunkCount; x++)
        {
            for (int z = 0; z < WorldData.ChunkCount; z++)
            {
                sw.Restart();
                GenerateChunk((x,z));
                cnt++;
                total += sw.ElapsedMilliseconds;
            }
        }

        UnityEngine.Debug.Log($"Chunks generated in {total} ms. Avg per chunk: {total/cnt} ms.");

        //Some debugging for external coordinates
        var c1 = chunks[1,1];
        UnityEngine.Debug.Log($"{c1.ContainsBlock(new Vector3(0,0,0))} . {c1.ContainsBlock(new Vector3(11,0,11))} . {c1.ContainsBlock(new Vector3(11,0,0))} . {c1.ContainsBlock(new Vector3(0,0,11))}");
    }

    private void GenerateChunk((int x, int z) location) {
        chunks[location.x, location.z] = new Chunk(this, location);
    }

    public Block GetBlock((int x, int y, int z) pos) {
        var cPos = pos.ToChunkCoordinates();
        return chunks[cPos.x, cPos.z].GetBlockFromGlobal(pos);
    }

    public bool IsInWorld((int x, int y, int z) pos)
        => (pos.x >= 0 && pos.x < WorldData.BlockCount -1 && pos.y >= 0 && pos.y < WorldData.ChunkHeight -1 && pos.z >= 0 && pos.z < WorldData.BlockCount -1);

    public bool IsChunkInWorld((int x, int z) pos)
        => ((pos.x >= 0 && pos.x < WorldData.ChunkCount -1 && pos.z >= 0 && pos.z < WorldData.ChunkCount -1) && chunks[pos.x, pos.z] != null);


}