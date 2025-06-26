using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Constants : MonoBehaviour
{
    // Add new constants to this script as needed, e.g.
    // public const int NUMBER_OF_PLAYERS = 4;

    // DEVELOPER SETTINGS
    public const bool IS_DEBUG = true;

    // KINECT GESTURE DETECTION THRESHOLDS
    public const float FLAP_UP= 0.15f;
    public const float FLAP_DOWN = 0.2f;

    // SCREEN
    public const float CAMERA_GAME_LEVEL_SPEED = 4f;
    public const float SCREEN_HEIGHT = 3.5f;

    // CHARACTER
    public const float CHARACTER_MAX_HEIGHT = SCREEN_HEIGHT;
    public const float CHARACTER_MAX_LEFT = -3f;
    public const float CHARACTER_MAX_RIGHT = 2.9f;
    public const float CHARACTER_MIN_HEIGHT = 0.8f;
    public const float CHARACTER_TILT_SPEED = .5f;
    public const float CHARACTER_FLAP_SPEED = 1.5f;
    public const float CHARACTER_FALL_SPEED = 1f;

    // UI TEXT
    public static readonly string PLAYER_TAG = "Player";
    public static readonly string TITLE_START = "3D Flappy Bird";
    public static readonly string TITLE_GAMEOVER = "Gameover";
    public static readonly string STATUS_START = "Flap to Start";
    public static readonly string STATUS_PLAYING = "Playing";
    public static readonly string STATUS_DEAD = "Flap to Restart";
    public static readonly string KINECT_WAITING = "WAITING FOR USERS";
    public static readonly string TUTORIAL = "Controls: Tilt Left, Tilt Right, or Flap.";
    // COLLECTIBLES
    public const float COLLECTIBLE_MIN_Y = 1f;
    public const float COLLECTIBLE_MAX_Y = 3f;
    public const float COLLECTIBLE_PERCENTAGE = 0.5f;

    // OBSTACLES
    public const float OBSTACLE_MIN_Y = 1f;
    public const float OBSTACLE_MAX_Y = 3f;
    public const float OBSTACLE_PERCENTAGE = 0.2f;
}
