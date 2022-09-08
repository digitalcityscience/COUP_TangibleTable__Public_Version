using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurfaceDialMouseEmulation : MonoBehaviour
{
    MenuSystem _menuSystem;
    SimulationMenu simulationMenu;
    void Start()
    {
        _menuSystem = GetComponent<MenuSystem>();
        simulationMenu = _menuSystem.GetComponent<SimulationMenu>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse2))
        {
            if (simulationMenu == true)
            {
                simulationMenu.Press();
            }
            else
            {
                _menuSystem.Press();
            }
            
        }

        if (Input.mouseScrollDelta.y != 0)
        {
            _menuSystem.OnRotation(Input.mouseScrollDelta.y);
        }
    }

}
