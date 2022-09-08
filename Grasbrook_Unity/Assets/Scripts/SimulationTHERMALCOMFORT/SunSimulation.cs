using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class SunSimulation : MonoBehaviour
{
    public SunScenario sunScenario;
    public List<Color> colors;
    public Material dataMat;

    BuildingManager buildingManager;
    CalculationModules_Interface calculationModules;
    GameObject sunParent;
    [SerializeField]
    LegendResults legendResults;

    Texture2D[] texture2Ds = new Texture2D[15];
    new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        sunScenario = new SunScenario();
        sunScenario.sun_time = 8;
        sunScenario.season = "spring";
        sunScenario.trees = true;
        sunScenario.groundConcept = true;
        sunScenario.buildingMaterial = true;

        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        calculationModules = GameObject.Find("NetworkingManager").GetComponent<CalculationModules_Interface>();
    }

    public void DisplaSunResult(SunData sunResponse)
    {
        GameObject previousParent = GameObject.Find("SunResult");
        
        if(previousParent != null)
        {
            sunParent = previousParent;
        }
        else
        {
            sunParent = new GameObject("SunResult");
        }

        string boxID = "Sun-Window";

        Transform existingBox = sunParent.transform.Find(boxID);

        if (existingBox)
        {
            Destroy(existingBox.gameObject);
        }

        List<Vector2> vertexList = new List<Vector2>();

        for (int k = 0; k < sunResponse.coordinates.bbox_coordinates.Length; k++)
        {
            Vector2 coords = new Vector2(sunResponse.coordinates.bbox_coordinates[k][1], sunResponse.coordinates.bbox_coordinates[k][0]);
            vertexList.Add(buildingManager.ConvertToUnitySpace(coords));
        }

        for (int t = 0; t < sunResponse.result.Length; t++)
        {
            byte[] tiffBytes = Convert.FromBase64String(sunResponse.result[t].image_base64_string);

            int dimX = sunResponse.coordinates.img_width;
            int dimY = sunResponse.coordinates.img_height;

            Texture2D tex = new Texture2D(dimX, dimY, TextureFormat.ARGB32, false);
            tex.filterMode = FilterMode.Point;

            var dirPath = Application.dataPath + "/../SaveImages";

            if (!Directory.Exists(dirPath))
            {
                Directory.CreateDirectory(dirPath);
            }

            tex.LoadImage(tiffBytes);
            tex.Apply();

            for (int y = 0; y < tex.height; y++)
            {
                for (int x = 0; x < tex.width; x++)
                {
                    Color color = tex.GetPixel(x, y);

                    if (color.grayscale == 0f)
                    {
                        tex.SetPixel(x, y, colors[0]);
                    }
                    else if (color.grayscale == 1f / 255f)
                    {
                        tex.SetPixel(x, y, colors[1]);
                    }
                    else if (color.grayscale == 2f / 255f)
                    {
                        tex.SetPixel(x, y, colors[2]);
                    }
                    else if (color.grayscale > 2 / 255f && color.grayscale <= 4 / 255f)
                    {
                        tex.SetPixel(x, y, colors[3]);
                    }
                    else if (color.grayscale > 4 / 255f && color.grayscale <= 6 / 255f)
                    {
                        tex.SetPixel(x, y, colors[4]);
                    }
                    else if (color.grayscale > 6 / 255f && color.grayscale <= 10 / 255f)
                    {
                        tex.SetPixel(x, y, colors[5]);
                    }
                    else if (color.grayscale > 10 / 255f && color.grayscale <= 20 / 255f)
                    {
                        tex.SetPixel(x, y, colors[6]);
                    }
                    else if (color.grayscale > 20 / 255f)
                    {
                        tex.SetPixel(x, y, colors[7]);
                    }
                    else
                    {
                        tex.SetPixel(x, y, Color.red);
                    }
                }
            }

            tex.Apply();

            //function to sort the right texture map to the right hour in the texture2D array
            switch (sunResponse.result[t].hour)
            {
                case 8:
                    texture2Ds[0] = tex;
                    break;
                case 9:
                    texture2Ds[1] = tex;
                    break;
                case 10:
                    texture2Ds[2] = tex;
                    break;
                case 11:
                    texture2Ds[3] = tex;
                    break;
                case 12:
                    texture2Ds[4] = tex;
                    break;
                case 13:
                    texture2Ds[5] = tex;
                    break;
                case 14:
                    texture2Ds[6] = tex;
                    break;
                case 15:
                    texture2Ds[7] = tex;
                    break;
                case 16:
                    texture2Ds[8] = tex;
                    break;
                case 17:
                    texture2Ds[9] = tex;
                    break;
                case 18:
                    texture2Ds[10] = tex;
                    break;
                case 19:
                    texture2Ds[11] = tex;
                    break;
                case 20:
                    texture2Ds[12] = tex;
                    break;
                case 21:
                    texture2Ds[13] = tex;
                    break;
                case 22:
                    texture2Ds[14] = tex;
                    break;

                default:
                    break;
            }

        }

        vertexList.RemoveAt(vertexList.Count - 1);

        GameObject plane = buildingManager.GenerateSurface(vertexList, 0.2f, boxID, dataMat);
        plane.transform.parent = sunParent.transform;
        plane.tag = "Simulation";

        calculationModules.CurrentlyLoading = false;
        calculationModules.SunResult = sunParent;
        calculationModules.DeactivateWaitingSwirl();
        calculationModules.SunResult.SetActive(true);

        renderer = plane.GetComponent<Renderer>();
        renderer.material.mainTexture = texture2Ds[0];
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        legendResults.AktivateLegendResults("sun");

    }

    public void ChangeSunTexture(int arrayNumber)
    {
        switch (arrayNumber)
        {
            case 0:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 1:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 2:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 3:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 4:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 5:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 6:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 7:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 8:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 9:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 10:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 11:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 12:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 13:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;
            case 14:
                renderer.material.mainTexture = texture2Ds[arrayNumber];
                break;

            default:
                break;
        }

    }

}
