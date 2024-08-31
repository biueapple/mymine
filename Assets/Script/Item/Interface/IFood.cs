
public interface IFood : IConsume
{
    /// <summary>
    /// 허기를 채워주는 양
    /// </summary>
    public int Fullness { get; }
}
