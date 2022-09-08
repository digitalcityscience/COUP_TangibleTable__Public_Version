using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegendResults : MonoBehaviour
{
    [SerializeField]
    GameObject resultLegend;
    [SerializeField]
    GameObject windResult;
    [SerializeField]
    GameObject noiseResult;
    [SerializeField]
    GameObject abmResult;

    public void AktivateLegendResults(string simName)
    {
        resultLegend.SetActive(true);
        if(simName == "noise")
        {
            noiseResult.SetActive(true);
            windResult.SetActive(false);
            abmResult.SetActive(false);
        }
        else if(simName == "wind")
        {
            windResult.SetActive(true);
            noiseResult.SetActive(false);
            abmResult.SetActive(false);
        }
        else if(simName == "abm")
        {
            abmResult.SetActive(true);
            noiseResult.SetActive(false);
            windResult.SetActive(false);
        }
        else if (simName == "hide")
        {
            resultLegend.SetActive(false);
            noiseResult.SetActive(false);
            windResult.SetActive(false);
            abmResult.SetActive(false);
        }
    }

}
