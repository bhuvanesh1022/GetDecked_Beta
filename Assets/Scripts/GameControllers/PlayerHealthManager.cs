using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;

public class PlayerHealthManager : MonoBehaviourPun, IPunObservable
{
    public PhotonView pv;
    public float currentHealth;
    public float maxHealth;
    public bool updateHealth;
    public Image healthBar;

    public void Update()
    {
        if (updateHealth)
        {
            healthBar.fillAmount = currentHealth / maxHealth;
            updateHealth = false;
        }
    }

    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        if (stream.IsWriting)
        {
            stream.SendNext(currentHealth);
            stream.SendNext(maxHealth);
        }
        if (stream.IsReading)
        {
            currentHealth = (float)stream.ReceiveNext();
            maxHealth = (float)stream.ReceiveNext();
        }
    }
}
