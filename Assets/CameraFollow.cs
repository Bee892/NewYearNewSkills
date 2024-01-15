using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFPlayer : MonoBehaviour
{
    public Transform player;

        void Update()
        {
            if (player != null)
            {
            {
                player.position = transform.position;
            }
        }
    }
}
