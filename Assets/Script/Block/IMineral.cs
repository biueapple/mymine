using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMineral
{
    /// <summary>
    /// 광물이 있을 확률 0.01정도면 평범함
    /// </summary>
    public float Probability { get; }
    /// <summary>
    /// 가장 아래에 나올 수 있는 좌표
    /// </summary>
    public int MinHeight { get; }
    /// <summary>
    /// 가장 위에 나올 수 있는 좌표
    /// </summary>
    public int MaxHeight { get; }
    /// <summary>
    /// 최소한의 깊이
    /// </summary>
    public int Depth { get; }  

    public Vector3Int[][] Shape { get; }

    public bool Possibility(int y, int yHeight)
    {
        //지상으로부터 depth만큼은 아래여야 함
        if (y <= yHeight - Depth)
        {
            //그러면서 maxHeight minHeight 사이의 높이여야 함
            if (y < MaxHeight && y > MinHeight)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3Int[] ShapeDir(Vector3Int dir)
    {
        // dir에 따른 행을 반환. 예를 들어, dir.x 값을 기준으로 처리할 수 있습니다.
        int row = Random.Range(0, Shape.Length); // 예시로 dir.x 값을 행 인덱스로 사용

        return Shape[row];
    }
}
