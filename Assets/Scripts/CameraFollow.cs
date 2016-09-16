using UnityEngine;
using System.Collections;
using DG.Tweening;

public class CameraFollow : MonoBehaviour
{
    public GameObject Player;

    // Update is called once per frame
    void Update()
    {
        if (Player != null && !LevelManager.manager.playing)
        {
            transform.position = new Vector3(Player.transform.position.x, transform.position.y , -10);
        }
        if (Player != null && !LevelManager.manager.jumping && LevelManager.manager.playing)
        {
            transform.DOLocalMove(new Vector3(Player.transform.position.x + 5f, transform.position.y, -10), 3f);
        }
    }
}
