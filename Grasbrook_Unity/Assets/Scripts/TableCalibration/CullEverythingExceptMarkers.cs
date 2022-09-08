using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CullEverythingExceptMarkers : MonoBehaviour
{
    [SerializeField]
    Camera mainCamera;

    CalibrationSetupTables calibrationSetup;

    [SerializeField]
    GameObject marker100;
    [SerializeField]
    GameObject marker101;
    [SerializeField]
    GameObject marker102;
    [SerializeField]
    GameObject marker103;


    int oldMask;
   void Start()
    {
        calibrationSetup = GameObject.Find("CalibrationManager").GetComponent<CalibrationSetupTables>();
        oldMask = mainCamera.cullingMask;
        ChangeMarkerPosition(0);
    }

    public void ChangeMarkerPosition(int positionNumber)
    {
        
        if (positionNumber == 0)
        {
            mainCamera.cullingMask = (1 << LayerMask.NameToLayer("UI"));
            Debug.Log("First Calibration Time");
            marker100.transform.localPosition = new Vector3(-0.031f, 1f, 0.479f);
            marker101.transform.localPosition = new Vector3(-0.475f, 1f, 0.48f);
            marker102.transform.localPosition = new Vector3(-0.476f, 1f, 0.033f);
            marker103.transform.localPosition = new Vector3(-0.033f, 1f, 0.04f);
            calibrationSetup.Calib = 1;
            ToggleVisibility(calibrationSetup.Calib);

        }
        else if (positionNumber == 1)
        {
            Debug.Log("Second Calibration Time");
            marker100.transform.localPosition = new Vector3(0.474f, 1f, 0.48f);
            marker101.transform.localPosition = new Vector3(0.03f, 1f, 0.482f);
            marker102.transform.localPosition = new Vector3(0.024f, 1f, 0.031f);
            marker103.transform.localPosition = new Vector3(0.467f, 1f, 0.03f);
            calibrationSetup.Calib = 2;
            ToggleVisibility(calibrationSetup.Calib);
        }

        //else if (timeSinceStartup > 6 && timeSinceStartup < 8)
        //{
        //    marker100.transform.localPosition = new Vector3(-0.03f, 1f, -0.039f);
        //    marker101.transform.localPosition = new Vector3(-0.463f, 1f, -0.036f);
        //    marker102.transform.localPosition = new Vector3(-0.473f, 1f, -0.474f);
        //    marker103.transform.localPosition = new Vector3(-0.028f, 1f, -0.47f);
        //    calibrationSetup.Calib = 3;
        //    ToggleVisibility(calibrationSetup.Calib);
        //}

        //else if (timeSinceStartup > 8 && timeSinceStartup < 9)
        //{
        //    calibrationSetup.Calib = 0;
        //    ToggleVisibility(calibrationSetup.Calib);
        //}

        //else if (timeSinceStartup > 9 && timeSinceStartup < 11)
        //{
        //    marker100.transform.localPosition = new Vector3(0.47f, 1f, -0.039f);
        //    marker101.transform.localPosition = new Vector3(0.0369f, 1f, -0.036f);
        //    marker102.transform.localPosition = new Vector3(0.027f, 1f, -0.474f);
        //    marker103.transform.localPosition = new Vector3(0.472f, 1f, -0.47f);
        //    calibrationSetup.Calib = 4;
        //    ToggleVisibility(calibrationSetup.Calib);
        //}
        else if (positionNumber == 2)
        {
            Debug.Log("WRONG");
            mainCamera.cullingMask = oldMask;
            ToggleVisibility(0);
        }
    }

    private void ToggleVisibility(int toggle)
    {
        if (toggle == 0)
        {
            marker100.SetActive(false);
            marker101.SetActive(false);
            marker102.SetActive(false);
            marker103.SetActive(false);
        }
        else
        {
            marker100.SetActive(true);
            marker101.SetActive(true);
            marker102.SetActive(true);
            marker103.SetActive(true);
        }
    }
}
