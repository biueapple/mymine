
public interface IConsume
{
    public int MaxDurability { get; }
    /// <summary>
    /// ������ (���� ��)
    /// </summary>
    public int Durability { get; }

    public void Consume();
}
