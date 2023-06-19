namespace Actors.Player;

using Godot;
using System;

public partial class Player : Actor
{
    public String PlayerAlias = "player";

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
    public Player Player => GetParent<PlayerState>().Player;
}


public partial class RallyInput : PlayerInput
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("player_rally"))
            this.Player.State = new RallyState();
    }
}


public partial class MoveInput : PlayerInput
{
    public override void _Process(double delta)
    {
        Vector2 direction = Input.GetVector(
            this.Player.PlayerAlias + "_left",
            this.Player.PlayerAlias + "_right",
            this.Player.PlayerAlias + "_up",
            this.Player.PlayerAlias + "_down"
        );
        
        direction = direction.Normalized();
        if (direction != Vector2.Zero)
        {
            this.Player.State = new MoveState();
            this.Player.Direction = direction;
        }
        else
        {
            this.Player.State = new IdleState();
        }
    }
}


/** STATE **/

public partial class PlayerState : Node
{
    public Player Player;

    public override void _Ready()
    {
        base._Ready();
        this.Player = GetParent<Player>();
    }
}


public partial class IdleState : PlayerState
{
    public override void _Ready()
    {
        base._Ready();
        this.Player.Direction = Vector2.Zero;
        this.Player.Animation.Play("idle");
        this.AddChild(new MoveInput());
        this.AddChild(new RallyInput());
    }
}


public partial class RallyState : PlayerState
{
    public override async void _Ready()
    {
        base._Ready();
        if (PlayerTurn.Instance.LastHit == this.Player) {
            this.Player.State = new IdleState();
            return;
        }
        this.Player.Direction *= new Vector2(0.33f, 0.33f);
        this.Player.Animation.Play("rally");
        await ToSignal(this.Player.Animation, "animation_finished");
        this.Player.State = new IdleState();
    }
}


public partial class MoveState : PlayerState
{
    public override void _Ready()
    {
        base._Ready();
        this.Player.Animation.Play("walking");
        this.AddChild(new MoveInput());
        this.AddChild(new RallyInput());
    }
}