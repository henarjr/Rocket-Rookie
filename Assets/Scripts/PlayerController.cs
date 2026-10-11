using UnityEngine;
using UnityEngine.InputSystem;  
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public InputAction moveForward;
    public InputAction lookPosition;

    private float elapsedTime = 0f;
    private float score = 0f;
    public float scoreMultiplier = 10f;
    public float thrustForce = 1f;
    public float maxSpeed = 5f;

    private float highScore = 0f;
    private Label scoreText;
    private Label highScoreText;
    private Label gameOverText;
    public GameObject boosterFlame;
    public GameObject explosionEffect;
    public GameObject borderParent;
    public UIDocument uiDocument;
    private Button restartButton;

    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        moveForward.Enable();
        lookPosition.Enable();

        highScore = PlayerPrefs.GetFloat("HighScore", 0f);
        scoreText = uiDocument.rootVisualElement.Q<Label>("ScoreLabel");

        highScoreText = uiDocument.rootVisualElement.Q<Label>("HighScoreLabel");
        highScoreText.style.display = DisplayStyle.None;

        gameOverText = uiDocument.rootVisualElement.Q<Label>("GameOverLabel");
        gameOverText.style.display = DisplayStyle.None;

        restartButton = uiDocument.rootVisualElement.Q<Button>("RestartButton");
        restartButton.style.display = DisplayStyle.None;
        restartButton.clicked += ReloadScene;

    }

    void Update()
    {
        UpdateScore();
        MovePlayer();
    }

    void UpdateScore()
    {
        elapsedTime += Time.deltaTime;
        score = Mathf.FloorToInt(elapsedTime * scoreMultiplier);
        scoreText.text = "Score: " + score;
    }
    void MovePlayer()
    {   

        if (moveForward.IsPressed())
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(lookPosition.ReadValue<Vector2>());


            Vector2 direction = (mousePos - transform.position).normalized;
            transform.up = direction;

            rb.AddForce(direction * thrustForce);
            if (rb.linearVelocity.magnitude > maxSpeed)
            {
                rb.linearVelocity = rb.linearVelocity.normalized * maxSpeed;
            }
        }
        if (moveForward.WasPressedThisFrame())
        {
            boosterFlame.SetActive(true);
        }
        else if (moveForward.WasReleasedThisFrame())
        {
            boosterFlame.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetFloat("HighScore", highScore);
            PlayerPrefs.Save();
        }
        highScoreText.text = "High Score: " + highScore;

        gameOverText.style.display = DisplayStyle.Flex;
        highScoreText.style.display = DisplayStyle.Flex;

        Destroy(gameObject);
        Instantiate(explosionEffect, transform.position, transform.rotation);
        borderParent.SetActive(false);
        restartButton.style.display = DisplayStyle.Flex;
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
