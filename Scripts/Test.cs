using UnityEngine;
using Unity.Netcode;

public class Test : NetworkBehaviour
{
    [SerializeField] GameObject managerPrefab;
    
    void Start()
    {
        if (IsServer || IsHost)
        {
            GameObject manager = Instantiate(managerPrefab);
            manager.GetComponent<NetworkObject>().Spawn();
        }
    }
    
}
