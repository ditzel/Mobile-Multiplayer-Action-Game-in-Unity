using System;
using System.Collections;
using Photon.Pun;
using UnityEngine;

namespace UnderdogCity
{
    public class Player : MonoBehaviourPunCallbacks
    {
        [HideInInspector]
        public InputStr Input;
        public struct InputStr
        {
            public float LookX;
            public float LookZ;
            public float RunX;
            public float RunZ;
            public bool Jump;
        }

        public const float Speed = 10f;
        public const float JumpForce = 5f;

        [HideInInspector]
        public PlayerState State = PlayerState.NORMAL;

        protected Rigidbody Rigidbody;
        protected Quaternion LookRotation;
        protected Collider MainCollider;
        protected Animator CharacterAnimator;
        protected GameObject CharacterRagdoll;

        public CarAnimation NearestCar { get; protected set; }

        protected bool Grounded = true;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
            CharacterAnimator = GetComponentInChildren<Animator>();
            CharacterRagdoll = transform.Find("CharacterRagdoll").gameObject;
            MainCollider = GetComponent<Collider>();
        }

        private void Start()
        {
            SetRagdoll(false);
        }

        public void Pickup()
        {
            //do something with it
            Debug.Log("Player picked it up!");
        }

        private void Update()
        {
            if (Rigidbody == null)
                return;

            CharacterAnimator.SetBool("Grounded", Grounded);

            var localVelocity = Quaternion.Inverse(transform.rotation) * (Rigidbody.velocity / Speed);
            CharacterAnimator.SetFloat("RunX", localVelocity.x);
            CharacterAnimator.SetFloat("RunZ", localVelocity.z);


        }

        void FixedUpdate()
        {
            if (Rigidbody == null)
                return;

            switch (State)
            {
                case PlayerState.NORMAL:

                    var inputRun = Vector3.ClampMagnitude(new Vector3(Input.RunX, 0, Input.RunZ), 1);
                    var inputLook = Vector3.ClampMagnitude(new Vector3(Input.LookX, 0, Input.LookZ), 1);

                    Rigidbody.velocity = new Vector3(inputRun.x * Speed, Rigidbody.velocity.y, inputRun.z * Speed);

                    //rotation to go target
                    if (inputLook.magnitude > 0.01f)
                        LookRotation = Quaternion.AngleAxis(Vector3.SignedAngle(Vector3.forward, inputLook, Vector3.up), Vector3.up);

                    transform.rotation = LookRotation;
                    Grounded = Physics.OverlapSphere(transform.position, 0.3f, 1).Length > 1;

                    if (Input.Jump)
                    {
                        if (Grounded)
                        {
                            Rigidbody.velocity = new Vector3(Rigidbody.velocity.x, JumpForce, Rigidbody.velocity.z);
                        }
                    }

                    break;
                case PlayerState.IN_CAR:

                    //Fix to seat
                    transform.position = NearestCar.AnimDrivePosition.transform.position;
                    transform.rotation = NearestCar.AnimDrivePosition.transform.rotation;


                    break;
            }

            Rigidbody.useGravity = State == PlayerState.NORMAL;
            MainCollider.enabled = State == PlayerState.NORMAL;
            CharacterAnimator.SetBool("InCar", State == PlayerState.IN_CAR);
        }

        private void LateUpdate()
        {
            CharacterAnimator.transform.localPosition = Vector3.zero;
            CharacterAnimator.transform.localRotation = Quaternion.identity;
        }

        public void OnHit(Vector3 direction)
        {
            //Remote Procedure call
            photonView.RPC("OnHitRpc", RpcTarget.All, direction);
        }

        [PunRPC]
        void OnHitRpc(Vector3 direction, PhotonMessageInfo info)
        {
            SetRagdoll(true);
            if(GetComponent<Controller>() != null)
                Destroy(GetComponent<Controller>());

            //Properties
            PhotonNetwork.LocalPlayer.CustomProperties.Add("state", "dead");
            //access by: PhotonNetwork.PlayerListOthers[0].CustomProperties["state"]
        }

        public void EnterCar()
        {
            if (State == PlayerState.IN_CAR)
            {
                State = PlayerState.TRANSITION;
                NearestCar.CarPhysics.State = CarPhysics.CarState.FREE;
                StartCoroutine(ExitCarAnimation());
                return;
            }

            if (NearestCar.CarPhysics.State == CarPhysics.CarState.FREE && NearestCar != null && State == PlayerState.NORMAL) { 
                NearestCar.CarPhysics.photonView.TransferOwnership(PhotonNetwork.LocalPlayer);
                State = PlayerState.TRANSITION;
                NearestCar.CarPhysics.State = CarPhysics.CarState.OCCUPIED;
                StartCoroutine(EnterCarAnimation());
            }
        }



        public IEnumerator EnterCarAnimation()
        {
            //get in
            var time = 0f;
            CharacterAnimator.SetTrigger("EnterCar");
            const float animTime = 1.8f;
            while (time < animTime)
            {
                NearestCar.Animator.SetBool("Open", 0.3f < time && time < 1f);
                transform.position = Vector3.Lerp(NearestCar.AnimEnterPosition.transform.position, NearestCar.AnimDrivePosition.transform.position, time / 1.3f);
                transform.rotation = Quaternion.Lerp(NearestCar.AnimEnterPosition.transform.rotation, NearestCar.AnimDrivePosition.transform.rotation, time / 1.3f);

                if (time < 0.2f && time + Time.fixedDeltaTime >= 0.2f)
                    NearestCar.AudioSource.PlayOneShot(NearestCar.OpenClip);
                if (time < 1.2f && time + Time.fixedDeltaTime >= 1.2f)
                    NearestCar.AudioSource.PlayOneShot(NearestCar.CloseClip);

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            NearestCar.AudioSource.Play();

            State = PlayerState.IN_CAR;
        }

        public IEnumerator ExitCarAnimation()
        {
            NearestCar.AudioSource.Stop();

            //get out
            var time = 0f;
            CharacterAnimator.SetBool("InCar", false);
            const float animTime = 1.8f;
            while (time < animTime)
            {
                NearestCar.Animator.SetBool("Open", 0.0f < time && time < 1f);
                transform.position = Vector3.Lerp(NearestCar.AnimDrivePosition.transform.position, NearestCar.AnimEnterPosition.transform.position, time / 1.3f);
                transform.rotation = Quaternion.Lerp(NearestCar.AnimDrivePosition.transform.rotation, NearestCar.AnimEnterPosition.transform.rotation, time / 1.3f);

                if (time < 0.2f && time + Time.fixedDeltaTime >= 0.2f)
                    NearestCar.AudioSource.PlayOneShot(NearestCar.OpenClip);
                if (time < 1.2f && time + Time.fixedDeltaTime >= 1.2f)
                    NearestCar.AudioSource.PlayOneShot(NearestCar.CloseClip);

                time += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }


            State = PlayerState.NORMAL;
        }

        public void SetRagdoll(bool on)
        {
            CharacterAnimator.gameObject.SetActive(!on);
            CharacterRagdoll.gameObject.SetActive(on);
            if (on)
            {
                Destroy(MainCollider);
                Destroy(Rigidbody);
            }
        }

        public void OnTriggerEnter(Collider other)
        {
            if (NearestCar == null && other.GetComponent<CarAnimation>() != null)
                NearestCar = other.GetComponent<CarAnimation>();
        }

        public void OnTriggerExit(Collider other)
        {
            if (other.GetComponent<CarAnimation>() == NearestCar)
                NearestCar = null;
        }

        public enum PlayerState
        {
            NORMAL,
            TRANSITION,
            IN_CAR
        }
    }
}