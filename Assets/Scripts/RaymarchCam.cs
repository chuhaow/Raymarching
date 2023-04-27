using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchCam : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private ComputeShader raymarch;
    private ComputeBuffer shapeBuffer;
    private ShapeData[] shapeDataArr;
    private ComputeBuffer lightBuffer;
    private LightData[] lightDataArr;
    Camera cam;
    [SerializeField] private bool AmbientOcclusion;
    [SerializeField] private bool FogOn;
    [Range(0, 1)]
    [SerializeField] private float FogRate;
    [SerializeField] private bool GlowOn;


    struct ShapeData
    {
        public int type;
        public int behaviour;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 rot;
        public Vector3 normal;
        public Vector3 ambient;
        public Vector4 diffuse;
        public Vector3 specular;
        public float blend;
        public float power;
    }

    struct LightData
    {
        public int type;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 rot;
        public Vector4 color;
        public Vector3 forward;
        public float cutOffAngle;
    }

    private void Awake()
    {
        cam = Camera.current;
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        cam = Camera.current;
        shapeDataArr = GetShapes();
        lightDataArr = GetLights();


        if (renderTexture == null || renderTexture.height != Screen.height || renderTexture.width != Screen.width)
        {
            if (renderTexture != null)
            {
                renderTexture.Release();

            }
            renderTexture = new RenderTexture(Screen.width, Screen.height, 0, RenderTextureFormat.ARGBFloat, RenderTextureReadWrite.Linear);
            renderTexture.enableRandomWrite = true;
            renderTexture.Create();

        }
        if (shapeDataArr.Length > 0)
        {
            SetShaderParam();
        }

        raymarch.SetTexture(0, "source", source);
        raymarch.SetTexture(0, "dest", renderTexture);

        raymarch.SetFloat("Resolution", renderTexture.width);
        raymarch.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        Graphics.Blit(renderTexture, destination);

        if(shapeBuffer != null)
        {
            shapeBuffer.Dispose();
        }
        if(lightBuffer != null)
        {
            lightBuffer.Dispose();
        }
    }

    private ShapeData[] GetShapes()
    {
        List<RaymarchShape> list = new List<RaymarchShape>(FindObjectsOfType<RaymarchShape>());
        list.Sort((a, b) => a.GetBehaviour().CompareTo(b.GetBehaviour()));
        ShapeData[] result = new ShapeData[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            
            result[i].type = list[i].GetShape();
            result[i].behaviour = list[i].GetBehaviour();
            result[i].position = list[i].GetPosition();
            result[i].scale = list[i].GetScale();
            result[i].rot = list[i].GetRotation();
            result[i].normal = list[i].GetNormal();
            result[i].ambient = list[i].GetAmbient();
            result[i].diffuse = list[i].GetColor();
            result[i].specular = list[i].GetSpecular();
            result[i].blend = list[i].GetBlendFactor();
            result[i].power = list[i].GetFractalPower();
            
        }
        return result;
    }



    private LightData[] GetLights()
    {
        List<RaymarchLight> list = new List<RaymarchLight>(FindObjectsOfType<RaymarchLight>());
        LightData[] result = new LightData[list.Count];

        for (int i = 0; i < list.Count; i++)
        {

            result[i].type = list[i].GetLight();
            result[i].position = list[i].GetPosition();
            result[i].scale = list[i].GetScale();
            result[i].rot = list[i].GetRotation();
            result[i].color = list[i].GetColor();
            result[i].forward = list[i].GetDirection();
            result[i].cutOffAngle = list[i].GetCutOff();

        }
        return result;
    }
    private void SetShaderParam()
    {
        
        //Shape data
        int shapeDataBytes = 2*sizeof(int) + sizeof(float)*(3+3+3+3+3+4+3 + 1 + 1);
        shapeBuffer = new ComputeBuffer(shapeDataArr.Length, shapeDataBytes);
        shapeBuffer.SetData(shapeDataArr);

        raymarch.SetBuffer(0, "shapes", shapeBuffer);
        raymarch.SetInt("shapesLen", shapeDataArr.Length);
        //Cam to World matrix
        raymarch.SetMatrix("CamToWorldMatrix", cam.cameraToWorldMatrix);
        //Cam inverse matrix 
        raymarch.SetMatrix("InverseProjMatrix", cam.projectionMatrix.inverse);
        //Lights
        int lightDataBytes = sizeof(int) + sizeof(float) * (3 + 3 + 3 + 4 + 3 + 1);
        lightBuffer = new ComputeBuffer(lightDataArr.Length, lightDataBytes);
        lightBuffer.SetData(lightDataArr);
        raymarch.SetBuffer(0, "lights", lightBuffer);
        raymarch.SetInt("lightsLen", lightDataArr.Length);

        raymarch.SetBool("AO", AmbientOcclusion);
        raymarch.SetBool("FogOn", FogOn);
        raymarch.SetFloat("FogRate", FogRate);
        raymarch.SetBool("Glow", GlowOn);

    }
}
