using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockData
{
	public static readonly int TextureBlockCnt = 16;
	public static readonly float TextureSize = 1f / (float)TextureBlockCnt;

    public static readonly Vector3[] vertices = new Vector3[8] {
		new Vector3(0.0f, 0.0f, 0.0f),
		new Vector3(1.0f, 0.0f, 0.0f),
		new Vector3(1.0f, 1.0f, 0.0f),
		new Vector3(0.0f, 1.0f, 0.0f),
		new Vector3(0.0f, 0.0f, 1.0f),
		new Vector3(1.0f, 0.0f, 1.0f),
		new Vector3(1.0f, 1.0f, 1.0f),
		new Vector3(0.0f, 1.0f, 1.0f),
	};

    public static readonly int[,] triangles = new int[6,4] {
        // 0 1 2 2 1 3
		{0, 3, 1, 2}, // Back Face
		{5, 6, 4, 7}, // Front Face
		{3, 7, 2, 6}, // Top Face
		{1, 5, 0, 4}, // Bottom Face
		{4, 7, 0, 3}, // Left Face
		{1, 2, 5, 6} // Right Face
	};

	public static readonly Vector2[] uvs = new Vector2[4] {
		new Vector2 (0.0f, 0.0f),
		new Vector2 (0.0f, 1.0f),
		new Vector2 (1.0f, 0.0f),
		new Vector2 (1.0f, 1.0f)
	};

	//This is used to calculate if there is a block that covers the current block
	public static readonly (int, int, int)[] faceChecks = new (int, int, int)[6] {
		(0, 0, -1), //Back
		(0, 0, 1), //Front
		(0, 1, 0), //Top
		(0, -1, 0), //Bottom
		(-1, 0, 0), //Left
		(1, 0, 0) //Right
	};

	//Helpers
	public static Vector3 ToVector3(this (int x, int y, int z) pos) => new Vector3(pos.x,pos.y,pos.z);
	public static (int x, int y, int z) ToCoordinates(this Vector3 v) => ((int)v.x, (int)v.y, (int)v.z);

	public static (int x, int z) ToChunkCoordinates(this (int x, int y, int z) pos)
		=> (pos.x / WorldData.ChunkWidth, pos.z / WorldData.ChunkWidth);
	
	//Coordinates
	public static (int x, int y, int z) Add(this (int x, int y, int z) s, (int x, int y, int z) c) => (s.x + c.x, s.y + c.y, s.z + c.z);
	public static (int x, int y, int z) Substract(this (int x, int y, int z) s, (int x, int y, int z) c) => (s.x - c.x, s.y - c.y, s.z - c.z);

	public static int GetBlockID(string name) {
        for (int i = 0; i < WorldData.BlockTypes.Length; i++)
            if(WorldData.BlockTypes[i].Name == name)
                return i;
        return 0;
    }

}