using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class to activate a result again when there was one before
/// </summary>
public class ToggleResult : MonoBehaviour
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
    public void ToggleResultActivitation(string sim)
    {        
        if(sim == "noise")
        {
            if(calculationModules.NoiseResultIsActive == false)
            {
                legendResults.ResultLegend.SetActive(true);
                legendResults.AktivateLegendResults("noise");

                calculationModules.NoiseResult.SetActive(true);
                calculationModules.NoiseResultIsActive = true;
            }
            else if(calculationModules.NoiseResultIsActive == true)
            {
                legendResults.ResultLegend.SetActive(false);
                calculationModules.NoiseResult.SetActive(false);
                calculationModules.NoiseResultIsActive = false;
            }
        }
        else if(sim == "wind")
        {
            if(calculationModules.WindResultIsActive == false)
            {
                legendResults.ResultLegend.SetActive(true);
                legendResults.AktivateLegendResults("wind");
                calculationModules.WindResult.SetActive(true);
                calculationModules.WindResultIsActive = true;
                directionalLight.shadows = LightShadows.Soft;
            }
            else if(calculationModules.WindResultIsActive == true)
            {
                legendResults.ResultLegend.SetActive(false);
                calculationModules.WindResult.SetActive(false);
                calculationModules.WindResultIsActive = false;
                directionalLight.shadows = LightShadows.Soft;
            }
        }
        else if(sim == "abm")
        {
            if(calculationModules.AbmResultIsActive == false)
            {
                legendResults.ResultLegend.SetActive(true);
                legendResults.AktivateLegendResults("abm");
                calculationModules.AbmResult.SetActive(true);
                calculationModules.AbmResultIsActive = true;
            }
            else if(calculationModules.AbmResultIsActive == true)
            {
                legendResults.ResultLegend.SetActive(false);
                calculationModules.AbmResult.SetActive(false);
                calculationModules.AbmResultIsActive = false;
            }
        }
        else if (sim == "sun")
        {
            calculationModules.SunResult.SetActive(true);
            calculationModules.SunResult.SetActive(false);
        }
        else if (sim == "none")
        {
                Debug.Log("Simulation Number is not choosed");
        }
        else
        {
            Debug.Log("you have choosed the wrong combination");
        }
        
    }
}
