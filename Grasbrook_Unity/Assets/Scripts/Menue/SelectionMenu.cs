using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;
using Blitzy.UnityRadialController;
public class SelectionMenu : MonoBehaviour
{
    [SerializeField]
    BuildingManager buildingManager;

    [SerializeField]
    GameObject infoBox;

    [SerializeField]
    Material buildingMat;

    [SerializeField]
    Material highlightBuildingMat;


    RadialController radialController;

    [SerializeField]
    GameObject label;

    [SerializeField]
    TextMeshProUGUI title;

    [SerializeField]
    TextMeshProUGUI landUse;

    [SerializeField]
    TextMeshProUGUI height;

    [SerializeField]
    TextMeshProUGUI area;

    [SerializeField]
    TextMeshProUGUI type;

    bool isActive = false;

    [SerializeField]
    LineRenderer line;

    [SerializeField]
    LineRenderer lineReflect;

    GameObject activeBuilding;

    float arrowRot = 0;

    private void Start()
    {
        //arrowTransform = arrow.GetComponent<RectTransform>();
        radialController = GameObject.Find("WheelController").GetComponent<RadialController>();

        line.startWidth = 0.3f;
        line.endWidth = 0.3f;

        lineReflect.startWidth = 0.2f;
        lineReflect.endWidth = 0.2f;

        //line.SetWidth(0.3f, 0.3f);
        //lineReflect.SetWidth(0.2f, 0.2f);
        //dataController_Wind = GameObject.Find("DataController_Wind").GetComponent<DataController_Wind>();
    }

    public void Open()
    {
        radialController.rotationResolutionInDegrees = 1;
        radialController.useAutoHapticFeedback = false;
        isActive = true;
        infoBox.SetActive(true);
        line.gameObject.SetActive(true);
        lineReflect.gameObject.SetActive(true);
    }

    private void Update()
    {
        if (isActive)
        {
            RunSelection();
        }
        
    }

    public void Close()
    {
        if (activeBuilding)
        {
            activeBuilding.GetComponent<Renderer>().material = buildingMat;
        }
        
        infoBox.SetActive(false);
        line.gameObject.SetActive(false);
        lineReflect.gameObject.SetActive(false);
        isActive = false;
    }

    void RunSelection()
    {
        GameObject prevHighlight = activeBuilding;
        CastRay(-arrowRot);

        if (prevHighlight != activeBuilding && prevHighlight != null)
        {
            if (prevHighlight.tag != "Wall")
            {
                prevHighlight.GetComponent<Renderer>().material = buildingMat;
            }

            if (activeBuilding.tag != "Wall")
            {
                activeBuilding.GetComponent<Renderer>().material = highlightBuildingMat;
            }

            if (activeBuilding.tag == "Building")
            {
                UpdateInfoDisplay(activeBuilding);
            }


        }
    }

    void CastRay(float currentRotation)
    {

        RaycastHit hit;
        RaycastHit hit2;

        
        Vector3 newDir = Quaternion.Euler(0, 0, currentRotation) * Vector3.up;    

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);
        lineReflect.SetPosition(0, transform.position);
        lineReflect.SetPosition(1, transform.position);

        if (Physics.Raycast(transform.position, transform.TransformDirection(newDir), out hit, Mathf.Infinity))
        {
            line.SetPosition(1, hit.point);
            Debug.DrawRay(transform.position, transform.TransformDirection(newDir * 1000), Color.green);
            if (hit.collider.gameObject.CompareTag("Building"))
            {
                activeBuilding = hit.collider.gameObject;
            }
            else
            {

                if (hit.collider.gameObject.CompareTag("Wall"))
                {
                    Debug.DrawRay(hit.point, Vector3.Reflect(transform.TransformDirection(newDir), hit.normal) * 1000, Color.red);
                    
                    if (Physics.Raycast(hit.point, Vector3.Reflect(transform.TransformDirection(newDir), hit.normal), out hit2, Mathf.Infinity))
                    {
                        lineReflect.SetPosition(0, hit.point);
                        lineReflect.SetPosition(1, hit2.point);

                        if (hit2.collider.gameObject.CompareTag("Building"))
                        {
                            activeBuilding = hit2.collider.gameObject;
                        }
                    }
                }
            }
        }
    }


    public void OnRotation(float deg)
    {

        arrowRot += deg;
        arrowRot = arrowRot % 360;

        if (arrowRot < 0)
        {
            arrowRot = 360 + arrowRot;
        }
    }

    void UpdateInfoDisplay(GameObject building)
    {
        if (buildingManager.buildingInformations.ContainsKey(activeBuilding.name)){
            BuildingInfo info = buildingManager.buildingInformations[activeBuilding.name];

            title.text = "Building - " + activeBuilding.name;
            landUse.text = info.land_use_detailed_type.ToString();
            height.text = info.building_height.ToString();
            area.text = info.floor_area.ToString();
            type.text = info.area_planning_type;
            
        }
    }
}


