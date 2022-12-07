using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MoveAndShot : MonoBehaviour
{
    [SerializeField]
    private CharacterController _controller;
    [SerializeField]
    private float _speed_move = 2;
    [SerializeField]
    private float _speed_run = 6;
    [SerializeField]
    private float _speed = 0;
    [SerializeField]
    private float _smoothSpeed = 3;
    [SerializeField]
    private float _gravity = -9.8f;
    [SerializeField]
    private Vector3 move;
    [SerializeField]
    private GameObject _bullet;
    [SerializeField]
    private GameObject _fire;
    [SerializeField]
    private GameObject _grenade;
    [SerializeField]
    private GameObject _hand;

    private Vector3 velocity;
    private GameObject _grenadeOnScene;

    private void Start()
    {
        _controller = GetComponent<CharacterController>();
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void Update()
    {
        Gravity();
        Run();
        Movebl();
        Shot();
        Grenade();

    }

    private void CharacterControllerHeight(float value)
    {
        _controller.height = value;
    }

    /*private void CharacterControllerIsActive(bool status)
    {
        _controller.enabled = status;
    }*/

    private void Movebl()
    {
        float X = Input.GetAxis("Horizontal");
        float Z = Input.GetAxis("Vertical");

        if ((X != 0) || (Z != 0))
        {
            move = transform.right * X + transform.forward * Z;
            _controller.Move(_speed * Time.deltaTime * move);
        }
    }

    private void Gravity()
    {
        if (_controller.isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }
        else
        {
            velocity.y += _gravity * Time.deltaTime;
            _controller.Move(velocity * Time.deltaTime);
        }

    }

    private void JumpAndSitDown()
    {
        float JumpHeight = 1;

        if (Input.GetButtonDown("Jump") && _controller.isGrounded)
        {
            velocity.y = Mathf.Sqrt(JumpHeight * -2f * _gravity);
        }

        if (Input.GetKey(KeyCode.C))
        {
            CharacterControllerHeight(1.4f);
        }
        else
        {
            CharacterControllerHeight(3.6f);
        }
    }

    private void Shot()
    {
        if (Input.GetMouseButtonDown(0))
            Instantiate(_bullet, _fire.transform.position, _fire.transform.rotation);
    }

    private void Grenade()
    {
        if (Input.GetMouseButtonDown(1))
        {
            _hand.SetActive(true);
        }
        if (Input.GetMouseButtonUp(1))
        {
            _hand.SetActive(false);
            var _grenadeOnScene = Instantiate(_grenade, _hand.transform.position, transform.rotation);
            _grenadeOnScene.GetComponent<Rigidbody>().AddForce(transform.forward * 500f);
            _grenadeOnScene.GetComponent<Explousion>().ExplodeAfterWait(5f);
        }
    }

    private void Run()
    {
        if (Input.GetKey(KeyCode.LeftShift))
        {
            _speed = Mathf.Lerp(_speed, _speed_run, Time.deltaTime * _smoothSpeed);
        }
        else
        {
            _speed = Mathf.Lerp(_speed, _speed_move, Time.deltaTime * _smoothSpeed);
        }
    }
}
