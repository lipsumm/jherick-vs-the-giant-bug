namespace Actors;

using Godot;
using Objects;
using System;

public partial class Hitbox : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
        this.BodyEntered += this.OnBodyEntered;
	}

    public void OnBodyEntered(Node2D body)
    {
        if (body is Ball ball)
        {
            ball.Hit(this.GlobalPosition.DirectionTo(ball.GlobalPosition));
            if (GetParent() is Actor actor)
            {
                PlayerTurn.Instance.LastHit = actor;
            }
        }
    }
}
