using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class KeyWindow : MonoBehaviour
{
    [SerializeField]
    private Text inventory;
    [SerializeField]
    private Text equip;
    [SerializeField]
    private Text skill;
    [SerializeField]
    private Text mode;
    [SerializeField]
    private Text module;

    private int index;
    private KeyCode code;
    // Start is called before the first frame update
    void Start()
    {
        //inventory.text = player.PlayerSetting.inventory.ToString();
    }

    private void Update()
    {
        
    }

    public void OnKeySetting(int i)
    {
        index = i;
        //버튼 감지 시작
        StartCoroutine(FindKeyCode());
    }

    //버튼을 누르고 최초 1회 키입력
    private IEnumerator FindKeyCode()
    {
        while (true)
        {
            yield return null;
            // 모든 키코드를 순회하여 어떤 키가 눌렸는지 확인
            if (Sensing())
            {
                Apply(index, code);
                break;
            }
        }
    }

    private bool Sensing()
    {
        foreach (KeyCode keyCode in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKeyDown(keyCode))
            {
                code = keyCode;
                Debug.Log("Pressed key: " + code);
                return true;
            }
        }
        return false;
    }

    private void Apply(int index, KeyCode code)
    {
        switch (index)
        {
            case 0:
                inventory.text = code.ToString();
                GameManager.Instance.Players[0].PlayerSetting.inventory = code;
                break;
            case 1:

                break;
            case 2:

                break;
            case 3:

                break;
            case 4:

                break;
            default:

                break;
        }
    }
}