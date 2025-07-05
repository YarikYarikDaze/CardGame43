using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] Player playerScript;
    public TMP_Text spellText;
    [SerializeField] Button cast;
    [SerializeField] Button pass;

    void Update()
    {
        if (playerScript != null) return;


        playerScript = GameObject.FindWithTag("Player").GetComponent<Player>();
        cast.onClick.AddListener(() =>
        {
            playerScript.CastSpell();
        });
        pass.onClick.AddListener(() =>
        {
            playerScript.EndTurn();
        });
    }

}
