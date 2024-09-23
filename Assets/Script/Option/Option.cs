using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Option : MonoBehaviour
{
    private static Option instance; 
    public static Option Instance
    {
        get
        {
            if(instance == null)
            {
                instance = ObjectPooling.Instance.CreateObject(Resources.Load<GameObject>("UI/Option"), UIManager.Instance.Canvas.transform, new Vector2(Screen.width * 0.5f, Screen.height * 0.5f), Quaternion.identity).GetComponent<Option>();
            }
            return instance;
        }
    }

    [SerializeField]
    private GameObject graphicWindow;
    [SerializeField]
    private GameObject keyWindow;

    private void Awake()
    {
        instance = this;
        Debug.Log(instance);
    }

    public void OnGraphicWindow()
    {
        keyWindow.SetActive(false);
        graphicWindow.SetActive(true);
    }

    public void OnKeyWindow()
    {
        keyWindow.SetActive(true);
        graphicWindow.SetActive(false);
    }
}
