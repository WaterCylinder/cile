using System;
using Godot;
using Godot.Collections;

public partial class EntityManager : Node
{
	private static EntityManager instance;
	public static EntityManager Instance
    {
        get
        {
            if(instance == null)
            {
                try
				{
					GD.Print("正在初始化实体管理器");
					SceneTree sceneTree = Engine.GetMainLoop() as SceneTree;
					instance = new EntityManager();
					instance.Name = "EntityManager";
					instance.ProcessMode = ProcessModeEnum.Always;
					sceneTree.Root.AddChild(instance);
				}
				catch(Exception e)
				{
					GD.PrintErr($"初始化实体管理器失败！{e}");
				}
            }
			return instance;
        }
    }

	[Export]public Dictionary<ulong, Node2D> EntityPool = new();

	public Node2D CreateEntity(PackedScene packedScene, Node parent, Vector2 position, float rotation, Vector2 scale)
    {
        try
        {
            Node2D entity = packedScene.Instantiate() as Node2D;
			parent.AddChild(entity);
			entity.Position = position;
			entity.Rotation = rotation;
			entity.Scale = scale;
			EntityPool[entity.GetInstanceId()] = entity;
			return entity;
        }
		catch(Exception e)
        {
			GD.PrintErr($"实体创建异常：{e}");
			return null;
        }
    }

	public Node2D CreateEntity(PackedScene packedScene, Node parent, Vector2 position)
        => CreateEntity(packedScene, parent, position, 0, Vector2.One);
	public Node2D CreateEntity(PackedScene packedScene, Node parent)
        => CreateEntity(packedScene, parent, Vector2.Zero, 0, Vector2.One);

	public void DeleteEntity(Node2D entity)
    {
        try
        {
            ulong eid = entity.GetInstanceId();
            if (EntityPool.ContainsKey(eid))
            {
                EntityPool.Remove(eid);
            }
            entity.QueueFree();
        }
		catch(Exception e)
        {
            GD.PrintErr($"实体删除异常：{e}");
        }
    }
}
