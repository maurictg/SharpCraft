using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FaceDirection {
    BACK, FRONT, TOP, BOTTOM, LEFT, RIGHT
}

[System.Serializable]
public class FaceTextures
{
    private int[] textures;
    public int GetTextureID(int direction) => textures[direction];
    public int GetTextureID(FaceDirection direction) => textures[(int)direction];

    public FaceTextures(int? front = 0, int? back = null, int? top = null, int? bottom = null, int? left = null, int? right = null) {
        textures = new int[6];
        textures[0] = (int)(back ?? front ?? 0);
        textures[1] = (int)(front ?? 0);
        textures[2] = (int)(top ?? front ?? 0);
        textures[3] = (int)(bottom ?? top ?? front ?? 0);
        textures[4] = (int)(left ?? front ?? 0);
        textures[5] = (int)(right ?? left ?? front ?? 0);
    }
}
