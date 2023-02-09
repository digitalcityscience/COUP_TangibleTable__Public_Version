using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[Serializable]
public class CalibrationPoint
{
    [ReadOnly]
    public Vector2 tCoords;
    [Space]
    [Space]
    [ReadOnly]
    public Vector2 uCoords;
}

/// <summary>
/// Mostly setup to save the corner positions and what camera looked at wha table.
/// at the bottom there is a function to calculate the diffrence between the table and the scanned position value
/// with this we can use the buildings right.
/// </summary>
public class CalibrationSetupTables : MonoBehaviour
{

	//CalibrationPoints for Table0
	public CalibrationPoint se0;
	[Space]	[Space]
	public CalibrationPoint sw0;
	[Space]	[Space]
	public CalibrationPoint ne0;
	[Space]	[Space]
	public CalibrationPoint nw0;
	[Space]	[Space]	[Space]

	//CalibrationPoints for Table1
	public CalibrationPoint se1;
	[Space]	[Space]
	public CalibrationPoint sw1;
	[Space]	[Space]
	public CalibrationPoint ne1;
	[Space]	[Space]
	public CalibrationPoint nw1;
	[Space]	[Space]	[Space]

	//CalibrationPoints for Table2
	public CalibrationPoint se2;
	[Space]	[Space]
	public CalibrationPoint sw2;
	[Space]	[Space]
	public CalibrationPoint ne2;
	[Space]	[Space]
	public CalibrationPoint nw2;
	[Space]	[Space]	[Space]

	//CalibrationPoints for Table3
	public CalibrationPoint se3;
	[Space]	[Space]
	public CalibrationPoint sw3;
	[Space]	[Space]
	public CalibrationPoint ne3;
	[Space]	[Space]
	public CalibrationPoint nw3;
	[Space][Space][Space]

	private CullEverythingExceptMarkers cullEverythingExceptMarkers;

	[SerializeField]
	private string[] tableNumber = new string[4];
    [SerializeField]
	private int calib = 0;
	private int changeMarcerPos = 0;
	private int calibrationNumber = 0;

	float convX = 0;
	float convY = 0;

	public int Calib { get => calib; set => calib = value; }
    public int ChangeMarcerPos { get => changeMarcerPos; set => changeMarcerPos = value; }
    public int CalibrationNumber { get => calibrationNumber; set => calibrationNumber = value; }

    private void Start()
    {
		cullEverythingExceptMarkers = GameObject.Find("Main Camera").GetComponent<CullEverythingExceptMarkers>();
	}


    /// <summary>
    /// Function to calibrate all corners of the tables, after a specific time the 4 Aruco Makrer are changing the position one Table after another.
    /// If there is a other Table combination then 2x2 then THIS function need to change!!!
    /// </summary>
    /// <param name="xPosition"></param>
    /// <param name="zPosition"></param>
    /// <param name="uPosition"></param>
    /// <param name="Key"></param>

    public void CalibrationTableConersNew(float xPosition, float zPosition, Vector3 uPosition, int markerKey, string cameraID)
    {
		switch (Calib)
        {
			case 1:
				if(markerKey == 100 && sw0.tCoords.x == 0)
                {
					sw0.tCoords = new Vector2(xPosition, zPosition);
					sw0.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 101 && se0.tCoords.x == 0)
				{
					se0.tCoords = new Vector2(xPosition, zPosition);
					se0.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 102 && ne0.tCoords.x == 0)
				{
					ne0.tCoords = new Vector2(xPosition, zPosition);
					ne0.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 103 && nw0.tCoords.x == 0)
				{
					nw0.tCoords = new Vector2(xPosition, zPosition);
					nw0.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if(sw0.tCoords.x > 0 && se0.tCoords.x > 0 && ne0.tCoords.x > 0  && nw0.tCoords.x > 0)
                {
					Debug.Log("NEXT the 1");
					cullEverythingExceptMarkers.ChangeMarkerPosition(1);
                }
				;
				break;

			case 2:
				if(cameraID == tableNumber[1] && tableNumber[1] != tableNumber[0])
                {
					if (markerKey == 100 && sw1.tCoords.x == 0)
					{
						sw1.tCoords = new Vector2(xPosition, zPosition);
						sw1.uCoords = new Vector2(uPosition.x, uPosition.z);
					}
					if (markerKey == 101 && se1.tCoords.x == 0)
					{
						se1.tCoords = new Vector2(xPosition, zPosition);
						se1.uCoords = new Vector2(uPosition.x, uPosition.z);
					}
					if (markerKey == 102 && ne1.tCoords.x == 0)
					{
						ne1.tCoords = new Vector2(xPosition, zPosition);
						ne1.uCoords = new Vector2(uPosition.x, uPosition.z);
					}
					if (markerKey == 103 && nw1.tCoords.x == 0)
					{
						nw1.tCoords = new Vector2(xPosition, zPosition);
						nw1.uCoords = new Vector2(uPosition.x, uPosition.z);
					}
					if (sw1.tCoords.x > 0 && se1.tCoords.x > 0 && ne1.tCoords.x > 0 && nw1.tCoords.x > 0)
					{
						cullEverythingExceptMarkers.ChangeMarkerPosition(2);
					}
				}
				
				break;

			case 3:
				if (markerKey == 100 && sw2.tCoords.x == 0)
				{
					sw2.tCoords = new Vector2(xPosition, zPosition);
					sw2.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 101 && se2.tCoords.x == 0)
				{
					se2.tCoords = new Vector2(xPosition, zPosition);
					se2.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 102 && ne2.tCoords.x == 0)
				{
					ne2.tCoords = new Vector2(xPosition, zPosition);
					ne2.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 103 && nw2.tCoords.x == 0)
				{
					nw2.tCoords = new Vector2(xPosition, zPosition);
					nw2.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				break;

			case 4:
				if (markerKey == 100 && sw3.tCoords.x == 0)
				{
					sw3.tCoords = new Vector2(xPosition, zPosition);
					sw3.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 101 && se3.tCoords.x == 0)
				{
					se3.tCoords = new Vector2(xPosition, zPosition);
					se3.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 102 && ne3.tCoords.x == 0)
				{
					ne3.tCoords = new Vector2(xPosition, zPosition);
					ne3.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				if (markerKey == 103 && nw3.tCoords.x == 0)
				{
					nw3.tCoords = new Vector2(xPosition, zPosition);
					nw3.uCoords = new Vector2(uPosition.x, uPosition.z);
				}
				break;


			default:
				break;
		}
	}

	/// <summary>
	/// function to get what camera is filming what table
	/// </summary>
	/// <param name="cameraID"></param>
	public void TableCamConnecetion(string cameraID)
    {
		if(calib == 1)
        {
			tableNumber[0] = cameraID;
		}
		else if (calib == 2)
        {
			tableNumber[1] = cameraID;
        }
		else if (calib == 3)
		{
			tableNumber[2] = cameraID;
		}
		else if (calib == 4)
		{
			tableNumber[3] = cameraID;
		}
	}

	/// <summary>
	/// This function looks on that table the marcer was dedected and choose through this which corner is needed to scale the position 
	/// between the table and unity
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="camerID"></param>
	/// <returns></returns>
	public Vector2 ScaleToTables(float x, float y, string camerID)
    {

		if(camerID == tableNumber [0])
		{
			convX = scale(sw0.tCoords.x, se0.tCoords.x, sw0.uCoords.x, se0.uCoords.x, x);
			convY = scale(se0.tCoords.y, ne0.tCoords.y, se0.uCoords.y, ne0.uCoords.y, y);
		}
		else if(camerID ==tableNumber [1])
		{
			convX = scale(sw1.tCoords.x, se1.tCoords.x, sw1.uCoords.x, se1.uCoords.x, x);
			convY = scale(se1.tCoords.y, ne1.tCoords.y, se1.uCoords.y, ne1.uCoords.y, y);
		}
		else if (camerID == tableNumber [2])
		{
			convX = scale(sw2.tCoords.x, se2.tCoords.x, sw2.uCoords.x, se2.uCoords.x, x);
			convY = scale(se2.tCoords.y, ne2.tCoords.y, se2.uCoords.y, ne2.uCoords.y, y);
		}
		else if (camerID == tableNumber [3])
		{
			convX = scale(sw3.tCoords.x, se3.tCoords.x, sw3.uCoords.x, se3.uCoords.x, x);
			convY = scale(se3.tCoords.y, ne3.tCoords.y, se3.uCoords.y, ne3.uCoords.y, y);
		}
		return new Vector2(convX, convY);

	}

	public float scale(float tCoordsMin, float tCoordsMax, float uCoordsMin, float uCoordsMax, float position)
	{

		float tCoord = (tCoordsMax - tCoordsMin);
		float uCoord = (uCoordsMax - uCoordsMin);
		float newPosition = (((position - tCoordsMin) * uCoord) / tCoord) + uCoordsMin;
		return newPosition;
	}
}
