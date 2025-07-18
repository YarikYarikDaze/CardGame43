using UnityEngine;

[CreateAssetMenu(fileName = "ThreePlayersTakeCards", menuName = "Scriptable Objects/ThreePlayersTakeCards")]
public class ThreePlayersTakeCards : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 0;
        this.spellType = 2;
        this.spellEffectsCount = 1;
    }

    public override void InitializeSpell(int newCaster, int[] newTargets, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.GetRandomPlayer();
    }

    void GetRandomPlayer()
    {
        this.targets = new int[1];
        this.targets[0] = spellManager.GetRandomPlayer();
    }
    public override void OnHit(SpellEffect spell)
    {
        if (!this.HasEnded())
        {
            this.Effect(null, targets[0], caster);
            this.EndSpell();
        }
    }

    public override void OnTurn()
    {
    }

    public override void OnCast() { }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.ReturnSpellToPrep(target);
    }
}
