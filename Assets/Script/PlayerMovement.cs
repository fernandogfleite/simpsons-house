    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerMovement : MonoBehaviour
    {
        [SerializeField] public PuzzleController puzzleController;

        public CharacterController controller;
    
        public float speed = 12f;
        public float gravity = -9.81f * 2;
        public float jumpHeight = 1f;
    
        Vector3 velocity;

        void Update()
        {
            if (puzzleController.gameFinished)
            {
                return;
            }
    
            float x = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            Vector3 move = transform.right * x + transform.forward * z;
    
            controller.Move(move * speed * Time.deltaTime);

            if (Input.GetButtonDown("Jump"))
            {
                velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
    
            velocity.y += gravity * Time.deltaTime;
    
            controller.Move(velocity * Time.deltaTime);
        }
    }