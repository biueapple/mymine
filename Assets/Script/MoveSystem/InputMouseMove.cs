using UnityEngine;

public class InputMouseMove : IInputMove
{
    private readonly Transform head;
    private readonly Transform body;
    private readonly float minVertical;
    private readonly float maxVertical;
    //이것은 설정에 따라 달라져야 하니 설정을 참조받아 사용하는걸로
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
        //몸 회전
        if(body != null)
        body.Rotate(Vector3.up * mouseHorizontal);
        //카메라 회전
        if (head != null)
        {
            head.Rotate(Vector3.right * mouseVertical);
            //NormalizeAngle를 하지 않으면 0도에서 더 커질시 maxVertical로 이동하는 일이 생김
            head.localEulerAngles = new Vector3(Mathf.Clamp(GameManager.Instance.NormalizeAngle(head.eulerAngles.x), minVertical, maxVertical), 0, 0);
        }
    }
}
