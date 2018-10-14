using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

namespace UnderdogCity
{
    public class GameManager : MonoBehaviourPunCallbacks
    {
        [Header("UC Game Manager")]

        public Player PlayerPrefab;

        [HideInInspector]
        public Player LocalPlayer;

        private void Awake()
        {
            if (!PhotonNetwork.IsConnected)
            {
                SceneManager.LoadScene("Menu");
                return;
            }
        }

        // Use this for initialization
        void Start()
        {
            Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
        {
            Player.RefreshInstance(ref LocalPlayer, PlayerPrefab);
        }
    }
}
