﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Noise  
{
    public static float Get2DPerlin (Vector2Int position, float offset, float scale) 
    {
        //(position.x + 0.1f) 을 하는 이유는 버그때문
        return Mathf.PerlinNoise((position.x + 0.1f) / BlockInfo.ChunkWidth * scale + offset, (position.y + 0.1f) / BlockInfo.ChunkWidth * scale + offset);
    }

    public static float Get3DPerlin (Vector3Int position, float offset, float scale) 
    {
        // https://www.youtube.com/watch?v=Aga0TBJkchM Carpilot on YouTube

        float x = (position.x + offset + 0.1f) * scale;
        float y = (position.y + offset + 0.1f) * scale;
        float z = (position.z + offset + 0.1f) * scale;

        float AB = Mathf.PerlinNoise(x, y);
        float BC = Mathf.PerlinNoise(y, z);
        float AC = Mathf.PerlinNoise(x, z);
        float BA = Mathf.PerlinNoise(y, x);
        float CB = Mathf.PerlinNoise(z, y);
        float CA = Mathf.PerlinNoise(z, x);

        return (AB + BC + AC + BA + CB + CA) / 6f;
    }
}
