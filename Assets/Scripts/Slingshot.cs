using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    [SerializeField] private LineRenderer rubber;
    [SerializeField] private Transform LeftArm;
    [SerializeField] private Transform RightArm;
    [SerializeField] private Transform launchPoint;
    [SerializeField] private Configuration configuration;
    [SerializeField] private GameObject projLinePrefab;
    [SerializeField] private AudioClip shootSound;
    private Transform ballPrefab;
    Vector3 ballPosition;
    private GameObject currentProjLine;
    private AudioSource audioSource;


    void Start()
    {
        rubber.SetPosition(0, LeftArm.position);
        rubber.SetPosition(2, RightArm.position);

        audioSource = GetComponent<AudioSource>();

        // Ensure that the audio source is not playing on start
        if (audioSource != null)
        {
            audioSource.Stop();  // Stop any sound playing at the start
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0) && ballPrefab == null)
        {
            // Instantiate the ball
            ballPrefab = Instantiate(configuration.BallPrefab).transform;

            Rigidbody rigidbody = ballPrefab.GetComponent<Rigidbody>();
            rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;


            // Instantiate the projectile line prefab
            currentProjLine = Instantiate(projLinePrefab);

            // Attach the projectile line to the ball
            currentProjLine.transform.SetParent(ballPrefab);
            currentProjLine.transform.localPosition = Vector3.zero; // Match ball's position

            BallLifetime ballLifetime = ballPrefab.GetComponent<BallLifetime>();
            ballLifetime.isAiming = true;

            // Initialize the projectile line
            ProjectileLine projLine = currentProjLine.GetComponent<ProjectileLine>();
            if (projLine != null)
            {
                projLine.StartDrawing(ballLifetime); // Pass BallLifetime to start drawing
            }
            else
            {
                Debug.LogError("ProjectileLine component is missing on projLinePrefab.");
            }
        }
        if (Input.GetMouseButton(0)) {
            ballPosition = GetMousePositionInWorld();
            ballPrefab.position = ballPosition;
            rubber.SetPosition(1, ballPosition);
        }
        if (Input.GetMouseButtonUp(0)) {
            Rigidbody rigidbody = ballPrefab.GetComponent<Rigidbody>();
            rigidbody.isKinematic = false;
            Vector3 launchDirection = (launchPoint.position - ballPosition).normalized;
            rigidbody.linearVelocity = launchDirection * configuration.velocityMulti;
            if (ballPrefab != null)
            {
                FollowCam.SWITCH_VIEW(FollowCam.eView.slingshot);
            }
            FollowCam.POI = ballPrefab.gameObject;

            if (shootSound != null)
            {
                audioSource.PlayOneShot(shootSound); // Play the sound effect
            }

            // Disable aiming state after launch
            BallLifetime ballLifetime = ballPrefab.GetComponent<BallLifetime>();
            ballLifetime.isAiming = false;

            ballPrefab = null;
            MissionDemolition.SHOT_FIRED();

        }
    }

    Vector3 GetMousePositionInWorld() {
        Vector3 mousePosition = Input.mousePosition;
        mousePosition.z += Camera.main.transform.position.z;
        Vector3 mousePositionInWorld = Camera.main.ScreenToWorldPoint(mousePosition);
        mousePositionInWorld -= launchPoint.position;
        
        if (mousePositionInWorld.magnitude > configuration.Radius) {
            mousePositionInWorld.Normalize();
            mousePositionInWorld *= configuration.Radius;
        }

        return mousePositionInWorld + launchPoint.position;
    }
}
