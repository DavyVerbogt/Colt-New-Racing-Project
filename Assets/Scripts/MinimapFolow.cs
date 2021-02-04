using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapFolow : MonoBehaviour
{
    public Transform Player;
    public bool StaticMinimap;

    private void LateUpdate()
    {
        Vector3 MinimapPosition = Player.position;
        MinimapPosition.y = transform.position.y;
        transform.position = MinimapPosition;

        if (!StaticMinimap)
        {
            transform.rotation = Quaternion.Euler(90f, Player.eulerAngles.y, 0f);
        }
    }
}
