using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TriangleNet;
using System;
using Newtonsoft.Json;
using System.IO;



// brauchen wir das hier überhaupt noch?
public class CalculationApiAuth
{
    // enter your credentials
    public string username = "USERNAME"; 
    public string password = "PASSWORD";
}

[Serializable]
public class SingleTaskResult
{
    public string taskId;
}

public class GroupTaskResult
{
    public string result;
}


public class CalculationModules_Interface : MonoBehaviour
{
    [Tooltip("BuildingManager gameObject from the Herachie")]
    [SerializeField]
    BuildingManager buildingManager;

    [Tooltip("NetworkManager gameObject from the Herachie")]
    [SerializeField]
    CityPyO_Interface CityPyo;

    [Tooltip("under Assets/Files")]
    [SerializeField]
    TextAsset ConfigData;

    [Tooltip("under Assets/Files")]
    [SerializeField]
    TextAsset ExampleWindResult;

    [Tooltip("under Assets/Files")]
    [SerializeField]
    TextAsset ExampleNoiseResult;
    
    [SerializeField]
    TextAsset ExampleAbmResult;

    [Tooltip("Simulation gameObject from the Herachie")]
    [SerializeField]
    WindSimulation windSimulation;

    [Tooltip("Simulation gameObject from the Herachie")]
    [SerializeField]
    NoiseSimulation noiseSimulation;

    [Tooltip("Simulation gameObject from the Herachie")]
    [SerializeField]
    AbmSimulation abmSimulation;

    [Tooltip("Loadingbar gameObject from the SurfaceDial")]
    [SerializeField]
    Loadingbar loadingbar;

    [SerializeField]
    GameObject waitingSwirl;

    bool currentlyLoading = false;

    string windGroupTaskUUID;
    string noiseTaskUUID;

    GameObject windResult = null;
    GameObject noiseResult = null;
    GameObject abmResult = null;
    GameObject sunResult = null;

    bool noiseResultIsActive = false;
    bool windResultIsActive = false;
    bool abmResultIsActive = false;
 
    Auth auth;

    public Material dataMat;

    //new connection principal 
    private string windCalculationRoute = null;
    private string windSingleTaskResultRoute = null;
    private string windGroupTaskResultRoute = null;

    private string noiseCalculationRoute = null;
    private string noiseResultRoute = null;

    private string abmRequestRoute = null;

    public bool CurrentlyLoading { get => currentlyLoading; set => currentlyLoading = value; }
    public GameObject NoiseResult { get => noiseResult; set => noiseResult = value; }
    public GameObject WindResult { get => windResult; set => windResult = value; }
    public GameObject AbmResult { get => abmResult; set => abmResult = value; }
    public GameObject SunResult { get => sunResult; set => sunResult = value; }
    public bool NoiseResultIsActive { get => noiseResultIsActive; set => noiseResultIsActive = value; }
    public bool WindResultIsActive { get => windResultIsActive; set => windResultIsActive = value; }
    public bool AbmResultIsActive { get => abmResultIsActive; set => abmResultIsActive = value; }

    /// <summary>
    /// declaration of the simulation Pathes
    /// through the use of a seperated Config Data we need to declare the way here and not at the variable initialization
    /// </summary>
    private void Start()
    {
        windCalculationRoute = ConfigData + "wind/windtask";
        windSingleTaskResultRoute = ConfigData + "wind/tasks/";
        windGroupTaskResultRoute = ConfigData + "wind/grouptasks/";

        noiseCalculationRoute = ConfigData + "noise/task";
        noiseResultRoute = ConfigData + "noise/tasks/";

        abmRequestRoute = ConfigData + "abm/abm_result_as_pngs";
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.W))
        //{
        //    // update building positions on cityPyo before triggering calculation
        //    StartCoroutine(CityPyo.PushBuildingPositions(buildingManager.UpdateAndReturnOutlinePositions()));
        //    TryWind();
        //}

        //if (Input.GetKeyDown(KeyCode.N))
        //{
        //    // update building positions on cityPyo before triggering calculation
        //    StartCoroutine(CityPyo.PushBuildingPositions(buildingManager.UpdateAndReturnOutlinePositions()));
        //    TryNoise();
        //}

        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    // update building positions on cityPyo before triggering calculation
        //    StartCoroutine(CityPyo.PushBuildingPositions(buildingManager.UpdateAndReturnOutlinePositions()));
        //    TryABM();
        //}

        //if (Input.GetKeyDown(KeyCode.S))
        //{
        //    StopCoroutine(CalculateWind());
        //    StopCoroutine(CalculateWind());
        //    StopCoroutine(PullWindResults());

        //    StopCoroutine(CalculateNoise());
        //    StopCoroutine(CalculateNoise());
        //    StopCoroutine(PullNoiseResults());

        //    waitingSwirl.SetActive(false);
        //    CurrentlyLoading = false;

        //    Debug.Log("S wurde gedrückt");

        //    StopAllCoroutines();
        //}

        if (currentlyLoading)
        {
            waitingSwirl.transform.Rotate(Vector3.up, 5);
        }

    }

    public void ActivateSimulation(string simName)
    {
        if (simName == "noise")
        {
            // update building positions on cityPyo before triggering calculation
            StartCoroutine(CityPyo.PushBuildingNoisePositions(buildingManager.UpdateAndReturnOutlinePositions()));
            TryNoise();
        }
        else if (simName == "wind")
        {
            // update building positions on cityPyo before triggering calculation
            StartCoroutine(CityPyo.PushBuildingPositions(buildingManager.UpdateAndReturnOutlinePositions()));
            TryWind();
        }
        else if (simName == "abm")
        {
            // update building positions on cityPyo before triggering calculation
            StartCoroutine(CityPyo.PushBuildingPositions(buildingManager.UpdateAndReturnOutlinePositions()));
            TryABM();
        }
        else if (simName == "none")
        {
            Debug.Log("Please choose Simulation before");
        }
    }

    #region Wind Calculation
    public void TryWind()
    {
        GameObject windParent = GameObject.Find("windResults");
        if (windParent)
        {
            foreach (Transform child in windParent.transform)
            {
                child.gameObject.GetComponent<Renderer>().material.color = new Color(0.3f, 0.3f, 0.3f, 1f);
            }
        }

        // trigger calculation of wind scenario
        StartCoroutine(CalculateWind());
        currentlyLoading = true;
        waitingSwirl.SetActive(true);

        print("Wind-Direction: " + "" + GlobalVariable.GlobalWindDirection);
        print("Wind-Speed: " + "" + GlobalVariable.GlobalWindSpeed);
    }

    // post current windScenario settings to calculation api and display incoming results
    public IEnumerator CalculateWind()
    {
        JSONObject arr = new JSONObject(JSONObject.Type.ARRAY);
        JSONObject scenarioBody = new JSONObject();
        scenarioBody.AddField("wind_speed", GlobalVariable.GlobalWindSpeed);
        scenarioBody.AddField("wind_direction", GlobalVariable.GlobalWindDirection);
        //scenarioBody.AddField("custom_roi", arr);
  
        UnityWebRequest request = CreatePostRequestObject(windCalculationRoute, scenarioBody);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
            currentlyLoading = false;
            yield break;
        }
        else
        {
            Debug.Log("Receive Single Task UUID");
            SingleTaskResult response = JsonConvert.DeserializeObject<SingleTaskResult>(request.downloadHandler.text);
            // get UUID of calculation group task that will return the wind results.
            StartCoroutine(SaveWindGroupTaskUUID(response.taskId));

        }       
    }

    // post current windScenario settings to calculation api return the resultUUID returned by API
    public IEnumerator SaveWindGroupTaskUUID(string taskID)
    {
        Debug.Log("SingleTaskID: " + taskID);
        var request = CreateGetRequest(windSingleTaskResultRoute + taskID);

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
        }
        else
        {
            Debug.Log("Status Code" + request.responseCode);
        
            GroupTaskResult response = JsonConvert.DeserializeObject<GroupTaskResult>(request.downloadHandler.text);
            
            windGroupTaskUUID = response.result;
            // start polling for results using windGroupTaskUUID
            InvokeRepeating("GetWindResults", 0.1f, 2f);
        }
    }

    void GetWindResults()
    {
        print("GetWindResults");
        StartCoroutine(PullWindResults());
        loadingbar.ToggleActive(true);
    }

    IEnumerator PullWindResults()
    {
        var request = CreateGetRequest(windGroupTaskResultRoute +  windGroupTaskUUID + "?result_format=png");

        Debug.Log("WindGroup Request is:" + " " + windGroupTaskResultRoute + windGroupTaskUUID + "?result_format=png");
        yield return request.SendWebRequest();
    
        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
        }
        else
        {
            WindData windData = JsonConvert.DeserializeObject<WindData>(request.downloadHandler.text);
            Debug.Log("Request Text: " + request.downloadHandler.text);
            print("TasksCompleted Count:" + windData.tasksCompleted);
            loadingbar.ChangeLoadingBar(windData.tasksCompleted, windData.tasksTotal);


            if(windData.grouptaskProcessed == true)
            {
                windSimulation.DisplayWindData(windData);
                CancelInvoke();
                loadingbar.ToggleActive(false);
                Debug.Log("WindTask is finished, now the result is loading");
            }

        }
    }
#endregion

    #region Noise Calculation
    /** RUN NOISE CALCULATION **/
    public void TryNoise()
    {
        StartCoroutine(CityPyo.PushBuildingNoisePositions(buildingManager.UpdateAndReturnOutlinePositions()));
        GameObject noiseParent = GameObject.Find("noiseResults");
        if (noiseParent)
        {
            foreach (Transform child in noiseParent.transform)
            {
                child.gameObject.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f, 0.8f);
            }
        }

        // trigger calculation of noise scenario
        StartCoroutine(CalculateNoise());
        currentlyLoading = true;
        waitingSwirl.SetActive(true);
    }

    // post current windScenario settings to calculation api return the resultUUID returned by API
    public IEnumerator CalculateNoise()
    {
        JSONObject scenarioBody = new JSONObject();
        scenarioBody.AddField("max_speed", GlobalVariable.GlobalNoiseCarSpeed);
        scenarioBody.AddField("traffic_quota", GlobalVariable.GlobalNoiseCarVolume);
        
        UnityWebRequest request = CreatePostRequestObject(noiseCalculationRoute, scenarioBody);

        yield return request.SendWebRequest();

        if (request.error != null) 
        {
            Debug.Log("Errno: " + request.error);
            yield break;
        }
        else
        {
            Debug.Log("Receive Single Task UUID");
            SingleTaskResult response = JsonConvert.DeserializeObject<SingleTaskResult>(request.downloadHandler.text);
            noiseTaskUUID = response.taskId;

            // start polling for results using noiseTaskUUID
            InvokeRepeating("GetNoiseResults", 0.1f, 2f);
        }       
    }

    void GetNoiseResults()
    {
        StartCoroutine(PullNoiseResults());
    }

    IEnumerator PullNoiseResults()
    {
        print("Pulling Noise Results!");
        var request = CreateGetRequest(noiseResultRoute + noiseTaskUUID);

        Debug.Log("RequestURL: " + request.url);
        Debug.Log("TasktUUID: " + noiseTaskUUID);
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
        }
        else
        {
            Debug.Log("Status Code" + request.responseCode);

            print("Noise Results received!");

            NoiseData noiseResult = JsonConvert.DeserializeObject<NoiseData>(request.downloadHandler.text);

            noiseSimulation.DisplayNoiseResult(noiseResult);
            CancelInvoke();
        }
    }
    #endregion

    #region ABM Calculation
    /** RUN ABM CALCULATION **/
    public void TryABM()
    {
        GameObject abmParent = GameObject.Find("abmResults");
        if (abmParent)
        {
            foreach (Transform child in abmParent.transform)
            {
                child.gameObject.GetComponent<Renderer>().material.color = new Color(0.4f, 0.4f, 0.4f, 0.8f);
            }
        }

        abmSimulation.abmScenario.blocks = GlobalVariable.GlobalABMBlocks;
        abmSimulation.abmScenario.bridge_hafencity = GlobalVariable.GlobalABMBridgeToHC;
        abmSimulation.abmScenario.main_street_orientation = GlobalVariable.GlobalABMStreetOrientation;
        abmSimulation.abmScenario.roof_amenities = GlobalVariable.GlobalABMAmenities;
        abmSimulation.abmScenario.underpass_veddel_north = GlobalVariable.GlobalABMUnderpassVN;

        // trigger calculation of the ibm scenario
        StartCoroutine(CalculateABM());
        currentlyLoading = true;
        waitingSwirl.SetActive(true);
    }

    // post current abmScenario settings to calculation api return the resultUUID returned by API
    public IEnumerator CalculateABM()
    {
        JSONObject scenarioBody = new JSONObject();

        scenarioBody.AddField("bridge_hafencity", GlobalVariable.GlobalABMBridgeToHC);
        scenarioBody.AddField("underpass_veddel_north", GlobalVariable.GlobalABMUnderpassVN);
        scenarioBody.AddField("main_street_orientation", GlobalVariable.GlobalABMStreetOrientation);
        scenarioBody.AddField("blocks", GlobalVariable.GlobalABMBlocks);
        scenarioBody.AddField("roof_amenities", GlobalVariable.GlobalABMAmenities);

        Debug.Log(scenarioBody.ToString());

        UnityWebRequest request = CreatePostRequestObjectABM(abmRequestRoute, scenarioBody);
        print("RequestRoute: " + abmRequestRoute + " " + "Request: " + request.ToString());

        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
            Debug.Log("ErrorBody: " + request.downloadHandler.text);
            //currentlyLoading = false;
            yield break;
        }
        else
        {
            Debug.Log("Receive Request");

            AbmData abmResult = JsonConvert.DeserializeObject<AbmData>(request.downloadHandler.text);

            currentlyLoading = true;
            waitingSwirl.SetActive(true);

            if (abmResult.results[14] != null)
            {
                abmSimulation.DisplayAbmResult(abmResult);
                currentlyLoading = false;
                waitingSwirl.SetActive(false);
            }
        }
    }
    #endregion

    public void DeactivateWaitingSwirl()
    {
        waitingSwirl.SetActive(false);
    }

    string GetAuthString()
    {

        string auth = "city-scope" + ":" + "TcsB3Db8g4";
        auth = System.Convert.ToBase64String(System.Text.Encoding.GetEncoding("ISO-8859-1").GetBytes(auth));
        auth = "Basic " + auth;
        return auth;
    }

    public UnityWebRequest CreatePostRequestObject(string url, JSONObject dataObject)
    {
        dataObject.AddField("city_pyo_user", CityPyo.Auth.user_id);
        dataObject.AddField("result_format", "png");  // TODO use this when collecting??
        string data = dataObject.Print();
        print(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);

        var request = new UnityWebRequest(url, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("AUTHORIZATION", GetAuthString());

        return request;
    }

    public UnityWebRequest CreatePostRequestObjectABM(string url, JSONObject dataObject)
    {
        string data = dataObject.Print();
        print(data);
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);

        var request = new UnityWebRequest(url+ "?userid=" + CityPyo.Auth.user_id, "POST");
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("AUTHORIZATION", GetAuthString());

        return request;
    }

    public UnityWebRequest CreateGetRequest(string url)
    {
        var request = new UnityWebRequest(url, "GET");

        JSONObject getBody = new JSONObject();
        string data = getBody.Print();
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        request.SetRequestHeader("AUTHORIZATION", GetAuthString());
        
        return request;
    }
}
