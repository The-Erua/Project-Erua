using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TimeControllerBase
{
}

public class TimeControllerableComponent<T> where T: Component
{
    private T controllerableComponent;
    public T ControllerableComponent
    {
        get
        {
            return controllerableComponent;
        }   
        
        set
        {
            controllerableComponent = value;
        }   
    }

    public TimeControllerableComponent(T component)
    {
        controllerableComponent = component;
    }
}

public interface ITimeController<T> where T: Component
{
    public void ControlTimeScale(float timeScaleFactor);
    public void ResetTimeScale();
}
public class RigidBodyTimeController : TimeControllerBase, ITimeController<Rigidbody>
{
    public TimeControllerableComponent<Rigidbody> ControllerableComponent;
    public Vector3 originalVelocity;
    private float originalImpulseVal;

    public RigidBodyTimeController(Rigidbody rigidbody)
    {
        ControllerableComponent = new TimeControllerableComponent<Rigidbody>(rigidbody);
    }
    public void ControlTimeScale(float timeScaleFactor)
    {
        var player = ControllerableComponent.ControllerableComponent.GetComponent<Player>();
        var playerVelocity = player.GetComponent<PlayerMovementManager>();
        var customGravity = player.GetComponent<CustomGravity>();
        customGravity.TimeScale = timeScaleFactor;
        originalVelocity = player.GetComponent<Rigidbody>().velocity;
        playerVelocity.ApplySlowMotion(timeScaleFactor);
    }

    public void ResetTimeScale()
    {
        var player = ControllerableComponent.ControllerableComponent.GetComponent<Player>();
        var customGravity = player.GetComponent<CustomGravity>();
        var playerVelocity = player.GetComponent<PlayerMovementManager>();
        playerVelocity.ResetSpeed();
        customGravity.TimeScale = 1;

    }
}
public class AnimationTimeController : TimeControllerBase, ITimeController<Animator>
{
    public TimeControllerableComponent<Animator> ControllerableComponent;
    public float originalSpeed;
    
    public AnimationTimeController(Animator animator)
    {
        ControllerableComponent = new TimeControllerableComponent<Animator>(animator);
    }
    
    public void ControlTimeScale(float timeScaleFactor)
    {
        originalSpeed = ControllerableComponent.ControllerableComponent.speed;
        ControllerableComponent.ControllerableComponent.speed *= timeScaleFactor;
    }

    public void ResetTimeScale()
    {
        ControllerableComponent.ControllerableComponent.speed = originalSpeed;
    }
}

public class ParticleSystemTimeController : TimeControllerBase, ITimeController<ParticleSystem>
{
    public TimeControllerableComponent<ParticleSystem> ControllerableComponent;
    public float originalSpeed;
    
    public ParticleSystemTimeController(ParticleSystem particleSystem)
    {
        ControllerableComponent = new TimeControllerableComponent<ParticleSystem>(particleSystem);
    }
    
    public void ControlTimeScale(float timeScaleFactor)
    {
        var main = ControllerableComponent.ControllerableComponent.main;
        originalSpeed = main.simulationSpeed;
        main.simulationSpeed *= timeScaleFactor;
    }

    public void ResetTimeScale()
    {
        var main = ControllerableComponent.ControllerableComponent.main;
        main.simulationSpeed = originalSpeed;
    }
}