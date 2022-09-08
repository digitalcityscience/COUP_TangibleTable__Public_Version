using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class AbmSimulation : MonoBehaviour
{

    public AbmScenario abmScenario;
    public List<Color> colors;
    public Material dataMat;
    [SerializeField]
    LegendResults legendResults;

    BuildingManager buildingManager;
    CalculationModules_Interface calculationModules;

    [SerializeField]
    Texture2D[] texture2Ds = new Texture2D[15];
    new Renderer renderer;

    // Start is called before the first frame update
    void Start()
    {
        abmScenario = new AbmScenario();
        abmScenario.abm_time = 8;
        abmScenario.blocks = "open";
        abmScenario.bridge_hafencity = true;
        abmScenario.main_street_orientation = "vertical";
        abmScenario.roof_amenities = "random";
        abmScenario.underpass_veddel_north = true;

        GlobalVariable.GlobalABMTime = abmScenario.abm_time;
        GlobalVariable.GlobalABMBlocks = abmScenario.blocks;
        GlobalVariable.GlobalABMBridgeToHC = abmScenario.bridge_hafencity;
        GlobalVariable.GlobalABMStreetOrientation = abmScenario.main_street_orientation;
        GlobalVariable.GlobalABMAmenities = abmScenario.roof_amenities;
        GlobalVariable.GlobalABMUnderpassVN = abmScenario.underpass_veddel_north;
        //abmScenario.result_format = "png";

        buildingManager = GameObject.Find("BuildingManager").GetComponent<BuildingManager>();
        calculationModules = GameObject.Find("NetworkingManager").GetComponent<CalculationModules_Interface>();
    }



    public void DisplayAbmResult(AbmData abmResponse)
    {
        GameObject previousParent = GameObject.Find("AbmResults");
        GameObject abmParent;
        
        if (previousParent != null)
        {
            abmParent = previousParent;
        }
        else
        {
            abmParent = new GameObject("AbmResults");
        }

        string boxID = "ABM-Window";

        Transform existingBox = abmParent.transform.Find(boxID);

        if (existingBox)
        {
            Destroy(existingBox.gameObject);
        }

        List<Vector2> vertexList = new List<Vector2>();

        Vector2 swcoords = new Vector2(abmResponse.coordinates.bbox_sw_corner[0][1], abmResponse.coordinates.bbox_sw_corner[0][0]);
        Vector2 swunityCoords = buildingManager.ConvertToUnitySpace(swcoords);

        for (int k = 0; k < abmResponse.coordinates.bbox_coordinates.Length; k++)
        {
            Vector2 coords = new Vector2(abmResponse.coordinates.bbox_coordinates[k][1], abmResponse.coordinates.bbox_coordinates[k][0]);
            vertexList.Add(buildingManager.ConvertToUnitySpace(coords));
        }

        

        for (int t = 0; t < abmResponse.results.Length; t++)
        {
            byte[] tiffBytes = Convert.FromBase64String(abmResponse.results[t].image_base64_string);

            int dimX = abmResponse.coordinates.img_width;
            int dimY = abmResponse.coordinates.img_height;

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
                        tex.SetPixel(x, y, Color.clear);
                    }
                    else if (color.grayscale == 1f/255f)
                    {
                        tex.SetPixel(x, y, colors[1]);
                    }
                    else if (color.grayscale == 2f/255f)
                    {
                        tex.SetPixel(x, y, colors[2]);
                    }
                    else if (color.grayscale > 2/255f && color.grayscale <= 4 / 255f)
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
            switch (abmResponse.results[t].hour)
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
        plane.transform.parent = abmParent.transform;
        plane.tag = "Simulation";

        calculationModules.CurrentlyLoading = false;
        calculationModules.AbmResult = abmParent;
        calculationModules.DeactivateWaitingSwirl();
        calculationModules.AbmResult.SetActive(true);
        calculationModules.AbmResultIsActive = true;

        renderer = plane.GetComponent<Renderer>();
        renderer.material.mainTexture = texture2Ds[0];
        renderer.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        renderer.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        renderer.material.EnableKeyword("_ALPHABLEND_ON");
        legendResults.AktivateLegendResults("abm");
    }


    public void ChangeAbmTexture(int arrayNumber)
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
