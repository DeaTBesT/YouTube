using Photon.Pun;
using UnityEngine;

public class CameraController : MonoBehaviourPun
{
    [Header("Components")]
    [SerializeField] private Transform handsPivot;
    [SerializeField] private Camera m_camera;

    [Header("Settings")]
    [SerializeField] private float sensitivity;

    [Header("Recoil effect")]
    [SerializeField] private float recoilX = -2;
    [SerializeField] private float recoilY = 2;
    [SerializeField] private float recoilZ = 0.35f;

    [Space, SerializeField] private float snappiness;
    [SerializeField] private float returnSpeed;

    private Vector3 currentRotation;
    private Vector3 targetRotation;

    private float mouseX;
    private float mouseY;

    private const string mouseAxisX = "Mouse X";
    private const string mouseAxisY = "Mouse Y";

    public Camera M_Camera { get { return m_camera; } }

    private void Start()
    {
        if (photonView.IsMine) { return; }
        else { Destroy(m_camera.gameObject); }
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }

        RotateCamera();
    }

    private void RotateCamera()
    {
        mouseX += Input.GetAxis(mouseAxisX) * sensitivity;
        mouseY -= Input.GetAxis(mouseAxisY) * sensitivity;

        mouseY = Mathf.Clamp(mouseY, -85, 85);

        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, returnSpeed * Time.deltaTime);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, snappiness * Time.deltaTime);

        handsPivot.transform.localRotation = Quaternion.Euler(new Vector3(mouseY + currentRotation.x, currentRotation.y, currentRotation.z));
        transform.rotation = Quaternion.Euler(transform.rotation.x, mouseX, transform.rotation.z);
    }

    public void RecoilEffect()
    {
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }
}
