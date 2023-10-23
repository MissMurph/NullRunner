using Godot;
using System;

public partial class IKTarget : Marker3D {

	[Export]
	private Node3D stepTarget;

	[Export]
	private float stepDistance = 3.0f;

	[Export]
	private IKTarget adjacentLeg;

	public bool isStepping = false;

	private Node3D parent;

	public override void _Ready () {
		parent = GetOwner<Node3D>();
	}

	public override void _Process (double delta) {
		if (!isStepping && !adjacentLeg.isStepping && Math.Abs(GlobalPosition.DistanceTo(stepTarget.GlobalPosition)) > stepDistance) {
			Step();
		}
	}

	private void Step () {
		Vector3 targetPos = stepTarget.GlobalPosition;
		Vector3 halfWay = (GlobalPosition + targetPos) / 2;

		isStepping = true;

		Tween tween = GetTree().CreateTween();
		tween.TweenProperty(this, "global_position", halfWay + parent.Basis.Y, 0.1);
		tween.TweenProperty(this, "global_position", targetPos, 0.1);
		tween.TweenCallback(Callable.From(() => isStepping = false));
	}
}