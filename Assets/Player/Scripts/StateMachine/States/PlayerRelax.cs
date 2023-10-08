using System.Collections.Generic;
using UnityEngine;

public class PlayerRelax : IState
{
    private readonly float _animationSwapDelay;
    private readonly List<int> _animationsHashes;
    private readonly Animator _animator;
    private readonly int _countAnimations;

    private float _animationSwapCurrentDelay;

    public PlayerRelax(Animator animator, float secondsAnimationSwapDelay, int countAnimations) 
    {
        _animationSwapDelay = secondsAnimationSwapDelay;
        _animationsHashes = new List<int>();
        _animator = animator;
        _countAnimations = countAnimations;

        for (int i = 0; i < countAnimations; i++)
            _animationsHashes.Add(Animator.StringToHash($"Stay{i}"));
    }

    public void Enter()
    {
    }

    public void Exist(float deltaTime)
    {
        _animationSwapCurrentDelay += deltaTime;
        if (_animationSwapCurrentDelay >= _animationSwapDelay)
        {
            _animator.SetTrigger(_animationsHashes[Random.Range(0, _countAnimations)]);
            _animationSwapCurrentDelay = 0;
        }
    }

    public void Exit()
    {
        _animationSwapCurrentDelay = 0;
    }
}