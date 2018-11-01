using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderdogCity
{
    public class Car : MonoBehaviour {

        [HideInInspector]
        public Animator Animator;
        [HideInInspector]
        public CarState State;
        [HideInInspector]
        public GameObject AnimEnterPosition;
        [HideInInspector]
        public GameObject AnimDrivePosition;

        // Use this for initialization
        void Awake () {
            Animator = GetComponent<Animator>();
            AnimEnterPosition = transform.Find("AnimEnterPosition").gameObject;
            AnimDrivePosition = transform.Find("AnimDrivePosition").gameObject;
        }

        public enum CarState
        {
            FREE,
            OCCUPIED
        }
    }
}
