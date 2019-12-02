using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    public GameObject catPrefab;
    public int maxCatsOnMap;
    public int baseCatCount;
    public int baseCatHealth;
    public int catCountMultiplier;
    public int catHealthMultiplier;
    public int round = 1;
    public int transitionDuration;
    [HideInInspector] public float catMaxHealth;
    public int spawnDelayDuration;
    public int playerFullBalloonStartCount;
    public int playerEmptyBalloonStartCount;
    public int effectDelayDuration;
    public int pointsOnHit;
    public int pointsOnDeath;
    public int pointsOnRebuild;
    public int points = 0;
    public List<GameObject> catSpawns;
    public int balloonPackCost;
    public int pickUpBalloonAmount;
    public int snackCost;
    public int snackAmount;
    public float playerMaxHealth;

    private bool transitioning = false;
    private int remainingTransitionTime;
    private int roundCatCount = 0;
    private int spawnedCats = 0;
    private int spawnDelay = 0;
    private Text roundNumber;
    private Text pointsText;
    private GameObject pointsAddEffect;
    private GameObject pointsSubtractEffect;
    private int pointsAddEffectDelay = 0;
    private int pointsSubtractEffectDelay = 0;

    public GameObject pauseMenu;
    public GameObject gameOverMenu;


    // Start is called before the first frame update
    void Start()
    {
        roundNumber = GameObject.FindGameObjectWithTag("roundNumber").GetComponent<UnityEngine.UI.Text>();
        pointsText = GameObject.FindGameObjectWithTag("pointsText").GetComponent<UnityEngine.UI.Text>();
        pointsAddEffect = GameObject.FindGameObjectWithTag("pointsAddEffect");
        pointsSubtractEffect = GameObject.FindGameObjectWithTag("pointsSubtractEffect");


        pointsAddEffect.SetActive(false);
        pointsSubtractEffect.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        bool inPlay = StillInPlay();

        if (!inPlay && !transitioning) {
            Transition();
        }

        if (transitioning && remainingTransitionTime > 0) {
            remainingTransitionTime--;
        }
        else if (transitioning && remainingTransitionTime == 0) {
            BeginRound();
        }

        if (!transitioning && 
            spawnDelay == 0 && 
            inPlay &&
            GameObject.FindGameObjectsWithTag("cat").Length < maxCatsOnMap &&
            spawnedCats < roundCatCount) {
            SpawnCat();
        }
        else if (!transitioning && 
            spawnDelay > 0 && 
            inPlay) {
            spawnDelay--;
        }

        if (pointsAddEffectDelay > 0) {
            pointsAddEffectDelay--;
        }
        if (pointsAddEffectDelay == 0) {
            pointsAddEffect.SetActive(false);
        }

        if (pointsSubtractEffectDelay > 0) {
            pointsSubtractEffectDelay--;
        }
        if (pointsSubtractEffectDelay == 0) {
            pointsSubtractEffect.SetActive(false);
        }

        pointsText.text = points.ToString();
    }

    void SpawnCat() {
        spawnDelay = spawnDelayDuration;

        int tmp = Random.Range(0,catSpawns.Count);
        GameObject newCat = Instantiate(catPrefab,
            catSpawns[tmp].transform.position,
            Quaternion.identity
        );
        newCat.GetComponent<catNavigation>().spawn = catSpawns[tmp];

        spawnedCats++;
    }

    void Transition() {
        transitioning = true;

        remainingTransitionTime = transitionDuration;

        round++;
        roundCatCount = baseCatCount + (round * catCountMultiplier);
        catMaxHealth = baseCatHealth + (round * catHealthMultiplier);

        roundNumber.GetComponent<Animator>().Play("fade");

        spawnedCats = 0;
    }

    void BeginRound() {
        transitioning = false;

        roundNumber.text = round.ToString();
        roundNumber.GetComponent<Animator>().Play("fadeIn");
    }

    bool StillInPlay() {
        if (spawnedCats == roundCatCount &&
            GameObject.FindGameObjectsWithTag("cat").Length == 0) {
            return false;
        }
        else {
            return true;
        }
    }

    public void AddPoints(int amount) {
        points += amount;

        if (pointsAddEffectDelay > 0) {
            pointsAddEffect.SetActive(false);
        }
        pointsAddEffect.SetActive(true);
        pointsAddEffect.GetComponent<Animator>().Play("pointsAddEffect");
        pointsAddEffect.GetComponent<UnityEngine.UI.Text>().text = "+" + amount;

        pointsAddEffectDelay = effectDelayDuration;
    }

    public void SubtractPoints(int amount) {
        points -= amount;

        if (pointsSubtractEffectDelay > 0) {
            pointsSubtractEffect.SetActive(false);
        }
        pointsSubtractEffect.SetActive(true);
        pointsSubtractEffect.GetComponent<Animator>().Play("pointsSubtractEffect");
        pointsSubtractEffect.GetComponent<UnityEngine.UI.Text>().text = "-" + amount;

        pointsSubtractEffectDelay = effectDelayDuration;
    }

    public void AddSpawns(GameObject[] newSpawns) {
        for (int x = 0; x < newSpawns.Length; x++) {
            if (catSpawns.IndexOf(newSpawns[x]) == -1) {
                catSpawns.Add(newSpawns[x]);
            }
        }
    }

    public void PurchaseBalloons() {
        SubtractPoints(balloonPackCost);
        GameObject.FindGameObjectWithTag("player").GetComponent<playerAttackController>().AddEmptyBalloons(pickUpBalloonAmount);
    }

    public void PurchaseSnack() {
        SubtractPoints(snackCost);
        GameObject.FindGameObjectWithTag("player").GetComponent<playerHealth>().AddHealth(snackAmount);
    }


    public void GameOver() {
        gameOverMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart() {
        SceneManager.LoadScene("Game");
    }

    public void Pause() {
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
    }

    public void Resume() {
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }

    public void Quit() {
        SceneManager.LoadScene("Menu");
    }

    public void ShowLeaderboard() {
        // show leaderboard UI
        Social.ShowLeaderboardUI();
    }
}
