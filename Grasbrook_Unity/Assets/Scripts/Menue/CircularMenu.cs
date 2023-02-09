using UnityEngine;
using TMPro;
using Blitzy.UnityRadialController;
public class CircularMenu : MonoBehaviour
{
    Transform arrowTransform;

    [SerializeField] // this corection is needed throug the diffrence between the north 0 in the real world and the z-axis 0 in the unity Scene
    float rotationCorrection; //60

    #region SerializeGameObjects
    [SerializeField]
    GameObject circularValue;
    [SerializeField]
    ParticleSystem windTrails;
    [SerializeField]
    GameObject arrow;
    [SerializeField]
    GameObject labelBox;
    [SerializeField]
    GameObject valueBox;
    [SerializeField]
    GameObject label;
    [SerializeField]
    GameObject directionPerl;
    #endregion

    WindSimulation dataController_Wind;
    TextMeshProUGUI valueText;
    RadialController radialController;

    LegendChanger legendChanger;

    TextMeshProUGUI labelText;

    float arrowRot = 0;

    private void Start()
    {
        arrowTransform = arrow.GetComponent<RectTransform>();
        valueText = valueBox.GetComponent<TextMeshProUGUI>();
        labelText = label.GetComponent<TextMeshProUGUI>();
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        dataController_Wind = GameObject.Find("Simulations").GetComponent<WindSimulation>();
        legendChanger = GameObject.Find("LegendBox").GetComponent<LegendChanger>();
    }

    public void Open()
    {
        radialController.rotationResolutionInDegrees = 1;
        radialController.useAutoHapticFeedback = false;

        circularValue.SetActive(true);
        labelBox.SetActive(true);

        string valueString = arrowRot.ToString() + " °";
        valueText.text = valueString;
        labelText.text = "Wind Direction";
        windTrails.gameObject.SetActive(true);
    }

    public void Close()
    {
        circularValue.SetActive(false);
        labelBox.SetActive(false);
        windTrails.gameObject.SetActive(false);
    }


    public void OnRotation(float deg)
    {

        arrowRot += deg;
        arrowRot = arrowRot % 360;

        if(arrowRot < 0)
        {
            arrowRot = 360 + arrowRot;
        }

        arrowTransform.localRotation = Quaternion.Euler(0,0, -arrowRot);

        SetVisuals(arrowRot);
    }

   
    public void SetVisuals(float arrowValue)
    {

        GlobalVariable.GlobalWindDirection = (int)arrowValue;
        string valueString = GlobalVariable.GlobalWindDirection.ToString() + " °";
        valueText.text = valueString;

        windTrails.gameObject.transform.rotation = Quaternion.Euler(0, arrowRot+rotationCorrection, 0); 
        directionPerl.transform.localEulerAngles = new Vector3(0, 0, -arrowRot);

        dataController_Wind.windScenario.wind_direction = (int)arrowRot+ (int)rotationCorrection;
        int valueKey = 0;
        legendChanger.ChangeLegendInforamtion("wind" , GlobalVariable.GlobalWindDirection, valueKey);
    }


}
