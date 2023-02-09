using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using UnityEngine;
using MiniJSON;
using Newtonsoft.Json;
using System.IO;
using DG.Tweening;


[Serializable]
public class BuildingMarkerOffset
{
	public Vector3 positionOffset;
	public Quaternion rotationOffset;
	public Vector3 axisOffset;
}

public class CameraInformations
{
	public int XValue { get; set; }
	public int YValue { get; set; }
	public float RotationValue { get; set; }
	public string CameraID { get; set; }

	CameraInformations(int x, int y, float r, string id)
    {
		XValue = x;
		YValue = y;
		RotationValue = r;
		CameraID = id;
    }
}

public class TableInterface : MonoBehaviour
{
	private TcpClient socketConnection;
	private Thread clientReceiveThread;
	CalibrationSetupTables calibrationSetupTables;

	[SerializeField]
	Material buildingMat;

	[SerializeField]
	GameObject AllBuildingVertices;

	[SerializeField]
	GameObject calibrationMarkers;

	[SerializeField]
	CanvasMovement canvas;
	public GameObject quarterTable;

	[SerializeField]
	BuildingManager buildingManager;

	[SerializeField]
	public GameObject tableSurface;

	[SerializeField]
	GameObject dialTransform;

	[SerializeField]
	Transform tableNW;

	[SerializeField]
	Transform tableSE;

	[SerializeField]
	float motionThreshhold;

	[SerializeField]
	float dialSmoothThreshhold;

	[SerializeField]
	[Range(0, 1)]
	float rotationSmoothing = 0.4f;

	[SerializeField]
	[Range(0, 1)]
	float motionSmoothing = 0.5f;

	[Tooltip("Aruco 150")]
	public GameObject OffsetMaskAruco;

	public GameObject aruco250;

	public Vector2 realPos;
	public Vector2 detectedPos;
	public Vector2 mmError;

    String serverMessage;
	bool updateOffsets = false;

	Dictionary<int, BuildingMarkerOffset> geometryToMarkerOffsets = new Dictionary<int, BuildingMarkerOffset>();

	void Start()
	{
		ConnectToTcpServer();
		serverMessage = "";

		print("Read file:");
		String readFile = ReadString("geometryToMarkerOffsets");

		calibrationSetupTables = GameObject.Find("CalibrationManager").GetComponent<CalibrationSetupTables>();

		if (readFile != "{}")
		{
			geometryToMarkerOffsets = JsonConvert.DeserializeObject<Dictionary<int, BuildingMarkerOffset>>(readFile);
			print("Read GeometryToMarkerOffsets into the Dictionary");
		}

		DOTween.SetTweensCapacity(500, 100);


	}

    private void Update()
	{
        if (serverMessage != "")
        {
			ParsePositionUpdateFromTable(serverMessage);
            serverMessage = "";
        }

        // saved the new offset for the building
        if (Input.GetKeyDown(KeyCode.Tab)) {
			String serialized = JsonConvert.SerializeObject(geometryToMarkerOffsets, new JsonSerializerSettings()
			{
				ReferenceLoopHandling = ReferenceLoopHandling.Ignore

			});
			WriteString(serialized, "geometryToMarkerOffsets");
			Debug.Log("Tab get pressed:" + serialized);

		}

		// stoped the update sequence so you can calibrate it in the editor
		if (Input.GetKeyDown(KeyCode.Q))
		{

			updateOffsets = !updateOffsets;
			print("Update offsets: " + updateOffsets);
			Debug.Log("Q get pressed");

		}

		if (AllBuildingVertices == null)
		{
			AllBuildingVertices = GameObject.Find("BuildingOutlines");
		}


	}

/// <summary>
/// Takes the raw Marker Positions as JSON from the sensors under the table
/// </summary>
/// <param name="jsonString"></param>
void ParsePositionUpdateFromTable(string jsonString)
	{

		
		Dictionary<int, float[]> receivedMarkerPositions = JsonConvert.DeserializeObject<Dictionary<int, float[]>>(jsonString);

		foreach (KeyValuePair<int, float[]> currentMarker in receivedMarkerPositions)
		{
            
			// Check if the received marker is stored as corresponding to a building
			if (buildingManager.MarkerToGameObject.ContainsKey(currentMarker.Key))
            {
				
				GameObject buildingParent = buildingManager.MarkerToGameObject[currentMarker.Key];
				GameObject buildingGeometry = buildingParent.transform.GetChild(0).gameObject;

				buildingGeometry.layer = LayerMask.NameToLayer("Default");

				int x = (int)currentMarker.Value[0] ;
                int z = (int)currentMarker.Value[1];
				float rot = currentMarker.Value[2];
				string camerID = currentMarker.Value[3].ToString();

				Vector2 p = calibrationSetupTables.ScaleToTables(x, z, camerID);

				// Some offsets need to be applied to building position/rotation and rotation axis because the marker
				// isnt placed in the exact spot/rotation on the physical model where the virtual model has its pivot points
				if (geometryToMarkerOffsets.ContainsKey(currentMarker.Key) && updateOffsets == false)
                {
					// Offsets for the actual building geometry against the marker
                    buildingGeometry.transform.localPosition = geometryToMarkerOffsets[currentMarker.Key].positionOffset;
                    buildingGeometry.transform.localRotation = geometryToMarkerOffsets[currentMarker.Key].rotationOffset;

					// Offset for the rotation axis around the marker 
					buildingParent.transform.parent.gameObject.transform.position = geometryToMarkerOffsets[currentMarker.Key].axisOffset;

					// Some temporary stuff for motion smoothing 
					if (Math.Abs(Vector3.Distance(buildingParent.transform.localPosition, new Vector3(p.x, 0, p.y))) > motionThreshhold)
					{
						// Highlights a buildings outline in green if it has moved. if just protects against a nullref during startup
						if (AllBuildingVertices)
						{
							GameObject outline = AllBuildingVertices.transform.Find(buildingManager.MarkerToID[currentMarker.Key]).gameObject;
							outline.GetComponent<LineRenderer>().material.DOColor(Color.green, 0.2f);
						}
					}
					else
					{
						GameObject outline = AllBuildingVertices.transform.Find(buildingManager.MarkerToID[currentMarker.Key]).gameObject;
						outline.GetComponent<LineRenderer>().material.DOColor(Color.black, 1f);
					}

					Quaternion localRotaion = buildingParent.transform.localRotation;

					buildingParent.GetComponentInParent<Transform>().DOLocalMove(new Vector3(p.x, 0, p.y), motionSmoothing).SetEase(Ease.OutQuad);

					Quaternion newRot = Quaternion.Lerp(buildingParent.transform.localRotation, Quaternion.Euler(-0,rot,0), rotationSmoothing);

					buildingParent.transform.localRotation = newRot;

				}

				// This else triggers if there wasnt an entry in the offsets file for this marker or "updating" is enabled. In both cases we 
				// create a new offset and write in in place of the old (or empty) one

                else
                {
					BuildingMarkerOffset newData = new BuildingMarkerOffset();

					newData.positionOffset = buildingGeometry.transform.localPosition;
					newData.rotationOffset = buildingGeometry.transform.localRotation;
					newData.axisOffset = buildingParent.transform.parent.gameObject.transform.position;

					geometryToMarkerOffsets[currentMarker.Key] = newData;
					print("Ready to Save new Offset from:"+ "" + currentMarker.Key);
				}

			}
			// If the found marker doesnt correspond to a building, we need to check some special cases
			else
            {
				int x = (int)currentMarker.Value[0];
				int z = (int)currentMarker.Value[1];
				//float rot = currentMarker.Value[2]; wird gerade nicht benutzt
				string cameraID = currentMarker.Value[3].ToString(); //this one is the new Value to identify with which camera ther Aruco Marker is scanned

				Vector2 p = calibrationSetupTables.ScaleToTables(x, z, cameraID);


				// Those are the calibration markers projected onto the surface
				if (currentMarker.Key >= 100 && currentMarker.Key <= 103)
                {
					Vector3 u = calibrationMarkers.transform.Find(currentMarker.Key.ToString()).transform.localPosition;

					calibrationSetupTables.CalibrationTableConersNew(x, z, u, currentMarker.Key, cameraID);
					calibrationSetupTables.TableCamConnecetion(cameraID);
                }

				// this is currently the surface dial. Marker-ID is mostly arbitrarily chosen but this is a marker that seems
				// to get recognized pretty well in all orientations
				if (currentMarker.Key == 112)
				{
					if(Mathf.Abs(Vector3.Distance(dialTransform.transform.localPosition, new Vector3(p.x, 0, p.y))) > dialSmoothThreshhold)
                    {
						dialTransform.transform.DOLocalMove(new Vector3(p.x, 0, p.y), motionSmoothing).SetEase(Ease.OutQuad);
					}
						
				}
					
				// Another virtual marker that can be enabled and then dragged over the surface to measure the expected vs. real position
				// on every point of the surface. Could be automated into an "autocalib" routine? 
				if(currentMarker.Key == 150)
                {
					detectedPos = p;
					realPos = new Vector2(OffsetMaskAruco.transform.localPosition.x, OffsetMaskAruco.transform.localPosition.z);

					float mmErrorX = scale(0, 0.5f, 0, 770, detectedPos.x - realPos.x);
					float mmErrorY = scale(0, 0.5f, 0, 770, detectedPos.y - realPos.y);

					mmError = new Vector2(mmErrorX, mmErrorY);
				}

			}


		}

    }

	
	public static void WriteString(String toSave, String fileName)
	{
		string path = Application.persistentDataPath + "/" + fileName;
		//Write some text to the test.txt file
		StreamWriter writer = new StreamWriter(path, false);
		writer.Write(toSave);
		print("wrote new file!");
		writer.Close();
	}
	String ReadString(String fileName)
	{
		string path = Application.persistentDataPath + "/" + fileName;
		//Read the text from directly from the test.txt file
		StreamReader reader = new StreamReader(path);
		String readString = reader.ReadToEnd();
		reader.Close();
		return readString;
	}

	private void ListenForData()
	{
		try
		{
			socketConnection = new TcpClient("localhost", 8052);
			Byte[] bytes = new Byte[4*1024];//1024 was it before
			while (true)
			{
				// Get a stream object for reading 				
				using (NetworkStream stream = socketConnection.GetStream())
				{
					int length;
					// Read incomming stream into byte arrary. 					
					while ((length = stream.Read(bytes, 0, bytes.Length)) != 0)
					{
						var incomingData = new byte[length];
						Array.Copy(bytes, 0, incomingData, 0, length);
						// Convert byte array to string message. 	

						string newMsg = Encoding.ASCII.GetString(incomingData);
						serverMessage = newMsg; // we need to store it to retrieve it in the main update thread
					}
				}
			}
		}
		catch (SocketException socketException)
		{
			Debug.Log("Socket exception: " + socketException);
		}
	}




	private void ConnectToTcpServer()
	{
		try
		{
			clientReceiveThread = new Thread(new ThreadStart(ListenForData));
			clientReceiveThread.IsBackground = true;
			clientReceiveThread.Start();
		}
		catch (Exception e)
		{
			Debug.Log("On client connect exception " + e);
		}
	}

	public float scale(float OldMin, float OldMax, float NewMin, float NewMax, float OldValue)
	{

		float OldRange = (OldMax - OldMin);
		float NewRange = (NewMax - NewMin);
		float NewValue = (((OldValue - OldMin) * NewRange) / OldRange) + NewMin;
		return NewValue;
	}

	private void OnDisable()
    {
		clientReceiveThread.Abort();
	}
}
