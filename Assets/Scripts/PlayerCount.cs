using TMPro;
using Unity.Netcode;
using UnityEngine;

public class PlayerCountUI : MonoBehaviour
{
    [SerializeField] private GameObject playerCounterObject;

    private TMP_Text counterText;

    private void Awake()
    {
        counterText = playerCounterObject.GetComponentInChildren<TMP_Text>();
    }

    private void Update()
    {
        if (NetworkManager.Singleton == null)
            return;

        int playerCount = NetworkManager.Singleton.ConnectedClientsList.Count;

        counterText.text = "Players: " + playerCount;
    }
}