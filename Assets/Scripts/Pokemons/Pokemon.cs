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

    //public Move Move1 { get; set; }
    //public Move Move2 { get; set; }
    //public Move Move3 { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        Base = pBase;
        Level = pLevel;
        HP = MaxHP;

        Moves = new List<Move>();
        
        //Move1 = Move1;
        //Move2 = Move2;
        //Move3 = Move3;

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
}
