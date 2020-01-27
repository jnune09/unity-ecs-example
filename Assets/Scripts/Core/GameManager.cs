using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.Collections; 
using Unity.Entities;
using Unity.Mathematics;
using Unity.Rendering;
using Unity.Transforms;
//using Unity.Physics;

public class GameManager : MonoBehaviour
{
    EntityManager entityManager;
    [SerializeField] private Material characterMaterial;
    [SerializeField] private Material foodMaterial;

    void Start()
    {
        entityManager = World.DefaultGameObjectInjectionWorld.EntityManager;

        SpawnActor();
        SpawnPlayer();
        SpawnFood();

        //print(Application.dataPath);
    }

    public void SpawnPlayer()
    {
        Entity player = entityManager.CreateEntity(
            typeof(PlayerTag),
            typeof(ActionBox),
            typeof(Collision),
            typeof(Direction),
            typeof(HitBox),
            typeof(Hunger),
            typeof(Inventory),
            typeof(LocalToWorld),
            typeof(PlayerInput),
            typeof(RenderMesh),
            typeof(Speed),
            typeof(Translation),
            typeof(Velocity)
            );

        //entityManager.GetBuffer<Inventory>(player).Add(new Inventory { Id = 9, Count = 7 });

        entityManager.SetName(player, "Player");
        entityManager.SetComponentData(player, new Collision { Position = new float3(-6, -16, 0), Size = new float3(12, 2, 0) });
        entityManager.SetComponentData(player, new ActionBox { Offset = -4f, Distance = 16f, Size = new float3(8, 8, 0) });
        entityManager.SetComponentData(player, new HitBox { Position = new float3(-8, -16, 0), Size = new float3(16, 32, 0) });
        entityManager.SetComponentData(player, new Hunger { Value = 10 });
        entityManager.SetComponentData(player, new Speed { Value = 80f });
        entityManager.SetComponentData(player, new Translation { Value = new float3(UnityEngine.Random.Range(-100, 100), 0, 0) });

        entityManager.SetSharedComponentData(player, new RenderMesh { mesh = SimpleGraphics.CreateMesh(24, 48), material = characterMaterial });
    }
    
    public void SpawnActor()
    {
        Entity actor = entityManager.CreateEntity(
            typeof(ActorTag),
            typeof(AgentTag),
            typeof(ActionBox),
            typeof(Collision),
            typeof(Direction),
            typeof(HitBox),
            typeof(Health),
            typeof(Hunger),
            typeof(Inventory),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(Speed),
            typeof(Translation),
            typeof(Velocity)
            );

        entityManager.SetComponentData(actor, new Collision { Position = new float3(-6, -16, 0), Size = new float3(12, 2, 0) });
        entityManager.SetComponentData(actor, new HitBox { Position = new float3(-8, -16, 0), Size = new float3(16, 32, 0) });
        entityManager.SetComponentData(actor, new Hunger { Value = 10 });
        entityManager.SetComponentData(actor, new Health { Value = 100 });
        entityManager.SetComponentData(actor, new Speed { Value = 60f });
        entityManager.SetComponentData(actor, new Translation { Value = new float3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0) });

        entityManager.SetSharedComponentData(actor, new RenderMesh { mesh = SimpleGraphics.CreateMesh(24, 48), material = characterMaterial });
    }

    public void SpawnFood()
    {
        Entity food = entityManager.CreateEntity(
            typeof(ActorTag),
            typeof(Item),
            typeof(HitBox),
            typeof(LocalToWorld),
            typeof(RenderMesh),
            typeof(Collision),
            typeof(Translation)
            );

        entityManager.SetComponentData(food, new Collision { Position = new float3(-4, -4, 0), Size = new float3(8, 8, 0) });
        entityManager.SetComponentData(food, new HitBox { Position = new float3(-8, -8, 0), Size = new float3(8, 8, 0) });
        entityManager.SetComponentData(food, new Item { Id = 1 });
        entityManager.SetComponentData(food, new Translation { Value = new float3(UnityEngine.Random.Range(-100, 100), UnityEngine.Random.Range(-100, 100), 0) });

        entityManager.SetSharedComponentData(food, new RenderMesh { mesh = SimpleGraphics.CreateMesh(16, 16), material = foodMaterial });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P) && Time.timeScale == 0)
        {
            Time.timeScale = 1;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Time.timeScale = 0;
        }
    }
}
