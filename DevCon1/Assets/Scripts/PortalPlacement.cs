using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class PortalPlacement : MonoBehaviour
{

    [SerializeField]
    private PortalManager pManager;
	public PortalManager portalManager
	{ get { return pManager; } set { pManager = portalManager; } }
    

	//public GameObject portalPrefab; // The portal prefab [No longer needed with portalManager class]
    public Transform placementPoint; // The point from where the portal will be placed
    public float placementRate = 0.5f; // Time between placing portals 

    [SerializeField]
    public float offsetAmount = -0.2f;

    private float nextPlacementTime = 0f; // Variable for resetting placement time

    public bool canPlace; // Bool check for collisions

    [SerializeField] LayerMask portalLayer;

    void OnTriggerExit(Collider other) // When not colliding
    {
        Debug.Log("Able to place portal");
        canPlace = true; 
    }

    void OnTriggerEnter(Collider other) // When colliding
    {
        Debug.Log("Cannot place portal");
        canPlace = false;
    }

    void Update()
    {
        if (canPlace == true) // Collision check
        {
            // Checks if enough time has passed since the last placement
            if (Input.GetButton("Fire1") && Time.time >= nextPlacementTime)
            {
                nextPlacementTime = Time.time + placementRate; // Sets a cooldown period for how often the player can place 
                Place();
            }
        }
    }
	private void Place()
    {
        Vector3 _startPos = Camera.main.transform.position;
        Vector3 _startDir = Camera.main.transform.forward;

        RaycastHit _HitInfo;

		if (Physics.Raycast(_startPos, _startDir, out _HitInfo, 100.0f, portalLayer))
        {
            Vector3 offset = _startPos - _HitInfo.point;
            offset.Normalize();
            offset *= offsetAmount;

            Quaternion rot = Quaternion.FromToRotation(-Vector3.forward, _HitInfo.normal);
            Vector3 loc = _HitInfo.point - offset;
			pManager.SpawnPortal(loc, rot);
        }

        //[Obsolete with portalManager class

        //if (portalPrefab != null) // Exception Check
        //{
        //    // Instantiate the portal
           
        //    //GameObject portal = Instantiate(portalPrefab, placementPoint.position, placementPoint.rotation);
        //}
    }
}
