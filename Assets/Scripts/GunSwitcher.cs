using System.Collections;
using System.Collections.Generic;
using System.Net;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunSwitcher : MonoBehaviour
{
    public int SelectedWeapon = 0;
    public Text WeaponText;
    private GunScript gunScript;

    public void Start()
    {
        SelectWeapon();
        gunScript = FindObjectOfType<GunScript>();
    }

    void Update()
    {
        int previousallySelectedWeapon = SelectedWeapon;
        
        // Allows you to scroll through weapons
        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
        {
            if (SelectedWeapon >= transform.childCount - 1) SelectedWeapon = 0;
            else SelectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f)
        {
            if (SelectedWeapon <= 0) SelectedWeapon = transform.childCount - 1;
            else SelectedWeapon--;
        }

        // Will only try to switch weapons if something changes
        if (previousallySelectedWeapon != SelectedWeapon) SelectWeapon();
        WeaponText.text = "Weapon: " + transform.GetChild(SelectedWeapon).name;
    }

    // Switches out weapons depending on which ones are selected
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform Weapons in transform)
        {
            if (i == SelectedWeapon) Weapons.gameObject.SetActive(true);
            else Weapons.gameObject.SetActive(false);
            i++;
            gunScript.UpdateText();
        }         
    }
}
