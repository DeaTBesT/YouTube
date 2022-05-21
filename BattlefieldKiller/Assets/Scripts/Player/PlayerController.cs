using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce;

    [SerializeField] private Transform groundCheck;
    [SerializeField] private float radius;
    [SerializeField] private LayerMask groundCheckLayer;

    private Rigidbody m_rigidbody;

    private const string horizontal = "Horizontal";
    private const string vertical = "Vertical";

    private void Start()
    {
        m_rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }

        Jump();
    }

    private void FixedUpdate()
    {
        if (!photonView.IsMine) { return; }

        Move();
    }

    private void Move()
    {
        float moveX = Input.GetAxis(horizontal);
        float moveY = Input.GetAxis(vertical);

        Vector3 move = transform.forward * moveY + transform.right * moveX;

        m_rigidbody.MovePosition(transform.position + move * speed * Time.deltaTime);
    }

    private void Jump()
    {
        if ((CheckGround()) && (Input.GetKeyDown(KeyCode.Space)))
        {
            m_rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
        }
    }

    private bool CheckGround() { return Physics.CheckSphere(groundCheck.position, radius, groundCheckLayer); }
}
