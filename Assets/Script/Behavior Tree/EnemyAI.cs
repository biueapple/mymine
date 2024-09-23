using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField]
    private float maxHealth;
    private float health;

    [SerializeField]
    private float chaseRange;
    [SerializeField]
    private float attackRange;

    [SerializeField]
    private Transform player;
    [SerializeField]
    private Cover[] avaliableCovers;

    private Material material;

    private Transform bestCoverSpot;

    private BehaviorTreeNode topNode;

    private void Start()
    {
        health = maxHealth;
        material = GetComponent<MeshRenderer>().material;
        ConstructBehaviourTree();
    }

    private void ConstructBehaviourTree()
    {
        IsCovereAvaliableNode coverAvaliableNode = new (avaliableCovers, player, this);
        GoToCoverNode goToCoverNode = new (this);
    }

    public float GetCurrentHealth()
    {
        return health;
    }

    public void SetColor(Color color)
    {
        material.color = color;
    }

    public void SetBestCoverSpot(Transform transform)
    {
        this.bestCoverSpot = transform;
    }

    public Transform GetBestCoverSpot()
    {
        return bestCoverSpot;
    }
}
