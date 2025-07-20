using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
using TMPro;
using UnityEngine.SceneManagement;

public class WinPanel : MonoBehaviour
{
    [SerializeField] TMP_Text announcement;
    [SerializeField] Button disconnect;

    public void AnnounceWinner(int winner)
    {
        announcement.text = $"The results are in. \nThe winner of this lobby is: \nPlayer {winner}!";
        disconnect.onClick.AddListener(() =>
        {
            NetworkManager.Singleton.Shutdown();
            SceneManager.LoadScene("Menu", LoadSceneMode.Single);
        });
    }
}
