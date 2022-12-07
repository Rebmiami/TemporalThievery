using System;

namespace TemporalThievery
{
	[Flags]
	public enum PuzzleStateLegality
	{
		Legal, // No problems
		ObjectInWall, // The move attempted will result in a movable object being pushed into a wall.
		OverlappingSolidObjects, // The move attempted will result in two solid movable objects overlapping.
		InvalidPlayerPosition, // The move attempted will place the player in an invalid position, such as inside of a wall or other solid object.
		NoTimelines, // The move attempted will result in the destruction of all timelines, which will make the game impossible. (although there may be a new mechanic possible by allowing this - reconsider?)
		TooManyTimelines, // The move attempted will cause the number of timelines to exceed the maximum number specified by the level.
		Caught, // No moves except undo are allowed because the player has been caught by a security camera.
		TemporalPin, // The attempted move has been blocked by a temporal pin.
	}
}