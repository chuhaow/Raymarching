using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchShape : MonoBehaviour
{
    [SerializeField] private Shape shape = Shape.CUBE;
    [SerializeField] private Vector3 ambient = new Vector3(0.3f, 0.3f, 0.3f);
    [SerializeField] private Color diffuse = Color.white;
    [SerializeField] private Vector3 specular = new Vector3(1, 1, 1); 
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
        return diffuse;
    }

    public Vector3 GetScale()
    {
        return transform.localScale;
    }

    public Vector3 GetRotation()
    {
        return transform.localEulerAngles * Mathf.Deg2Rad;
    }

    public Vector3 GetAmbient()
    {
        return ambient;
    }

    public Vector3 GetSpecular()
    {
        return specular;
    }
}
