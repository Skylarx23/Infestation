using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class GunSwitcher : MonoBehaviour
{
    public int SelectedWeapon = 0;
    private GunScript gunScript;
    private GameManager GM;

    public void Start()
    {
        SelectWeapon();
        gunScript = FindObjectOfType<GunScript>();
        GM = FindObjectOfType<GameManager>();

    }

    void Update()
    {
        int previousallySelectedWeapon = SelectedWeapon;
        
        // Allows you to scroll through weapons if you're not reloading
        if (Input.GetAxis("Mouse ScrollWheel") > 0f && !transform.GetChild(SelectedWeapon).GetComponent<GunScript>().Reloading)
        {
            if (SelectedWeapon >= transform.childCount - 1) SelectedWeapon = 0;
            else SelectedWeapon++;
        }

        if (Input.GetAxis("Mouse ScrollWheel") < 0f && !transform.GetChild(SelectedWeapon).GetComponent<GunScript>().Reloading)
        {
            if (SelectedWeapon <= 0) SelectedWeapon = transform.childCount - 1;
            else SelectedWeapon--;
        }

        GM.GunUI(this.gameObject);

        // Will only try to switch weapons if something changes
        if (previousallySelectedWeapon != SelectedWeapon) SelectWeapon();
    }

    // Switches out weapons depending on which ones are selected
    void SelectWeapon()
    {
        int i = 0;
        foreach (Transform Weapons in transform)
        {
            if (i == SelectedWeapon)
            {
                Weapons.gameObject.SetActive(true);
            }
            else Weapons.gameObject.SetActive(false);
            i++;
        }         
    }
}
