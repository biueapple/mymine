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
    /// ������ �����۰� ������ �ٲ�
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

    //�ִ����� ��� ������ ���� ���������� �޴����� ��� ������ ���� �Ұ��� (�� �ްų� �ƿ� �ȹްų�)

    /// <summary>
    /// ���� ��ģ������ ��� �ٰ�����
    /// </summary>
    /// <param name="itemSlot">���� ģ��</param>
    /// <param name="amount">��� ����</param>
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
        //���� ��� ����������
        if(Necessary(itemSculpture))
        {
            //������ �߰��Ǵ� ���� ���� ���� ���ų� ���� ����ִ� ��쿡�� �޴°Ŵϱ�
            Update(itemSculpture.Item, itemSculpture.Amount + Amount);
            itemSculpture.Input(null, 0);
            return true;
        }

        return false;
    }

    /// <summary>
    /// ���� �ٲ��� �Ұ����ϸ� �����̶� �ϰڴ�
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
            //����
            ItemSculpture first = new (Item, Amount);
            ItemSculpture second = new (itemSlot.Item, itemSlot.Amount);

            //���� ����� ������ ����ΰ�
            itemSculpture.Input(null, 0);
            itemSlot.itemSculpture.Input(null, 0);
            //���� ���� �� �ִ��� Ȯ��
            if(Take(second) && itemSlot.Take(first))
            {
                //�����ϸ� ���Ҽ���
                return true;
            }
            else
            {
                //�׷��� �ʴٸ� �ٽ� ������ ���� �����ְ� ��
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
            //������ �ִ³��� �� �������� ������
            if (itemSculpture.Item == null || itemSculpture.Amount == 0)
            {
                return true;
            }
                
            //���� �������
            if (Item == null || Amount == 0)
            {
                return true;
            }
            //���� ����� �������� ����
            else if(Item == itemSculpture.Item && Item.Stackable)
            {
                return true;
            }
        }
        return false;
    }
}
