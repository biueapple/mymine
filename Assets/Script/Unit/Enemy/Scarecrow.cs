using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scarecrow : Unit
{
    [Header("공격받았을때 밀려나지 않을 것인가")]
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
