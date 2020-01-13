﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    [System.Serializable]
    public class Settings
    {
        /// <summary>
        /// Range of colors that newly-spawned boids can have
        /// </summary>
        public Gradient colorRange;

        #region BoidBehaviorSettings

        [Header("Boid Behavior Settings")]
        /// <summary>
        /// Weight used to control separation force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float separation;
        
        /// <summary>
        /// Weight used to control alignment force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float alignment;
        
        /// <summary>
        /// Weight used to control cohesion force
        /// </summary>
        [SerializeField]
        [Range(0,1)]
        public float cohesion;

        #endregion

        #region DetectionSettings

        [Header("Detection Settings")]

        /// <summary>
        /// Number of raycasts to perform for detection
        /// </summary>
        [Range(3,30)]
        public int raycastCount;

        /// <summary>
        /// Width (in degrees) of cone used for detection
        /// </summary>
        [Range(0, 360)]
        public float degrees;

        /// <summary>
        /// Radius of cone used for detection
        /// </summary>
        [Range(0,10)]
        public float range;

        /// <summary>
        /// Acceleration used when calculating separation
        /// </summary>
        public float maxAcceleration = 15;

        #endregion

        #region MovementSettings
        
        [Header("Movement")]
        /// <summary>
        /// Boid movement speed
        /// </summary>
        public float moveSpeed = 15;

        /// <summary>
        /// Boid rotation speed
        /// </summary>
        public float rotationSpeed = 360;

        #endregion
    }

    private Settings settings => BoidManager.Instance.settings;

    private SpriteRenderer spriteRenderer;

    void Start()
    {
        // Set random boid rotation
        float degrees = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0,0,degrees);

        // Pick random color from colorRange gradient
        if (TryGetComponent(out spriteRenderer)) {
            spriteRenderer.color = settings.colorRange.Evaluate(Random.value);
        }
    }

    void Update()
    {
        // Get movement in straight line
        Vector3 forward = transform.up;

        List<Boid> boids = BoidDetection.NearbyBoids(this, settings);
        
        Vector3 separation = BoidDetection.GetSeparation(this, boids, settings).normalized * settings.separation;
        Vector3 alignment = BoidDetection.GetAlignment(this, boids, settings).normalized * settings.alignment;
        Vector3 cohesion = BoidDetection.GetCohesion(this, boids, settings).normalized * settings.cohesion;

        // Get averages of all vectors
        Vector3 sum = (forward + separation + alignment + cohesion).normalized;
        Debug.DrawRay(transform.position, sum * settings.moveSpeed, Color.green);

        // Apply resulting rotation to boid
        RotateTowards(sum);

        // Move in direction of new rotation (scaled by movement speed)
        transform.position += transform.up * settings.moveSpeed * Time.fixedDeltaTime;
    }

    /// <summary>
    /// Rotates boid in given direction
    /// </summary>
    /// <param name="direction">Target rotation</param>
    void RotateTowards(Vector3 direction)
    {
        // Get angle to target (in degrees)
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

        // Calculate rotation towards target direction
        Quaternion targetRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // Smooth rotation towards target
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.fixedDeltaTime * settings.rotationSpeed);
    }
}
