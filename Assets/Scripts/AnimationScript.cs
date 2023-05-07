using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationScript : MonoBehaviour
{
    public Animator animationSource;
    private GunScript gunScript;
    // Start is called before the first frame update

    public void Reload()
    {
        animationSource.SetTrigger("trReload");
    }

    public void Shoot()
    {
        animationSource.SetTrigger("trShoot");
    }

}
