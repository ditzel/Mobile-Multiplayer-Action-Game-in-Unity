using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderdogCity
{
    public class Pickup : MonoBehaviour
    {

        public float TimeToRespawn;
        protected float TimeLeft;
        protected bool Pickable;

        // Start is called before the first frame update
        void Start()
        {
            Deactivate();
        }

        private void Deactivate()
        {
            Pickable = false;
            TimeLeft = TimeToRespawn;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(false);
        }

        private void Activate()
        {
            Pickable = true;
            for (int i = 0; i < transform.childCount; i++)
                transform.GetChild(i).gameObject.SetActive(true);
        }

        // Update is called once per frame
        void Update()
        {
            if(TimeLeft > 0)
                TimeLeft -= Time.deltaTime;
            if (!Pickable && TimeLeft <= 0)
                Activate();
        }

        void OnTriggerEnter(Collider other)
        {
            var player = other.GetComponent<Player>();
            if (player != null)
            {
                Deactivate();
                player.Pickup();
            }
        }

        void OnTriggerExit(Collider other)
        {
            
        }
    }
}
