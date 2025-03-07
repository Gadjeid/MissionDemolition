using UnityEngine;

[CreateAssetMenu(fileName = "Configuration", menuName = "Configuration")]
public class Configuration : ScriptableObject
{
    [field:SerializeField] public int Radius {get; private set;} = 4;
    [field:SerializeField] public float velocityMulti {get; private set;} = 10;
    [field:SerializeField] public GameObject BallPrefab {get; private set;}
}
