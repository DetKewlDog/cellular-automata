using UnityEngine;

public class AutomataRenderer : MonoBehaviour
{
    public ComputeShader computeShader;
    public Texture2D inputTexture;
    public UnityEngine.UI.RawImage rawImage;
    public Material material;
    RenderTexture rt1, rt2, output;
    int kernel;

    void Start()
    {
        rt1 = new RenderTexture(inputTexture.width, inputTexture.height, 24);
        rt1.enableRandomWrite = true;
        rt1.filterMode = FilterMode.Point;
        rt1.Create();

        rt2 = new RenderTexture(inputTexture.width, inputTexture.height, 24);
        rt2.enableRandomWrite = true;
        rt2.filterMode = FilterMode.Point;
        rt2.Create();

        output = new RenderTexture(inputTexture.width, inputTexture.height, 24);
        output.enableRandomWrite = true;
        output.filterMode = FilterMode.Point;
        output.Create();

        rawImage.texture = output;

        Graphics.Blit(inputTexture, rt1);
        kernel = computeShader.FindKernel("CSMain");
        InvokeRepeating("Compute", 0, 0.05f);
    }

    void Compute() {
        computeShader.SetTexture(kernel, "Input", rt1);
        computeShader.SetTexture(kernel, "Result", rt2);
        computeShader.Dispatch(kernel, 32, 32, 1);
        Graphics.Blit(rt2, rt1);
        Graphics.Blit(rt1, output);
    }
}
