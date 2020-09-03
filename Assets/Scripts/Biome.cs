using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Biome
{
    public string name {get; set;}
    public int groundHeight {get; set;}
    public int terrainHeight {get; set;}
    public float terrainScale {get; set;}
    public Lode[] lodes {get;set;}
}

public class Lode
{
    public Block block {get; set;}
    public int minHeight {get; set;}
    public int maxHeight {get; set;}
    public float scale {get; set;}
    public float threshold {get; set;}
    public int noiseOffset {get; set;}
}