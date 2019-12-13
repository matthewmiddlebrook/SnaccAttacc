using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class gameManager : MonoBehaviour
{
    [Header("Game")]
    public int round = 1;
    public int transitionDuration;
    public int startTransitionDuration;
    public int effectDelayDuration;
    public int pointsOnHit;
    public int pointsOnDeath;
    public int pointsOnRebuild;
    public int points = 0;
    public GameObject pauseMenu;
    public GameObject gameOverMenu;
    public Text scoreText;

    [Header("Audio")]
    public AudioClip backgroundMusic;
    public AudioClip roundSound;
    public AudioClip playerDefeatedSound;
    public AudioClip[] catMeowSounds;
    public AudioClip[] catHitSounds;
    public AudioClip doorOpenSound;
    public AudioClip snackPickupSound;
    public AudioClip balloonPickupSound;
    public AudioClip[] woodPlankSounds;
    public AudioClip[] balloonThrowSounds;

    private AudioSource sfxAudioSource;

    [Header("Cats")]
    public GameObject catPrefab;
    public List<GameObject> catSpawns;
    public int maxCatsOnMap;
    public int spawnDelayDuration;
    public int baseCatCount;
    public int baseCatHealth;
    public int difficulty;
    public float kittenSpeed;
    public int kittenCountMultiplier;
    public int kittenHealthMultiplier;
    public float catSpeed;
    public int catCountMultiplier;
    public int catHealthMultiplier;
    public float wildcatSpeed;
    public int wildcatCountMultiplier;
    public int wildcatHealthMultiplier;
    [HideInInspector] public float catMaxHealth;

    [Header("Balloons")]
    public int balloonPackCost;
    public int pickUpBalloonAmount;
    
    [Header("Snacks")]
    public List<GameObject> snacks;
    public int snackCost;
    public int snackAmount;

    [Header("Player")]
    public float playerMaxHealth;
    public int playerFullBalloonStartCount;
    public int playerEmptyBalloonStartCount;

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
    private GameObject infoTextObject;
    public bool isPaused = false;
    public bool gameOver = false;

    [Header("Stats")]
    public int roundsSurvived;
    public float surviveTime;
    public int catsSpawned;
    public int catsDefeated;
    public int beanBucksEarned;
    public int beanBucksSpent;
    public int waterBalloonsCollected;
    public int waterBalloonsThrown;
    public int waterBalloonsBought;
    public int snacksBought;
    public int planksKnockedOff;
    public int planksPutUp;
    public Text[] endGameScoreTexts;


    // Start is called before the first frame update
    void Start()
    {
        difficulty = PlayerPrefs.GetInt("Difficulty");

        roundNumber = GameObject.FindGameObjectWithTag("roundNumber").GetComponent<UnityEngine.UI.Text>();
        pointsText = GameObject.FindGameObjectWithTag("pointsText").GetComponent<UnityEngine.UI.Text>();
        pointsAddEffect = GameObject.FindGameObjectWithTag("pointsAddEffect");
        pointsSubtractEffect = GameObject.FindGameObjectWithTag("pointsSubtractEffect");
        infoTextObject = GameObject.FindGameObjectWithTag("infoTextObject");

        sfxAudioSource = GameObject.FindGameObjectWithTag("sfxAudioSource").GetComponent<AudioSource>();

        pointsAddEffect.SetActive(false);
        pointsSubtractEffect.SetActive(false);

        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        bool inPlay = StillInPlay();

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused) {
                Resume();
            } else {
                Pause();
            }
        }

        if (!isPaused && !gameOver) {
            if (inPlay && !transitioning) {
                surviveTime += Time.deltaTime;
            }

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
        catsSpawned++;
    }

    void Transition() {
        transitioning = true;

        remainingTransitionTime = transitionDuration;

        if (round != 0) {
            infoTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
                "ROUND " + round + " OVER";
            infoTextObject.GetComponent<Animator>().Play("infoTextFade");
            roundsSurvived++;
        } else {
            remainingTransitionTime = startTransitionDuration;
        }

        round++;

        if (difficulty == 0) {
            roundCatCount = baseCatCount + (round * kittenCountMultiplier);
            catMaxHealth = baseCatHealth + (round * kittenHealthMultiplier);
        } else if (difficulty == 1) {
            roundCatCount = baseCatCount + (round * catCountMultiplier);
            catMaxHealth = baseCatHealth + (round * catHealthMultiplier);
        } else {
            roundCatCount = baseCatCount + (round * wildcatCountMultiplier);
            catMaxHealth = baseCatHealth + (round * wildcatHealthMultiplier);
        }

        roundNumber.GetComponent<Animator>().Play("fade");

        spawnedCats = 0;
    }

    void BeginRound() {
        transitioning = false;

        sfxAudioSource.clip = roundSound;
        sfxAudioSource.Play();

        infoTextObject.transform.GetChild(1).GetComponent<UnityEngine.UI.Text>().text = 
            "ROUND " + round + " STARTING";
        infoTextObject.GetComponent<Animator>().Play("infoTextFade");
        
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
        beanBucksEarned += amount;

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
        beanBucksSpent += amount;

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
        waterBalloonsBought += pickUpBalloonAmount;
    }

    public void PurchaseSnack() {
        SubtractPoints(snackCost);
        snacksBought += snackAmount;
    }


    public void GameOver() {
        gameOver = true;

        sfxAudioSource.clip = playerDefeatedSound;
        sfxAudioSource.Play();

        scoreText.text = "ROUNDS SURVIVED: " + round;

        endGameScoreTexts[0].text = 
            "ROUNDS:\t\t" + roundsSurvived.ToString() + 
            "\nTIME:\t\t\t" + (int)surviveTime;
        endGameScoreTexts[1].text = 
            "SPAWNED:\t" + catsSpawned.ToString() + 
            "\nDEFEATED:\t" + catsDefeated.ToString();
        endGameScoreTexts[2].text = 
            "EARNED:\t" + beanBucksEarned.ToString() + 
            "\nSPENT:\t\t" + beanBucksSpent.ToString();
        endGameScoreTexts[3].text = 
            "FALLEN:\t" + planksKnockedOff.ToString() + 
            "\nREBUILT:\t" + planksPutUp.ToString();
        endGameScoreTexts[4].text = 
            "COLLECTED:\t\t" + waterBalloonsCollected.ToString() + 
            "\nTHROWN:\t\t\t" + waterBalloonsThrown.ToString() + 
            "\nPURCHASED:\t" + waterBalloonsBought.ToString();
        endGameScoreTexts[5].text = 
            "PURCHASED:\t" + snacksBought.ToString();

        gameOverMenu.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
    }

    public void Restart() {
        SceneManager.LoadScene("Game");
    }

    public void Pause() {
        isPaused = true;
        pauseMenu.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("balloon")) {
            waterBalloonScript balloon = b.GetComponent<waterBalloonScript>();
            balloon.Paused();
        }
    }

    public void Resume() {
        print("Unpaused");
        isPaused = false;
        pauseMenu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        foreach (GameObject b in GameObject.FindGameObjectsWithTag("balloon")) {
            waterBalloonScript balloon = b.GetComponent<waterBalloonScript>();
            balloon.Unpaused();
        }
    }

    public void Quit() {
        SceneManager.LoadScene("Menu");
    }

    public void ShowLeaderboard() {
        // show leaderboard UI
        // Social.ShowLeaderboardUI();
    }
}
