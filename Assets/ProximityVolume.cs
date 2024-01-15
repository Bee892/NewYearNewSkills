using UnityEngine;

public class CameraPlayer : MonoBehaviour
{
    public Transform player;
    public float maxDistance = 10f;  // Maximum distance at which the volume remains constant.
    public float minDistance = 1f;   // Minimum distance at which the volume is maximum.

    private AudioSource audioSource;

    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
        audioSource = GetComponent<AudioSource>();
        // Ensure the AudioSource has the necessary settings for 3D sound.
        audioSource.spatialBlend = 1f;  // 3D sound.
        audioSource.minDistance = minDistance;
        audioSource.maxDistance = maxDistance;
    }

    void Update()
    {
        if (player != null)
        {
            // Calculate the distance between the audio source and the player.
            float distance = Vector3.Distance(transform.position, player.position);

            // Adjust the volume based on the distance.
            float volume = 1f - Mathf.Clamp01((distance - minDistance) / (maxDistance - minDistance));
            audioSource.volume = volume;
        }
    }
}