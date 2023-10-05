using Godot;
using System;

public partial class Weapon : Node3D {

	[Export]
	public Node3D forwardGrip;

	[Export]
	public Node3D backwardGrip;

	[Export]
	private AnimationPlayer animator;

	[Export]
	private RayCast3D muzzle;

	[Export]
	private PackedScene bulletResource;

	public override void _Ready() {
		
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (Input.IsActionPressed("shoot")) {
			if (!animator.IsPlaying()) {
				animator.Play("recoil");
				Node3D bullet = bulletResource.Instantiate() as Node3D;
				bullet.Position = muzzle.GlobalPosition;
				bullet.Basis = muzzle.GlobalTransform.Basis;
				//GD.Print(bullet);
				//Owner.AddChild(bullet);
				//RemoveChild(bullet);
				this.GetTree().Root.AddChild(bullet);
			}
		}
	}
}