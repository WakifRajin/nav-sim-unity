using UnityEngine;
using System.Collections.Generic;

public class DiffDriveController : MonoBehaviour
{
    [Header("Wheel Colliders")]
    public WheelCollider frontLeft;
    public WheelCollider frontRight;
    public WheelCollider backLeft;
    public WheelCollider backRight;

    [Header("Visual Meshes")]
    public Transform flMesh;
    public Transform frMesh;
    public Transform blMesh;
    public Transform brMesh;

    [Header("Settings")]
    public float motorTorque = 1000f;
    public float steeringSensitivity = 0.5f; // How much it slows one side to turn
    public float brakeTorque = 2000f;

    void FixedUpdate()
    {
        // W/S = forward/backward, A/D = left/right
        float move = Input.GetAxis("Vertical");
        float steer = Input.GetAxis("Horizontal");

        // Differential Logic:
        // To turn right (D), the left wheels go faster and right wheels go slower (or reverse)
        float leftPower = (move - steer * steeringSensitivity) * motorTorque;
        float rightPower = (move + steer * steeringSensitivity) * motorTorque;

        // Apply Torque
        frontLeft.motorTorque = leftPower;
        backLeft.motorTorque = leftPower;
        frontRight.motorTorque = rightPower;
        backRight.motorTorque = rightPower;

        // Braking (Spacebar)
        float currentBrake = Input.GetKey(KeyCode.Space) ? brakeTorque : 0;
        ApplyBrakes(currentBrake);

        // Sync Meshes
        UpdateWheelVisual(frontLeft, flMesh);
        UpdateWheelVisual(frontRight, frMesh);
        UpdateWheelVisual(backLeft, blMesh);
        UpdateWheelVisual(backRight, brMesh);
    }

    void ApplyBrakes(float amount)
    {
        frontLeft.brakeTorque = amount;
        frontRight.brakeTorque = amount;
        backLeft.brakeTorque = amount;
        backRight.brakeTorque = amount;
    }

    void UpdateWheelVisual(WheelCollider collider, Transform mesh)
    {
        if (mesh == null) return;
        
        Vector3 pos;
        Quaternion rot;
        collider.GetWorldPose(out pos, out rot);
        mesh.position = pos;
        mesh.rotation = rot;
    }
}