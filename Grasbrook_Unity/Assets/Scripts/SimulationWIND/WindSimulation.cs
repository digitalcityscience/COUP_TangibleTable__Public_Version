using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class WindSimulation : MonoBehaviour
{
    public WindScenario windScenario;
    public List<Color> colors;
    public Material dataMat;

    BuildingManager buildingManager;
    CalculationModules_Interface calculationModules;
    GameObject windParent;

    [SerializeField]
    Light directionalLight;
    [SerializeField]
    LegendResults legendResults;

    void Start()
    {
        windScenario = new WindScenario();
        windScenario.wind_direction = 0;
        windScenario.wind_speed = 30;
        GlobalVariable.GlobalWindDirection = windScenario.wind_direction;
        GlobalVariable.GlobalWindSpeed = windScenario.wind_speed;
        windScenario.result_format = "png";

        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        calculationModules = GameObject.Find("NetworkingManager").GetComponent<CalculationModules_Interface>();
    }

    public void DisplayWindData(WindData windResponse)
    {
        GameObject previousParent = GameObject.Find("WindResults");
        GameObject windParent;

        if (previousParent != null)
        {
            windParent = previousParent;
        }
        else
        {
            windParent = new GameObject("WindResults");
        }

        string boxID = "Wind-Window";

        Transform existingBox = windParent.transform.Find(boxID);

        if (existingBox)
        {
            Destroy(existingBox.gameObject);
        }

        List<Vector2> vertexList = new List<Vector2>();

        Vector2 swcoords = new Vector2(windResponse.results.bbox_sw_corner[0][1], windResponse.results.bbox_sw_corner[0][0]);

        for (int k = 0; k < windResponse.results.bbox_coordinates.Length; k++)
        {
            Vector2 coords = new Vector2(windResponse.results.bbox_coordinates[k][1], windResponse.results.bbox_coordinates[k][0]);
            vertexList.Add(buildingManager.ConvertToUnitySpace(coords));
        }

        byte[] tiffBytes = Convert.FromBase64String(windResponse.results.image_base64_string);
        print("Tiffbytes");
        print(tiffBytes.Length);

        int dimX = windResponse.results.img_width;
        int dimY = windResponse.results.img_height;

        Texture2D tex = new Texture2D(dimX, dimY);
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
                
                switch (color.grayscale)
                {
                    case 0f:
                        tex.SetPixel(x, y, colors[0]);
                        break;

                    case 2f / 255f:
                        tex.SetPixel(x, y, colors[1]);
                        break;

                    case 4f / 255f:
                        tex.SetPixel(x, y, colors[2]);
                        break;

                    case 6f / 255f:
                        tex.SetPixel(x, y, colors[3]);
                        break;

                    case 8f / 255f:
                        tex.SetPixel(x, y, colors[4]);
                        break;

                    case 10f / 255f:
                        tex.SetPixel(x, y, colors[5]);
                        break;

                    default:
                        tex.SetPixel(x, y, colors[0]);
                        break;
                }
            }
        }

        tex.Apply();

        vertexList.RemoveAt(vertexList.Count - 1);

        GameObject plane = buildingManager.GenerateSurface(vertexList, 0.2f, boxID, dataMat);
        plane.transform.parent = windParent.transform;
        plane.tag = "Simulation";
        windParent.transform.position = new Vector3(-2, 3, 2);

        calculationModules.CurrentlyLoading = false;
        calculationModules.WindResult = windParent;
        calculationModules.DeactivateWaitingSwirl();
        calculationModules.WindResult.SetActive(true);
        calculationModules.WindResultIsActive = true;

        Renderer renderer = plane.GetComponent<Renderer>();
        renderer.material.mainTexture = tex;
        directionalLight.shadows = LightShadows.None;
        legendResults.AktivateLegendResults("wind");

    }

}