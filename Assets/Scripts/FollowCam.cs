using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static private FollowCam S;
    static public GameObject POI;

    public enum eView { none, slingshot, castle, both};

    [Header("Inscribed")]
    public float easing = 0.05f;
    public Vector2 minXY = Vector2.zero;
    public GameObject viewBothGO;

    [Header("Dynamic")]
    public float camZ;
    private Vector3 initialPosition;
    public eView nextView = eView.slingshot;
    
    void Awake() {
        S = this;
        camZ = this.transform.position.z;
        initialPosition = this.transform.position;
    }

    void FixedUpdate() {
        Vector3 destination = initialPosition;

        if (POI != null) {
            Rigidbody poiRigid = POI.GetComponent<Rigidbody>();
            if ((poiRigid != null) && poiRigid.IsSleeping()) {
                POI = null;
            }
        }

        if (POI != null) {
            destination = POI.transform.position;
        }
        
        destination = Vector3.Lerp(transform.position, destination, easing);
        destination.z = camZ;

        transform.position = destination;
    }

    public void SwitchView(eView newView) {
        // If the newView is "none", use the nextView
        if (newView == eView.none) {
            newView = nextView;
        }

        // Switch the camera view based on the selected newView
        switch (newView) {
            case eView.slingshot:
                POI = null;  // No POI for slingshot, so the camera is not following anything
                nextView = eView.castle;  // Next view will be the castle
                break;
            case eView.castle:
                POI = MissionDemolition.GET_CASTLE();  // Set POI to the castle
                nextView = eView.slingshot;  // Next view will be the slingshot
                break;
            case eView.both:
                POI = viewBothGO;  // Set POI to the 'both' view object
                nextView = eView.slingshot;  // Next view will be the slingshot
                break;
        }
    }

    void Update() {
        // Check if the "G" key is pressed
        if (Input.GetKeyDown(KeyCode.G)) {
            SwitchView();  // Call SwitchView to cycle through the views
        }
    }

    public void SwitchView() {
        SwitchView(eView.none);
    }

    static public void SWITCH_VIEW(eView newView) {
        S.SwitchView(newView);
    }
}
