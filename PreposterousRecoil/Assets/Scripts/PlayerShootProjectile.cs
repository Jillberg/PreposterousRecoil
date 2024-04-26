using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShootProjectile : MonoBehaviour
{
    [SerializeField] private Transform BulletPrefab;
    [SerializeField] private Transform ShootingEffectPrefab;
    [SerializeField] private GameObject RotatePoint;
    [SerializeField] private GameObject Player;

    private Sprite bulletToRender;
    private Sprite effectToRender;
    private Vector2 cursorHotspot;

    [Header("Defalut Bullet")]
    public Sprite defaultAmmo;
    public Sprite defaultAmmoEffect;
    public Texture2D defaultCursor;

    [Header("Air Fryer")]
    public Sprite airAmmo;
    public Sprite airAmmoEffect;
    public Texture2D airCursor;

    [Header("Fire Blaster")]
    public Sprite fireAmmo;
    public Sprite fireAmmoEffect;
    public Texture2D fireCursor;


    [Header("Elf Legacy")]
    public Sprite magicAmmo;
    public Sprite magicAmmoEffect;
    public Texture2D magicCursor;

    void Start()
    {
        bulletToRender = defaultAmmo; 
        effectToRender=defaultAmmoEffect;
        cursorHotspot=new Vector2(defaultCursor.width/2,defaultCursor.height/2);
    }
    private void Awake()
    {
        RotatePoint.GetComponent<Aiming>().OnShoot+= PlayerShootProjectile_OnShoot;
        Player.GetComponent<PlayerController>().OnAmmoChange += HandleAmmoChange;
    }

    private void HandleAmmoChange(object sender, PlayerController.OnAmmoChangeEventArgs e)
    {

        if (e.ammoType == 1)
        {

            bulletToRender = null;
            effectToRender =airAmmoEffect;
            Cursor.SetCursor(airCursor, cursorHotspot, CursorMode.Auto);

            Debug.Log("bullet1");
        }
        else if (e.ammoType == 2)
        {
            bulletToRender = fireAmmo;
            effectToRender =fireAmmoEffect;
            Cursor.SetCursor(fireCursor, cursorHotspot, CursorMode.Auto);
            Debug.Log("bullet2");
        }
        else if (e.ammoType == 3)
        {
            bulletToRender = magicAmmo;
            effectToRender =magicAmmoEffect;
            Cursor.SetCursor(magicCursor, cursorHotspot, CursorMode.Auto);
            Debug.Log(cursorHotspot);
        }
    }


    private void PlayerShootProjectile_OnShoot(object sender,Aiming.OnShootEventArgs e)
    {
        Transform bulletTransform=Instantiate(BulletPrefab, e.gunEndPointPosition, Quaternion.identity);
        Transform shootingEffectTransform = Instantiate(ShootingEffectPrefab, e.gunEndPointPosition, Quaternion.identity);
        Vector3 shootDirection = (e.shootPosition - e.gunEndPointPosition).normalized;
        

        SpriteRenderer bulletSpriteRenderer = bulletTransform.Find("BulletSprite").GetComponent<SpriteRenderer>();
        if (bulletSpriteRenderer != null)
        {
            bulletSpriteRenderer.sprite = bulletToRender;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the bullet child object!");


        }
        if (bulletSpriteRenderer.sprite != null)
        {
            bulletTransform.GetComponent<Bullet>().Setup(shootDirection);
        }
        else
        {
            Destroy(bulletTransform.gameObject, 0.5  f);
        }
        shootingEffectTransform.GetComponent<ShootingEffect>().Setup(shootDirection);
        SpriteRenderer shootingEffectSpriteRenderer = shootingEffectTransform.Find("ShootingEffect").GetComponent<SpriteRenderer>();
        if (shootingEffectSpriteRenderer != null)
        {
            shootingEffectSpriteRenderer.sprite = effectToRender;
        }
        else
        {
            Debug.LogError("SpriteRenderer not found on the bullet child object!");


        }
        Debug.DrawLine(e.gunEndPointPosition,e.shootPosition);

    }
}
