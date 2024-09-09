
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
    /// 기본이 되는 수치
    /// </summary>
    public float Figure { get => figure; }

    private readonly float levelFigure;
    /// <summary>
    /// 스킬이 1레벨 올라갈때마다 올라가는 수치
    /// </summary>
    public float LevelFigure { get => levelFigure; }

    private readonly Attribute_Property coefficientType;
    /// <summary>
    /// 어떤 능력치가 계수의 영향을 끼치는지
    /// </summary>
    public Attribute_Property CoefficientType { get => coefficientType; }

    private readonly float coefficientFigure;
    /// <summary>
    /// 계수가 얼마인지
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
