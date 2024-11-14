using UnityEngine;

namespace EmptyCommon.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimatorEventListner : MonoBehaviour
    {

#if UNITY_EDITOR
        [SerializeField] private bool _isShowDebug;
#endif
        public IAnimationEventExecuter Executer { get; set; }

        protected virtual void Awake()
        {
            DebugEvents("BEFORE");
            RuntimeFlipAnimationEvents();
            DebugEvents("AFTER");
        }

        private void RuntimeFlipAnimationEvents()
        {
            var controller = GetComponent<Animator>().runtimeAnimatorController;

            for (int i = 0; i < controller.animationClips.Length; i++)
            {
                if (controller.animationClips[i].events.Length > 0)
                {
                    var newEvents = new AnimationEvent[controller.animationClips[i].events.Length];

                    for (int j = 0; j < newEvents.Length; j++)
                    {
                        newEvents[j] = Copy(controller.animationClips[i].events[j]);
                        newEvents[j].functionName = nameof(ExecuteAnimationEvent);
                    }

                    controller.animationClips[i].events = newEvents;
                }
            }
        }

        private AnimationEvent Copy(in AnimationEvent animationEvent)
        {
            var newAnimationEvent = new AnimationEvent();
            newAnimationEvent.time = animationEvent.time;
            newAnimationEvent.messageOptions = animationEvent.messageOptions;
            newAnimationEvent.functionName = animationEvent.functionName;
            newAnimationEvent.objectReferenceParameter = animationEvent.objectReferenceParameter;
            newAnimationEvent.intParameter = animationEvent.intParameter;
            newAnimationEvent.floatParameter = animationEvent.floatParameter;
            newAnimationEvent.stringParameter = animationEvent.stringParameter;

            return newAnimationEvent;
        }

#if UNITY_EDITOR
        private void DebugEvents(string context)
        {
            if (!_isShowDebug) return;

            Debug.Log($"<b><color=orange>{context}</color></b>");

            var controller = GetComponent<Animator>().runtimeAnimatorController;

            for (int i = 0; i < controller.animationClips.Length; i++)
            {
                var bindedEvents = controller.animationClips[i].events;

                if (bindedEvents.Length > 0)
                {
                    Debug.Log($"<b><color=red>Animation Clip - {controller.animationClips[i].name}</color></b>");
                    for (int j = 0; j < bindedEvents.Length; j++)
                    {
                        var paramInfo = string.Empty;

                        if (bindedEvents[j].intParameter != default)
                        {
                            paramInfo += $"Int - {bindedEvents[j].intParameter} ";
                        }
                        if (bindedEvents[j].floatParameter != default)
                        {
                            paramInfo += $"Float - {bindedEvents[j].floatParameter} ";
                        }
                        if (bindedEvents[j].objectReferenceParameter != default)
                        {
                            paramInfo += $"Object - {bindedEvents[j].objectReferenceParameter} ";
                        }
                        if (!string.IsNullOrEmpty(bindedEvents[j].stringParameter))
                        {
                            paramInfo += $"String - {bindedEvents[j].stringParameter} ";
                        }
                        Debug.Log($"Event - {j + 1} info - {bindedEvents[j].functionName} || {bindedEvents[j].time}");
                        Debug.Log($"<b>Event info = <color=green>{paramInfo}</color></b>");
                    }
                }
            }
        }
#endif

        public void ExecuteAnimationEvent(AnimationEvent @event)
        {
            if (Executer == null) return;

            Executer.Execute(@event);
            Executer.Dispose();
            Executer = null;
        }
    }
}
