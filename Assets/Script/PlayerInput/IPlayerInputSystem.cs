public interface IPlayerInputSystem
{
    public void LeftClick();
    public void LeftDown();
    public void LeftUp();
    public void RightClick();
    public void RightDown();
    public void RightUp();
    public void Update();
    public void Enter();
    public void Exit();
    /// <summary>
    /// ������ �ִ� �������� ��带 ���� (statenomal, statebattle ���� �ϳ�)
    /// </summary>
    public IPlayerInputSystem Mode { get; }
}
