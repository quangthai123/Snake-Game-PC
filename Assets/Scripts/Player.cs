using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private float horizontalInput;
    private float verticalInput;
    public bool startGame = false;
    public float timePerStep;
    public int scoreToAdd = 5;
    [SerializeField] private Vector3 moveDir;
    [SerializeField] private Transform snakePartPrefab;
    public List<Transform> snakeParts;
    void Start()
    {
        transform.position = new Vector2(0f, -2f);
        moveDir = Vector2.zero;
        StartCoroutine(MoveToTheNextPosition());
        snakeParts = new List<Transform>();
        snakeParts.Add(transform);
    }
    void Update()
    {

        if (!GameManager.instance.pause)
            UpdateMoveDirByInput();     
        if(GameManager.instance.pause)
            StopAllCoroutines();
    }

    private void UpdateMoveDirByInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");
        if (horizontalInput != 0f)
        {
            startGame = true;
            if (moveDir.x == -horizontalInput && snakeParts.Count > 1)
                return;
            moveDir = new Vector2(horizontalInput, 0f);
        }
        if (verticalInput != 0f)
        {
            startGame = true;
            if (moveDir.y == -verticalInput && snakeParts.Count > 1)
                return;
            moveDir = new Vector2(0f, verticalInput);
        }
    }

    public IEnumerator MoveToTheNextPosition()
    { 
        yield return new WaitForSeconds(timePerStep);
        for (int i = snakeParts.Count-1; i > 0; i--)
        {
            snakeParts[i].position = snakeParts[i - 1].position;
        }
        transform.position += moveDir;
        StartCoroutine(MoveToTheNextPosition());
    }
    private void AddOnePartIntoSnakeParts()
    {
        Transform newPart = Instantiate(snakePartPrefab, snakeParts[snakeParts.Count-1].position, Quaternion.identity);
        snakeParts.Add(newPart);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Food")
        {
            AddOnePartIntoSnakeParts();
            Destroy(collision.gameObject);
            GameManager.instance.AddScore(scoreToAdd);
            GameManager.instance.SpawnFoodInARandomPos();
            GameAudio.instance.PlaySFX(0);
        }
        if ((collision.gameObject.tag == "Player" && snakeParts.Count > 2) || collision.gameObject.tag == "Wall")
        {
            transform.position -= moveDir;
            GameManager.instance.SetEndGame();
            GameAudio.instance.PlaySFX(1);
        }
    }
}
