using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;

public class ShowLeaderboard : MonoBehaviour
{
    public void Show() {
        // show leaderboard UI
        Social.ShowLeaderboardUI();
    }
}
