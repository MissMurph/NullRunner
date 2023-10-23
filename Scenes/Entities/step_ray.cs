using Godot;
using System;

public partial class step_ray : RayCast3D {

	[Export]
	private Node3D stepTarget;

	public override void _PhysicsProcess (double delta) {
		Vector3 hitPoint = GetCollisionPoint();

		if (IsColliding()) {
			stepTarget.GlobalPosition = hitPoint;
		}
	}
}