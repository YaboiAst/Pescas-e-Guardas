using UnityEngine;

public class EventoPesca : MonoBehaviour
{
    public PlayerMovement movement;
    public GameObject CaixaTexto;

    private void OnCollisionEnter(Collision collisionInfo)
    {
        if (collisionInfo.collider.tag == "PontosDePesca")
        {
            movement.enabled = false;
            movement.rb.velocity = Vector3.zero;
            movement.rb.angularVelocity = Vector3.zero;
            CaixaTexto.SetActive(true);
        } 
    }
}
