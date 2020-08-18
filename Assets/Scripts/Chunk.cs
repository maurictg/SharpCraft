using System;
using System.Diagnostics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chunk
{
	private GameObject chunckObj;
    private MeshRenderer meshRenderer;
	private MeshFilter meshFilter;
	private World world;


	//Vertice Index
    private int v_index = 0;
    private List<int> triangles = new List<int>();
    private List<Vector2> uvs = new List<Vector2>();
    private List<Vector3> vertices = new List<Vector3>();

	//Data
	public Block[,,] blocks {get; private set;}

	//Location
	public int X {get; private set;}
	public int Z {get; private set;}

	public bool Active {get { return chunckObj.activeSelf; } set { chunckObj.SetActive(value); }}
	public Vector3 Location {get {return chunckObj.transform.position;}}

	public Chunk((int x, int z) location, World world) {
		this.X = location.x;
		this.Z = location.z;

		this.blocks = new Block[WorldData.ChunkWidth, WorldData.ChunkHeight, WorldData.ChunkWidth];
		this.world = world;
		this.chunckObj = new GameObject();
		this.meshFilter = chunckObj.AddComponent<MeshFilter>();
		this.meshRenderer = chunckObj.AddComponent<MeshRenderer>();

		meshRenderer.material = world.material;
		chunckObj.transform.SetParent(world.transform);
		chunckObj.transform.position = new Vector3(X * WorldData.ChunkWidth, 0f, Z * WorldData.ChunkWidth);
		chunckObj.name = $"Chunk {X}, {Z} ({Location.x},{Location.z})";


		//Fill chunk with furnaces
		for (int y = 0; y < WorldData.ChunkHeight; y++)
			for (int x = 0; x < WorldData.ChunkWidth; x++)
				for (int z = 0; z < WorldData.ChunkWidth; z++)
				{
					//Basic terrain
					if(y == 0) {
						SetBlock((x,y,z), BlockData.GetBlockID("Bedrock"));
					} else if(y > 0 && y < WorldData.ChunkHeight - 3) {
						SetBlock((x,y,z), BlockData.GetBlockID("Stone"));
					} else if(y < WorldData.ChunkHeight - 1) {
						SetBlock((x,y,z), BlockData.GetBlockID("Dirt"));
					} else {
						SetBlock((x,y,z), BlockData.GetBlockID("Grass"));
					}
				}
	}

	public void Render(World world) {
		//Update world
		this.world = world;

		//Draw all blocks in the chunk and generate the mesh data
		DrawChunk();
		GenerateMesh();
	}

	private void DrawChunk() {
		for (int y = 0; y < WorldData.ChunkHeight; y++)
			for (int x = 0; x < WorldData.ChunkWidth; x++)
				for (int z = 0; z < WorldData.ChunkWidth; z++)
					DrawBlock((x,y,z));
	}

	private bool IsCovered((int x, int y, int z) pos, int face) {
		var faceCoord = pos.Add(BlockData.faceChecks[face]);
		var worldCoord = faceCoord.Add(this.Location.ToCoordinates());

		if(IsInChunk(faceCoord) && GetBlock(faceCoord).Type.IsSolid) {
			return true;
		} else {
			if(!world.IsChunkInWorld(worldCoord.ToChunkCoordinates()) || !world.IsInWorld(worldCoord))
				return false;
			
			//Get block from world
			return world.GetBlock(worldCoord).Type.IsSolid;
		}
	}

	private void DrawBlock((int x, int y, int z) pos) {
		Block block = GetBlock(pos);

		//Don't draw block if it is not solid or not in the chunk
		if(block.Type.IsSolid && IsInChunk(pos)) {
			Vector3 vec = pos.ToVector3();

			for (int i = 0; i < 6; i++)
            {
				//Check if face is covered
				if(IsCovered(pos, i))
					continue;
					

                vertices.Add(vec + BlockData.vertices[BlockData.triangles[i, 0]]);
                vertices.Add(vec + BlockData.vertices[BlockData.triangles[i, 1]]);
                vertices.Add(vec + BlockData.vertices[BlockData.triangles[i, 2]]);
                vertices.Add(vec + BlockData.vertices[BlockData.triangles[i, 3]]);

                DrawTexture(block.Type.FaceTextures.GetTextureID(i));

                triangles.Add(v_index + 0);
                triangles.Add(v_index + 1);
                triangles.Add(v_index + 2);
                triangles.Add(v_index + 2);
                triangles.Add(v_index + 1);
                triangles.Add(v_index + 3);

                v_index += 4;
            }
		}
	}

	private void DrawTexture(int id) {
		float y = id / BlockData.TextureBlockCnt;
		float x = id - (y * BlockData.TextureBlockCnt);

		x *= BlockData.TextureSize;
		y *= BlockData.TextureSize;

		y  = 1f - y - BlockData.TextureSize;

		uvs.Add(new Vector2(x, y));
		uvs.Add(new Vector2(x, y + BlockData.TextureSize));
		uvs.Add(new Vector2(x + BlockData.TextureSize, y));
		uvs.Add(new Vector2(x + BlockData.TextureSize, y + BlockData.TextureSize));
	}

	private void GenerateMesh() {
        //Generate Mesh data
        Mesh mesh = new Mesh();

        mesh.vertices = vertices.ToArray();
        mesh.triangles = triangles.ToArray();
        mesh.uv = uvs.ToArray();
        mesh.RecalculateNormals();

        meshFilter.mesh = mesh;
	}

	private void ClearMesh() {
		//Clear mesh data
		v_index = 0;
		vertices.Clear();
		triangles.Clear();
		uvs.Clear();
	}

	//Internal getters/setters
	private Block GetBlock((int x, int y, int z) pos)
		=> blocks[pos.x, pos.y, pos.z];

	private void SetBlock((int x, int y, int z) pos, int blockType = 0)
		=> blocks[pos.x,pos.y,pos.z] = new Block((byte)blockType);

	private bool IsInChunk((int x, int y, int z) pos)
		=> (pos.x >= 0 && pos.x < WorldData.ChunkWidth && pos.y >= 0 && pos.y < WorldData.ChunkHeight && pos.z >= 0 && pos.z < WorldData.ChunkWidth);


	//External getters/setters
	public Block GetBlockFromGlobal((int x, int y, int z) pos) {
		pos = pos.Substract(Location.ToCoordinates());
		return (IsInChunk(pos)) ? GetBlock(pos) : new Block();
	}
}
