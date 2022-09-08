using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Networking;
using TriangleNet;
using System;
using Newtonsoft.Json;
using System.IO;


public class Auth
{
    public bool restricted;
    public string user_id;
}


public class CityPyO_Interface : MonoBehaviour
{

    [SerializeField]
    TextAsset ConfigData;

    [SerializeField]
    BuildingManager buildingManager;

    [SerializeField]
    UnityEngine.TextAsset ExampleWindResult;

    [SerializeField]
    UnityEngine.TextAsset ExampleNoiseResult;

    [SerializeField]
    WindSimulation windSimulation;

    [SerializeField]
    NoiseSimulation noiseSimulation;

    [SerializeField]
    AbmSimulation abmSimulation;

    [SerializeField]
    GameObject waitingSwirl;

    JSONObject loginBody;

    private Auth auth;

    public Material dataMat;

    //This is the path to the .txt data with only the login datas for the simulations
    
    // TODO adapt to your local path
    private string path = "C:/***/***/Documents/Grasbrook_Table_TrackingSoftware/login.txt";
    
    //Carefull!! if you change the order or anything in the login.txt you have to change it in the code too!!
    public String [] LoginData;

    private string POSTAddUserURL = null;
    private string POSTUpdateURL = null;
    private string POSTUpdateNoiseURL = null; //this one is just for the presentation, it need to changed in the End

    private string USR = null;
    private string PWD = null;


    public Auth Auth { get => auth; set => auth = value; }

    void Start()
    {
        POSTAddUserURL = ConfigData + "citypyo/login";
        POSTUpdateURL = ConfigData + "citypyo/addLayerData/buildings";
        POSTUpdateNoiseURL = ConfigData + "citypyo/addLayerData/upperfloor";

        //[0]=username [1]=password
        LoginData = File.ReadAllLines(path); 

        LoginToCityPyO();
    }

    private void LoginToCityPyO()
    {

        if(USR != null)
        {
            if(PWD != null)
            {
                loginBody = new JSONObject();
                loginBody.AddField("username", USR);
                loginBody.AddField("password", PWD);
                string data = loginBody.Print();
                StartCoroutine(CallLogin(POSTAddUserURL, data));
            }
            else
            {
                // 1 = password in the .txt - file
                PWD = LoginData[1];
                LoginToCityPyO();
            }
        }
        else
        {
            // 0 = username in the .txt - file
            USR = LoginData[0];
            LoginToCityPyO();
        }
        
    }

    public IEnumerator CallLogin(string url, string loginDataJsonString)
    {
        var request = new UnityWebRequest(url, "POST");
        byte[] bodyRaw = Encoding.UTF8.GetBytes(loginDataJsonString);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
        }
        else
        {
            Debug.Log("Status Code: " + request.responseCode);

            string authAnswer = request.downloadHandler.text;

            Auth = JsonUtility.FromJson<Auth>(authAnswer);
           
        }
    }


    public void SendBuildingPositions()
    {
        StartCoroutine(PushBuildingPositions(buildingManager.UpdateAndReturnOutlinePositions()));
    }

    public IEnumerator PushBuildingPositions(JSONObject buildings)
    {
        var request =  new UnityWebRequest(POSTUpdateURL, "POST");
        
        JSONObject updateBody = new JSONObject();
        updateBody.AddField("userid", Auth.user_id);
        updateBody.AddField("data", buildings);

        string data = updateBody.Print();
        print(data);
            
        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
        }
        else
        {
            Debug.Log("Building Positions updated");
            Debug.Log("Status Code" + request.responseCode);

            string authAnswer = request.downloadHandler.text;

        }
    }

    //This is just for the presentation it´s needed to be deletet after, bzw. we need to change Noise or Wind so the building information comes from the same place
    public IEnumerator PushBuildingNoisePositions(JSONObject buildings)
    {
        var request = new UnityWebRequest(POSTUpdateNoiseURL, "POST");

        JSONObject updateBody = new JSONObject();
        updateBody.AddField("userid", Auth.user_id);
        updateBody.AddField("data", buildings);

        string data = updateBody.Print();
        print(data);

        byte[] bodyRaw = Encoding.UTF8.GetBytes(data);
        request.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
        request.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();
        request.SetRequestHeader("Content-Type", "application/json");
        yield return request.SendWebRequest();

        if (request.error != null)
        {
            Debug.Log("Errno: " + request.error);
        }
        else
        {
            Debug.Log("Building Positions updated");
            Debug.Log("Status Code" + request.responseCode);

            string authAnswer = request.downloadHandler.text;

        }
    }
}  
