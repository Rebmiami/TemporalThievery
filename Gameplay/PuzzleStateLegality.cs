using System;

namespace TemporalThievery
{
	public enum PuzzleStateLegality
	{
		/// <summary>
		/// No problems.
		/// </summary>
		Legal,
		/// <summary>
		/// The move attempted will result in a movable object being pushed outside of the board.
		/// </summary>
		ObjectOutOfBounds,
		/// <summary>
		/// The move attempted will result in a movable object being pushed into a wall.
		/// </summary>
		ObjectInWall,
		/// <summary>
		/// The move attempted will result in two solid objects overlapping.
		/// </summary>
		OverlappingSolidObjects,
		/// <summary>
		/// The move attempted will place the player off the board.
		/// </summary>
		PlayerOutOfBounds,
		/// <summary>
		/// The move attempted will place the player inside of a solid object.
		/// </summary>
		PlayerInSolidObject,
		/// <summary>
		/// The move attempted will place the player inside of a wall.
		/// </summary>
		PLayerInWall,

		/// <summary>
		/// The move attempted will result in the destruction of all timelines, which will make the game impossible. (although there may be a new mechanic possible by allowing this - reconsider?)
		/// </summary>
		NoTimelines,
		/// <summary>
		/// The move attempted will cause the number of timelines to exceed the maximum number specified by the level.
		/// </summary>
		TooManyTimelines,

		/// <summary>
		/// No moves except undo are allowed because the player has been caught by a security camera.
		/// </summary>
		Caught,

		/// <summary>
		/// The attempted move has been blocked by a topological pin.
		/// </summary>
		TopologicalPin,
		/// <summary>
		/// Cannot jump to a timeline because it contains multiple active anchors.
		/// </summary>
		AnchorSplit,
	}
}