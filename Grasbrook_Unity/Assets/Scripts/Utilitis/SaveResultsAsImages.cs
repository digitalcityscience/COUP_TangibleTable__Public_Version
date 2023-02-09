using System;
using UnityEditor;
using UnityEngine;

public class SaveResultsAsImages : MonoBehaviour
{

    public void TakeScreenShot()
    {
        RenderCameraToFile();
    }

    public static void RenderCameraToFile()
    {
        Camera camera = Camera.main;

        RenderTexture rt = new RenderTexture(2560, 1440, 24, RenderTextureFormat.ARGB32, RenderTextureReadWrite.Default);
        RenderTexture oldRT = camera.targetTexture;

        camera.targetTexture = rt;
        camera.Render();

        camera.targetTexture = oldRT;

        RenderTexture.active = rt;
        Texture2D tex = new Texture2D(rt.width, rt.height, TextureFormat.ARGB32, false);
        tex.ReadPixels(new Rect(0, 0, rt.width, rt.height), 0, 0);
        RenderTexture.active = null;

        byte[] bytes = tex.EncodeToPNG();
        string path = "Assets/SavedResults/" + DateTime.Now.ToString("ddMMyyyy_hhmmss") + ".png";
        System.IO.File.WriteAllBytes(path, bytes);
        AssetDatabase.ImportAsset(path);
        Debug.Log("Saved to"+ path);
    }
}
