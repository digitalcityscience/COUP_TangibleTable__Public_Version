using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;
using System.Linq;
using TriangleNet;
using TriangleNet.Geometry;



public class Outline
{
    public float[][] coords;
}

public class BuildingManager : MonoBehaviour
{
    // JSON files that store the building positions of the design as well as the 
    // outlines of the 2 main landmasses
    public bool usingTwoModules;
    public UnityEngine.TextAsset groundTruthCityModelJSON;
    public UnityEngine.TextAsset groundTruthCityModelJSON_halfTable;
    public UnityEngine.TextAsset markerToBuildingJSON;
    public UnityEngine.TextAsset outlineVertices_1;
    public UnityEngine.TextAsset outlineVertices_2;

    public Material invisibleMat;
    public Material buildingMat;

    GeoJSON.FeatureCollection groundTruthcityModelCollection;

    public Transform marker_SE;
    public Transform marker_NW;

    public GameObject vertexPrefab;
    public GameObject tableSurface;
    public Material landMat;
   
    private Vector2 latLon_SE;
    private Vector2 latLon_NW;

    GameObject vertexGroupParent;
    GameObject buildingParentObject;

    // Dictionary to translate between a Building ID (Like "B34A") to its Aruco Marker Value (like 5)
    public Dictionary<string, int> IDtoMarker = new Dictionary<string, int>();

    // Dictionary to translate between an Aruco Marker Value and the Building ID its attached to
    // This is a seperate Dict to the above because its very inefficient to search the above dict by Value and not Key
    public Dictionary<int, string> MarkerToID = new Dictionary<int, string>();

    // Dictionary to translate from a Marker ID to the GameObject it moves in Unity
    public Dictionary<int, GameObject> MarkerToGameObject = new Dictionary<int, GameObject>();

    // Dictionary that stores the Building Info (Like height, use-case etc. for every building ID (Used for selection Ray)
    public Dictionary<string, BuildingInfo> buildingInformations = new Dictionary<string, BuildingInfo>();

    // Stores at which point in the base model file a specific building was/is stored
    Dictionary<string, int> buildingIndices = new Dictionary<string, int>();

    void Start()
    {
        // Fixed positions on where the 2 reference markers for coordinate conversion are located
        latLon_NW = new Vector2(53.523129f, 10.000853f);
        latLon_SE = new Vector2(53.535468f, 10.021132f);
        
        // All buildings should be parented to an empty in the Hierarchy. This is this empty
        buildingParentObject = new GameObject("BuildingOutlines");

        // Which marker corresponds to which ID is stored in a markerToBuildingJSON so it can be modified externally
        // if a marker or building changes. It is read once in the beginning and not expected to change during runtime
        IDtoMarker = JsonConvert.DeserializeObject<Dictionary<string, int>>(markerToBuildingJSON.text);

        // For faster Lookups we want MarkerToID as a Dict which is the direct inverse to "IDToMarker". Just generate it
        foreach (KeyValuePair<string, int> entry in IDtoMarker)
        {
            MarkerToID[entry.Value] = entry.Key;
        }


        // Deserialises the initial city model (where the building vertices are placed, its types, names, use-cases etc.) 
        // into a collection we can worth it
        if (!usingTwoModules)
        {
            groundTruthcityModelCollection = GeoJSON.GeoJSONObject.Deserialize(groundTruthCityModelJSON.text);
            print("using full table");
        }
        else if (usingTwoModules)
        {
            groundTruthcityModelCollection = GeoJSON.GeoJSONObject.Deserialize(groundTruthCityModelJSON_halfTable.text);
            print("using half table");
        }
        

        PopulateTableFromGeoJSONFeatureCollection(groundTruthcityModelCollection);
        CreateEmptyParentForEachEntry(MarkerToGameObject);

    }

    /// <summary>
    /// Takes the supplied GeoJSON feature collection and spawns the appropriate building outlines as well as loading
    /// and placing the actual building geometries
    /// </summary>
    /// <param name="cityModel"></param>
    void PopulateTableFromGeoJSONFeatureCollection(GeoJSON.FeatureCollection cityModel)
    {
        for (int i = 0; i < cityModel.features.Count; i++)
        {
            GeoJSON.FeatureObject feature = cityModel.features[i];

       // Spawns Building Outline
       //----------------------------------------------------
            if (feature.geometry.type == "Polygon")
            {
                string bdID = feature.properties["building_id"];
                buildingIndices.Add(bdID, i);
                buildingInformations[bdID] = ReadBuildingInfoFromFeature(feature);

                //The outline of a building consists of vertices. Those will be spawned as empty GameObjects
                //and therefore need a parent. This is this parent. 
                vertexGroupParent = new GameObject(bdID);
                vertexGroupParent.transform.parent = buildingParentObject.transform;

                Vector3 avgPosition = new Vector3();
                GameObject[] allVertices = SpawnVertexGroup(feature.geometry.AllPositions(), out avgPosition);

                // Creates the Line-Renderer that connects all vertex points
                LineRenderer line = vertexGroupParent.AddComponent<LineRenderer>();
                line.material = new Material(Shader.Find("Sprites/Default"));
                line.useWorldSpace = false;
                line.widthMultiplier = 0.4f;
                line.positionCount = feature.geometry.AllPositions().Count;
                line.material.color = Color.black;

                //Iterate through all the vertices, set their coordinates as points into the line renderer
                for (int k = 0; k < allVertices.Length; k++)
                {
                    GameObject vtx = allVertices[k];
                    vtx.transform.parent = vertexGroupParent.transform;
                    line.SetPosition(k, vtx.transform.localPosition);
                }



      // Spawns Building Geometry
      //----------------------------------------------------

                string assetName = "_building_" + feature.properties["building_id"];
       
                if(Resources.Load(assetName, typeof(GameObject)))
                {
                    GameObject buildingGeometry = Instantiate(Resources.Load(assetName, typeof(GameObject))) as GameObject;
                    buildingGeometry.name = feature.properties["building_id"];

                    // This is mostly used for Debug to see where the center of the building lies
                    GameObject MarkerPositionCube = GameObject.CreatePrimitive(PrimitiveType.Cube);

                    // This is needed so the building can be hit with a selection raycast
                    buildingGeometry.AddComponent<MeshCollider>();
                    
                    if (buildingGeometry)
                    {
                        // height of the markerPositionCube so it barely shows up above the building
                        MarkerPositionCube.transform.position = avgPosition;
                        float mpHeight = buildingGeometry.GetComponent<Renderer>().bounds.extents.y * 4 + 0.2f ;
                        MarkerPositionCube.transform.localScale = new Vector3(0.1f, mpHeight, 0.1f);
                        MarkerPositionCube.transform.parent = tableSurface.transform;
                        MarkerPositionCube.GetComponent<Renderer>().material.color = Color.red;

                        buildingGeometry.transform.position = new Vector3(0, 0, 0);
                        buildingGeometry.transform.parent = MarkerPositionCube.transform;
                        buildingGeometry.transform.localPosition = new Vector3(0, 0, 0);
                        buildingGeometry.tag = "Building";
                        buildingGeometry.GetComponent<MeshCollider>().convex = true;
                        buildingGeometry.GetComponent<MeshCollider>().isTrigger = true;
                        buildingGeometry.AddComponent<BuildingCollision>();
                        
                        if (IDtoMarker[bdID] != -1)
                        {
                            buildingGeometry.GetComponent<Renderer>().material = buildingMat;
                            MarkerToGameObject[IDtoMarker[bdID]] = MarkerPositionCube;
                        }
                    }
                }
            }
        }
    }


    /// <summary>
    /// Takes each entry in the dictionary, creates an empty parent with its name and then parents that
    /// construct under the table surface again. This "extra" null between building and table surface is needed
    /// so we can offset the Building-Geometry Center from the marker, as those might not be identical (the marker is
    /// just placed wherever and needs to be calibrated once.
    /// </summary>
    /// <param name="dict"></param>
    void CreateEmptyParentForEachEntry(Dictionary<int, GameObject> dict)
    {
        foreach (KeyValuePair<int, GameObject> entry in dict)
        {
            GameObject parent = new GameObject(entry.Key.ToString());
            parent.transform.SetParent(tableSurface.transform, false);
            entry.Value.transform.SetParent(parent.transform, true);
            parent.transform.localPosition = new Vector3(0, 0, 0);
        }
    }

    BuildingInfo ReadBuildingInfoFromFeature(GeoJSON.FeatureObject feature)
    {
        BuildingInfo info = new BuildingInfo();
        info.area_planning_type = feature.properties["area_planning_type"];
        info.land_use_detailed_type = feature.properties["land_use_detailed_type"];
        info.building_height = float.Parse(feature.properties["building_height"]);
        info.building_id = feature.properties["building_id"];
        info.city_scope_id = feature.properties["city_scope_id"];
        info.floor_area = StringToFloat(feature.properties["floor_area"]);

        return info;
    }


    GameObject[] SpawnVertexGroup(List<GeoJSON.PositionObject> vertexList, out Vector3 averagePosition)
    {
        // Convert every vertex-coordinate from LatLon to unity space, instantiate a gameobject there
        Vector3 avgPosition = new Vector3(0, 0, 0);
        GameObject[] allVertices = new GameObject[vertexList.Count];
        for (int j = 0; j < vertexList.Count; j++)
        {
            var xy = vertexList[j].ToArray();

            //Switch lat-lon. A bit ugly fix for readability
            Vector2 coords = new Vector2(xy[1], xy[0]);
            allVertices[j] = PlaceVertex(coords, j.ToString(), vertexGroupParent.transform);
            Vector3 unityPos = allVertices[j].transform.position;
            avgPosition += unityPos;
        }
        avgPosition /= vertexList.Count;
        vertexGroupParent.transform.position = avgPosition;
        averagePosition = avgPosition;
        return allVertices;
    }

    /// <summary>
    /// Takes a String that shouldve been a Number from the GeoJSON and converts it back to the number. 
    /// </summary>
    /// <param name="numberString"></param>
    /// <returns></returns>
    float StringToFloat(string numberString)
    {
        numberString = numberString.Replace("\"", "");
        float cleanedNumber = float.Parse(numberString);
        return cleanedNumber;
    }
    /// <summary>
    /// Takes a pair of Lat/Lon coordinates, a name and a parent and spawns an empty and the appropriate point in unity
    /// coordinates on the table. 
    /// </summary>
    /// <param name="coords"></param>
    /// <param name="name"></param>
    /// <param name="vertexParent"></param>
    /// <returns></returns>
    /// 
    public GameObject PlaceVertex(Vector2 coords, string name, Transform vertexParent)
    {
        var unityPos = LatLon.GetUnityPosition(coords, latLon_NW, latLon_SE, marker_NW.position, marker_SE.position);
        GameObject vertex = Instantiate(vertexPrefab, new Vector3(unityPos.x, 0, unityPos.y), Quaternion.identity);
        vertex.name = name;
        vertex.tag = "Vertex";
        vertex.layer = LayerMask.NameToLayer("Ignore Raycast");

        return vertex;
    }


    void DrawLandPolygons(UnityEngine.TextAsset outlineFile, string parentName)
    {
        Outline deserializedOutline = JsonConvert.DeserializeObject<Outline>(outlineFile.text);

        GameObject shape = new GameObject(parentName);
        LineRenderer contourLine = shape.AddComponent<LineRenderer>();
        contourLine.material = new Material(Shader.Find("Sprites/Default"));
        contourLine.useWorldSpace = false;
        contourLine.widthMultiplier =1.4f;
        contourLine.positionCount = deserializedOutline.coords.Length;
        contourLine.material.color = Color.black;


        // Vertices are stored in Lat/Lon in GeoJSON so have to be first deserialized
        // then converted to unity space
        // then added as point to the lineRenderer that defines the outline
        List<Vector2> points = new List<Vector2>();

        for (int i = 0; i < deserializedOutline.coords.Length; i++)
        {
            var x = deserializedOutline.coords[i][0];
            var y = deserializedOutline.coords[i][1];

            Vector2 vertexLatLon = new Vector2(y, x);

            GameObject vertexObject = PlaceVertex(vertexLatLon, i.ToString(), shape.transform);
            vertexObject.transform.parent = shape.transform;
            Vector3 vertexUnity = vertexObject.transform.position;
            points.Add(new Vector2(vertexUnity.x, vertexUnity.z));

            contourLine.SetPosition(i, vertexUnity);
        }
   
        // From the points a Surface is triangulated and displayed
        // The function returns its parent object which is then parented to the shape
       GenerateSurface(points, -0.02f, parentName, landMat).transform.parent = shape.transform;
    }

    /// <summary>
    /// Moves the building outlines to the current positions of their respective buildings (which couldve been moved by the user)
    /// and then returns the full-finished GeoJSON of all the buildings as it came in from the GroundTruth, just with the moved buildings
    /// </summary>
    /// <returns></returns>
    public JSONObject UpdateAndReturnOutlinePositions()
    {
        // new collection to fill with data
        GeoJSON.FeatureCollection exportCollection = new GeoJSON.FeatureCollection();


        //check every child of the buildingsParent
        foreach (Transform building in buildingParentObject.transform)
        {
            string buildingID = building.gameObject.name;

            Transform buildingModel = null; 

            if (IDtoMarker.ContainsKey(buildingID))
            {
                buildingModel = tableSurface.transform.Find(IDtoMarker[buildingID].ToString());

                if (buildingModel)
                {
                    building.position = transform.TransformPoint(buildingModel.GetChild(0).GetChild(0).transform.position);
                    //building.position = new Vector3(building.position.x - 25.5f, 0, building.position.z - 21.4f);
                    building.position = new Vector3(building.position.x, 0, building.position.z);

                    building.rotation =Quaternion.Euler(new Vector3(0,  buildingModel.GetChild(0).GetChild(0).eulerAngles.y, 0));              
                }           
            }


            if (buildingIndices.ContainsKey(buildingID))
            {
                int idx = buildingIndices[buildingID];
                JSONObject storedFeature = groundTruthcityModelCollection.features[idx].Serialize();


                // Method:
                // GeoJSONs are hard and annoying. there is polygon winding order, holes in geometry, stuff like that. To circumvent all of this, 
                // the following approach is used. At the beginning, all vertices are instantiated as empties. They are then transformed and moved
                // as necessary. Now we want to write the moved buildings back into a GeoJSON. So we just take the instantiated vertices which
                // should still be in the correct order, convert them back to Lat/Lon and there we have it. 
                foreach (Transform vertex in building.transform)
                {
                    // exclude actual building geometry
                    if(vertex.tag == "Vertex")
                    {
                        Vector2 latLonPos = LatLon.GetLatLonPosition(vertex.position, latLon_NW, latLon_SE, marker_NW.position, marker_SE.position);
                        latLonPos = new Vector2(latLonPos.y, latLonPos.x);

                        JSONObject pos = new JSONObject(JSONObject.Type.ARRAY);
                        pos.Add(latLonPos.x);
                        pos.Add(latLonPos.y);

                        storedFeature["geometry"]["coordinates"][0][int.Parse(vertex.gameObject.name)] = pos;
                    }
                }

                GeoJSON.FeatureObject updatedFeature = new GeoJSON.FeatureObject(storedFeature);
                exportCollection.features.Add(updatedFeature);
            }
            else
            {
                print("Building not found!");
            }
        }
        JSONObject serializedCollection = exportCollection.Serialize();


        // Some stuff isnt serialized correctly so this is a not-so-nice hack to fix that. works, though. 
        for (int i = 0; i < serializedCollection["features"].Count; i++)
        {
            serializedCollection = CleanPropertyField("building_height", serializedCollection, i);
            serializedCollection = CleanPropertyField("floor_area", serializedCollection, i);
        }

        return serializedCollection;
    }

    JSONObject CleanPropertyField(string property, JSONObject serializedCollection, int featureIndex)
    {
        string numberString = serializedCollection["features"][featureIndex]["properties"][property].ToString();
        numberString = numberString.Replace("\"", "");
        float cleanedNumber = float.Parse(numberString);

        serializedCollection["features"][featureIndex]["properties"].RemoveField(property);
        serializedCollection["features"][featureIndex]["properties"].AddField(property, cleanedNumber);

        return serializedCollection;
    }

    public Vector2 ConvertToUnitySpace(Vector2 coords)
    {
        Vector2 unityPos = LatLon.GetUnityPosition(coords, latLon_NW, latLon_SE, marker_NW.position, marker_SE.position);
        return unityPos;
    }

    public GameObject GenerateSurface(List<Vector2> vertexList, float worldHeight, string parentName, Material mat)
    {
        Polygon poly = new Polygon();
        poly.Add(vertexList);

        var triangleNetMesh = (TriangleNetMesh)poly.Triangulate();
        
        GameObject go = new GameObject(parentName);
        go.transform.Rotate(new Vector3(90, 0, 0));
        go.transform.localPosition = new Vector3(0, worldHeight, 0);

        var mf = go.AddComponent<MeshFilter>();
        
        var mesh = triangleNetMesh.GenerateUnityMesh();


        Vector2[] uvs = new Vector2[4];
        uvs[2] = new Vector2(1, 1);
        uvs[3] = new Vector2(0, 1);
        uvs[0] = new Vector2(0, 0);
        uvs[1] = new Vector2(1, 0);

        // mesh.uv = GenerateUV(mesh.vertices, 1, 1);
        mesh.uv = uvs;
        mf.mesh = mesh;
        var mr = go.AddComponent<MeshRenderer>();
        mr.material = mat;
        

        return go;
    }

    private Vector2[] GenerateUV(Vector3[] vertices, int dimX, int dimY)
    {
        Vector2[] uvs = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++)
        {
            uvs[i] = new Vector2(vertices[i].x / dimX, vertices[i].y / dimY);
        }

        return uvs;
    }

}	

