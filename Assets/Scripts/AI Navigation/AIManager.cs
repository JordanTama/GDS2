﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class AIManager : ScriptableObject
{
    [Header("Simulation Settings")] 
    [SerializeField] private float simulationSpeed;

    [Header("Spatial Partitioning Settings")]
    [SerializeField] private float size;
    [SerializeField] private int numCells;
    
    
    
    private List<AIAgent> _agents = new List<AIAgent>();
    private List<AISpawner> _spawners = new List<AISpawner>();
    private float _speed;
    private List<AIAgent>[] _map;
    private Vector3 _mapCentre;

    
    
    public float Speed => _speed;
    public IEnumerable<AIAgent> Navigators => _agents.ToArray();
    public AISpawner[] Spawners => _spawners.ToArray();
    public float CellSize => size / numCells;


    
    public void Initialize(Vector3 centre)
    {
        _agents = new List<AIAgent>();
        _spawners = new List<AISpawner>();
        
        Pause();
    }

    [ContextMenu("Pause")]
    public void Pause()
    {
        _speed = 0f;
    }

    [ContextMenu("Play")]
    public void Play()
    {
        _speed = simulationSpeed;
    }
    
    [ContextMenu("Clear Agents")]
    public void ClearAgents()
    {
        for (int i = _agents.Count - 1; i >= 0; i--)
        {
            _agents[i].Clear();
        }
    }

    
    
    public void AddAgent(AIAgent agent)
    {
        if (!_agents.Contains(agent))
            _agents.Add(agent);
    }

    public void RemoveAgent(AIAgent agent)
    {
        if (_agents.Contains(agent))
            _agents.Remove(agent);
    }

    
    
    public void AddSpawner(AISpawner spawner)
    {
        if (!_spawners.Contains(spawner))
            _spawners.Add(spawner);
    }

    public void RemoveSpawner(AISpawner spawner)
    {
        if (_spawners.Contains(spawner))
            _spawners.Remove(spawner);
    }

    public AISpawner RandomSpawner(AISpawner exclude)
    {
        List<AISpawner> destinations = _spawners.FindAll(spawner => spawner.IsDestination && spawner != exclude);
        return destinations.Count == 0 ? exclude : destinations[Random.Range(0, destinations.Count)];
    }
}
