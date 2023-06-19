namespace Objects;

using Godot;
using System;

public partial class Ball : RigidBody2D
{
    private  float _combo = 1f;

    public CpuParticles2D Particles => GetNode<CpuParticles2D>("CPUParticles2D");

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _PhysicsProcess(double delta)
	{
        this.Particles.Gravity = this.LinearVelocity * -1;
	}

    public void Hit(Vector2 force)
    {
        Vector2 oldForce = this.LinearVelocity * -1;
        Vector2 newForce = force * (500 * this._combo);
        this.ApplyCentralForce(oldForce + newForce);
        this._combo += 0.2f;
    }
}
