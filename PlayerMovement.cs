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

	[Export]
	private float slideVelocityThreshold = 5f;

	[Export]
	private float slideBoost = 5f;

	//This is how many units the crouching would change each second, in theory 10 = 0.1 seconds to crouch
	[Export]
	private float crouchSpeed = 10f;

	private bool crouching;

	private bool sliding;

	private CollisionShape3D bodyCollider;
	private Vector3 originalBodyPos;

	private Node3D feetCollider;
	private Vector3 originalFeetPos;

	private Vector3 originalFeetRayPos;

	private Vector3 originalHeadPos;

	private Vector3 direction = new();

	//false = pulling head down
	//true = pulling feat up
	private bool crouchDirection;

	public override void _Ready() {
		head = GetNode<Node3D>("Head");
		feet = GetNode<RayCast3D>("Feet");
		bodyCollider = GetNode<CollisionShape3D>("MovementCollider");
		feetCollider = GetNode<CollisionShape3D>("FeetCollider");
		
		originalBodyPos = bodyCollider.Position;
		originalFeetPos = feetCollider.Position;
		originalFeetRayPos = feet.Position;
		originalHeadPos = head.Position;

		currentMovementModifier = baseMovementModifier;
		currentDrag = baseDrag;
		crouching = false;
		sliding = false;
	}

	public override void _Process(double delta) {
		crouching = Input.IsActionPressed("crouch");

		if (crouching) {
			
		}

		if (sliding) {
			PhysicsMaterialOverride.Friction = 0f;

			if (velocity.Length() <= slideVelocityThreshold) {
				sliding = false;
			}
		}
		else {
			PhysicsMaterialOverride.Friction = 0.15f;
		}

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

		if (!sliding) {
			direction = new();
			direction += head.GlobalTransform.Basis.X * moveInput.X;
			direction -= head.GlobalTransform.Basis.Z * moveInput.Y;
		}

		if (crouching) {
			if (crouchDirection) {
				bodyCollider.Position = bodyCollider.Position.Lerp(originalBodyPos + new Vector3(0, 0.5f, 0), crouchSpeed * (float)delta);
				(bodyCollider.Shape as CapsuleShape3D).Height = Mathf.Lerp(1, 0.5f, crouchSpeed * (float)delta);
				feet.Position = feet.Position.Lerp(originalFeetRayPos + new Vector3(0, 1f, 0), crouchSpeed * (float)delta);
				feetCollider.Position = feetCollider.Position.Lerp(originalFeetPos + new Vector3(0, 1f, 0), crouchSpeed * (float)delta);
			}
			else {
				bodyCollider.Position = bodyCollider.Position.Lerp(originalBodyPos - new Vector3(0, 0.5f, 0), crouchSpeed * (float)delta);
				(bodyCollider.Shape as CapsuleShape3D).Height = Mathf.Lerp(1, 0.5f, crouchSpeed * (float)delta);
				head.Position = head.Position.Lerp(originalHeadPos - new Vector3(0, 1f, 0), crouchSpeed * (float)delta);
			}
		}
		else {
			if (crouchDirection) {
				bodyCollider.Position = bodyCollider.Position.Lerp(originalBodyPos, crouchSpeed * (float)delta);
				(bodyCollider.Shape as CapsuleShape3D).Height = Mathf.Lerp(0.5f, 1.0f, crouchSpeed * (float)delta);
				feet.Position = feet.Position.Lerp(originalFeetRayPos, crouchSpeed * (float)delta);
				feetCollider.Position = feetCollider.Position.Lerp(originalFeetPos, crouchSpeed * (float)delta);
			}
			else {
				bodyCollider.Position = bodyCollider.Position.Lerp(originalBodyPos, crouchSpeed * (float)delta);
				(bodyCollider.Shape as CapsuleShape3D).Height = Mathf.Lerp(0.5f, 1f, crouchSpeed * (float)delta);
				head.Position = head.Position.Lerp(originalHeadPos, crouchSpeed * (float)delta);
			}
		}

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

		/*if (Input.IsActionJustPressed("test_command")) {
			var spaceState = GetWorld3D().DirectSpaceState;
			Vector3 direction = (head.GlobalTransform.Basis.X * head.GlobalTransform.Basis.Z * head.GetNode<Node3D>("ViewPort").GlobalTransform.Basis.Y).Normalized();

			PhysicsRayQueryParameters3D query = PhysicsRayQueryParameters3D.Create(head.GlobalPosition, direction * 100f);
			var result = spaceState.IntersectRay(query);

			if (result.Count > 0) {
				EmitSignal(SignalName.TestCommand, result["position"]);
			}
		}*/
	}

	public override void _Input (InputEvent @event) {
		if (@event.IsActionPressed("jump") && grounded) {
			ApplyCentralImpulse(new Vector3(0, jumpForce, 0));
		}
		if (@event.IsActionPressed("crouch")) {
			if (grounded) {
				crouchDirection = false;

				if (velocity.Length() >= slideVelocityThreshold) {
					ApplyCentralImpulse(direction * slideBoost * currentMovementModifier);
					sliding = true;
				}
			}
			else {
				crouchDirection = true;
			}
		}
		if (@event.IsActionReleased("crouch")) {
			sliding = false;
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