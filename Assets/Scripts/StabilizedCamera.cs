using UnityEngine;

public class StabilizedCamera : MonoBehaviour
{
    [Header("Setup")]
    public Transform target; // Your 4-wheeler
    public Vector3 offset = new Vector3(0, 2, -5); // Position relative to bot
    
    [Header("Smoothing")]
    public float positionSmoothSpeed = 0.125f;
    public float rotationSmoothSpeed = 5.0f;
    public bool smoothVerticalOnly = true;

    void LateUpdate()
    {
        if (!target) return;

        // 1. Calculate the ideal position based on the bot's current rotation
        Vector3 desiredPosition = target.TransformPoint(offset);
        
        // 2. Smooth the position
        // Using SmoothDamp or Lerp here filters out the "jitters"
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, positionSmoothSpeed);
        
        // 3. Apply position
        transform.position = smoothedPosition;

        // 4. Smoothly look at the bot (filters out sudden tilting/pitching)
        Quaternion desiredRotation = Quaternion.LookRotation(target.position - transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, desiredRotation, rotationSmoothSpeed * Time.deltaTime);
    }
}