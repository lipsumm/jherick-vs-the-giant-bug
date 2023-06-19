namespace Actors.Player;


using Godot;
using System;

public partial class Bug : Player
{

    public override void _Ready()
    {
        this.PlayerAlias = "bug";
        base._Ready();
    }
}
