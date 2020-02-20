﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Yasuhiro.FPSGame {
    public class Movement : MonoBehaviour
    {
        // Start is called before the first frame update
        private Rigidbody player_rig; 
        public Camera playerCamera;
        public LayerMask ground;
        public Transform groundDetector;
        private float baseFOV;
        public float moveSpeed;
        public float jumpForce;
        public float sprintModifier;
        public float sprintFOVModifier = 1.25f;
        void Start()
        {
            baseFOV = playerCamera.fieldOfView;
            Camera.main.enabled = false;
            player_rig = GetComponent<Rigidbody>();
        }

        void FixedUpdate()
        {
            float _hMove = Input.GetAxisRaw("Horizontal");
            float _vMove = Input.GetAxisRaw("Vertical");
            bool _sprint = Input.GetKey(KeyCode.LeftShift);
            bool _jump = Input.GetKey(KeyCode.Space);

            bool isGrounded = Physics.Raycast(groundDetector.position, Vector3.down, 0.1f, ground);
            bool isJumping = _jump && isGrounded;
            bool isSprinting = _sprint && _vMove > 0 && isGrounded;

            if (isJumping) {
                player_rig.AddForce(Vector3.up * jumpForce);
            }

            Vector3 _direction = new Vector3(_hMove, 0, _vMove);
            _direction.Normalize();

            float _adjSpeed = moveSpeed;
            if (isSprinting) _adjSpeed *= sprintModifier;
            Vector3 _targetVolocity = transform.TransformDirection(_direction) * _adjSpeed * Time.fixedDeltaTime;
            _targetVolocity.y = player_rig.velocity.y;
            player_rig.velocity = _targetVolocity;

            if (isSprinting) {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, baseFOV * sprintFOVModifier, Time.fixedDeltaTime * 8f);
            } else {
                playerCamera.fieldOfView = Mathf.Lerp(playerCamera.fieldOfView, baseFOV, Time.fixedDeltaTime * 8f);
            }
        }
    }
}