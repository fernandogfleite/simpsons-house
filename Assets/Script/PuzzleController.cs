using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class PuzzleController : MonoBehaviour
{

    public const float ROUND_DURATION = 60f; 
    private const float NEAR_DISTANCE_THRESHOLD = 5f;
    private const float FAR_DISTANCE_THRESHOLD = 15f;

    public GameObject[] puzzlePieces;
    public GameObject currentPuzzlePiece;
    public GameObject finishPanel;
    public GameObject gameInfo;
    private List<GameObject> remainingPieces;

    public GameObject player;

    public Transform[] positions;

    public TMP_Text timerText;
    public TMP_Text clueText;
    public TMP_Text goalText;
    public TMP_Text scoreText;

    private int score = 0;
    private float distance = 0f;

    private float timeRemaining = ROUND_DURATION;
    private float totalTime = 0f;
    private bool isTimerRunning = false;

    public bool gameFinished = false;
    

    void Start() 
    {
        finishPanel.SetActive(false);
        Debug.Log("Start");
        remainingPieces = new List<GameObject>(puzzlePieces);
        StartNewRound();
    }

    void Update()
    {
        if (isTimerRunning)
        {
            if (timeRemaining > 0)
            {
                timeRemaining -= Time.deltaTime;
                UpdateTimerText();
                
                UpdateDistance();
                UpdateDistanceText();

                if (distance <= NEAR_DISTANCE_THRESHOLD && Input.GetKeyDown(KeyCode.E))
                {
                    // The player found the puzzle piece
                   SuccessfulRound();
                   totalTime += ROUND_DURATION - timeRemaining;
                }
            }
            else
            {
                timeRemaining = 0;
                totalTime += ROUND_DURATION;
                isTimerRunning = false;
                // Handle the end of the round (e.g., player failed to find the piece)
                FailedRound();
            }
        }

    }

    void StartNewRound()
    {
        if (remainingPieces.Count > 0)
        {
            SelectRandomPuzzlePiece();
            timeRemaining = ROUND_DURATION;
            isTimerRunning = true;
        }
        else
        {
            Debug.Log("You found all the puzzle pieces!");
            Debug.Log("RemainingPieces " + remainingPieces.ToString() + " " +  Mathf.Round(remainingPieces.Count).ToString());
            Debug.Log("No more puzzle pieces left.");

           FinishGame(true);
        }
    }

    void SelectRandomPuzzlePiece()
    {
        // Spawn the current puzzle piece at a random position
        int randomPositionIndex = Random.Range(0, positions.Length);
        Debug.Log("Random position index: " + randomPositionIndex);

        int randomIndex = Random.Range(0, remainingPieces.Count);
        currentPuzzlePiece = Instantiate(remainingPieces[randomIndex], positions[randomPositionIndex].position, Quaternion.identity);
       
        Debug.Log("New puzzle piece selected: " + currentPuzzlePiece.name);

        // Update the goal text
        goalText.text = "Find the " + remainingPieces[randomIndex].name + "!";

        remainingPieces.RemoveAt(randomIndex);
        
    }

    void UpdateTimerText()
    {
        
        timerText.text = Mathf.Round(timeRemaining).ToString() + "s";
    }

    void UpdateDistance()
    {
        distance = CalculateDistanceToPuzzlePiece();
    }

    void UpdateDistanceText(){
        if (float.IsInfinity(distance))
        {
            clueText.text = "";
            return;
        }
      

        if (distance < NEAR_DISTANCE_THRESHOLD)
        {
           
            clueText.text = "<color=#00FF00>You found the piece!</color>";
        }
        else if (distance < FAR_DISTANCE_THRESHOLD)
        {
        
            clueText.text = "You're getting closer!";
        }
        else
        {
            
            clueText.text = "<color=#ff0000>You are far from the puzzle piece!</color>";
        }
    } 

    void FailedRound() {
        Debug.Log("Time's up! You failed to find the piece.");

        Destroy(currentPuzzlePiece);

        clueText.color = Color.red;
        clueText.text = "Time's up! You failed to find the piece.";

        goalText.text = "";
        timerText.text = "";

       FinishGame(false);
    }

    void SuccessfulRound() {
        OnPuzzlePieceFound();
        StartNewRound();
    }

    // Call this method when the player finds the current puzzle piece
    public void OnPuzzlePieceFound()
    {
        isTimerRunning = false;
        Debug.Log("You found the piece!");

        score += 1;
        scoreText.text = score.ToString();
        // Destroy the current puzzle piece
        Destroy(currentPuzzlePiece);
    }

    float CalculateDistanceToPuzzlePiece()
    {

        if (currentPuzzlePiece == null)
        {
            return Mathf.Infinity;
        }

        Vector3 playerPosition = player.transform.position;
        Vector3 puzzlePiecePosition = currentPuzzlePiece.transform.position;

        // Calculate the distance
        float distance = Vector3.Distance(playerPosition, puzzlePiecePosition);

        return distance;
    }

    void FinishGame(bool success)
    {
        if (success)
        {
            Debug.Log("You found all the puzzle pieces!");
            // goalText.text = "You found all the puzzle pieces!";
        }
        else
        {
            
            Debug.Log("You failed to find all the puzzle pieces.");
            // goalText.text = "You failed to find all the puzzle pieces.";
        }

        isTimerRunning = false;

        Destroy(gameInfo);
        // clueText.text = "";
        // timerText.text = "";
        // scoreText.text = "";

        gameFinished = true;

        finishPanel.SetActive(true);
        TMP_Text[] childTexts = finishPanel.GetComponentsInChildren<TMP_Text>();



        childTexts[1].text = success ? "You found all the puzzle pieces!" : "You failed to find all the puzzle pieces.";
        childTexts[3].text = Mathf.Round(ROUND_DURATION).ToString() + "s";
        childTexts[5].text = score.ToString();
    }
    

}
