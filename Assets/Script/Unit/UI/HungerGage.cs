using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HungerGage : MonoBehaviour
{
    [SerializeField]
    private HungerSystem hungerSystem;

    [SerializeField]
    private Player player;

    [SerializeField]
    private Image[] icons;

    // Start is called before the first frame update
    void Start()
    {
        hungerSystem = new HungerSystem(player.MoveSystem);
        hungerSystem.HungerCallback += IconControl;
    }

    // Update is called once per frame
    void Update()
    {
        hungerSystem.Calculation();
    }

    private void IconControl(float hunger)
    {
        //
        for(int i = 0; i < icons.Length; i++)
        {
            icons[i].fillAmount = hunger;
            hunger -= 2;
        }
    }
}
