using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{
    public Player player;
    public Scarecrow scarecrow;



    void Start()
    {
        //player.STAT.Be_Attacked_Bleeding(new DotInfomation(scarecrow.STAT, 10, 10));

        //player.STAT.Be_Attacked_Bleeding(new DotInfomation(scarecrow.STAT, 10, 1));
        //player.STAT.Be_Attacked_Bleeding(new DotInfomation(player.STAT, 10, 1));
    }

    void Update()
    {
        //CentralCollision();
        //SurfaceCollision(new Vector3(0.5f, 1, 0.5f));
    }

    public void CentralCollision()
    {
        for (int i = 0; i < Unit.AllUnits.Count; i++)
        {
            if (transform.position.x >= Unit.AllUnits[i].transform.position.x - Unit.AllUnits[i].Width && transform.position.x <= Unit.AllUnits[i].transform.position.x + Unit.AllUnits[i].Width)
            {
                if (transform.position.y >= Unit.AllUnits[i].transform.position.y && transform.position.y <= Unit.AllUnits[i].transform.position.y + Unit.AllUnits[i].Height)
                {
                    if (transform.position.z >= Unit.AllUnits[i].transform.position.z - Unit.AllUnits[i].Depth && transform.position.z <= Unit.AllUnits[i].transform.position.z + Unit.AllUnits[i].Depth )
                    {
                        Debug.Log(Unit.AllUnits[i].name + "과 중심 충돌!");
                    }
                }
            }
        }
    }
    public void SurfaceCollision(Vector3 size)
    {
        for (int i = 0; i < Unit.AllUnits.Count; i++)
        {
            if (transform.position.x + size.x >= Unit.AllUnits[i].transform.position.x - Unit.AllUnits[i].Width && transform.position.x - size.x <= Unit.AllUnits[i].transform.position.x + Unit.AllUnits[i].Width )
            {
                if (transform.position.y + size.y >= Unit.AllUnits[i].transform.position.y && transform.position.y <= Unit.AllUnits[i].transform.position.y + Unit.AllUnits[i].Height)
                {
                    if (transform.position.z + size.z >= Unit.AllUnits[i].transform.position.z - Unit.AllUnits[i].Depth && transform.position.z - size.z <= Unit.AllUnits[i].transform.position.z + Unit.AllUnits[i].Depth)
                    {
                        Debug.Log(Unit.AllUnits[i].name + "과 겉면 충돌!");
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.TryGetComponent(out Substance substance))
        {
            Debug.Log("Hit");
            AttackInformation attackInformation = new (GetComponent<Stat>(), AttackType.NONE);
            attackInformation.Additional.Add(new (1, DamageType.AD, false));
            substance.Hit(attackInformation);
        }
    }
}
