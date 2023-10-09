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

	[Export]
	private BoneAttachment3D grappleSource;

	private AnimationPlayer animator;

	private AnimationTree animTree;

	[Export]
	private PackedScene grappleResource;

	[Signal]
	public delegate void GrappleHitEventHandler ();

	[Signal]
	public delegate void GrappleReleaseEventHandler ();

	public override void _Ready () {
		animator = GetNode<AnimationPlayer>("AnimationPlayer");
		animTree = GetNode<AnimationTree>("AnimationTree");

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
		if (@event.IsAction("grapple")) {
			if (@event.IsPressed()) {
				GD.Print("Grapple Pressed");
				leftIK.Stop();
				animTree.Set("parameters/conditions/grappleEnd", false);
				animTree.Set("parameters/conditions/fireGrapple", true);
			}

			if (@event.IsReleased()) {
				GD.Print("Grapple Released");
				animTree.Set("parameters/conditions/grappleEnd", true);
				animTree.Set("parameters/conditions/fireGrapple", false);
				leftIK.Start();
			}
		}
	}

	public void FireGrapple () {
		Node3D hookProjectile = grappleResource.Instantiate() as Node3D;

		hookProjectile.Position = grappleSource.GlobalPosition;
		hookProjectile.Basis = GlobalTransform.Basis;
		//GrappleHook yeet = hookProjectile.GetScript<GrappleHook>();

		hookProjectile.Connect("GrappleHit", Callable.From(OnGrappleHit));

		GetTree().Root.AddChild(hookProjectile);
	}

	private void OnGrappleHit () {
		GD.Print("Grapple Hit Something");
	}
}