using UnityEngine;
using Unity.Netcode;
using UnityEngine.UI;
using TMPro;

public class MultiplayerMenu : MonoBehaviour
{
    public GameObject UI; //reference to the menu UI
    public TextMeshProUGUI playerCountText; //reference to the text field for player count

      void Update()
    {
        if (NetworkManager.Singleton != null)
        {
            int playerCount = NetworkManager.Singleton.ConnectedClients.Count;
            playerCountText.text = "Players: " + playerCount;
        }
    }
    public void StartHost() //starts the game as both server and client
    { 
        UI.SetActive(false);
        NetworkManager.Singleton.StartHost();
        // gameObject.SetActive(false);
        Debug.Log("Host click");
    }

    public void StartClient() //starts the game as client that connects to host or server
    {
        UI.SetActive(false);
        NetworkManager.Singleton.StartClient();
        // gameObject.SetActive(false);
        Debug.Log("Client click");
    }

    public void StartServer()//starts game as server only
    {
     NetworkManager.Singleton.StartServer();
        Debug.Log("Server click");
    }

    // public void disableUI() //disables the menu UI
    // {
    //     gameObject.SetActive(false);
    // }
}
