using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class FlowManager : MonoBehaviour
{
    private static FlowManager instance;
    public static FlowManager Instance
    { 
        get
        {
            if (instance == null)
                instance = new GameObject("FlowManager").AddComponent<FlowManager>();
            return instance; 
        }
    }

    private GroundItem pre;
    private List<GroundItem> groundItems = new ();
    public GroundItem[] GroundItems { get { return groundItems.ToArray(); } }
    public GroundItem ADD { set { groundItems.Add(value); } }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        pre = Resources.Load<GroundItem>("World/GroundItem");
    }

    public GroundItem DropItem(Vector3 position, int id, int amount, Vector3 dir)
    {
        GroundItem item = ObjectPooling.Instance.CreateItem();
        StartCoroutine(WatingTime(item));
        item.transform.position = position;
        item.ItemSculpture = new ItemSculpture(GameManager.Instance.GetItem(id), amount);
        MoveSystem system = item.gameObject.AddComponent<MoveSystem>();
        system.AddMoveMode(new GravityForce(system.Machine));
        system.AddMoveMode(new WorldMovePhysicsShift(system.Machine, null));
        system.AddExternalForcesGround(dir * 3);

        return item;
    }

    private IEnumerator WatingTime(GroundItem item)
    {
        yield return new WaitForSeconds(1);
        groundItems.Add(item);
    }

    public void RemoveItem(GroundItem groundItem)
    {
        groundItems.Remove(groundItem);
        ObjectPooling.Instance.RemoveItem(groundItem);
    }
}
