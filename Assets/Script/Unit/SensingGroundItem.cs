using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// 주변에 있는 grounditem을 찾아 먹는 스크립트 inventory가 필수적
/// </summary>
public class SensingGroundItem : MonoBehaviour
{
    [SerializeField]
    private float range;
    private float height;

    private InventorySystem inventorySystem;

    private DistanceDetection distanceDetection;

    // Start is called before the first frame update
    void Start()
    {
        inventorySystem = GetComponent<InventorySystem>();
        distanceDetection = new DistanceDetection(transform, range);
        height = GetComponent<Unit>() != null ? GetComponent<Unit>().Height * 0.5f : 0;
    }

    private GroundItem target;

    // Update is called once per frame
    void Update()
    {
        target = distanceDetection.Sensing(FlowManager.Instance.GroundItems);
        if (target != null)
        {
            target.ItemSculpture.Input(target.ItemSculpture.Item, inventorySystem.Acquisition(target.ItemSculpture));
            if (target.ItemSculpture.Amount <= 0)
            {
                FlowManager.Instance.RemoveItem(target);
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(transform.position + new Vector3(0,height,0), range);
    }
}
