using Blitzy.UnityRadialController;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Class to change the main values for the ABM Simulation
/// </summary>
public class AbmSetupMenu : MonoBehaviour
{

    [SerializeField]
    MenuSystem menuSystem;

#region Serialize GameObjects
    [SerializeField]
    GameObject arrowObj;
    [SerializeField]
    GameObject abmUnderMenue;
    [SerializeField]
    GameObject abmBridgeHC;
    [SerializeField]
    GameObject abmUnderpassVN;
    [SerializeField]
    GameObject abmStreetOrientation;
    [SerializeField]
    GameObject abmCityBlock;
    [SerializeField]
    GameObject abmAmenityDistribution;
    [SerializeField]
    GameObject abmBack;

    [SerializeField]
    GameObject abmBridgeHC_Object;
    [SerializeField]
    GameObject abmCityBlock_Object;
#endregion

    RadialController radialController;
    LegendChanger legendChanger;

    float arrowRot;
    float rotationPoint;

    Transform arrowTransform;

    // Start is called before the first frame update
    void Start()
    {
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        arrowTransform = arrowObj.GetComponent<RectTransform>();
        legendChanger = GameObject.Find("LegendBox").GetComponent<LegendChanger>();
    }

    // Open sets the visuals for the menu on true and sets the setup of the surface dial
    public void Open()
    {
        radialController.rotationResolutionInDegrees = 30;
        radialController.useAutoHapticFeedback = false;

        abmUnderMenue.SetActive(true);
    }

    // Close set the visuals of false and changed the menue back to the Simualtion Setup
    public void Close()
    {
        abmUnderMenue.SetActive(false);
        menuSystem.SwitchTo(MenuSystem.Screen.simSetup);
    }

    // The diffrent values that will be changed at what rotation when the surface dial get pressed 
    public void Press()
    {
        if (rotationPoint == -90 || rotationPoint == 270)
        {
            //Middle-Left Back
            Close();
            print("Get Back to the other Men");
        }
        else if (rotationPoint == -30 || rotationPoint == 330)
        {
            //Top-Left Bridge HafenCity
            if (GlobalVariable.GlobalABMBridgeToHC)
            {
                GlobalVariable.GlobalABMBridgeToHC = false;
                abmBridgeHC_Object.SetActive(false);
            }
            else if (!GlobalVariable.GlobalABMBridgeToHC)
            {
                GlobalVariable.GlobalABMBridgeToHC = true;
                abmBridgeHC_Object.SetActive(true);
            }    
            print("BridgeToHC changed : " + GlobalVariable.GlobalABMBridgeToHC);
        }
        else if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Top-Right Underpass VeddelNorth
            GlobalVariable.GlobalABMUnderpassVN = !GlobalVariable.GlobalABMUnderpassVN;
            print("UnderpassVn changed : " + GlobalVariable.GlobalABMUnderpassVN);
        }
        else if (rotationPoint == 90 || rotationPoint == -270)
        {
            //Middle-Right Main Street Orientation
            if (GlobalVariable.GlobalABMStreetOrientation.Equals("vertical"))
            {
                GlobalVariable.GlobalABMStreetOrientation = "horizontal";
            }
            else if (GlobalVariable.GlobalABMStreetOrientation.Equals("horizontal"))
            {
                GlobalVariable.GlobalABMStreetOrientation = "vertical";
            }
            print("Street Orientation : " + GlobalVariable.GlobalABMStreetOrientation);
        }
        else if (rotationPoint == 150 || rotationPoint == -210)
        {
            //Bottom-Right City Block Structure
            if (GlobalVariable.GlobalABMBlocks.Equals("open"))
            {
                abmCityBlock_Object.SetActive(true);
                GlobalVariable.GlobalABMBlocks = "closed";
            }
            else if (GlobalVariable.GlobalABMBlocks.Equals("closed"))
            {
                abmCityBlock_Object.SetActive(false);
                GlobalVariable.GlobalABMBlocks = "open";
            }
            print("Blocks : " + GlobalVariable.GlobalABMBlocks);
        }
        else if (rotationPoint == 210 || rotationPoint == -150)
        {
            //Bottom-Left Amenity Distribution
            if (GlobalVariable.GlobalABMAmenities.Equals("random"))
            {
                GlobalVariable.GlobalABMAmenities = "complementary";
            }
            else if (GlobalVariable.GlobalABMAmenities.Equals("complementary"))
            {
                GlobalVariable.GlobalABMAmenities = "random";
            }
            print("Amenities : " + GlobalVariable.GlobalABMAmenities);
        }
        legendChanger.ChangeLegendInforamtion("abm", 0, 0);
    }

    // Scaled a Icon at a specific rotation value
    #region OnRotation Scaling
    public void OnRotation(float deg)
    {
        arrowRot += deg;
        arrowRot = arrowRot % 360;

        arrowTransform.localRotation = Quaternion.Euler(0, 0, -arrowRot);
        rotationPoint = arrowRot;
        Vector3 scaleing = new Vector3(1.5f, 1.5f, 1.5f);

        if (rotationPoint == -90 || rotationPoint == 270)
        {
            //Middle-Left Back
            abmBack.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == -30 || rotationPoint == 330)
        {
            //Top-Left Bridge HafenCity
            abmBridgeHC.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Top-Right Underpass VeddelNorth
            abmUnderpassVN.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 90 || rotationPoint == -270)
        {
            //Middle-Right Main Street Orientation
            abmStreetOrientation.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 150 || rotationPoint == -210)
        {
            //Bottom-Right City Block Structure
            abmCityBlock.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 210 || rotationPoint == -150)
        {
            //Bottom-Left Amenity Distribution
            abmAmenityDistribution.transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else
        {
            abmBack.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmBridgeHC.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmUnderpassVN.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmStreetOrientation.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmCityBlock.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
            abmAmenityDistribution.transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
        }
    }
    #endregion
}
