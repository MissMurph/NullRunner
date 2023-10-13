using Godot;
using System;

public partial class PlayerMovement : RigidBody3D {

	[Export]
	private float acceleration = 10f;

	[Export]
	private float speed = 30f;

	[Export]
	private float maxSpeed = 60f;

	[Export]
	private float baseMovementModifier = 6f;

	private float currentMovementModifier;

	[Export]
	private float airModifier = 0.1f;

	[Export]
	private Vector3 velocity = new();

	[Export]
	private float jumpForce = 25f;

	[Export]
	private float stopSpeed = 0.1f;

	[Export]
	private bool drag = true;

	[Export]
	private float baseDrag = 1f;

	[Export]
	private float airDrag = 0f;

	private float currentDrag;

	private Vector2 moveInput;

	private Node3D head;

	private RayCast3D feet;

	private bool grounded;

	private Node3D grappleNode;

	private bool grappling;

	[Export]
	private float grappleAcceleration = 6f;

	public override void _Ready() {
		head = GetNode<Node3D>("Head");
		feet = GetNode<RayCast3D>("Feet");

		currentMovementModifier = baseMovementModifier;
		currentDrag = baseDrag;
	}

	public override void _Process(double delta) {
		

		if (feet.IsColliding()) {
			grounded = true;
			currentMovementModifier = baseMovementModifier;
			currentDrag = baseDrag;
		}
		else {
			grounded = false;
			currentMovementModifier = airModifier;
			currentDrag = airDrag;
		}
	}

	public override void _PhysicsProcess (double delta) {
		moveInput = Input.GetVector("move_left", "move_right", "move_backward", "move_forward");

		Vector3 direction = new();

		direction += head.GlobalTransform.Basis.X * moveInput.X;
		direction -= head.GlobalTransform.Basis.Z * moveInput.Y;

		velocity = velocity.Lerp(direction * speed, acceleration * currentMovementModifier * (float)delta);

		ApplyCentralForce(velocity);

		if (grappling) {
			Vector3 grappleDirection = grappleNode.GlobalPosition - GlobalPosition;
			float distanceToAnchor = grappleDirection.Length();

			//grappleProgress = distanceToAnchor / initialDistance;

			ApplyCentralForce(grappleDirection.Normalized() * grappleAcceleration * (float)delta);
		}
	}

	public override void _IntegrateForces (PhysicsDirectBodyState3D state) {
		Vector3 currentVelocity = state.LinearVelocity;

		if (moveInput.Length() < 0.2f) {
			/*float x = state.LinearVelocity.X;
			float y = state.LinearVelocity.Z;

			x = Mathf.Lerp(x, 0, stopSpeed);
			y = Mathf.Lerp(y, 0, stopSpeed);

			state.LinearVelocity = new Vector3(x, state.LinearVelocity.Y, y);*/
		}

		if (drag) {
			float magnitude = currentVelocity.LengthSquared() * currentDrag;
			state.LinearVelocity -= currentVelocity.Normalized() * magnitude * state.Step;
		}
	}

	public override void _Input (InputEvent @event) {
		if (@event.IsActionPressed("jump") && grounded) {
			ApplyCentralImpulse(new Vector3(0, jumpForce, 0));
		}
	}

	public void OnGrappleHit (Node3D grapplePoint) {
		grappleNode = grapplePoint;
		grappling = true;
	}

	public void OnGrappleRelease () {
		grappling = false;
		grappleNode = null;
	}
}