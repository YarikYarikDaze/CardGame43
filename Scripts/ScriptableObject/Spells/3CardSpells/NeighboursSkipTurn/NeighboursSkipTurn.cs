using UnityEngine;

[CreateAssetMenu(fileName = "NeighboursSkipTurn", menuName = "Scriptable Objects/NeighboursSkipTurn")]
public class NeighboursSkipTurn : SpellEffect
{
    void Awake()
    {
        this.duration = 1;
        this.targetsNumber = 3;
        this.spellType = 1;
        this.spellEffectsCount = 1;
        this.SelfCasted = false;
        this.spellIndex = 23;
    }

    public override void InitializeSpell(int newCaster, int target, SpellManager spellManager)
    {
        this.caster = newCaster;
        this.spellManager = spellManager;
        this.targets = new int[targetsNumber];
        this.GetNeighbours(target);
    }

    void GetNeighbours(int target)
    {
        this.targets[0] = target;
        this.targets[1] = this.spellManager.GetNeighbours(target)[0];
        this.targets[2] = this.spellManager.GetNeighbours(target)[1];
    }

    public override void OnCast()
    {
        if (!HasEnded())
        {
            foreach (int index in targets)
            {
                if (index != -1)
                {
                    if (index == targets[0]) {
                        this.Effect(null, index, caster);
                    } else {
                        this.AdditionalEffect(index, caster);
                    }
                }
            }
            this.EndSpell();
        }
    }

    public override void Effect(SpellEffect spell, int target, int caster)
    {
        this.spellManager.EndPlayerTurn(target);
        SendIdToClients();
    }

    void AdditionalEffect(int target, int caster)
    {
        this.spellManager.SkipTurnPostponed(target, caster);
        SendIdToClients();
    }
}
