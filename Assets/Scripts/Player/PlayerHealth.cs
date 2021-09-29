using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public int currentHealth;
    public Slider healthSlider;
    public Image damageImage;
    public AudioClip deathClip;
    public float flashSpeed = 5f;
    public Color flashColour = new Color(1f, 0f, 0f, 0.1f);

    Animator anim;
    AudioSource playerAudio;
    PlayerMovement playerMovement;
    PlayerShooting playerShooting;
    bool isDead;
    bool damaged;

    private void Awake()
    {
        // Mendapatkan reference komponen
        anim = GetComponent<Animator>();
        playerAudio = GetComponent<AudioSource>();
        playerMovement = GetComponent<PlayerMovement>();

        playerShooting = GetComponentInChildren<PlayerShooting>();
        currentHealth = startingHealth;
    }

    private void Update()
    {
        // Jika terkena damage
        if (damaged)
        {
            // Merubah warna gambar menjadi value dari FlashColour
            damageImage.color = flashColour;
        }

        else
        {
            // Fade out damage image
            damageImage.color = Color.Lerp(damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
        }

        //Set daage to false
        damaged = false;
    }

    // fungsi untuk menambahkan damage

    public void TakeDamage(int amount)
    {
        damaged = true;

        // Mengurangi health
        currentHealth -= amount;

        // Merubah tampilan dari health slider
        healthSlider.value = currentHealth;

        // Memainkan suara ketika terkena damage
        playerAudio.Play();

        // Memanggil method Death() jika darahnya <= 10 & belum mati
        if(currentHealth <= 0 && !isDead)
        {
            Death();
        }
    }

    void Death()
    {
        isDead = true;

        playerShooting.DisableEffects();

        // Mentrigger animasi die
        anim.SetTrigger("Die");

        // Memainkan suara ketika mati
        playerAudio.clip = deathClip;
        playerAudio.Play();

        // Mematikan script player movement
        playerMovement.enabled = false;
    }

    public void RestartLevel()
    {
        // Meload ulang scene dengan index 0 pada build setting
        SceneManager.LoadScene(0);
    }
}
