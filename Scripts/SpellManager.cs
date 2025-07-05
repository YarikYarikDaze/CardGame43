using UnityEngine;
using System;
using System.Collections.Generic;
public class SpellManager : MonoBehaviour
{
    GameManager gameManager;

    string[,,] effectsArray;

    public void InitializeItself(GameManager gameManager)
    {
        gameManager = gameManager;
        this.InitializeEffects();
    }

    void InitializeEffects()
    {
        effectsArray = new string[3, 3, 3]
        {
            {
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                },
                                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                },
                                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                }
            },
            {
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                },
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                },
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                }
            },
            {
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                },
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                },
                {
                "TakeCard",
                "TakeCard",
                "TakeCard"
                }
            }
        };
    }

    //
    //SpellManager Logic
    //

    SpellEffect TraverseEffectsOnHit(SpellEffect newSpell, int index)
    {
        List<SpellEffect> playerEffects = gameManager.GetEffectsOnPlayer(index);

        foreach (SpellEffect spell in playerEffects)
        {
            spell.OnHit(newSpell);
        }

        gameManager.SetEffectsOnPlayer(index, playerEffects);

        return newSpell;
    }

    void TraverseEffectsOnTurn(int index)
    {
        List<SpellEffect> playerEffects = gameManager.GetEffectsOnPlayer(index);

        foreach (SpellEffect spell in playerEffects)
        {
            spell.OnTurn(spell);
        }

        gameManager.SetEffectsOnPlayer(index, playerEffects);

        this.DeleteEndedSpells(index);
    }

    public void CreateSpell(int index, int[,,] playerCards, int[] targets)
    {
        SpellEffect newSpell = (SpellEffect)ScriptableObject.CreateInstance(this.effectsArray[playerCards[index, 1, 0], playerCards[index, 1, 1], playerCards[index, 1, 2]]);
        newSpell.InitializeSpell(index, targets, this);

        this.HandleNewSpell(newSpell, index);
    }
    void HandleNewSpell(SpellEffect newSpell, int index)
    {
        SpellEffect spellAfterHandling = this.TraverseEffectsOnHit(newSpell, index);

        spellAfterHandling.OnCast(spellAfterHandling);

        gameManager.AddEffect(index, spellAfterHandling);

        this.DeleteEndedSpells(index);
    }

    void DeleteEndedSpells(int index)
    {
        List<SpellEffect> endedSpells = new List<SpellEffect>();

        List<SpellEffect> spellsOnPlayer = gameManager.GetEffectsOnPlayer(index);

        foreach (SpellEffect spell in spellsOnPlayer)
        {
            if (spell.HasEnded())
            {
                endedSpells.Add(spell);
            }
        }

        foreach (SpellEffect spell in endedSpells)
        {
            spellsOnPlayer.Remove(spell);
        }

        gameManager.SetEffectsOnPlayer(index, spellsOnPlayer);
    }

    //
    //Spells Logic
    //

    public void GiveCardToPlayer(int index)
    {
        gameManager.GiveCardToPlayer(index);
    }

    public void EndPlayerTurn()
    {
        gameManager.EndPlayerTurn();
    }

    public void ClearPlayerEffects(int index)
    {
        List<SpellEffect> newList = new List<SpellEffect>();
        this.gameManager.SetEffectsOnPlayer(index, newList);
    }

    public int[] GetNeighbours(int index)
    {
        int[] neighbours = new int[2];
        return neighbours;
    }
}
