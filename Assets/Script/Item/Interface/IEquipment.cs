
//��� ������ ���������� �� �������̽��� ����� ��) ITopEquipment �������̽��� �ִٸ� ���ǿ� ������ �����ϰ� �߰��� IBottomEquipment�� �ִٸ� ���ǿ��� ������ ������ ������
//�߰��� ��� IConsume�� �ٿ��� �Ҹ�ǰ���� ���� �� ����
public interface IEquipment
{
    /// <summary>
    /// �ɷ�ġ
    /// </summary>
    public AttributePiece[] AttributePieces { get; }
    public Equipment_Part EquipmentPart { get; }
}
