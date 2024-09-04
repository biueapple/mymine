using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StartButton : MonoBehaviour
{
    public void OnStartButton()
    {
        GameManager.Instance.Initialization();
    }
}
