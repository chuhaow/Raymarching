using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchShape : MonoBehaviour
{
    [SerializeField] private Shape shape = Shape.CUBE;
    [SerializeField] private Color color = Color.white;
    private enum Shape
    {
        SPHERE,
        CUBE,
    }

    public int GetShape()
    {
        return (int)shape;
    }

    public Vector3 GetPosition()
    {
        return transform.position;
    }

    public Color GetColor()
    {
        return color;
    }

    public Vector3 GetScale()
    {
        return transform.localScale;
    }

}
