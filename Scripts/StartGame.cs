using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : NetworkBehaviour
{
    [SerializeField] string gameSceneName = "Game";
    [SerializeField] GameObject managerPrefab;

    public void TransitionAllPlayersToGameScene()
    // Transitions everyone to Game
    {
        Debug.Log("Server initiating scene transition");
        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);

        // GameObject manager = Instantiate(managerPrefab);
        // manager.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkSpawn()
    // Logs client entrance
    {
        NetworkManager.Singleton.SceneManager.OnLoadComplete += (clientId, sceneName, loadMode) => 
        {
            Debug.Log($"Client {clientId} loaded {sceneName}");
        };
    }
}