using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
/// <summary>
/// Class to manage the lifetime of the portals
/// </summary>
public class PortalManager : MonoBehaviour
{
    //Properties with a private backend
    [SerializeField]
    private GameObject portalPrefab;
    public GameObject PortalObject
    { get { return portalPrefab; } set { portalPrefab = PortalObject; } }

    //Rigid body of the portal prefab
    private Rigidbody rb;

    //Queue of portals active in the game using a first in first out approach
    [SerializeField]
    private Queue<GameObject> portalQueue = new Queue<GameObject>();

    /// <summary>
    /// The amount of time between teleports in milliseconds
    /// </summary>
    public float PortalCooldown = 200;

    private float TimeSinceLastTeleport = 0;

    private bool CanTeleport = true;


    // Start is called before the first frame update
    void Start()
    {
        if (portalPrefab == null) 
        {
            Console.Error.WriteLine("You forgot to add a valid portal prefab to the portal manager class!");
        }
        //Portal must have a rigid body component!
        rb = portalPrefab.GetComponent<Rigidbody>();
    }

	public void Update()
	{
		if(PortalCooldown > TimeSinceLastTeleport && CanTeleport == false)
        {
            TimeSinceLastTeleport += Time.deltaTime;
        }
        else
        {
            CanTeleport = true;
        }


	}

	/// <summary>
	/// Make a portal appear at a specified location, position, and scale.
	/// </summary>
	/// <param name="transform"></param>
	public void SpawnPortal(Vector3 pLocation, Quaternion pRotation)
    {
        if(portalQueue.Count < 2)
        {
            GameObject p = Instantiate(portalPrefab, pLocation, pRotation);
            portalQueue.Enqueue(p);
        }
        else
        {
            DestroyPortal(portalQueue.Peek());
			GameObject p = Instantiate(portalPrefab, pLocation, pRotation);
			portalQueue.Enqueue(p);
		}
    }
    /// <summary>
    /// Attempt to call OnDestroy method of the given portal and remove it from the queue
    /// </summary>
    /// <param name="portal"></param>
    public void DestroyPortal(GameObject portal)
    {
        portalQueue.Dequeue();
        Destroy(portal);
	}
    
    /// <summary>
    /// Call this from the OnCollisionEnter method in the portal prefab. <paramref name="portal"/> 
    /// should be "this" and <paramref name="other"> should be the player or whatever collided with the portal.
    /// </summary>
    /// <param name="portal"></param>
    /// <param name="other"></param>
    public void EnterPortal(GameObject portal, GameObject other)
    {
        if (CanTeleport)
        {
            if (!portalQueue.Contains(portal))
            {
                Debug.LogError("The portal you entered is not managed properly or doesn't exist!");
            }

            if (portalQueue.First().transform.position == portal.transform.position)
            {
                Debug.Log("Entered First Portal");
                other.transform.position = portalQueue.Last().transform.position + Vector3.down * 0.5f;
            }
            else if (portalQueue.Last().transform.position == portal.transform.position)
            {
                Debug.Log("Entered Last portal");
                other.transform.position = portalQueue.First().transform.position + Vector3.down * 0.5f;
            }
            else
            {
                Debug.LogError("The portal you entered is not first or last in the queue! (It should be)");
            }

            TimeSinceLastTeleport = 0;
            CanTeleport = false;
        }

        //TODO: Add velocity leaving the portal so you do not immediatly go back through


    }
   
}
