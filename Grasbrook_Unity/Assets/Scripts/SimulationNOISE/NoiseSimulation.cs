using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class NoiseSimulation : MonoBehaviour
{

    public NoiseScenario noiseScenario;
    public List<Color> colors;
    public Material dataMat;

    BuildingManager buildingManager;
    CalculationModules_Interface calculationModules;
    [SerializeField]
    LegendResults legendResults;


    void Start()
    {
        noiseScenario = new NoiseScenario();
        noiseScenario.noise_max_speed = 30;
        noiseScenario.noise_traffic_volume_percent = 15;
        GlobalVariable.GlobalNoiseCarSpeed = noiseScenario.noise_max_speed;
        GlobalVariable.GlobalNoiseCarVolume = noiseScenario.noise_traffic_volume_percent;
        noiseScenario.result_format = "png";

        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        calculationModules = GameObject.Find("NetworkingManager").GetComponent<CalculationModules_Interface>();
    }

    public void DisplayNoiseResult(NoiseData noiseResponse)
    {
        GameObject previousParent = GameObject.Find("NoiseResults");
        //GameObject previousParent = calculationModules.NoiseResult;
        GameObject noiseParent;
        if (previousParent != null)
        {
            noiseParent = previousParent;
        }
        else
        {
            noiseParent = new GameObject("NoiseResults");
        }



        string boxID = "Noise-Window";

        Transform existingBox = noiseParent.transform.Find(boxID);

        if (existingBox)
        {
            Destroy(existingBox.gameObject);
        }

        List<Vector2> vertexList = new List<Vector2>();

        Vector2 swcoords = new Vector2(noiseResponse.result.bbox_sw_corner[0][1], noiseResponse.result.bbox_sw_corner[0][0]);

        //GameObject swsphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        Vector2 swunityCoords = buildingManager.ConvertToUnitySpace(swcoords);
        //swsphere.transform.position = new Vector3(swunityCoords.x, 2, swunityCoords.y);
        //swsphere.transform.localScale = new Vector3(8, 8, 8);
        //swsphere.GetComponent<Renderer>().material.color = Color.yellow;

        for (int k = 0; k < noiseResponse.result.bbox_coordinates.Length; k++)
        {
            Vector2 coords = new Vector2(noiseResponse.result.bbox_coordinates[k][1], noiseResponse.result.bbox_coordinates[k][0]);
            vertexList.Add(buildingManager.ConvertToUnitySpace(coords));

            //GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            Vector2 unityCoords = buildingManager.ConvertToUnitySpace(coords);
            //sphere.transform.position = new Vector3(unityCoords.x, 2, unityCoords.y);
            //sphere.transform.localScale = new Vector3(5, 5, 5);
            //sphere.name = k.ToString();
            //sphere.GetComponent<Renderer>().material.color = Color.red;
        }

        byte[] tiffBytes = Convert.FromBase64String(noiseResponse.result.image_base64_string);
        print("Tiffbytes");
        print(tiffBytes.Length);

        int dimX = noiseResponse.result.img_width;
        int dimY = noiseResponse.result.img_height;

        Texture2D tex = new Texture2D(dimX, dimY);
        tex.filterMode = FilterMode.Point;

        tex.LoadImage(tiffBytes);
        tex.Apply();
        //NEVER CHANGE /255f its a unity thing to calculate all color values 
        //through 255f from normal RGB Space to Unity RGB Space that is from 0 to 1
        //the value in front is the mapped value from the API
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

                    case 1f/ 255f:
                        tex.SetPixel(x, y, colors[1]);
                        break;

                    case 2f / 255f:
                        tex.SetPixel(x, y, colors[2]);
                        break;

                    case 3f / 255f:
                        tex.SetPixel(x, y, colors[3]);
                        break;

                    case 4f / 255f:
                        tex.SetPixel(x, y, colors[4]);
                        break;

                    case 5f / 255f:
                        tex.SetPixel(x, y, colors[5]);
                        break;

                    case 6f / 255f:
                        tex.SetPixel(x, y, colors[6]);
                        break;

                    case 7f / 255f:
                        tex.SetPixel(x, y, colors[7]);
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
        plane.transform.parent = noiseParent.transform;
        plane.tag = "Simulation";
        noiseParent.transform.position = new Vector3(-2, 3, 2);
        calculationModules.NoiseResult = noiseParent;
        calculationModules.CurrentlyLoading = false;
        calculationModules.DeactivateWaitingSwirl();
        calculationModules.NoiseResult.SetActive(true);
        calculationModules.NoiseResultIsActive = true;

        Renderer renderer = plane.GetComponent<Renderer>();
        renderer.material.mainTexture = tex;
        legendResults.AktivateLegendResults("noise");

    }
}
