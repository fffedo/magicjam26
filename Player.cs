using Godot;
using System;

public partial class Player : CharacterBody2D
{
	[Export]
	private MainScene mainScene;
	[Export]
	private Node2D world;
	[Export]
	private CollisionShape2D collisionShape;
	[Export]
	private Node2D spriteNode; // Can be Sprite2D or AnimatedSprite2D

	[Export]
	public float Speed = 300.0f;
	[Export]
	public float CrouchSpeed = 150.0f;
	[Export]
	public float JumpVelocity = -400.0f;
	
	private PlayerInstanceState playerInstanceState = PlayerInstanceState.Slave;

	// Variables to store original collision shape and sprite values
	private Vector2 originalShapeSize;
	private Vector2 originalShapePosition;
	private Vector2 originalSpriteScale;
	private Vector2 originalSpritePosition;

	public override void _Ready()
	{
		// Duplicate the shape resource so it doesn't affect other instances, then cache original values
		if (collisionShape != null && collisionShape.Shape != null)
		{
			collisionShape.Shape = (Shape2D)collisionShape.Shape.Duplicate();

			if (collisionShape.Shape is RectangleShape2D rectShape)
			{
				originalShapeSize = rectShape.Size;
				originalShapePosition = collisionShape.Position;
			}
		}

		// Cache original sprite transform values
		if (spriteNode != null)
		{
			originalSpriteScale = spriteNode.Scale;
			originalSpritePosition = spriteNode.Position;
		}
	}

	public override void _Process(double delta)
	{
		if(mainScene.IsCurrentWorld(world))
		{
			// is master
			if(Input.IsActionJustPressed("switch_world"))
			{
				
			}
		}
		else
		{
			// is slave
		}
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 velocity = Velocity;

		// Check if crouch action is held down
		bool isCrouching = Input.IsActionPressed("playerCrouch");
		float currentSpeed = Speed;

		// Handle Collision Shape and Speed changes for crouching
		if (collisionShape?.Shape is RectangleShape2D rect)
		{
			if (isCrouching)
			{
				currentSpeed = CrouchSpeed;
				// Shrink collision height by half
				rect.Size = new Vector2(originalShapeSize.X, originalShapeSize.Y * 0.5f);
				// Shift collision position down so the player stays planted on the floor
				collisionShape.Position = new Vector2(originalShapePosition.X, originalShapePosition.Y + (originalShapeSize.Y * 0.25f));

				// Shrink sprite vertical scale by half and shift it down to match
				if (spriteNode != null)
				{
					spriteNode.Scale = new Vector2(originalSpriteScale.X, originalSpriteScale.Y * 0.5f);
					spriteNode.Position = new Vector2(originalSpritePosition.X, originalSpritePosition.Y + (originalShapeSize.Y * 0.25f));
				}
			}
			else
			{
				// Reset collision to original size and position
				rect.Size = originalShapeSize;
				collisionShape.Position = originalShapePosition;

				// Reset sprite to original scale and position
				if (spriteNode != null)
				{
					spriteNode.Scale = originalSpriteScale;
					spriteNode.Position = originalSpritePosition;
				}
			}
		}

		// Add the gravity.
		if (!IsOnFloor())
		{
			velocity += GetGravity() * (float)delta;
		}

		// Handle Jump (prevent jumping while crouching)
		if (Input.IsActionJustPressed("playerJump") && IsOnFloor() && !isCrouching)
		{
			velocity.Y = JumpVelocity;
		}

		// Get the input direction and handle movement/deceleration using currentSpeed
		Vector2 direction = Input.GetVector("playerLeft", "playerRight", "playerUp", "playerDown");
		if (direction != Vector2.Zero)
		{
			velocity.X = direction.X * currentSpeed;
		}
		else
		{
			velocity.X = Mathf.MoveToward(Velocity.X, 0, currentSpeed);
		}

		Velocity = velocity;
		MoveAndSlide();
	}
}
