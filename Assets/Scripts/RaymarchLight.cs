using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchLight : MonoBehaviour
{
    [SerializeField] private Light _light = Light.DIRECTIONAL;
    [SerializeField] private Color color = Color.white;
    [SerializeField] private float cutOff = 0;
    private enum Light
    {
        DIRECTIONAL,
        POINT,
        SPOT,
        AMBIENT
    }

    public int GetLight()
    {
        return (int)_light;
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

    public Vector3 GetDirection()
    {
        return transform.forward;
    }

    public Vector3 GetRotation()
    {
        return transform.localEulerAngles * Mathf.Deg2Rad;
    }

    public float GetCutOff()
    {
        return cutOff;
    }

}
