using UnityEngine;

public class InputMouseMove : IInputMove
{
    private Transform head;
    private Transform body;
    private float minVertical;
    private float maxVertical;
    //�̰��� ������ ���� �޶����� �ϴ� ������ �����޾� ����ϴ°ɷ�
    private float verticalSpeed = 1;
    private float horizontalSpeed = 1;

    public InputMouseMove(Transform head, Transform body, float min, float max)
    {
        this.head = head;
        this.body = body;
        this.minVertical = min;
        this.maxVertical = max;
    }

    public void InputMove(Transform transform, ref Vector3 velocity, ref Vector3 velocityMomemtum)
    {
        Update(Input.GetAxisRaw("Mouse X") * horizontalSpeed, -Input.GetAxisRaw("Mouse Y") * verticalSpeed);
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
