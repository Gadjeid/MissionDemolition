using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallLifetime : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;
    [SerializeField] private float maxZThreshold = 50f;
    const int LOOKBACK_COUNT = 10;
    static List<BallLifetime> PROJECTILES = new List<BallLifetime>();
    private Vector3 prevPos;
    private List<float> deltas = new List<float>();

    [SerializeField]
    private bool _awake = true;
    public bool awake {
        get {return _awake;}
        private set {_awake = value;}
    }
    
    private bool _isAiming = false; 
    public bool isAiming
    {
        get { return _isAiming; }
        set { _isAiming = value; }
    }
    private Rigidbody rigid;
    void Start()
    {
        rigid = GetComponent<Rigidbody>();
        awake = true;
        prevPos = transform.position;

        PROJECTILES.Add(this);
        Invoke(nameof(SelfDestruct), lifetime);

    }

    void FixedUpdate()
    {
        if (rigid.isKinematic || _isAiming) // Prevent sleep check while aiming
        {
            awake = true;
            return;
        }

        if (transform.position.z > maxZThreshold)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 deltaV3 = transform.position - prevPos;
        deltas.Add(deltaV3.magnitude);
        prevPos = transform.position;

        while (deltas.Count > LOOKBACK_COUNT)
        {
            deltas.RemoveAt(0);
        }

        float maxDelta = 0;
        foreach (float f in deltas)
        {
            if (f > maxDelta) maxDelta = f;
        }

        if (maxDelta <= Physics.sleepThreshold)
        {
            awake = false;
            rigid.Sleep();
        }
    }

    private void OnDestroy() {
        PROJECTILES.Remove(this);
    }

    static public void DESTROY_PROJECTILES() {
        foreach (BallLifetime p in PROJECTILES) {
            Destroy(p.gameObject);
        }
    }
    private void SelfDestruct()
    {
        // Double-check z-threshold before destroying the ball
        if (transform.position.z > maxZThreshold || !awake)
        {
            Destroy(gameObject);
        }
    }
}
