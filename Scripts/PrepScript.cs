using UnityEngine;

public class PrepScript : MonoBehaviour
{
    [SerializeField] Sprite[] sprites;
    [SerializeField] float shift = 0.625f;

    public void SetCards(int[] colors)
    {
        int amount = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            amount += colors[i] == 0 ? 0 : 1;
        }
        for (int i = 0; i < transform.childCount; i++)
        {
            if (colors[i] > 3)
            {
                //Debug.Log("uh-oh");
                continue;
            }

            transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = sprites[colors[i]];
            transform.GetChild(i).localPosition = new Vector3(
                (shift - shift * amount) + shift * i * 2,
                0f,
                0f
            );
        }
    }
}
