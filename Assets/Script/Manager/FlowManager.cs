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
        Item item = GameManager.Instance.GetItem(id);
        if (item != null)
        {
            GroundItem ground = ObjectPooling.Instance.CreateItem();
            StartCoroutine(WatingTime(ground));
            ground.transform.position = position;
            ground.ItemSculpture = new ItemSculpture(item, amount);
            MoveSystem system = ground.gameObject.AddComponent<MoveSystem>();
            system.AddMoveMode(new GravityForce(system.Machine));
            system.AddMoveMode(new WorldMovePhysicsShift(system.Machine, null));
            system.AddExternalForcesGround(dir * 3);

            return ground;
        }
        return null;
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
