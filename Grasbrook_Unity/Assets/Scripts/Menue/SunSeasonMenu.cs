using UnityEngine;
using Blitzy.UnityRadialController;
using DG.Tweening;

/// <summary>
/// Class to change what month will be used to calculate the TermalComfort Simulation
/// </summary>
public class SunSeasonMenu : MonoBehaviour
{

    [SerializeField]
    private GameObject sunMonthChanger;

    [SerializeField]
    private MenuSystem menuSystem;

    // The four icons for the four seasons
    [SerializeField]
    GameObject[] season = new GameObject[4];

    RadialController radialController;
    LegendChanger legendChanger;

    float rotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();
        legendChanger = GameObject.Find("LegendBox").GetComponent<LegendChanger>();
    }

    // Open sets the visuals for the menu on true and sets the setup of the surface dial
    public void Open()
    {
        radialController.rotationResolutionInDegrees = 30;
        radialController.useAutoHapticFeedback = false;
        sunMonthChanger.SetActive(true);
    }

    // Close set the visuals of false and changed the menue back to the Simualtion Setup
    public void Close()
    {
        sunMonthChanger.SetActive(false);
        menuSystem.SwitchTo(MenuSystem.Screen.simSetup);
    }

    // The diffrent values that will be changed at what rotation when the surface dial get pressed 
    public void Press()
    {
        if (rotationPoint == 330 || rotationPoint == -30)
        {
            //Top-Left Spring
            Debug.Log("Spring");
            GlobalVariable.GlobalMonth = 1;
            legendChanger.ChangeLegendInforamtion("sun", 0, 12);
            Close();
        }
        else if (rotationPoint == 30 || rotationPoint == -330)
        {
            //Top-rigth Summer
            Debug.Log("Summer");
            GlobalVariable.GlobalMonth = 2;
            legendChanger.ChangeLegendInforamtion("sun", 0, 1);
            Close();
        }
        else if (rotationPoint == 90 || rotationPoint == -270)
        {
            //Right Autumn
            Debug.Log("Autumn");
            GlobalVariable.GlobalMonth = 3;
            legendChanger.ChangeLegendInforamtion("sun", 0, 2);
            Close();
        }
        else if (rotationPoint == 150 || rotationPoint == -210)
        {
            //Bottom-right Winter
            Debug.Log("Winter");
            GlobalVariable.GlobalMonth = 4;
            legendChanger.ChangeLegendInforamtion("sun", 0, 6);
            Close();
        }
        
    }

    // Scaled a Icon at a specific rotation value
    public void OnRotation(float degs)
    {
        rotationPoint = degs;
        Vector3 scaleing = new Vector3(1.5f, 1.5f, 1.5f);

        if (rotationPoint == 330 || rotationPoint == -30 || rotationPoint == -360)
        {
            season[0].transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 30 || rotationPoint == -330)
        {
            season[1].transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 90 || rotationPoint == -270)
        {
            season[2].transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else if (rotationPoint == 150 || rotationPoint == -210)
        {
            season[3].transform.DOScale(scaleing, .3f).SetEase(Ease.OutQuad);
        }
        else
        {
            for (int i = 0; i < season.Length; i++) season[i].transform.DOScale(Vector3.one, .3f).SetEase(Ease.OutQuad);
        }
    }

}
