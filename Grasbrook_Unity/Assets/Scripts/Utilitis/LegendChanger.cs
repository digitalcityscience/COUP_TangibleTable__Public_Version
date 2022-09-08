using UnityEngine;
using TMPro;

/// <summary>
/// Class to change the Main Legend for the difrent simulations
/// with diffrent values
/// </summary>
public class LegendChanger : MonoBehaviour
{

    [SerializeField]
    TextMeshProUGUI simulationName;
    [SerializeField]
    TextMeshProUGUI simDataOne;
    [SerializeField]
    TextMeshProUGUI simDataTwo;
    [SerializeField]
    TextMeshProUGUI simDataThree;
    [SerializeField]
    TextMeshProUGUI simDataFour;
    [SerializeField]
    TextMeshProUGUI simDataFive;

    private string[] season = {"Spring", "Summer", "Autumn", "Winter"};

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sim"></param>
    /// <param name="value"></param>
    /// <param name="valueKey"></param>
    public void ChangeLegendInforamtion(string sim, int value, int valueKey)
    {
        //1 = Noise
        if (sim == "noise")
        {
            simulationName.text = "Noise";
            if (valueKey == 0)
            {
                simDataOne.text = "Traffic Speed:" + " " + value + "km/h";
            }
            else if (valueKey == 1)
            {
                simDataTwo.text = "Tarffic Volume:" + " " + value + "%";
            }
            else if (valueKey == 3)
            {
                simDataOne.text = "Traffic Speed:" + " " + GlobalVariable.GlobalNoiseCarSpeed + "km/h";
                simDataTwo.text = "Tarffic Volume:" + " " + GlobalVariable.GlobalNoiseCarVolume + "%";
            }
            simDataThree.text = " ";
            simDataFour.text = " ";
            simDataFive.text = " ";

        }
        //2 = Wind
        else if (sim == "wind")
        {
            simulationName.text = "Wind";
            if (valueKey == 0)
            {
                simDataOne.text = "Wind Direction:" + " " + value +"°";
            }
            else if (valueKey == 1)
            {
                simDataTwo.text = "Wind Speed:" + " " + value + "km/h";
            }
            else if (valueKey == 3)
            {
                simDataOne.text = "Wind Direction" + " " + GlobalVariable.GlobalWindDirection + "°";
                simDataTwo.text = "Wind Speed:" + " " + GlobalVariable.GlobalWindSpeed + "km/h";
            }
            simDataThree.text = " ";
            simDataFour.text = " ";
            simDataFive.text = " ";

        }
        //3 = ABM
        else if (sim == "abm")
        {
            simulationName.text = "Pedestrians";
            simDataOne.text = "Bridge to HafenCity:" + " " + GlobalVariable.GlobalABMBridgeToHC;
            simDataTwo.text = "Underpass to Veddel N:" + " " + GlobalVariable.GlobalABMUnderpassVN;
            simDataThree.text = "Main Street Orientation: " + " " + GlobalVariable.GlobalABMStreetOrientation;
            if (GlobalVariable.GlobalABMBlocks == "open")
            {
                simDataFour.text = "City Block Structure:" + " " + GlobalVariable.GlobalABMBlocks;
            }
            else
            {
                simDataFour.text = "City Block Structure: private";
            }
            simDataFive.text = "Amenity Distribution: " + " " + GlobalVariable.GlobalABMAmenities;
        }
        //4 = SUN / Thermal Comfort
        else if (sim == "sun")
        {
            simulationName.text = "Thermal Comfort";
            simDataOne.text = "Season: " + " " + season[valueKey+1]; 
            simDataTwo.text = "Information Two: " + " " + "Something";
            simDataThree.text = "Information Three: " + " " + "Something";
            simDataFour.text = "Information Four: " + " " + "Something";
            simDataFive.text = " ";
        }
    }
}
