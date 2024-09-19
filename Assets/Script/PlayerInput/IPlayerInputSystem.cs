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
    /// 가지고 있는 오리지널 모드를 리턴 (statenomal, statebattle 둘중 하나)
    /// </summary>
    public IPlayerInputSystem Mode { get; }
}
