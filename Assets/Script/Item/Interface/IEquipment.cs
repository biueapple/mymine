
//어디에 장착이 가능하지는 또 인터페이스로 만들기 예) ITopEquipment 인터페이스가 있다면 상의에 장착이 가능하고 추가로 IBottomEquipment가 있다면 하의에도 장착이 가능한 식으로
//추가로 장비에 IConsume을 붙여서 소모품으로 만들 수 있음
public interface IEquipment
{
    /// <summary>
    /// 능력치
    /// </summary>
    public AttributePiece[] AttributePieces { get; }
    public Equipment_Part EquipmentPart { get; }
}
