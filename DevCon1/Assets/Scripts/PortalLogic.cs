using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLogic : MonoBehaviour
{
	[SerializeField]
	private PortalManager pManager;
	public PortalManager portalManager
	{ get { return pManager; } set { pManager = portalManager; } }

	// Start is called before the first frame update
	void Start()
    {
        pManager = GameObject.FindGameObjectWithTag("PortalManager").GetComponent<PortalManager>();

        if(pManager == null)
        {
            Console.Error.WriteLine("No portal manager attached to portal prefab");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    /// <summary>
    /// When some object <paramref name="other"/> collides with the portal.
    /// </summary>
    /// <param name="other"></param>
    private void OnTriggerEnter(Collider other)
    {
		if (other.gameObject.CompareTag("Player"))
        {
            //The character controller does not like to be teleported around, so turn it off before moving player
            CharacterController cc = other.GetComponent<CharacterController>();
            cc.enabled = false;
            pManager.EnterPortal(gameObject, other.gameObject);
            cc.enabled = true;
        }
    }

	private void OnDestroy()
	{
        Destroy(gameObject);
	}
}
