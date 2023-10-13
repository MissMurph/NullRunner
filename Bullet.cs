using Godot;
using System;

public partial class Bullet : Node3D {

	[Export]
	private float speed = 40f;

	private RayCast3D collider;

	public override void _Ready () {
		collider = GetNode("Mesh/Ray") as RayCast3D;
	}

	public override void _Process(double delta) {
		Position += Transform.Basis * new Vector3(0, 0, -speed) * (float)delta;
		if (collider.IsColliding() 
			//&& collider.GetCollider() is PhysicalBone3D bodyPart
			) {
			//bodyPart.Connect

			//GD.Print(collider.GetCollider().GetSignalList());

			//collider.GetCollider().Call("LimbTest");

			if (collider.GetCollider() is Limb bodyPart) {
				bodyPart.AttackLimb(GlobalRotation);
			}

			QueueFree();
		}
	}
}