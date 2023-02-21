using UnityEngine;
using UnityEngine.UI;

public class HitMarker : MonoBehaviour
{
    private float lastUpdateTime = 0;
    private string redHitMarker = "Images\\red-hit-marker";
    private string hitmarker = "Images\\hit-marker";
    private string defaultRetical = "Images\\reticle";
    private bool updated = false;

    [SerializeField]
    private Image cursor;

    private void UpdateRetical(string path)
    {
        cursor.sprite = Resources.Load<Sprite>(path);
    }

    private void Update()
    {
        if (updated)
        {
            if (Time.time - lastUpdateTime > 1f)
            {
                updated = false;
                UpdateRetical(defaultRetical);
            }
        }
    }

    public void UpdateCursor(Hitmarker hitmarkerType)
    {
        updated = true;
        lastUpdateTime = Time.time;
        switch(hitmarkerType)
        {
            case Hitmarker.hitmarker:
                UpdateRetical(hitmarker);
                break;
            default:
                UpdateRetical(redHitMarker);
                break;
        }
    }
}
