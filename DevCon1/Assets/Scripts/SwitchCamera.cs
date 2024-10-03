using Cinemachine;
using UnityEngine;

public class SwitchCamera : MonoBehaviour
{

    public GameObject FirstPersonCamera;
    CinemachineVirtualCamera FPcvc;

    public GameObject ThirdPersonCamera;
	CinemachineVirtualCamera TPcvc;

    public bool IsFirstPerson = false;

	private GameObject _player;

	private void Awake()
	{
		// get a reference to our main camera
		if (_player == null)
		{
			_player = GameObject.FindGameObjectWithTag("Player");
		}
	}
	// Start is called before the first frame update
	void Start()
    {
        FirstPersonCamera.TryGetComponent(out FPcvc);
		ThirdPersonCamera.TryGetComponent(out TPcvc);
	}

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Fire2"))
        {
            FPcvc.Priority = 10;
            TPcvc.Priority = 1;
            IsFirstPerson = true;
        }
        else if(Input.GetButtonUp("Fire2"))
        {
            FPcvc.Priority = 1;
            TPcvc.Priority = 10;
			IsFirstPerson = false;
        }

        //FirstPersonCamera.transform.rotation = _player.transform.rotation;

    }
}
