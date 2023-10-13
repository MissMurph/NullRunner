using Godot;
using System;

public partial class GrappleHook : Node3D {

	[Export]
	private float speed = 40f;

	private RayCast3D collider;

	[Signal]
	public delegate void GrappleHitEventHandler (Node3D grapplePoint);

	public override void _Ready () {
		collider = GetNode("Ray") as RayCast3D;
	}

	public override void _Process (double delta) {
		Position += Transform.Basis * new Vector3(0, 0, -speed) * (float)delta;

		if (collider.IsColliding()
			//&& collider.GetCollider() is PhysicalBone3D bodyPart
			) {
			//bodyPart.Connect

			GD.Print("Hit!");
			speed = 0f;
			EmitSignal(SignalName.GrappleHit, this);
		}

		
	}
}