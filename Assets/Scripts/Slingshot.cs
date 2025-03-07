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
    private Transform ballPrefab;
    Vector3 ballPosition;


    void Start()
    {
        rubber.SetPosition(0, LeftArm.position);
        rubber.SetPosition(2, RightArm.position);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) {
            ballPrefab = Instantiate(configuration.BallPrefab).transform;  
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
            FollowCam.POI = ballPrefab.gameObject;
            ballPrefab = null;

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
