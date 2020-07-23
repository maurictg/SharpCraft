using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Block {
    public byte BlockType {get; private set;}
    public BlockType Type { get {return (BlockType < WorldData.BlockTypes.Length) ? WorldData.BlockTypes[(BlockType)] : new BlockType("Unknown");} }

    public Block(byte type = 0) {
        BlockType = type;
    }

}