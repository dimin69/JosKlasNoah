﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Custom.StoryEventManager;
using UnityEngine.Events;

enum TriggerType
{
    Player
}

[System.Serializable]
public class StoryContainer
{
    public StoryEvent_SO storyEventToExecute;
    public UnityEvent OnStoryEventTriggerExecute = new UnityEvent();

    public static List<StoryEvent_SO> GetStorysToExecute( List<StoryContainer> data)
    {
        List<StoryEvent_SO> temp = new List<StoryEvent_SO>();

        foreach (StoryContainer sc in data)
        {
            temp.Add(sc.storyEventToExecute);
        }

        return temp;
    }
}

[RequireComponent(typeof(BoxCollider))]
public class StoryEventTrigger : MonoBehaviour
{
   
    [SerializeField]
    TriggerType _storyEventTriggerType = TriggerType.Player;

    [SerializeField]
    List<StoryContainer> _storyChainEvents = new List<StoryContainer>();

    bool Once = false;

    BoxCollider _collider;

    private void OnValidate()
    {
        Debug.Log("validating");
        if (_collider == null)
        {
            _collider = GetComponent<BoxCollider>();
        }
        _collider.isTrigger = true;

        for (int i = 0; i < _storyChainEvents.Count; i++)
        {
            if (_storyChainEvents[i].storyEventToExecute != null)
                _storyChainEvents[i].storyEventToExecute.AddStoryListeners(_storyChainEvents[i].OnStoryEventTriggerExecute);
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (Once)
            return;

        Once = true;

        switch (_storyEventTriggerType)
        {
            case TriggerType.Player:
                if (other.gameObject.GetComponent<PlayerController>() != null)
                    StoryEventManager.QueStoryEvents(StoryContainer.GetStorysToExecute(_storyChainEvents));
                break;
            default:
                Debug.LogError(_storyEventTriggerType + " has not been a story event trigger thats implemented");
                break;
        }
    }
}
