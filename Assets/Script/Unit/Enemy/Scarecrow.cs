using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Unit
{
    [Header("���ݹ޾����� �з����� ���� ���ΰ�")]
    [SerializeField]
    private bool fixation;

    // Start is called before the first frame update
    void Start()
    {
        moveSystem.AddMoveMode(new GravityForce(moveSystem.Machine));

        moveSystem.AddMoveMode(new WorldMovePhysicsShift(moveSystem.Machine, GetComponent<Unit>()));

        //stat.Be_Attacked_Poison(new DotInfomation(10, 3));

    }

    Color original;
    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Pushed(Vector3 dir, float power)
    {
        if(!fixation)
            base.Pushed(dir, power);
    }
}
