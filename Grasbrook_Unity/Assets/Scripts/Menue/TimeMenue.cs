using UnityEngine;
using TMPro;
using Blitzy.UnityRadialController;

/// <summary>
/// Class to change the time for the Abm-Simulation results and the ThermalComfort-Simulation
/// The time changer is needed to switch between the diffrent pictures that shows the result 
/// for an hour of the specific day
/// </summary>
public class TimeMenue : MonoBehaviour
{

    [SerializeField]
    GameObject timeValue;
    [SerializeField]
    GameObject lableBox;
    [SerializeField]
    GameObject valueBox;
    [SerializeField]
    GameObject label;
    [SerializeField]
    GameObject pointerObj;
    
    Transform pointerTransform;

    AbmSimulation abmSimulation;
    SunSimulation sunSimulation;

    TextMeshProUGUI hourText;
    TextMeshProUGUI labelText;

    RadialController radialController;
    
    float pointerRot = 0;


    // Start is called before the first frame update
    void Start()
    {
        pointerTransform = pointerObj.GetComponent<RectTransform>();
        hourText = valueBox.GetComponent<TextMeshProUGUI>();
        labelText = label.GetComponent<TextMeshProUGUI>();
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        abmSimulation = GameObject.Find("Simulations").GetComponent<AbmSimulation>();
        sunSimulation = GameObject.Find("Simulations").GetComponent<SunSimulation>();
    }

    // Open sets the visuals for the menu on true and sets the setup of the surface dial
    public void Open()
    {
        radialController.rotationResolutionInDegrees = 360/15;
        radialController.useAutoHapticFeedback = false;

        timeValue.SetActive(true);
        lableBox.SetActive(true);

        labelText.text = "Hour:";
    }

    // Close set the visuals of false and changed the menue back to the Simualtion Setup
    public void Close()
    {
        timeValue.SetActive(false);
        lableBox.SetActive(false);
    }

    /// <summary>
    /// OnRotation changed the material of the result to the right material for every hour
    /// </summary>
    /// <param name="deg"></param>
    /// <param name="simName"></param>
    public void OnRotation(float deg, string simName)
    {
        pointerRot += deg;
        pointerRot = pointerRot % 360;

        pointerTransform.localRotation = Quaternion.Euler(0, 0, -pointerRot);

        //this is a save question to clear that the pointRot value is positiv for the switch case later
        if(pointerRot < 0)
        {
            pointerRot = 360 + pointerRot;
        }
        else if(pointerRot > 360)
        {
            pointerRot = 360 - pointerRot;
        }

        switch (pointerRot)
        {
            case 0:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(0);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(0);
                        break;
                }
                hourText.text = "8am";
                break;
            case 360 / 15:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(1);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(1);
                        break;
                }
                hourText.text = "9am";
                break;
            case 360/ 15 * 2:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(2);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(2);
                        break;
                }
                hourText.text = "10am";
                break;
            case 360/ 15 * 3:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(3);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(3);
                        break;
                }
                hourText.text = "11am";
                break;
            case 360/ 15 * 4:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(4);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(4);
                        break;
                }
                hourText.text = "12pm";
                break;
            case 360/ 15 * 5:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(5);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(5);
                        break;
                }
                hourText.text = "1pm";
                break;
            case 360/ 15 * 6:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(6);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(6);
                        break;
                }
                hourText.text = "2pm";
                break;
            case 360/ 15 * 7:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(7);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(7);
                        break;
                }
                hourText.text = "3pm";
                break;
            case 360 / 15 * 8:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(8);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(8);
                        break;
                }
                hourText.text = "4pm";
                break;
            case 360 / 15 * 9:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(9);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(9);
                        break;
                }
                hourText.text = "5pm";
                break;
            case 360 / 15 * 10:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(10);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(10);
                        break;
                }
                hourText.text = "6pm";
                break;
            case 360 / 15 * 11:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(11);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(11);
                        break;
                }
                hourText.text = "7pm";
                break;
            case 360 / 15 * 12:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(12);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(12);
                        break;
                }
                hourText.text = "8pm";
                break;
            case 360 / 15 * 13:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(13);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(13);
                        break;
                }
                hourText.text = "9pm";
                break;
            case 360 / 15 * 14:
                switch (simName)
                {
                    case "abm":
                        abmSimulation.ChangeAbmTexture(14);
                        break;
                    case "sun":
                        sunSimulation.ChangeSunTexture(14);
                        break;
                }
                hourText.text = "10pm";
                break;
        }

    }

}
