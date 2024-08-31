
public interface IConsume
{
    public int MaxDurability { get; }
    /// <summary>
    /// 내구도 (남은 양)
    /// </summary>
    public int Durability { get; }

    public void Consume();
}
