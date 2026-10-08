using UnityEngine;

public class ScoreItem : MonoBehaviour
{
    public ItemData itemdata;

    private void Start()
    {
        // 自分自身の絵を割り当てられた絵に差し替える
        GetComponent<SpriteRenderer>().sprite = itemdata.itemSprite;
    }
}
