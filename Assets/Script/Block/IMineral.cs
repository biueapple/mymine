using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMineral
{
    /// <summary>
    /// ������ ���� Ȯ�� 0.01������ �����
    /// </summary>
    public float Probability { get; }
    /// <summary>
    /// ���� �Ʒ��� ���� �� �ִ� ��ǥ
    /// </summary>
    public int MinHeight { get; }
    /// <summary>
    /// ���� ���� ���� �� �ִ� ��ǥ
    /// </summary>
    public int MaxHeight { get; }
    /// <summary>
    /// �ּ����� ����
    /// </summary>
    public int Depth { get; }  

    public Vector3Int[][] Shape { get; }

    public bool Possibility(int y, int yHeight)
    {
        //�������κ��� depth��ŭ�� �Ʒ����� ��
        if (y <= yHeight - Depth)
        {
            //�׷��鼭 maxHeight minHeight ������ ���̿��� ��
            if (y < MaxHeight && y > MinHeight)
            {
                return true;
            }
        }

        return false;
    }

    public Vector3Int[] ShapeDir(Vector3Int dir)
    {
        // dir�� ���� ���� ��ȯ. ���� ���, dir.x ���� �������� ó���� �� �ֽ��ϴ�.
        int row = Random.Range(0, Shape.Length); // ���÷� dir.x ���� �� �ε����� ���

        return Shape[row];
    }
}
