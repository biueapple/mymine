using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Test : MonoBehaviour
{

    public GameObject cube1;

    public GameObject cube2;

    public GameObject cube3;

    void Start()
    {
        //player.STAT.Be_Attacked_Bleeding(new DotInfomation(scarecrow.STAT, 10, 10));

        //player.STAT.Be_Attacked_Bleeding(new DotInfomation(scarecrow.STAT, 10, 1));
        //player.STAT.Be_Attacked_Bleeding(new DotInfomation(player.STAT, 10, 1));

        List<(Vector3Int, int)> dir = new()
        {
            (new Vector3Int(1, 0, 0), 4),
            (new Vector3Int(-1, 0, 0), 4),
            (new Vector3Int(0, 0, 1), 4),
            (new Vector3Int(0, 0, -1), 4),
            (new Vector3Int(0, -1, 0), 8),
            (new Vector3Int(0, 1, 0), 1)
        };
        Worm worm = Worm_Algorithm.Instance.Start(new Vector3Int(0, 0, 0), dir);

        for (int i = 0; i < worm.pathRange.Count; i++)
        {
            if (!worm.path.Contains(worm.pathRange[i]))
                Instantiate(cube1, worm.pathRange[i], Quaternion.identity);
        }

        for (int i = 0; i < worm.path.Count; i++)
        {
            Instantiate(cube2, worm.path[i], Quaternion.identity);
        }

        for (int i = 0; i < worm.wall.Count; i++)
        {
            Instantiate(cube3, worm.wall[i], Quaternion.identity);
        }
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
