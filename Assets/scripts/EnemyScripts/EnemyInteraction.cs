using System;
using System.Collections;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR;

public class PlatformInputHandler : MonoBehaviour
{
    // Define a delegate for handling input
    private delegate void InputHandler();
    private InputHandler handleInput;
    [SerializeField] private Animator cloudAnimator;
    [SerializeField] private Animator InEnemyAnimator;
    Animator animator;
    private HearthHealthManager EnemyHealth;
    private HearthHealthManager PlayerHealth;
    [SerializeField] private HealthSerializableConst EnemyhealthSerializableConst;
    [SerializeField] private HealthSerializableConst PlayerhealthSerializableConst;
    private float EnemyDamage;
    private float PlayerDamage;
    [SerializeField] private Sprite ImpactSprite;
    public bool EnemycanBeAttacked { get; private set; }
    public int enemyDazeHits = 1;
    private int hitedWhileDazed = 0;

    private int toDazedCounter = 1;
    private bool CanSwingSword;

    private void Awake()
    {
        // Assign the appropriate input handler at startup
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Running on Android. Using touch input.");
            handleInput = HandleTouchInput; // Assign touch input handler
        }
        else
        {
            Debug.Log("Not running on Android. Using mouse input.");
            handleInput = HandleMouseInput; // Assign mouse input handler
        }
        animator = GetComponent<Animator>();
        EnemyDamage = EnemyhealthSerializableConst.singleHit;
        PlayerDamage = PlayerhealthSerializableConst.singleHit;
        EnemyHealth = GameObject.Find("EnemyManagerBar").GetComponent<HearthHealthManager>();
        PlayerHealth = GameObject.Find("PlayerManagerBar").GetComponent<HearthHealthManager>();
        // PlayerHealth = GameObject.Find("PlayerHealthBar").GetComponent<HealthManager>();
        // EnemyHealth = GameObject.Find("EnemyHealthBar").GetComponent<HealthManager>();
    }

    public void leaveTheScene() {
        if (isDefeated()) 
        {
            cloudAnimator.SetTrigger("leave");
            animator.SetTrigger("defeated");
        } 
    }
    private void getDazed() {
        animator.SetBool("Dazed", true);
    }

    void Start()
    {
        HandleDaze.instance.onDaze += getDazed;
    } 

    private void Update()
    {
        // Call the assigned input handler
        handleInput?.Invoke();

        CanSwingSword = animator.GetCurrentAnimatorStateInfo(0).IsName("idle") || 
                        animator.GetCurrentAnimatorStateInfo(0).IsName("dazed") ||
                        animator.GetCurrentAnimatorStateInfo(0).IsName("EntryDazed");
        
        if (!isDefeated()) 
        {
            // Trigger attack
            if (!HandleDaze.instance.isDazed) {
                if (Time.time % UnityEngine.Random.Range(4f, 5f) < Time.deltaTime)
                {
                    StartAttack();
                }
            }

            if (Time.time % 4f < Time.deltaTime)
            {
                EnemyHealth.Heal();
            }
        }
    }

    // Start an attack on the enemy
    private void StartAttack()
    {
        if (EnemyHealth != null && PlayerHealth != null)
        {
            animator.SetTrigger("attack");
        }
    }

    public void Dazed()
    {
        Debug.Log("Dazed");
        if (EnemyHealth != null && PlayerHealth != null)
        {
            hitedWhileDazed = 0;
            StartCoroutine(StopDazed());
        }
    }

    private IEnumerator StopDazed()
    {
        float timer = 5f;
        while (timer > 0f)
        {
            if (hitedWhileDazed >= enemyDazeHits) // Check if enemy has been hit already
            {
                break;
            }
            timer -= Time.deltaTime;
            yield return null;
        }
        // Reset daze state
        EnemycanBeAttacked = false;
        animator.SetBool("Dazed", false);
        HandleDaze.instance.stopDaze();
        hitedWhileDazed = 0;
    }

    void Attack() {
        // PlayerHealth.TakeDamage(UnityEngine.Random.Range(PlayerDamage, 41));
        PlayerHealth.TakeDamage(EnemyDamage);
    }

    private void Restart()
    {
        if (PlayerHealth.currentHealth <= 0) {
            SceneManager.LoadScene("AR test");     
        }
    }

    // Handle mouse input (for PC)
    private void HandleMouseInput()
    {
        if (Input.GetMouseButtonDown(0)) // Left mouse button
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if this object was clicked
                {
                    OnInteraction();
                }
            }
        }
    }

    // Handle touch input (for Android)
    private void HandleTouchInput()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                if (hit.collider.gameObject == gameObject) // Check if this object was touched
                {
                    OnInteraction();
                }
            }
        }
    }

    // Common interaction logic
    private void OnInteraction()
    {
        if (animator != null)
        {
            if (!isDefeated()) {
                if (BattleManager.Instance.sword.activeSelf && EnemycanBeAttacked == true)
                {
                    // Start the coroutine to show the ImpactSprite for one frame
                    StartCoroutine(ShowImpactSpriteFrame());
                    BattleManager.Instance.sword.GetComponent<ObjectToggle>().ShowHide();
                    
                    // Continue with the rest of your logic
                    EnemyHealth.TakeDamage(PlayerDamage);
                    hitedWhileDazed++;
                }
                else 
                {
                    if (CanSwingSword)
                    {
                        InEnemyAnimator.Play("defenceIn");
                        BattleManager.Instance.sword.GetComponent<ObjectToggle>().ShowHide();
                    }
                }
            }
        }
        else
        {
            Debug.LogError("Animator not found!");
        }
    }

    private IEnumerator ShowImpactSpriteFrame()
    {
        if (ImpactSprite != null)
        {
            // Get the SpriteRenderer and Animator components
            SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
            Animator anim = GetComponent<Animator>();

            // Save the current sprite
            Sprite originalSprite = spriteRenderer.sprite;

            // Temporarily disable the Animator so it doesn't override the sprite change
            if (anim != null) { anim.enabled = false; }

            // Change to the ImpactSprite
            spriteRenderer.sprite = ImpactSprite;

            // Wait for the desired time (see below for timing)
            yield return new WaitForSeconds(0.2f);

            // Revert back to the original sprite
            spriteRenderer.sprite = originalSprite;

            // Re-enable the Animator
            if (anim != null) { anim.enabled = true; }
        }
    }


    public void OnDefeatedAnimationComplete()
    {
        SceneManager.LoadScene("EPILOGUE"); // Replace with your next scene name
    }

    bool isDefeated() {
        return EnemyHealth.currentHealth <= 0;
    }

    void canBeAttacked() {
        EnemycanBeAttacked = true;
    }

}
