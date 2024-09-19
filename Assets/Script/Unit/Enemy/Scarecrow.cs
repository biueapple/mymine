using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Enemy
{
    [Header("공격받았을때 밀려나지 않을 것인가")]
    [SerializeField]
    private bool fixation;

    // Start is called before the first frame update
    new void Start()
    {
        //base.Start();
        moveSystem.AddMoveMode(new GravityForce(moveSystem.Machine));

        moveSystem.AddMoveMode(new WorldMovePhysicsShift(moveSystem.Machine, GetComponent<Unit>()));

        //stat.Be_Attacked_Poison(new DotInfomation(10, 3));

    }

    public override Transform Head => null;

    public override Transform Body => null;

    // Update is called once per frame
    void Update()
    {
        
    }

    public override void Pushed(Vector3 dir, float power)
    {
        if(!fixation)
            base.Pushed(dir, power);
    }

    protected override void Dead()
    {
        
    }
}
