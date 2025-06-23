using Unity.Netcode;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransitionManager : NetworkBehaviour
{
    [SerializeField] private string gameSceneName = "Game";
    [SerializeField] GameObject managerPrefab;

    public void TransitionAllPlayersToGameScene()
    {
        Debug.Log("Server initiating scene transition");
        NetworkManager.Singleton.SceneManager.LoadScene(gameSceneName, LoadSceneMode.Single);

        // GameObject manager = Instantiate(managerPrefab);
        // manager.GetComponent<NetworkObject>().Spawn();
    }

    public override void OnNetworkSpawn()
    {
        // When any client loads the new scene, log it
        NetworkManager.Singleton.SceneManager.OnLoadComplete += (clientId, sceneName, loadMode) => 
        {
            Debug.Log($"Client {clientId} loaded {sceneName}");
        };
    }
}