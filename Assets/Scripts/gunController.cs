using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class gunController : MonoBehaviour
{
    private int currentBullets;
    public int maxBullets;
    private int currentBulletsMag;
    public int maxBulletsMag;

    public Animator armsAnim;
    private Animator gunAnim;
    public TMPro.TMP_Text ammoTotalUI;
    public TMPro.TMP_Text ammoMagUI;


    void Start () {
        gunAnim = gameObject.GetComponent<Animator>();
        currentBullets = maxBullets;
        currentBulletsMag = maxBulletsMag;
    }

    void Update () {
        if (Input.GetKeyDown(KeyCode.Mouse0) && currentBulletsMag > 0 && isAnimPlaying() == false) {
            gunFire();
        }
        if (Input.GetKeyDown(KeyCode.R) && (currentBullets - currentBulletsMag) > 0 && isAnimPlaying() == false) {
            gunReload();
        }
        ammoTotalUI.text = (currentBullets-currentBulletsMag).ToString();
        ammoMagUI.text = currentBulletsMag.ToString();
    }
    
    void gunFire () {
        gunAnim.SetTrigger("isFiring");
        armsAnim.SetTrigger("isFiring");
        currentBulletsMag -= 1;
        currentBullets -= 1;
    }


    void gunReload () {
        gunAnim.SetTrigger("isReloading");
        armsAnim.SetTrigger("isReloading");
        if (currentBullets > maxBulletsMag) {
            currentBulletsMag = maxBulletsMag;
        } else {
            currentBulletsMag = currentBullets;
        }
    } 
    bool isAnimPlaying () {
        if (armsAnim.GetCurrentAnimatorStateInfo(0).IsName("armsFire") || armsAnim.GetCurrentAnimatorStateInfo(0).IsName("armsReload")) {
            return true;
        } else {
            return false;
        }
    }
}
