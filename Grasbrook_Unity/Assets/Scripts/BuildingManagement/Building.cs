using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/// <summary>
/// Class that is used in the BuildingManagerClass to save all
/// inforamtions over a building
/// </summary>

public class BuildingInfo
{
    public string building_id;
    public string land_use_detailed_type;
    public float building_height;
    public string area_planning_type;
    public float floor_area;
    public string city_scope_id; 
}


public class BuildingPosition : MonoBehaviour
{
    public float[] position;
}

