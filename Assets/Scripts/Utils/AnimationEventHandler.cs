using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// This script acts as a bridge to handle animation events when the Animator is not on the same GameObject.
/// </summary>
namespace Utils
{
	public class AnimationEventHandler : MonoBehaviour
	{
		[System.Serializable]
		public class AnimationEvent
		{
			public string eventName; // Name of the event for identification
			public UnityEvent callback; // UnityEvent to trigger when this animation event occurs
		}

		[Header("Animation Events")]
		public AnimationEvent[] animationEvents; // List of animation events to configure in the inspector

		/// <summary>
		/// Call this method from the Animation Event in the Animator.
		/// </summary>
		/// <param name="eventName">The name of the animation event.</param>
		public void TriggerEvent(string eventName)
		{
			foreach (var animationEvent in animationEvents)
			{
				if (animationEvent.eventName == eventName)
				{
					animationEvent.callback?.Invoke();
					return;
				}
			}

			Debug.LogWarning($"AnimationEventHandler: No event found with the name '{eventName}'");
		}
	}
}