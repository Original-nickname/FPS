using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowBowScript : MonoBehaviour
{
    private Rigidbody myBody;
    public float speed = 30f;
    public float deactivateTimer = 3f;
    public float damage = 15f;

    void Awake()
    {
        myBody = GetComponent<Rigidbody>();
    }

    void DeactivateGameObject()
    {
        if (gameObject.activeInHierarchy)
        {
            gameObject.SetActive(false);
        }
    }

    private void Start()
    {
        Invoke("DeactivateGameObject", deactivateTimer);
    }

    public void Launch(Camera mainCamera)
    {
        myBody.velocity = mainCamera.transform.forward * speed;
        transform.LookAt(transform.position + myBody.velocity);
    }

    private void OnTriggerEnter(Collider target)
    {
        // after touch enemy deactivate this object
        if (target.tag == "Enemy")
        {
            target.GetComponent<HealthScript>().ApplyDamage(damage);
            gameObject.SetActive(false);
        }
    }
}
