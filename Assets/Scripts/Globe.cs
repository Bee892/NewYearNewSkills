using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Globe : MonoBehaviour
{
    public Settings MainSettings;
    
    private bool rotating;
    private float rotationSensitivity;
    private Vector3 mousePos;
    private Vector3 deltaMousePos;

    public bool Rotating
    {
        get
        {
            return rotating;
        }
        set
        {
            if (value && !rotating)
            {
                mousePos = Input.mousePosition;
                deltaMousePos = Vector3.zero;
            }
            rotating = value;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        rotationSensitivity = MainSettings.RotationSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        if (Rotating)
        {
            if (mousePos != Input.mousePosition)
            {
                deltaMousePos = Input.mousePosition - mousePos;
                if (Vector3.Dot(transform.up, Vector3.up) >= 0)
                {
					transform.Rotate(transform.up, -Vector3.Dot(deltaMousePos * rotationSensitivity, Camera.main.transform.right), Space.World);
				}
                else
                {
					transform.Rotate(transform.up, Vector3.Dot(deltaMousePos * rotationSensitivity, Camera.main.transform.right), Space.World);
				}
                
                transform.Rotate(Camera.main.transform.right, Vector3.Dot(deltaMousePos * rotationSensitivity, Camera.main.transform.up), Space.World);
            }

            mousePos = Input.mousePosition;
        }
    }
}
