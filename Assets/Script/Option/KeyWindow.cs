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
        //��ư ���� ����
        StartCoroutine(FindKeyCode());
    }

    //��ư�� ������ ���� 1ȸ Ű�Է�
    private IEnumerator FindKeyCode()
    {
        while (true)
        {
            yield return null;
            // ��� Ű�ڵ带 ��ȸ�Ͽ� � Ű�� ���ȴ��� Ȯ��
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