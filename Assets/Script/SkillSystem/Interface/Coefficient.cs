
public class Coefficient
{
    public Coefficient(float figure, float levelFigure, Attribute_Property coefficientType, float coefficientFigure)
    {
        this.figure = figure;
        this.levelFigure = levelFigure;
        this.coefficientType = coefficientType;
        this.coefficientFigure = coefficientFigure;
    }

    private readonly float figure;
    /// <summary>
    /// �⺻�� �Ǵ� ��ġ
    /// </summary>
    public float Figure { get => figure; }

    private readonly float levelFigure;
    /// <summary>
    /// ��ų�� 1���� �ö󰥶����� �ö󰡴� ��ġ
    /// </summary>
    public float LevelFigure { get => levelFigure; }

    private readonly Attribute_Property coefficientType;
    /// <summary>
    /// � �ɷ�ġ�� ����� ������ ��ġ����
    /// </summary>
    public Attribute_Property CoefficientType { get => coefficientType; }

    private readonly float coefficientFigure;
    /// <summary>
    /// ����� ������
    /// </summary>
    public float CoefficientFigure { get => coefficientFigure; }

    public float Calculate(int level, Stat stat)
    {
        return Figure + (LevelFigure * level) + (stat.GetAttribute(coefficientType) * CoefficientFigure) ;
    }
    public float Value(Stat stat)
    {
        return stat.GetAttribute(coefficientType);
    }
}
