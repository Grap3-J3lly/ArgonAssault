using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControls : MonoBehaviour
{
    
    [Header("Laser Gun Array")]
    [SerializeField] GameObject[] lasers;   

    [Header("Screen Position based tuning")]
    [SerializeField] float xRange = 10f;
    [SerializeField] float minYRange = -5f;
    [SerializeField] float maxYRange = 10f;
    [SerializeField] float positionPitchFactor = -2f;
    [SerializeField] float positionYawFactor = 2f;

    [Header("General Input Settings")]
    [SerializeField] InputAction movement;
    [SerializeField] InputAction combat;

    [Header("Player Input based tuning")]
    [Tooltip("How fast the ship moves")]
    [SerializeField] float controlSpeed = 30f;
    [SerializeField] float controlPitchFactor = -10f; 
    [SerializeField] float controlRollFactor = -20f;

    float xThrow;
    float yThrow;

    private void OnEnable() 
    {
        movement.Enable();
        combat.Enable();
    }

    private void OnDisable() 
    {
        movement.Disable();    
        combat.Disable();
    }

    private void Update() 
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
    }

    private void ProcessTranslation()
    {
        xThrow = movement.ReadValue<Vector2>().x;
        yThrow = movement.ReadValue<Vector2>().y;

        float xOffset = xThrow * controlSpeed * Time.deltaTime;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange);

        float yOffset = yThrow * controlSpeed * Time.deltaTime;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, minYRange, maxYRange);

        transform.localPosition = new Vector3(
        clampedXPos,
        clampedYPos,
        transform.localPosition.z);
    }

    private void ProcessRotation() 
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControlThrow = yThrow * controlPitchFactor;

        float yawDueToPosition = transform.localPosition.x * positionYawFactor;

        float rollDueToControlThrow = xThrow * controlRollFactor;

        float pitch = pitchDueToPosition + pitchDueToControlThrow;
        float yaw = yawDueToPosition;
        float roll = rollDueToControlThrow;

        transform.localRotation = Quaternion.Euler(pitch, yaw, roll);
    }

    private void ProcessFiring()
    {
        if(combat.ReadValue<float>() > .5f)
        {
            SetLasersActive(true);
        }
        else
        {
            SetLasersActive(false);
        }
    }

    private void SetLasersActive(bool isActive)
    {
        foreach(GameObject laser in lasers)
        {
            var laserEmission = laser.GetComponent<ParticleSystem>().emission;
            laserEmission.enabled = isActive;
        }
    }

}
