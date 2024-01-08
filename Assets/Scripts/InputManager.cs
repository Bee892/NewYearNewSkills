using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    public Globe Planet;
    public Settings MainSettings;

    #region Actions
    public Action MiddleMousePressed;
    public Action MiddleMouseReleased;
    public Action Scroll;
	#endregion Actions
	// Start is called before the first frame update
	void Start()
    {
        MiddleMousePressed += onMiddlePress;
        MiddleMouseReleased += onMiddleRelease;
        Scroll += onScroll;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Fire3"))
        {
            MiddleMousePressed.Invoke();
        }
        else if (Input.GetButtonUp("Fire3"))
        {
            MiddleMouseReleased.Invoke();
        }

        if (Input.mouseScrollDelta != Vector2.zero)
        {
            Scroll.Invoke();
        }
    }

    private void onMiddlePress()
    {
        Planet.Rotating = true;
    }

	private void onMiddleRelease()
	{
        Planet.Rotating = false;
	}

    private void onScroll()
    {
        if (Camera.main.transform.position.z + (Input.mouseScrollDelta.y * MainSettings.ZoomSpeed * Constants.ZoomMod) > Constants.MaxZoomLevel)
        {
			Camera.main.transform.position.Set(Camera.main.transform.position.x, Camera.main.transform.position.y, Constants.MaxZoomLevel);
		}
        else if (Camera.main.transform.position.z + (Input.mouseScrollDelta.y * MainSettings.ZoomSpeed * Constants.ZoomMod) < Constants.MinZoomLevel)
        {
			Camera.main.transform.position.Set(Camera.main.transform.position.x, Camera.main.transform.position.y, Constants.MinZoomLevel);
		}
        else
        {
			Camera.main.transform.position = new Vector3(Camera.main.transform.position.x, Camera.main.transform.position.y, Camera.main.transform.position.z + (Input.mouseScrollDelta.y * MainSettings.ZoomSpeed * Constants.ZoomMod));
		}
	}
}
