using UnityEngine;
using System.Collections;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;
using ExitGames.Client.Photon;

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
            LocalPlayer = PhotonNetwork.Instantiate(PlayerPrefab.gameObject.name, Vector3.zero, Quaternion.identity).GetComponent<Player>();
        }

        // Update is called once per frame
        void Update()
        {

        }

        public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player target, ExitGames.Client.Photon.Hashtable changedProps)
        {
            foreach(var change in changedProps)
                Debug.Log("Property " + change.Key + " of player " + target.UserId + " changed to " + change.Value);
        }
    }
}
