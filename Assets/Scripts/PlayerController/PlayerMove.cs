using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    CharacterController _controller;
    [SerializeField] float _speed = 6;

    Vector3 _direction, _move;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
    }

    public void Move()
    {
        float x = Input.GetAxis("Horizontal");
        float z = Input.GetAxis("Vertical");

        _direction = transform.right * x + transform.forward * z;
        _move = _direction * _speed * Time.deltaTime;

        _controller.Move(_move);
    }
}