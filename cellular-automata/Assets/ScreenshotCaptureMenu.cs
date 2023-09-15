#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;
using Bitmap = System.Drawing.Bitmap;
using Graphics = System.Drawing.Graphics;

public class ScreenshotCaptureMenu : Editor
{
    [MenuItem("Tools/Take Screenshot _F12")]
    public static void MenuScreenshot() {
        var go = new GameObject();
        go.AddComponent<ScreenshotCapture>();
    }
}

public class ScreenshotCapture : MonoBehaviour
{
    public static Texture tex;
    void Start() => StartCoroutine(CaptureScreenCO());

    System.Collections.IEnumerator CaptureScreenCO()
    {
        yield return new WaitForEndOfFrame();
        ScreenshotCapture.tex = FindObjectOfType<UnityEngine.UI.RawImage>().texture;
        CopyToClipboard((Texture2D)ScreenCapture.CaptureScreenshotAsTexture(1));
        Destroy(gameObject);
        yield return null;
    }

    private static void CopyToClipboard(Texture2D texture)
    {
        var s = new System.IO.MemoryStream(texture.width * texture.height);
        byte[] bits = texture.EncodeToPNG();
        s.Write(bits, 0, bits.Length);
        var image = System.Drawing.Image.FromStream(s);
        image = (System.Drawing.Image)ResizeBitmap(image as Bitmap, tex.width, tex.height);
        System.Windows.Forms.Clipboard.SetImage(image);
        s.Close();
        s.Dispose();
    }

    private static Bitmap ResizeBitmap(Bitmap sourceBMP, int width, int height)
    {
        Bitmap result = new Bitmap(width, height);
        using (Graphics g = Graphics.FromImage(result))
        {
            g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            g.DrawImage(sourceBMP, 0, 0, width, height);
        }
        return result;
    }
}
#endif