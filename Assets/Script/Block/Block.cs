using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static World;

[System.Serializable]
public class Block
{
    [SerializeField]
    protected readonly int id;
    public int ID { get => id; }

    [SerializeField]
    protected readonly string blockName;
    public string BlockName { get => blockName; }

    [SerializeField]
    protected readonly HardnessType type;
    public HardnessType Type { get => type; }

    [SerializeField]
    protected readonly int hard;
    public int Hard { get => hard; }

    //������ �κ��� �� ������� (�������ִ� ����� �׷��� �ϴ��� �Ǵ��ؾ� �ϱ⿡)
    [SerializeField]
    protected readonly bool transparent;
    public bool Transparent { get => transparent; }

    //����� �ܴ����� (������ �� �ִ���)
    [SerializeField]
    protected readonly bool isSolid;
    public bool IsSolid { get => isSolid; }

    //�� ����� �ı������� ���� �������� ����� ���ΰ�
    [SerializeField]
    protected readonly int itemID;
    public int ItemID { get => itemID; }

    [Header("Texture Values ������ BlockInfo.faceChecks ����")]
    [SerializeField]
    protected readonly int[] FaceTexture;
    public int GetTextureID(int index) 
    { 
        if (FaceTexture == null || FaceTexture.Length == 0)
        {
            Debug.Log("�ؽ��İ� �������� �ʽ��ϴ�.");
            return -1;
        }
        if(index < 0 || index >= FaceTexture.Length)
        {
            Debug.Log("������ �ִ� �ؽ����� ������ �Ѿ���ϴ�.");
            return -1;
        }
            
        return FaceTexture[index];
    }

    public Block(int id, string blockName, HardnessType type, int hard, bool transparent, bool isSolid, int itemID, int[] faceTexture)
    {
        this.id = id;
        this.blockName = blockName;
        this.type = type;
        this.hard = hard;
        this.transparent = transparent;
        this.isSolid = isSolid;
        this.itemID = itemID;
        FaceTexture = faceTexture;
    }

    public float HardnessFigure(Item item)
    {
        if(hard == 0)
            return 0;

        switch (type)
        {
            case HardnessType.PICKAX:
                if(item is Strength_Pickax pickax)
                    return pickax.Strength / hard * Time.deltaTime;
                break;
            case HardnessType.AXE:
                if (item is Strength_Axe axe)
                    return axe.Strength / hard * Time.deltaTime;
                break;
            case HardnessType.SHOVELS:
                if (item is Strength_Shovels  shovels)
                    return shovels.Strength / hard * Time.deltaTime;
                break;
            case HardnessType.HOE:
                if (item is Strength_Hoe hoe)
                    return hoe.Strength / hard * Time.deltaTime;
                break;
        }
        return 1 / (float)hard * Time.deltaTime;
    }
}
