using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Light_Cannon : MonoBehaviour
{
    public TextMeshProUGUI tool_tip;
    public GameObject cannonball_prefab; // Reference to the cannonball prefab
    public Transform fire_point; // Reference to the firing point
    public float fire_force = 500f; // The force with which to fire the cannonball
    public float angle = 90f; // The angle at which to fire the cannonball
    private bool can_fire = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player" )
        {
            can_fire = true;
            tool_tip.text = "Press X to light the cannon fuse";
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            can_fire = false;
            tool_tip.text = "";
        }
    }

    private void LightCannon()
    { 
        // Instantiate the cannonball at the fire point
        GameObject cannonball = Instantiate(cannonball_prefab, fire_point.position, fire_point.rotation);

        // Get the Rigidbody component of the cannonball
        Rigidbody rb = cannonball.GetComponent<Rigidbody>();

        // Calculate the direction based on the angle
        Vector3 fireDirection = Quaternion.AngleAxis(angle, -transform.right) * transform.forward;

        // Apply the force to the cannonball
        rb.AddForce(fireDirection * fire_force);

        // Play Cannon Fire Sound
        Sound_Manager.instance.ShootCannonSound();

    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && can_fire) // Change to your desired input
        {
            LightCannon();
        }
    }
}
