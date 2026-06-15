using Godot;
using System;

enum PlayerState
{
	Idle,
	Walk,
	Jump,
	Duck
}

public partial class Player : CharacterBody2D
{
	public const float Speed = 100.0f;
	public const float JumpVelocity = -300.0f;
	private AnimatedSprite2D anim;

	private CollisionShape2D cs;

	private PlayerState state;

	private Vector2 velocity;

	private float direction = 0;

	int jumps = 0;
	int maxJumps = 2;

	public override void _Ready()
	{
		cs = GetNode<CollisionShape2D>("CollisionShape2D");
		anim = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		GoIdle();
	}

	public override void _PhysicsProcess(double delta)
	{
		velocity = Velocity;

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		switch (state)
		{
			case PlayerState.Idle:
				IdleState();
				break;
			case PlayerState.Walk:
				WalkState();
				break;
			case PlayerState.Jump:
				JumpState();
				break;
			case PlayerState.Duck:
				DuckState();
				break;
		}

		Velocity = velocity;
		MoveAndSlide();
	}

	private void GoIdle()
	{
		state = PlayerState.Idle;
		anim.Play("idle");
	}

	private void GoWalk()
	{
		state = PlayerState.Walk;
		anim.Play("walk");
	}

	private void GoJump()
	{
		state = PlayerState.Jump;
		anim.Play("jump");
		velocity.Y = JumpVelocity;
		jumps++;
	}

	private void GoDuck()
	{
		state = PlayerState.Duck;
		anim.Play("duck");
		cs.Shape = new CapsuleShape2D() { Radius = 8, Height = 10 };
		cs.Position = new Vector2(0, 2.0f);
	}

	private void ExitDuck()
	{
		cs.Shape = new CapsuleShape2D() { Radius = 6, Height = 16 };
		cs.Position = new Vector2(0, 0);
	}

	private void IdleState()
	{
		Move();
		if (velocity.X != 0)
		{
			GoWalk();
			return;
		}
		if (Input.IsActionJustPressed("up"))
		{
			GoJump();
			return;
		}

		if (Input.IsActionJustPressed("down"))
		{
			GoDuck();
			return;
		}
	}

	private void WalkState()
	{
		Move();

		if (velocity.X == 0)
		{
			GoIdle();
			return;
		}
		if (Input.IsActionJustPressed("up"))
		{
			GoJump();
			return;
		}
	}

	private void JumpState()
	{
		Move();

		if (Input.IsActionJustPressed("up") && jumps < maxJumps)
		{
			GoJump();
			return;
		}

		if (IsOnFloor())
		{
			jumps = 0;
			if (velocity.X == 0)
			{
				GoIdle();
				return;
			}
			else
			{
				GoWalk();
				return;
			}
		}
	}

	private void DuckState()
	{
		UpdateDirection();

		if (Input.IsActionJustReleased("down"))
		{
			ExitDuck();
			GoIdle();
			return;
		}
	}

	private void Move()
	{

		UpdateDirection();

		if (direction != 0)
		{
			velocity.X = direction * Speed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(velocity.X, 0, Speed);
		}

	}

	private void UpdateDirection()
	{
		direction = Input.GetAxis("left", "right");

		if (direction > 0)
		{
			anim.FlipH = false;
		}
		else if (direction < 0)
		{
			anim.FlipH = true;
		}
	}

}
