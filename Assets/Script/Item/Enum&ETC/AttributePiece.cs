using UnityEngine;

[System.Serializable]
public class AttributePiece
{
    [SerializeField]
    private Attribute_Property property;
    public Attribute_Property Property { get { return property; } }
    [SerializeField]
    private float value;
    public float Value { get { return value; } }

    public AttributePiece(Attribute_Property property, float value)
    {
        this.property = property;
        this.value = value;
    }
}
