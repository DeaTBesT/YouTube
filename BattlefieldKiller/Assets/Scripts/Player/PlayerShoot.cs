using Photon.Pun;
using UnityEngine;

[RequireComponent(typeof(CameraController))]
[RequireComponent(typeof(AudioSource))]
public class PlayerShoot : MonoBehaviourPun
{
    [Header("Weapon")]
    [SerializeField] private WeaponSettings currentWeapon;

    [Space, SerializeField] private float damage;
    [SerializeField] private float fireRate;
    private float nextTime2Fire = 0;

    [Header("Sway")]
    [SerializeField] private Transform swayPivot; 

    [SerializeField] private float smooth;
    [SerializeField] private float swayMultiplier;

    private CameraController m_cameraController;

    private const string mouseAxisX = "Mouse X";
    private const string mouseAxisY = "Mouse Y";

    private void Start()
    {
        m_cameraController = GetComponent<CameraController>();
    }

    private void Update()
    {
        if (!photonView.IsMine) { return; }

        if ((Input.GetMouseButton(0)) && (Time.time >= nextTime2Fire))
        {
            nextTime2Fire = Time.time + 1f / fireRate;

            Shoot();
        }

        Sway();
    }

    private void Shoot()
    {
        Ray m_ray = new Ray(m_cameraController.M_Camera.transform.position, m_cameraController.M_Camera.transform.forward);
        RaycastHit m_hit;

        if (Physics.Raycast(m_ray, out m_hit, 100))
        {
            if (m_hit.transform.TryGetComponent(out PlayerStats m_playerStats))
            {
                m_playerStats.TakeDamage(damage, PhotonNetwork.LocalPlayer);
            }
        }

        m_cameraController.RecoilEffect();
        currentWeapon?.UseWeapon();
    }

    private void Sway()
    {
        float mouseX = Input.GetAxisRaw(mouseAxisX) * swayMultiplier;
        float mouseY = Input.GetAxisRaw(mouseAxisY) * swayMultiplier;

        Quaternion rotationX = Quaternion.AngleAxis(mouseY, Vector3.right);
        Quaternion rotationY = Quaternion.AngleAxis(mouseX, Vector3.up);

        Quaternion targetRotation = rotationX * rotationY;

        swayPivot.transform.localRotation = Quaternion.Slerp(swayPivot.transform.localRotation, targetRotation, smooth * Time.deltaTime);
    }
}
