using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pokemon
{
    PokemonBase _base;
    int level;
    public int HP { get; set; } //We'll import 3 possible attacks for the pokemon saved in a list to make it easier to code
    public List<Move> Moves { get; set; }

    //public Move Move1 { get; set; }
    //public Move Move2 { get; set; }
    //public Move Move3 { get; set; }

    public Pokemon(PokemonBase pBase, int pLevel)
    {
        _base = pBase;
        level = pLevel;
        HP = _base.MaxHp;
        Moves = new List<Move>();
        
        //Move1 = Move1;
        //Move2 = Move2;
        //Move3 = Move3;

        //In our case it is necessary to find another way
        foreach (var move in _base.LearnableMoves)
        {
            if (move.Level <= level)
            {
                Moves.Add(new Move(move.Base));
            }
            if (Moves.Count >= 3)
                break;
        }
    }

    public int Attack
    {
        get { return Mathf.FloorToInt((_base.Attack * level) / 100f) + 5; } //Formula to calculate the damage of the attacks
    }
    public int Defense
    {
        get { return Mathf.FloorToInt((_base.Defense * level) / 100f) + 5; } //Formula to calculate the damage of the defense
    }
    public int Speed
    {
        get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 5; } //This formula originally uses Speed, do we need it?
    }
    public int MaxHp
    {
        get { return Mathf.FloorToInt((_base.Speed * level) / 100f) + 10; } //This formula originally uses Speed, do we need it?
    }
}
