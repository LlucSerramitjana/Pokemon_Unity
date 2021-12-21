using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pokemon
{
    public PokemonBase Base { get; set; }
    public int Level { get; set; }
    public int HP { get; set; } //We'll import 3 possible attacks for the pokemon saved in a list to make it easier to code
    public int MaxHP { get; set; }
    public List<Move> Moves { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHP;
        Moves = new List<Move>();
        
        //In our case it is necessary to find another way
        foreach (var move in Base.LearnableMoves)
        {
            if (move.Level <= Level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 3)
                break;
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((Base.Attack * Level) / 100f) + 5; } //Formula to calculate the damage of the attacks
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((Base.Defense * Level) / 100f) + 5; } //Formula to calculate the damage of the defense
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 5; } //This formula originally uses Speed, do we need it?
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((Base.Speed * Level) / 100f) + 10; } //This formula originally uses Speed, do we need it?
    }

    public bool TakeDamage(Move move, Pokemon attacker)
    {
        float modifiers = Random.Range(0.85f, 1f);
        float a = (2 * attacker.Level + 10) / 250f;
        float d = a * move.Base.Power * ((float)attacker.Attack / Defense) + 2;
        int damage = Mathf.FloorToInt(d * modifiers);

        HP -= damage;
        if (HP <= 0)
        {
            HP = 0;
            return true;
        }
        else
            return false;
    }

    public Move GetRandomMove()
    {
        int r = Random.Range(0, Moves.Count);
        return Moves[r];
    }
}
