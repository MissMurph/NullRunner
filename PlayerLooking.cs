using Godot;
using System;

public partial class PlayerLooking : Node3D {

	[Export]
	private float sensitivity = 1f;

	private Vector2 mouseDelta = new();

	//Vertical looking
	private float pitch = 0f;
	//Horizontal looking
	private float yaw = 0f;

	private Camera3D viewPort;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready() {
		Input.MouseMode = Input.MouseModeEnum.Captured;
		viewPort = GetNode<Camera3D>("ViewPort");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta) {
		//RotationDegrees.X -= mouseDelta.Y * sensitivity * delta;

		pitch -= mouseDelta.Y * sensitivity * (float)delta;
		pitch = Mathf.Clamp(pitch, -90, 90);

		yaw -= mouseDelta.X * sensitivity * (float)delta;

		RotationDegrees = new(0f, yaw, 0f);
		viewPort.RotationDegrees = new(pitch, 0f, 0f);

		mouseDelta = new();
	}

	public override void _Input (InputEvent @event) {
		if (@event is InputEventMouseMotion eventMouse) {
			mouseDelta = eventMouse.Relative;
		}
	}
}