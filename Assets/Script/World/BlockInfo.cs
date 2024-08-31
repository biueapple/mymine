using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class BlockInfo
{
    //����� �ִ�ġ (�ִ�ġ�� ��θ��� ��)
    public static readonly int maxHunger = 20;
    //����� ���� (�̰� ���ϴ� �޸��� ���Ѵٴ� ��)
    public static readonly int minHunger = 6;
    //�߷�
    public static readonly float gravity = -9.8f;


    /// startChunkSize * startChunkSize + startChunkSize * startChunkSize + startChunkSize * startChunkSize + startChunkSize * startChunkSize
    public static int startChunkSize = 5;
    //���ο� ������ ũ��
    public static readonly int ChunkWidth = 16;
    //����
    public static readonly int ChunkHeight = 30;

    //���忡 ûũ�� � �ִ°�
    public static readonly int WorldSizeInChunks = 10;

    //�ؽ����� ���ο� ��� ����� �� �ִ°� 
    public static readonly int TextureAtlasSizeInBlocks = 32;
    //�ؽ����� (0~1) �ϳ��� ��� �̹����� �����ϴ� ũ�� 
    public static float NormalizedBlockTextureSize
    {
        get { return 1f / (float)TextureAtlasSizeInBlocks; }
    }

    public enum Shape
    {
        Cube,
        Stairs,
        Half,

    }

    //ť��

    //��� �ϳ��� �׸��� �ʿ��� �ﰢ���� ��ġ��
    public static readonly Vector3Int[] voxelVerts = new Vector3Int[8]
   {
        new Vector3Int(0, 0, 0),
        new Vector3Int(1, 0, 0),
        new Vector3Int(1, 1, 0),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, 0, 1),
        new Vector3Int(1, 0, 1),
        new Vector3Int(1, 1, 1),
        new Vector3Int(0, 1, 1),
   };

    //����ϳ��� ���鿡���� ��ġ
    public static readonly Vector3Int[] faceChecks = new Vector3Int[6]
    {
        new Vector3Int(0, 0, -1),
        new Vector3Int(0, 0, 1),
        new Vector3Int(0, 1, 0),
        new Vector3Int(0, -1, 0),
        new Vector3Int(-1, 0, 0),
        new Vector3Int(1, 0, 0)
    };

    //��� �ϳ��� �׸��� ����� ������ �ε�����
    public static readonly int[,] voxelTris = new int[6, 4]
    {
		// 0 1 2 2 1 3
		{0, 3, 1, 2}, // Back Face
		{5, 6, 4, 7}, // Front Face
		{3, 7, 2, 6}, // Top Face
		{1, 5, 0, 4}, // Bottom Face
		{4, 7, 0, 3}, // Left Face
		{1, 2, 5, 6} // Right Face
	};

    //

  //  //���

  //  public static readonly Vector3[] stairsVerts = new Vector3[14]
  //  {
  //      new Vector3(-0.5f,  -0.5f,  -0.5f),
  //      new Vector3(-0.5f,  0,      -0.5f),
  //      new Vector3(0.5f,   -0.5f,  -0.5f),
  //      new Vector3(0.5f,   0,      -0.5f),
  //      new Vector3(0,      0,      -0.5f),
  //      new Vector3(0,      0.5f,   -0.5f),
  //      new Vector3(0.5f,   0.5f,   -0.5f),

  //      new Vector3(-0.5f,  -0.5f,  0.5f),
  //      new Vector3(-0.5f,  0,      0.5f),
  //      new Vector3(0.5f,   -0.5f,  0.5f),
  //      new Vector3(0.5f,   0,      0.5f),
  //      new Vector3(0,      0,      0.5f),
  //      new Vector3(0,      0.5f,   0.5f),
  //      new Vector3(0.5f,   0.5f,   0.5f),
  //  };

  //  ////����ϳ��� ���鿡���� ��ġ
  //  //public static readonly Vector3Int[] stairsfaceChecks = new Vector3Int[6]
  //  //{
  //  //    new Vector3Int(0, 0, -1),     // Back Body Face                         //new Vector3Int(0, 0, -1),     // Back Head Face              
  //  //    new Vector3Int(0, 0, 1),      // Front Body Face                        //new Vector3Int(0, 0, 1),      // Front Head Face
  //  //    new Vector3Int(0, 1, 0),      // Top Body Face                          //new Vector3Int(0, 1, 0),      // Top Head Face
  //  //    new Vector3Int(0, -1, 0),     // Bottom Face
  //  //    new Vector3Int(-1, 0, 0),     // Left Body Face                         //new Vector3Int(-1, 0, 0),     // Left Head Face
  //  //    new Vector3Int(1, 0, 0)       // Right Face
  //  //};

  //  public static readonly int[][] stairsTris = new int[][]
  //  {
  //      // 0 1 2 2 1 3
		//new int[] {0, 1, 2, 3, 4, 5, 3, 6},         // Back Body Face           //{4, 5, 3, 6},       // Back Head Face
		//new int[] {9, 10, 7, 8,10, 13, 11, 12},     // Front Body Face          //{10, 13, 11, 12},   // Front Head Face
		//new int[] {1, 8, 4, 11, 5,12,6,13},         // Top Body Face            //{ 5,12,6,13},       // Top Head Face
		//new int[] {9, 7, 2, 0},                     // Bottom Face
  //      new int[] {7, 8, 0, 1,11, 12, 4, 5},        // Left Body Face           //{11, 12, 4, 5},     // Left Head Face
		//new int[] {2, 6, 9, 13},                    // Right Face
  //  };

    //���

    //�ؽ��� uvs
    public static readonly Vector2[] voxelUvs = new Vector2[4]
    {
        new Vector2 (0.0f, 0.0f),
        new Vector2 (0.0f, 1.0f),
        new Vector2 (1.0f, 0.0f),
        new Vector2 (1.0f, 1.0f)
    };
}
