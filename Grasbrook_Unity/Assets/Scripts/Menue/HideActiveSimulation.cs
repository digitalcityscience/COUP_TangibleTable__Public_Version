using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Menu Class to Hide the active Simulation Result
/// </summary>
public class HideActiveSimulation : MonoBehaviour
{
    CalculationModules_Interface calculationModules;
    Light directionalLight;

    [SerializeField]
    LegendResults legendResults;

    private void Start()
    {
        calculationModules = GameObject.Find("NetworkingManager").GetComponent<CalculationModules_Interface>();
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
    }

    /// <summary>
    /// Function that choose the right Simulation that is active and set the activitie on false
    /// </summary>
    /// <param name="sim"></param>
    public void HideOldSimulation(string sim)
    {
        string simName = sim;

        legendResults.AktivateLegendResults("hide");

        if (simName == "noise" && calculationModules.NoiseResultIsActive == true && calculationModules.NoiseResult)
        {
            calculationModules.NoiseResult.SetActive(false);
            calculationModules.NoiseResultIsActive = false;
        }
        else if (simName == "wind" && calculationModules.WindResultIsActive == true && calculationModules.WindResult)
        {
            calculationModules.WindResult.SetActive(false);
            calculationModules.WindResultIsActive = false;
            directionalLight.shadows = LightShadows.Soft;
        }
        else if (simName == "abm" && calculationModules.AbmResultIsActive == true && calculationModules.AbmResult)
        {
            calculationModules.AbmResult.SetActive(false);
            calculationModules.AbmResultIsActive = false;
        }
        else if (simName == "sun")
        {
            calculationModules.SunResult.SetActive(false);
        }
        else if (simName == "none")
        {
            Debug.Log("Simulation Number is not choosed");
        }
        else
        {
            Debug.Log("you have choosed the wrong combination");
        }
    }

}
