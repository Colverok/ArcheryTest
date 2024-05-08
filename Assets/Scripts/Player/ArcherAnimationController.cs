using UnityEngine;
using UnityEngine.Events;
using Spine.Unity;

/// <summary>
/// This class manages archer animation
/// </summary>
public class ArcherAnimationController : MonoBehaviour
{
    /// <summary>
    /// Archer SkeletonAnimation component
    /// </summary>
    [SerializeField] private SkeletonAnimation skeletonAnimation;
    /// <summary>
    /// Archer animations
    /// </summary>
    [SerializeField] private AnimationReferenceAsset idle, attackStart, attackTarget, attackFinish; 
    /// <summary>
    /// Bone tied to gun
    /// </summary>
    [SpineBone]
    [SerializeField] private string gunBoneName;
    [SpineEvent]
    [SerializeField] private string shootEventName;

    public UnityEvent OnArcherShooting;

    private Spine.Bone bodyBone;
    private Spine.AnimationState animationState;
    private Spine.EventData eventData;


    void Start()
    {
        // saving animation state
        animationState = skeletonAnimation.AnimationState;
        // saving bone that will be rotated
        bodyBone = skeletonAnimation.Skeleton.FindBone(gunBoneName);
        // saving shooting event data
        eventData = skeletonAnimation.Skeleton.Data.FindEvent(shootEventName);
        // adding event handler for shooting event
        animationState.Event += HandleShootEvent;
    }
    
    /// <summary>
    /// Start shooting (targeting) animation
    /// </summary>
    public void StartShoot()
    {
        animationState.SetAnimation(0, attackStart, false);
        AddOneAnimation(attackTarget, true);
    }

    /// <summary>
    /// Calls when the archer is targeting to animate body rotation
    /// </summary>
    public void Targeting(float angle)
    {
        bodyBone.Rotation = angle;
    }

    /// <summary>
    /// End targeting animation with shooting
    /// </summary>
    public void EndShoot()
    {
        AddOneAnimation(attackFinish, false);
    }

    /// <summary>
    /// End targeting animation without shooting
    /// </summary>
    public void EndTarget()
    {
        AddOneAnimation(idle, true);
    }


    private void AddOneAnimation(AnimationReferenceAsset anim, bool loop)
    {
        animationState.AddAnimation(0, anim, loop, 0);
    }

    private void HandleShootEvent(Spine.TrackEntry trackEntry, Spine.Event e)
    {
        if (e.Data == eventData)
        {
            OnArcherShooting.Invoke();
        }
    }

}
