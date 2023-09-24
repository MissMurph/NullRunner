using Godot;
using System;

public partial class ArmController : Node3D {

	[Export]
	private Weapon activeWeapon;

	[Export]
	private Node3D leftHandTarget;

	[Export]
	private Node3D rightHandTarget;

	[Export]
	private SkeletonIK3D leftIK;

	[Export]
	private SkeletonIK3D rightIK;

	private AnimationPlayer animator;

	public override void _Ready () {
		animator = GetNode<AnimationPlayer>("AnimationPlayer");

		if (activeWeapon is not null) {
			GD.Print("yeet");

			leftHandTarget.GlobalPosition = activeWeapon.forwardGrip.GlobalPosition;
			rightHandTarget.GlobalPosition = activeWeapon.backwardGrip.GlobalPosition;
			leftIK.Start();
			rightIK.Start();
		}
	}

	public override void _Process (double delta) {
		leftHandTarget.GlobalPosition = activeWeapon.forwardGrip.GlobalPosition;
		rightHandTarget.GlobalPosition = activeWeapon.backwardGrip.GlobalPosition;
	}

	public override void _Input (InputEvent @event) {
		
	}
}