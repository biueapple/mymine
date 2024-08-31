using UnityEngine;

[System.Serializable]
public class AttributePossibility
{
    [SerializeField]
    private Attribute_Property property;
    public Attribute_Property Property { get { return property; } }
    [SerializeField]
    private int min;
    public int Min { get { return min; } }
    [SerializeField]
    private int max;
    public int Max { get { return max; } }

    public AttributePossibility(Attribute_Property property, int min, int max)
    {
        this.property = property;
        this.min = min;
        this.max = max;
    }

    public int Value()
    {
        return Random.Range(min, max + 1);
    }
}
