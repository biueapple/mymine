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

    //투명한 부분이 들어간 블록인지 (주위에있는 블록을 그려야 하는지 판단해야 하기에)
    [SerializeField]
    protected readonly bool transparent;
    public bool Transparent { get => transparent; }

    //블록이 단단한지 (지나갈 수 있는지)
    [SerializeField]
    protected readonly bool isSolid;
    public bool IsSolid { get => isSolid; }

    //이 블록을 파괴했을때 무슨 아이템을 드랍할 것인가
    [SerializeField]
    protected readonly int itemID;
    public int ItemID { get => itemID; }

    [Header("Texture Values 순서는 BlockInfo.faceChecks 참고")]
    [SerializeField]
    protected readonly int[] FaceTexture;
    public int GetTextureID(int index) 
    { 
        if (FaceTexture == null || FaceTexture.Length == 0)
        {
            Debug.Log("텍스쳐가 존재하지 않습니다.");
            return -1;
        }
        if(index < 0 || index >= FaceTexture.Length)
        {
            Debug.Log("가지고 있는 텍스쳐의 범위를 넘어갔습니다.");
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
