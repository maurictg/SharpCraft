using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BlockType {
    public string Name {get; private set;}
    public bool IsSolid {get; private set;}
    public FaceTextures FaceTextures {get; private set;}

    public BlockType(string name, FaceTextures textures = null, bool isSolid = true) {
        Name = name;
        FaceTextures = textures ?? new FaceTextures();
        IsSolid = isSolid;
    }
}