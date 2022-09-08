using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Not finished class to start simulations and other thinks over the 
/// keyboard.
/// </summary>
public class InteractionHandler : MonoBehaviour
{
    GameObject networkingManager;
    CityPyO_Interface cpyo;
    CalculationModules_Interface calculationModules_Interface;
    void Start()
    {
        networkingManager = GameObject.Find("NetworkingManager");
        cpyo = networkingManager.GetComponent<CityPyO_Interface>();
        calculationModules_Interface = networkingManager.GetComponent<CalculationModules_Interface>();

    }

    // Update is called once per frame
    void Update()
    {

    }
}
