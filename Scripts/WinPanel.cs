using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField] TMP_Text announcement;
    [SerializeField] Button disconnect;

    public void AnnounceWinner(int winner)
    {
        announcement.text = $"The results are in. \nThe winner of this lobby is: \nPlayer {winner + 1}!";
        disconnect.gameObject.SetActive(true);
    }
    public void Exit()
    {
        NetworkManager.Singleton.Shutdown();
        SceneManager.LoadScene("Menu", LoadSceneMode.Single);
    }
}
