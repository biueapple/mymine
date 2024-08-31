using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ItemSlot
{
    [SerializeField]
    protected ItemSculpture itemSculpture;
    public ItemSculpture ItemSculpture { get { return  itemSculpture; } }
    public Item Item { get { return itemSculpture.Item; } }
    public int Amount { get { return itemSculpture.Amount; } }

    protected bool input;
    public bool Input { get { return input; } }

    protected bool output;
    public bool Output { get { return output; } }

    private Action<ItemSlot> beforeUpdate;
    public Action<ItemSlot> BeforeUpdate { get { return beforeUpdate; } set { beforeUpdate = value; } }
    private Action<ItemSlot> afterUpdate;
    public Action<ItemSlot> AfterUpdate { get { return afterUpdate; } set {  afterUpdate = value; } }

    public ItemSlot(bool input, bool output)
    {
        this.input = input;
        this.output = output;
        itemSculpture = new ItemSculpture(null, 0);
    }

    /// <summary>
    /// 무조건 아이템과 갯수를 바꿈
    /// </summary>
    /// <param name="item"></param>
    /// <param name="amount"></param>
    public virtual void Update(ItemSculpture itemSculpture)
    {
        beforeUpdate?.Invoke(this);

        this.itemSculpture.Input(itemSculpture);

        afterUpdate?.Invoke(this);
    }
    public virtual void Update(Item item, int amount)
    {
        beforeUpdate?.Invoke(this);

        itemSculpture.Input(item, amount);

        afterUpdate?.Invoke(this);
    }

    //주는쪽이 몇개를 줄지는 선택 가능하지만 받는쪽은 몇개를 받을지 선택 불가능 (다 받거나 아예 안받거나)

    /// <summary>
    /// 내가 이친구에게 몇개를 줄것인지
    /// </summary>
    /// <param name="itemSlot">받을 친구</param>
    /// <param name="amount">몇개를 줄지</param>
    /// <returns></returns>
    public virtual bool Give(ItemSlot itemSlot, int amount)
    {
        if(!output) return false;

        int min = Mathf.Min(amount, Amount);
        if (itemSlot.Take(new ItemSculpture(Item, min)))
        {
            Update(Item, Amount - min);
            return true;
        }
        return false;
    }

    public virtual bool Take(ItemSculpture itemSculpture)
    {
        //내가 몇개를 받을것인지
        if(Necessary(itemSculpture))
        {
            //무조건 추가되는 형식 상대와 내가 같거나 내가 비어있는 경우에만 받는거니까
            Update(itemSculpture.Item, itemSculpture.Amount + Amount);
            itemSculpture.Input(null, 0);
            return true;
        }

        return false;
    }

    /// <summary>
    /// 전부 줄껀데 불가능하면 스왑이라도 하겠다
    /// </summary>
    /// <returns></returns>
    public virtual bool Reference(ItemSlot itemSlot)
    {
        if(Give(itemSlot, Amount))
        {
            return true;
        }
        else
        {
            //스왑
            ItemSculpture first = new (Item, Amount);
            ItemSculpture second = new (itemSlot.Item, itemSlot.Amount);

            //나와 상대의 슬롯을 비워두고
            itemSculpture.Input(null, 0);
            itemSlot.itemSculpture.Input(null, 0);
            //서로 받을 수 있는지 확인
            if(Take(second) && itemSlot.Take(first))
            {
                //가능하면 스왑성공
                return true;
            }
            else
            {
                //그렇지 않다면 다시 서로의 것을 돌려주고 끝
                itemSculpture = first;
                itemSlot.itemSculpture = second;
            }
        }
        return false;
    }

    public virtual bool Necessary(ItemSculpture itemSculpture)
    {
        if(input)
        {
            //나한테 주는놈이 빈 아이템을 들고왔음
            if (itemSculpture.Item == null || itemSculpture.Amount == 0)
            {
                return true;
            }
                
            //내가 비어있음
            if (Item == null || Amount == 0)
            {
                return true;
            }
            //나와 상대의 아이템이 같음
            else if(Item == itemSculpture.Item && Item.Stackable)
            {
                return true;
            }
        }
        return false;
    }
}
