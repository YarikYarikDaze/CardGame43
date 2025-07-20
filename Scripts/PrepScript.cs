using UnityEngine;
using TMPro;

public class PrepScript : MonoBehaviour
{
    public int id;
    [SerializeField] Sprite[] sprites;
    [SerializeField] float shift = 0.625f;
    [SerializeField] TMP_Text cardCount;
    [SerializeField] TMP_Text idDemo;

    public void SetCards(int[] colors)
    {
        int amount = 0;
        for (int i = 0; i < colors.Length; i++)
        {
            amount += colors[i] == 0 ? 0 : 1;
        }
        for (int i = 0; i < 3; i++)
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
    public void KeepRot(){
        cardCount.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        idDemo.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }
    public void SetCount(int count){
        cardCount.text = count.ToString();
    } 
    public void Demo(){
        idDemo.text = (id+1).ToString();
    }


    void OnMouseOver()
    {
        if (Input.GetMouseButtonDown(0))
        {
            GameObject.FindWithTag("Player").GetComponent<Player>().AddTarget(this.id);
        }
    }
}
