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
    private List<RaymarchShape> shapes;
    Camera cam;
    [SerializeField]  private Light lightSource;

    struct ShapeData
    {
        public int type;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 rot;
        public Vector3 ambient;
        public Vector4 diffuse;
        public Vector3 specular;
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

    void Init()
    {
        
        
    }

    private void Start()
    {
        //renderTexture = new RenderTexture(256, 256, 24);
       // renderTexture.enableRandomWrite = true;
       // renderTexture.Create();
       // raymarch.SetTexture(0, "Result", renderTexture);
       // raymarch.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        cam = Camera.current;
        //shapes = new List<RaymarchShape>(FindObjectsOfType<RaymarchShape>());
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
        //Debug.Log(destination);
        raymarch.SetTexture(0, "source", source);
        raymarch.SetTexture(0, "dest", renderTexture);
        //raymarch.SetTexture(0, "Result", renderTexture);
        raymarch.SetFloat("Resolution", renderTexture.width);
        raymarch.Dispatch(0, renderTexture.width / 8, renderTexture.height / 8, 1);

        Graphics.Blit(renderTexture, destination);

        if(shapeBuffer != null)
        {
            shapeBuffer.Dispose();
        }
        
    }

    private ShapeData[] GetShapes()
    {
        List<RaymarchShape> list = new List<RaymarchShape>(FindObjectsOfType<RaymarchShape>());
        ShapeData[] result = new ShapeData[list.Count];

        for (int i = 0; i < list.Count; i++)
        {
            
            result[i].type = list[i].GetShape();
            result[i].position = list[i].GetPosition();
            result[i].scale = list[i].GetScale();
            result[i].rot = list[i].GetRotation();
            result[i].ambient = list[i].GetAmbient();
            result[i].diffuse = list[i].GetColor();
            result[i].specular = list[i].GetSpecular();
            
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
        int shapeDataBytes = sizeof(int) + sizeof(float)*(3+3+3+3+4+3);
        shapeBuffer = new ComputeBuffer(shapeDataArr.Length, shapeDataBytes);
        shapeBuffer.SetData(shapeDataArr);
        ShapeData[] temp = new ShapeData[2];
        //shapeBuffer.GetData(temp);
        //Debug.Log(temp[0].position);
        //Debug.Log(shapeBuffer[0].position);
        raymarch.SetBuffer(0, "shapes", shapeBuffer);
        raymarch.SetInt("shapesLen", shapeDataArr.Length);
        //Render Texture
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
        raymarch.SetVector("light", lightSource.transform.position);
        
    }
}
