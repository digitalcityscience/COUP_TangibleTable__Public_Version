using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to activate a result again when there was one before
/// </summary>
public class ShowSimulation : MonoBehaviour
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
    /// Function that choose the right Simulation that is not active and set the activitie on true
    /// </summary>
    /// <param name="sim"></param>
    public void ShowOldSimulations(string sim)
    {
        string simName = sim;
        if (simName == "noise" && calculationModules.NoiseResultIsActive == false && calculationModules.NoiseResult)
        {
            legendResults.AktivateLegendResults("noise");

            calculationModules.NoiseResult.SetActive(true);
            calculationModules.NoiseResultIsActive = true;
        }
        else if (simName == "wind" && calculationModules.WindResultIsActive == false && calculationModules.WindResult)
        {
            legendResults.AktivateLegendResults("wind");

            calculationModules.WindResult.SetActive(true);
            calculationModules.WindResultIsActive = true;
            directionalLight.shadows = LightShadows.Soft;
        }
        else if (simName == "abm" && calculationModules.AbmResultIsActive == false && calculationModules.AbmResult)
        {
            legendResults.AktivateLegendResults("abm");

            calculationModules.AbmResult.SetActive(true);
            calculationModules.AbmResultIsActive = true;
        }
        else if (simName == "sun")
        {
            calculationModules.SunResult.SetActive(true);
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
