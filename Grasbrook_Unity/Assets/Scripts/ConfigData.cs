using UnityEngine;
using Newtonsoft.Json;
using System.Collections.Generic;

public class ConfigDictionary
{
    public string cityPyoUserName;
    public string cityPyoUserPw;
    public string calcApiUserName;
    public string calcApiUserPw;
    public string calcApiUrl;
}


[System.Serializable]
public class ConfigData : MonoBehaviour
{
    private UnityEngine.TextAsset configJSON;

    public Dictionary<string, ConfigDictionary> configDict = new Dictionary<string, ConfigDictionary>();


    void Start() {
        ReadConfigData();
    }

    public void ReadConfigData ()
    {

        configDict = JsonConvert.DeserializeObject<Dictionary<string, ConfigDictionary>>(configJSON.text);

        //Andre was willst du hier mir machen? Das Dictionary ist dafür da zu gucken wo was mit welchem "key" normalerweise steht
        //z.B. bei den Gebäuden fragen wir ja nach nem Aruco Nummer. Diese haben wir mit nem Gebäude verbunden. Deswegen bekommen wir
        // die informationen über das Gebäude heraus. Was ist das Ziel dieses Dictionarys?
        //string cityPyoUserName = configDict.cityPyoUserName;
        //string cityPyoUserPw = configDict.cityPyoUserPw;
        //string calcApiUserName = configDict.calcApiUserName;
        //string calcApiUserPw = configDict.calcApiUserPw;
        //string calcApiUrl = configDict.calcApiUrl;

        Debug.Log(configDict["cityPyoUserName"]);
    }


}
 