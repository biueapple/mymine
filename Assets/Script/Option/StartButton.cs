using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public GameObject cube1;

    public GameObject cube2;

    private void Start()
    {
        List<(Vector3Int, int)> dir = new()
        {
            (new Vector3Int(1, 0, 0), 4),
            (new Vector3Int(-1, 0, 0), 4),
            (new Vector3Int(0, 0, 1), 4),
            (new Vector3Int(0, 0, -1), 4),
            (new Vector3Int(0, -1, 0), 8),
            (new Vector3Int(0, 1, 0), 1)
        };
        Worm worm = Worm_Algorithm.Instance.Start(new Vector3Int(0,0,0), dir);
    
        for(int i = 0; i < worm.pathRange.Count;i++)
        {
            if (!worm.path.Contains(worm.pathRange[i]))
                Instantiate(cube2, worm.pathRange[i], Quaternion.identity);
        }

        for (int i = 0; i < worm.path.Count; i++)
        {
            Instantiate(cube1, worm.path[i], Quaternion.identity);
        }
    }

    public void OnStartButton()
    {
        GameManager.Instance.Initialization();
    }
}
