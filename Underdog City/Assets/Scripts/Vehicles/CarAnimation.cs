using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnderdogCity
{
    public class CarAnimation : MonoBehaviour {

        [HideInInspector]
        public Animator Animator;
        [HideInInspector]
        public CarState State;
        [HideInInspector]
        public GameObject AnimEnterPosition;
        [HideInInspector]
        public GameObject AnimDrivePosition;
        [HideInInspector]
        public CarPhysics CarPhysics;

        // Use this for initialization
        void Awake () {
            Animator = GetComponentInChildren<Animator>();
            AnimEnterPosition = transform.Find("CarMesh").Find("AnimEnterPosition").gameObject;
            AnimDrivePosition = transform.Find("CarMesh").Find("AnimDrivePosition").gameObject;
            CarPhysics = GetComponent<CarPhysics>();
        }

        public enum CarState
        {
            FREE,
            OCCUPIED
        }
    }
}
