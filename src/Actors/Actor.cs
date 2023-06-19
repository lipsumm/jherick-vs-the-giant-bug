namespace Actors;

using Godot;
using System;

public partial class Actor : CharacterBody2D
{
	public Node State
	{
		get => GetNodeOrNull<Node>("State");
		set
		{
			if (this.State != null)
				this.RemoveChild(this.State);
			if (value == null)
				return;
			value.Name = "State";
			this.AddChild(value, true);
		}
	}

    public AnimationPlayer Animation => GetNode<AnimationPlayer>("AnimationPlayer");
}

