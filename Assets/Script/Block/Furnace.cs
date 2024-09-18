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

    private float fuelTime = 0;   // Fire에 대한 타이머
    private float foodTime = 0;   // Completion에 대한 타이머

    //0번 슬롯이 연료 1번슬롯이 태워질 물체 2번 슬롯이 결과물
    public Furnace(int id) : base(id, "화로", HardnessType.PICKAX, 3, false, true, 12, new int[6] { 261, 259, 262, 262, 261, 261 })
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

    //아이템이 들어오면 확인하고 불 부분의 아이템을 하나 없애고 그 시간만큼 불을 붙여 내용물을 굽기
    private void Check(ItemSlot itemSlot)
    {
        //0번과 1번에 올바른 물체가 들어오고 3번이 비어있거나 결과물과 똑같은 아이템이 았어야지만 태우기 시작함
        //한번 타기 시작하면 그다음은 그냥 뭐든 소모됨
        //0번엔 연료 //1번엔 태워질 물체
        if(storage.Slots[0].Item is IFlammable flammable && storage.Slots[1].Item is IThermal && baking == null)
        {
            //타기 시작
            fuelTime = flammable.Flammable;
            baking = GameManager.Instance.StartCoroutine(Baking(flammable.Flammable));
            //연료 소모
            storage.Slots[0].Update(storage.Slots[0].Item, storage.Slots[0].Amount - 1);
        }
    }

    private IEnumerator Baking(float burnTime)
    {
        // 연료가 태워지고, 구워지는 동안 기다림
        while (fuelTime > 0)
        {
            if (storage.Slots[1].Item is IThermal thermal)
            {
                // 연료 진행 상태 (0에서 1 사이)
                furnaceInterface.Fire = fuelTime / burnTime;

                // 음식 구워지는 진행 상태 계산 (0에서 1 사이)
                furnaceInterface.Completion = foodTime / thermal.Thermal;

                if (foodTime >= thermal.Thermal)
                {
                    Result(thermal);
                    foodTime = 0;  // 음식이 구워지면 음식 타이머만 초기화
                }

                //음식의 완성도는 음식이 있어야만 진행
                foodTime += Time.deltaTime;
            }
            else
            {
                // 음식이 없을 때는 구워지는 진행 상태를 초기화
                foodTime = 0;
            }

            yield return null;
            //연료의 소모는 무조건적으로 이뤄짐
            fuelTime -= Time.deltaTime;
        }

        baking = null;  // 연료가 모두 타면 baking 상태를 종료
        Check(null);
    }

    //이 함수는 콜백으로 들어가는 함수가 아님
    //1번 슬롯의 아이템이 충분히 구워졌으면 2번슬롯에 결과물이 나와야 함 Done함수로 작동함
    private void Result(IThermal thermal)
    {
        if(thermal != null)
        {
            // 구워질 아이템이 구워졌으면, 그 결과물이 결과 슬롯에 나와야 함
            if (storage.Slots[2].Item == null)
            {
                // 결과 슬롯이 비어 있으면 결과 아이템을 넣음
                storage.Slots[2].Update(new ItemSculpture(thermal.Done(), 1));
            }
            else
            {
                // 이미 결과 아이템이 있다면 해당 아이템의 수량을 증가시킴
                storage.Slots[2].Update(storage.Slots[2].Item, storage.Slots[2].Amount + 1);
            }

            // 구워진 아이템 하나 제거
            storage.Slots[1].Update(storage.Slots[1].Item, storage.Slots[1].Amount - 1);
        }
    }
}
