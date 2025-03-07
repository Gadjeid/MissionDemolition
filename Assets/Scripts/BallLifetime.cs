using UnityEngine;

public class BallLifetime : MonoBehaviour
{
    [SerializeField] private float lifetime = 10f;

    void Start()
    {
        Destroy(gameObject, lifetime);
    }
}
