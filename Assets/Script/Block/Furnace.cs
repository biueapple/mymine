using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Furnace : Block, IBlockOpenUI
{
    private static FurnaceInterface furnaceInterface;
    public static FurnaceInterface FurnaceInterface
    {
        get
        {
            if (furnaceInterface == null)
            {
                furnaceInterface = Object.Instantiate(Resources.Load<FurnaceInterface>("UI/UserInterface/FurnaceUI"));
                furnaceInterface.transform.SetParent(UIManager.Instance.Canvas.transform);
                furnaceInterface.transform.localPosition = new Vector3(0, 0, 0);
            }
            return furnaceInterface;
        }
    }
    private readonly Storage storage;
    private Coroutine baking;

    private float fuelTime = 0;   // Fire�� ���� Ÿ�̸�
    private float foodTime = 0;   // Completion�� ���� Ÿ�̸�

    //0�� ������ ���� 1�������� �¿��� ��ü 2�� ������ �����
    public Furnace(int id) : base(id, "ȭ��", HardnessType.PICKAX, 3, false, true, 12, new int[6] { 261, 259, 262, 262, 261, 261 })
    {
        ItemSlot[] itemSlots = new ItemSlot[3];
        for (int i = 0; i < 2; i++)
        {
            itemSlots[i] = new ItemSlot(true, true);
            itemSlots[i].AfterUpdate += Check;
        }
        itemSlots[2] = new ItemSlot(false, true);

        storage = new Storage(itemSlots);
    }

    public void OpenUI(Player player, World.BlockLaycast blockLaycast)
    {
        FurnaceInterface.Interlock(storage);
        UIManager.Instance.OpenUI(FurnaceInterface.gameObject);
    }

    //�������� ������ Ȯ���ϰ� �� �κ��� �������� �ϳ� ���ְ� �� �ð���ŭ ���� �ٿ� ���빰�� ����
    private void Check(ItemSlot itemSlot)
    {
        //0���� 1���� �ùٸ� ��ü�� ������ 3���� ����ְų� ������� �Ȱ��� �������� �Ҿ������ �¿�� ������
        //�ѹ� Ÿ�� �����ϸ� �״����� �׳� ���� �Ҹ��
        //0���� ���� //1���� �¿��� ��ü
        if(storage.Slots[0].Item is IFlammable flammable && storage.Slots[1].Item is IThermal && baking == null)
        {
            //Ÿ�� ����
            fuelTime = flammable.Flammable;
            baking = GameManager.Instance.StartCoroutine(Baking(flammable.Flammable));
            //���� �Ҹ�
            storage.Slots[0].Update(storage.Slots[0].Item, storage.Slots[0].Amount - 1);
        }
    }

    private IEnumerator Baking(float burnTime)
    {
        // ���ᰡ �¿�����, �������� ���� ��ٸ�
        while (fuelTime > 0)
        {
            if (storage.Slots[1].Item is IThermal thermal)
            {
                // ���� ���� ���� (0���� 1 ����)
                furnaceInterface.Fire = fuelTime / burnTime;

                // ���� �������� ���� ���� ��� (0���� 1 ����)
                furnaceInterface.Completion = foodTime / thermal.Thermal;

                if (foodTime >= thermal.Thermal)
                {
                    Result(thermal);
                    foodTime = 0;  // ������ �������� ���� Ÿ�̸Ӹ� �ʱ�ȭ
                }

                //������ �ϼ����� ������ �־�߸� ����
                foodTime += Time.deltaTime;
            }
            else
            {
                // ������ ���� ���� �������� ���� ���¸� �ʱ�ȭ
                foodTime = 0;
            }

            yield return null;
            //������ �Ҹ�� ������������ �̷���
            fuelTime -= Time.deltaTime;
        }

        baking = null;  // ���ᰡ ��� Ÿ�� baking ���¸� ����
        Check(null);
    }

    //�� �Լ��� �ݹ����� ���� �Լ��� �ƴ�
    //1�� ������ �������� ����� ���������� 2�����Կ� ������� ���;� �� Done�Լ��� �۵���
    private void Result(IThermal thermal)
    {
        if(thermal != null)
        {
            // ������ �������� ����������, �� ������� ��� ���Կ� ���;� ��
            if (storage.Slots[2].Item == null)
            {
                // ��� ������ ��� ������ ��� �������� ����
                storage.Slots[2].Update(new ItemSculpture(thermal.Done(), 1));
            }
            else
            {
                // �̹� ��� �������� �ִٸ� �ش� �������� ������ ������Ŵ
                storage.Slots[2].Update(storage.Slots[2].Item, storage.Slots[2].Amount + 1);
            }

            // ������ ������ �ϳ� ����
            storage.Slots[1].Update(storage.Slots[1].Item, storage.Slots[1].Amount - 1);
        }
    }
}
