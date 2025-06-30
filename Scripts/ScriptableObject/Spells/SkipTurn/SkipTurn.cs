using UnityEngine;

[CreateAssetMenu(fileName = "SkipTurn", menuName = "Scriptable Objects/SkipTurn")]
public class SkipTurn : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
    }
    public override void OnHit(SpellEffect spell) { }

    public override void OnTurn(SpellEffect spell)
    {
        //this.gameManager.EndTurn();
        this.duration = 0;
    }

    public override void OnCast(SpellEffect spell) {}
}
