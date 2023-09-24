using Godot;
using System;

public partial class Bullet : Node3D {

	[Export]
	private const float speed = 40f;

	public override void _Process(double delta) {
		Position += Transform.Basis * new Vector3(0, 0, -speed) * (float)delta;
	}
}