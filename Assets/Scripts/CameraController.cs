using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CameraController : MonoBehaviour
{
    public Transform playerTransform;

    public Tilemap groundTileMap;
    private Vector3 topRightLimit;
    private Vector3 bottomLeftLimit;

    private float halfWidth;
    private float halfHeight;

    //Set the song number in bgm array
    public int musicToPlay;

    //Set if the song is playing or not
    private bool isMusicPlaying;

    // Start is called before the first frame update
    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = halfHeight * Camera.main.aspect;

        bottomLeftLimit = groundTileMap.localBounds.min + new Vector3(halfWidth, halfHeight, 0f);
        topRightLimit = groundTileMap.localBounds.max + new Vector3(-halfWidth, -halfHeight, 0f);

        if (PlayerController.player != null)
        {
            playerTransform = PlayerController.player.transform;
            PlayerController.player.SetBounds(groundTileMap.localBounds.min, groundTileMap.localBounds.max);
        }


    }

    // LateUpdate is called once per frame after update
    void LateUpdate()
    {
        //Follow the player
        if (playerTransform != null)
        {
            transform.position = new Vector3(playerTransform.position.x, playerTransform.position.y, transform.position.z);
        }

        //Keep the camera inside bounds
        transform.position = new Vector3(Mathf.Clamp(transform.position.x, bottomLeftLimit.x, topRightLimit.x), Mathf.Clamp(transform.position.y, bottomLeftLimit.y, topRightLimit.y), transform.position.z);

        if (!isMusicPlaying)
        {
            AudioManager.instance.PlayMusic(musicToPlay);
            isMusicPlaying = true;
        }
    }
}
