using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

public class MovementScript : MonoBehaviour
{
    private CharacterController _controller;
    private NavMeshAgent _agent;
    private Animator _animator;
    private Camera _cam;
    private Vector2 _moveInput;
    private Vector2 _lookInput;
    public float movementSpeed = 40f;
    public float rotationSpeed = 500f;
    private readonly float _gravity = 9.81f;
    void Start()
    {
        _controller = gameObject.GetComponent<CharacterController>();
        _cam = Camera.main;
        _animator = GetComponent<Animator>();
        _agent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _cam.transform.RotateAround(transform.position, Vector3.up, _lookInput.x * rotationSpeed * Time.deltaTime);
        _cam.transform.position = transform.position - _cam.transform.forward * 12;
        transform.rotation = Quaternion.Euler(0, _cam.transform.rotation.eulerAngles.y, 0);
    }

    void FixedUpdate()
    {
        // move the player horizontally
        Vector3 move = transform.right * _moveInput.x + transform.forward * _moveInput.y;
        move.y -= _gravity;
        _controller.Move(move * movementSpeed * Time.deltaTime);
        
        // _agent.velocity = _controller.velocity;
        chooseAnimation();
    }
    
    void OnMove(InputValue value)
    {
        _moveInput = value.Get<Vector2>();
    }
    void OnLook(InputValue value)
    {
        _lookInput = value.Get<Vector2>();
    }
    private void chooseAnimation()
    {
        if (_moveInput != Vector2.zero)
        {
            if (_moveInput.y < 0)
            {
                _animator.SetFloat("state", 1);
            }
            else
            {
                _animator.SetFloat("state", 0.5f);
            }
        }
        else
        {
            _animator.SetFloat("state", 0);
        }
    }
}