﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HackGun : MonoBehaviour 
{
	TP_ShoulderAiming aimingSys;
	public GameObject sphere;
	Camera playerView;

	public GameObject currentTarget;
	GameObject currentQuarrel;
	bool shot;

    public Image currentIcon;
    public Sprite[] icons;


	// Use this for initialization
	void Start () 
	{
		aimingSys = transform.GetComponent<TP_ShoulderAiming> ();
		playerView = Camera.main;
        currentIcon.sprite = icons[0];
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetAxisRaw ("Aim") >= 1 && !Chara_PlayerController.isAiming) 
		{
            Chara_PlayerController.isAiming = true;
			aimingSys.AimingMode (true);
		}

        else if (Input.GetAxisRaw ("Aim") <= 0 && (Chara_PlayerController.isAiming || aimingSys.aimingView.gameObject.active == true)) 
		{
            Chara_PlayerController.isAiming = false;
			aimingSys.AimingMode (false);
        }


		if (Input.GetAxisRaw ("Hack") > 0.4 && !shot) 
		{
			RaycastHit hit;
			Vector3 targetPoint = aimingSys.GetTargetPoint ();

			LayerMask layerMask = (1 << 2);
			layerMask = ~layerMask;

			if (Physics.Raycast (transform.position, targetPoint - transform.position, out hit, Mathf.Infinity, layerMask)) 
			{
				if (hit.collider.gameObject.tag == "Hackable")
                {
                    if (currentQuarrel != null)
                            Destroy(currentQuarrel);
                    currentQuarrel = Instantiate(sphere, hit.point, Quaternion.identity);
                    currentQuarrel.transform.parent = hit.collider.gameObject.transform;
                    currentQuarrel.layer = 1;
               
					currentTarget = hit.collider.gameObject;
                    currentIcon.sprite = icons[currentTarget.GetComponent<HackSwitch>().targets[0].icon];
				}
			}
			shot = true;
		} 
		else if (Input.GetAxisRaw ("Hack") < 0.8) 
		{
			shot = false;
		}


		if (Input.GetButtonDown ("Interaction") && currentTarget != null) 
		{
			if (currentTarget.GetComponent<Viewable>() != null) 
			{
				ViewInterpolation(currentTarget.GetComponent<Viewable>().GetView ());

				if (Chara_PlayerController.isAiming) 
				{
					aimingSys.AimingMode (false);
				}
			}

			else if (currentTarget.GetComponent<HackSwitch> () != null) 
			{
				currentTarget.GetComponent<HackSwitch> ().SwitchTargets ();
			}
		}
	}

	void ViewInterpolation(Camera enemyView)
	{
		Chara_PlayerController.controlsAble = !Chara_PlayerController.controlsAble;
		playerView.enabled = !playerView.enabled;
		enemyView.enabled = !enemyView.enabled;
	}

    Transform GetParentFromTag(Transform target, string _tag)
    {
        Transform parent = target.parent;

        while (parent != null)
        {
            if (parent.tag == _tag)
            {
                return parent.transform;
            }

            parent = parent.transform.parent;
        }

        return null;
    }
		
}
