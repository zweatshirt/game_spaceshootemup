using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour { 

    static public Hero S; // Singleton

    [Header("Set in Inspector")]
    public float gameRestartDelay = 4f;
    public GameObject projectilePrefab; 
    public float projectileSpeed = 40;
    // These fields control the movement of the ship
    public float speed = 30;
    public float rollMult = -45;
    public float pitchMult = 30;

    [Header("Set Dynamically")]
    [SerializeField] private float _shieldLevel = 1;
    // public float shieldLevel = 1;
    private GameObject lastTriggerGo = null;
    
    public delegate void WeaponFireDelegate(); 
    public WeaponFireDelegate fireDelegate;


    void Awake() {
        if (S == null) {
            S = this; // Set the Singleton
        } else {
            Debug.LogError("Hero.Awake() - Attempted to assign second Hero.S!"); 
        }
        fireDelegate += TempFire;    
    }

    void Update () {
    // Pull in information from the Input class 
        float xAxis = Input.GetAxis("Horizontal");
        float yAxis = Input.GetAxis("Vertical");

        // Change transform.position based on the axes
        Vector3 pos = transform.position;
        pos.x += xAxis * speed * Time.deltaTime;
        pos.y += yAxis * speed * Time.deltaTime;
        transform.position = pos;

        // Rotate the ship to make it feel more dynamic
        transform.rotation = 
            Quaternion.Euler(yAxis * pitchMult, xAxis * rollMult, 0); 
       
        if (Input.GetAxis("Jump") == 1 && fireDelegate != null) { 
            fireDelegate(); 
        }

    }

    void TempFire() {
        GameObject projGO = Instantiate<GameObject>(projectilePrefab);
        projGO.transform.position = transform.position;
        Rigidbody rigidB = projGO.GetComponent<Rigidbody>(); 
        // rigidB.velocity = Vector3.up * projectileSpeed;
        Projectile proj = projGO.GetComponent<Projectile>();
        proj.type = WeaponType.blaster;
        float tSpeed = Main.GetWeaponDefinition(proj.type).velocity; 
        rigidB.velocity = Vector3.up * tSpeed;
    }
    


    void OnTriggerEnter(Collider other) { 
        Transform rootT = other.gameObject.transform.root;
        GameObject go = rootT.gameObject;
        print("Triggered: "+ go.gameObject.name); 

        if (go == lastTriggerGo) {
            return;
        }
        lastTriggerGo = go;

        if (go.tag == "Enemy") {
            shieldLevel--;
            Destroy(go);
        } 
        else {
            print("Triggered by non-Enemy" + go.name);
        }
    }

    public float shieldLevel { 
        get {
            return(_shieldLevel); 
            }

        set {
            _shieldLevel = Mathf.Min(value, 4);
            // If the shield is going to be set to less than zero 
            if (value < 0) { // c
                Destroy(this.gameObject); }
                Main.S.DelayedRestart(gameRestartDelay);
            }
    }

}
