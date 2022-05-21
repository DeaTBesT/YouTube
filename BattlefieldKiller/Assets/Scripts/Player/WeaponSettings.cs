using Photon.Pun;
using UnityEngine;

public class WeaponSettings : MonoBehaviourPun
{
    [SerializeField] private GameObject weaponLocal;
    [SerializeField] private GameObject weaponOnline;

    [Header("Effects")]
    [SerializeField] private ParticleSystem particleFireLocal;
    [SerializeField] private ParticleSystem particleFireOnline;

    [Space, SerializeField] private AudioClip sfxFire;

    private ParticleSystem currentParticle;

    [Header("Components")]
    [SerializeField] private AudioSource audioSource;

    private void Start()
    {
        if (photonView.IsMine) 
        { 
            weaponLocal.SetActive(true);
            weaponOnline.SetActive(false);

            currentParticle = particleFireLocal;
        }
        else 
        {
            weaponLocal.SetActive(false);
            weaponOnline.SetActive(true);

            currentParticle = particleFireOnline;
        }
    }

    public virtual void UseWeapon() 
    {
        photonView.RPC(nameof(UseWeaponRPC), RpcTarget.All);
    }

    [PunRPC]
    protected void UseWeaponRPC()
    {
        currentParticle.Play();
        audioSource.PlayOneShot(sfxFire);
    }
}
