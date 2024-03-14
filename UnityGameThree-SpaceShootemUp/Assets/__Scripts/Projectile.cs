using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private BoundChecker boundCheck;
    private Renderer rend;

    [Header("Set Dynamically")] 
    public Rigidbody rigid; 
    [SerializeField] 
    private WeaponType _type; 

    // This public property masks the field _type and takes action when it is set
    public WeaponType type {
        get {
            return( _type );
        }
        set {
            SetType(value);
        }
    }
    
    void Awake() {
        boundCheck = GetComponent<BoundChecker>(); 
        rend = GetComponent<Renderer>();
        rigid = GetComponent<Rigidbody>();
    }
    
    void Update () {
        if (boundCheck.offUp) {
            Destroy(gameObject);
        }
    }

    public void SetType(WeaponType eType) {
        // Set the _type
        _type = eType;
        WeaponDefinition def = Main.GetWeaponDefinition(_type);
        rend.material.color = def.projectileColor;
    } 
}

