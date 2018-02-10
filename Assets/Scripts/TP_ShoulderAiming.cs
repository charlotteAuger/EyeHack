﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class TP_ShoulderAiming : MonoBehaviour {
	public Transform aimMarker;
	public CinemachineVirtualCamera aimingView;
	public CinemachineFreeLook normalView;

	public Vector3[] cameraCorners;
	public float aimMarkerSpeed;



	// Use this for initialization
	void Start () {
		cameraCorners = new Vector3[4];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxis ("OrbitHorizontal") > 0.2f || Input.GetAxis ("OrbitHorizontal") < -0.2f || Input.GetAxis ("OrbitVertical") > 0.2f || Input.GetAxis ("OrbitVertical") < -0.2f) 
		{
			float vChange = -Input.GetAxis("OrbitVertical")*aimMarkerSpeed;
			float hChange = Input.GetAxis("OrbitHorizontal")*aimMarkerSpeed;

			aimMarker.localPosition = new Vector3 (aimMarker.localPosition.x+hChange, aimMarker.localPosition.y+vChange, aimMarker.localPosition.z);
		}

		UpdateCameraCorners (Camera.main.transform.position, Camera.main.transform.rotation);
		CheckCameraBoundraries ();
	}

	public void AimingMode (bool value)
	{
		normalView.enabled = !value;
		aimingView.gameObject.SetActive(value);
	}

	public void CheckCameraBoundraries()
	{
		RaycastHit hit;
		Ray topBound = new Ray (cameraCorners[0], cameraCorners[1] - cameraCorners[0]);
		Ray leftBound = new Ray (cameraCorners[0], cameraCorners[2] - cameraCorners[0]);
		Ray bottomBound = new Ray (cameraCorners [2], cameraCorners [3] - cameraCorners [2]);
		Ray rightBound = new Ray (cameraCorners [1], cameraCorners [3] - cameraCorners [1]);

		Debug.DrawRay (cameraCorners[0], cameraCorners[1] - cameraCorners[0], Color.magenta); //top
		Debug.DrawRay (cameraCorners[0], cameraCorners[2] - cameraCorners[0], Color.magenta); //left
		Debug.DrawRay (cameraCorners[2], cameraCorners[3] - cameraCorners[2], Color.magenta); //bottom
		Debug.DrawRay (cameraCorners[1], cameraCorners[3] - cameraCorners[1], Color.magenta); //right

		///////////////////////////////////////////////
		/// //////////////////////////////////////////// beware of parenting
		/// ////////////////////////////////////////////
		/// ///////////////////////////////////////////// 
		if (Physics.Raycast (topBound, out hit)) 
		{
			if (hit.collider.transform == aimMarker) 
			{
				aimingView.transform.localRotation = Quaternion.Euler(new Vector3 (aimingView.transform.localRotation.eulerAngles.x-0.2f, aimingView.transform.localRotation.eulerAngles.y, aimingView.transform.localRotation.eulerAngles.z));
			}
		}

		if (Physics.Raycast (leftBound, out hit)) 
		{
			if (hit.collider.transform == aimMarker) 
			{

			}
		}

		if (Physics.Raycast (bottomBound, out hit)) 
		{
			if (hit.collider.transform == aimMarker) 
			{

			}
		}

		if (Physics.Raycast (rightBound, out hit)) 
		{
			if (hit.collider.transform == aimMarker) 
			{

			}
		}

	}

	public Vector3 GetTargetPoint()
	{
		Vector3 aimingPoint = new Vector3(0,0,0);
		RaycastHit hit;

		if (Physics.Raycast (aimMarker.position, Camera.main.transform.position-aimMarker.position, out hit)) 
		{
			aimingPoint = hit.point;
		}
		return aimingPoint;
	}

	public void UpdateCameraCorners(Vector3 cameraPosition, Quaternion atRotation)
	{
		//clear the content of intoArray
		cameraCorners = new Vector3[4];

		float z = aimMarker.localPosition.z;
		float x = Mathf.Tan (Camera.main.fieldOfView / 4f) * z;
		float y = x / Camera.main.aspect;

		//top left
		cameraCorners[0] = (atRotation * new Vector3(-x,y,z))+ cameraPosition; //added and rotated the point relative to camera
		//top right
		cameraCorners[1] = (atRotation * new Vector3(x,y,z))+ cameraPosition;
		//bottom left
		cameraCorners[2] = (atRotation * new Vector3(-x,-y,z))+ cameraPosition;
		//bottom right
		cameraCorners[3] = (atRotation * new Vector3(x,-y,z))+ cameraPosition;
	}


}
