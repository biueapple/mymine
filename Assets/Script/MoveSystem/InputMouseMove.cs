using UnityEngine;

public class InputMouseMove : IInputMove
{
    private readonly Transform head;
    private readonly Transform body;
    private readonly float minVertical;
    private readonly float maxVertical;
    //�̰��� ������ ���� �޶����� �ϴ� ������ �����޾� ����ϴ°ɷ�
    private PlayerSetting playerSetting;

    public InputMouseMove(Transform head, Transform body, float min, float max, PlayerSetting playerSetting)
    {
        this.head = head;
        this.body = body;
        this.minVertical = min;
        this.maxVertical = max;
        this.playerSetting = playerSetting;
    }

    public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        Update(Input.GetAxisRaw("Mouse X") * playerSetting.horizontalSpeed, -Input.GetAxisRaw("Mouse Y") * playerSetting.verticalSpeed);
    }

    private void Update(float mouseHorizontal, float mouseVertical)
    {
        //�� ȸ��
        if(body != null)
        body.Rotate(Vector3.up * mouseHorizontal);
        //ī�޶� ȸ��
        if (head != null)
        {
            head.Rotate(Vector3.right * mouseVertical);
            //NormalizeAngle�� ���� ������ 0������ �� Ŀ���� maxVertical�� �̵��ϴ� ���� ����
            head.localEulerAngles = new Vector3(Mathf.Clamp(GameManager.Instance.NormalizeAngle(head.eulerAngles.x), minVertical, maxVertical), 0, 0);
        }
    }
}
