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

	private Sprite3D muzzleFlash;

	private RandomNumberGenerator rando;

	[Export]
	public bool firingComplete;

	public override void _Ready() {
		muzzleFlash = GetNode<Sprite3D>("GunRig/Muzzle/MuzzleFlash");
		muzzleFlash.Visible = false;
		firingComplete = false;
		rando = new RandomNumberGenerator();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		if (Input.IsActionPressed("shoot")) {
			if (!animator.IsPlaying()) {
				animator.Play("recoil");
				Node3D bullet = bulletResource.Instantiate() as Node3D;
				bullet.Position = muzzle.GlobalPosition;
				bullet.Basis = muzzle.GlobalTransform.Basis;

				float angle = rando.RandfRange(0, 360);
				float size = rando.RandfRange(0.7f, 1.2f);

				muzzleFlash.RotationDegrees = new Vector3(0, 0, angle);
				muzzleFlash.Scale = Vector3.One * size;

				muzzleFlash.Visible = true;
				
				GetTree().Root.AddChild(bullet);
				firingComplete = false;
			}
		}

		if (firingComplete) muzzleFlash.Visible = false;
	}
}