using Godot;
using System;

public partial class step_ray : RayCast3D {

	[Export]
	private Node3D stepTarget;

	public override void _Process (double delta) {
		Vector3 hitPoint = GetCollisionPoint();

		if (IsColliding()) {
			//GD.Print("Step Pos: " + stepTarget.GlobalPosition);
			stepTarget.GlobalPosition = hitPoint;
			//GD.Print("hit point: " + hitPoint);
		}
	}
}