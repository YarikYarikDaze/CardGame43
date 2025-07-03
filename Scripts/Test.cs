using UnityEngine;
using Unity.Netcode;

public class Test : NetworkBehaviour
{
    [SerializeField] GameObject managerPrefab;
    
    void Start()
    {
        if (NetworkManager.Singleton.IsHost)
        {
            GameObject manager = Instantiate(managerPrefab);
            if (manager)
                manager.GetComponent<NetworkObject>().Spawn();
        }
    }
    
}
