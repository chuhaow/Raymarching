using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaymarchCam : MonoBehaviour
{
    [SerializeField] private RenderTexture renderTexture;
    [SerializeField] private ComputeShader raymarch;
    private ComputeBuffer shapeBuffer;
    private ShapeData[] shapeDataArr;
    private List<RaymarchShape> shapes;
    Camera cam;
    [SerializeField]  private Light lightSource;

    struct ShapeData
    {
        public int type;
        public Vector3 position;
        public Vector3 scale;
        public Vector3 rot;
        public Vector4 color;
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
            result[i].color = list[i].GetColor();
            
        }
        return result;
    }

    private void SetShaderParam()
    {
        
        //Shape data
        int shapeDataBytes = sizeof(int) + sizeof(float)*(3+3+3+4);
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
        raymarch.SetVector("light", lightSource.transform.position);
        
    }
}
