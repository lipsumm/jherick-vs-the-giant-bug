namespace Actors;

using Godot;
using System;

public partial class Jherick : Actor
{
    public Vector2 Direction = Vector2.Zero;

    public override void _Ready()
    {
        base._Ready();
        this.State = new IdleState();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);
        this.Velocity = this.Direction * 80;
        this.MoveAndSlide();
    }

}

/** INPUTS **/

public partial class PlayerInput : Node
{
    public Jherick Jherick => GetParent<JherickState>().Jherick;
}


public partial class RallyInput : PlayerInput
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("player_rally"))
            this.Jherick.State = new RallyState();
    }
}


public partial class MoveInput : PlayerInput
{
    public override void _Process(double delta)
    {
        Vector2 direction = Input.GetVector(
            "player_left",
            "player_right",
            "player_up",
            "player_down"
        );
        
        direction = direction.Normalized();
        if (direction != Vector2.Zero)
        {
            this.Jherick.State = new MoveState();
            this.Jherick.Direction = direction;
        }
        else
        {
            this.Jherick.State = new IdleState();
        }
    }
}


/** STATE **/

public partial class JherickState : Node
{
    public Jherick Jherick;

    public override void _Ready()
    {
        base._Ready();
        this.Jherick = GetParent<Jherick>();
    }
}


public partial class IdleState : JherickState
{
    public override void _Ready()
    {
        base._Ready();
        this.Jherick.Direction = Vector2.Zero;
        this.Jherick.Animation.Play("idle");
        this.AddChild(new MoveInput());
        this.AddChild(new RallyInput());
    }
}


public partial class RallyState : JherickState
{
    public override async void _Ready()
    {
        base._Ready();
        this.Jherick.Direction *= new Vector2(0.33f, 0.33f);
        this.Jherick.Animation.Play("rally");
        await ToSignal(this.Jherick.Animation, "animation_finished");
        this.Jherick.State = new IdleState();
    }
}


public partial class MoveState : JherickState
{
    public override void _Ready()
    {
        base._Ready();
        this.Jherick.Animation.Play("walking");
        this.AddChild(new MoveInput());
        this.AddChild(new RallyInput());
    }
}