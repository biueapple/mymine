using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMovePhysicsShift : IPhysicsShift
{
    readonly StateMachine _machine;
    readonly Unit unit;
    public WorldMovePhysicsShift(StateMachine stateMachine, Unit unit)
    {
        _machine = stateMachine;
        this.unit = unit;
    }
    public void Shift(Transform transform, ref Vector3 velocity, ref Vector3 momemtum)
    {
        if (velocity.y + momemtum.y * Time.deltaTime > 0)
        {
            if (Up(transform.position, velocity.y + momemtum.y * Time.deltaTime))
            {
                if (velocity.y > 0)
                    velocity.y = 0;
                if (momemtum.y > 0)
                    momemtum.y = 0;
            }
            _machine.isGround = false;
            _machine.isAir = true;
        }
        else
        {
            //아래에 블록이 있는지
            if (Down(transform.position, velocity.y + momemtum.y * Time.deltaTime))
            {
                if (velocity.y < 0)
                    velocity.y = 0;
                if (momemtum.y < 0)
                    momemtum.y = 0;

                _machine.isGround = true;
                _machine.isAir = false;
            }
            else
            {
                _machine.isGround = false;
                _machine.isAir = true;
            }
        }
        if (velocity.z + momemtum.z * Time.deltaTime > 0)
        {
            if (Front(transform.position, velocity.z + momemtum.z * Time.deltaTime))
            {
                if (velocity.z > 0)
                {
                    velocity.z = 0;
                }
                if (momemtum.z > 0)
                {
                    momemtum.z = 0;
                }
            }
        }
        else
        {
            if (Back(transform.position, velocity.z + momemtum.z * Time.deltaTime))
            {
                if (velocity.z < 0)
                {
                    velocity.z = 0;
                }
                if (momemtum.z < 0)
                {
                    momemtum.z = 0;
                }
            }
        }
        if (velocity.x + momemtum.x * Time.deltaTime > 0)
        {
            if (Right(transform.position, velocity.x + momemtum.x * Time.deltaTime))
            {
                if (velocity.x > 0)
                {
                    velocity.x = 0;
                }
                if (momemtum.x > 0)
                {
                    momemtum.x = 0;
                }
            }
        }
        else
        {
            if (Left(transform.position, velocity.x + momemtum.x * Time.deltaTime))
            {
                if (velocity.x < 0)
                {
                    velocity.x = 0;
                }
                if (momemtum.x < 0)
                {
                    momemtum.x = 0;
                }
            }
        }
    }

    private bool Up(Vector3 position, float y)
    {
        if (unit != null)
        {
            if
            (
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x - unit.Width, position.y + unit.Height + y, position.z - unit.Depth)) ||
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x + unit.Width, position.y + unit.Height + y, position.z - unit.Depth)) ||
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x + unit.Width, position.y + unit.Height + y, position.z + unit.Depth)) ||
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x - unit.Width, position.y + unit.Height + y, position.z + unit.Depth))
            )
            {
                return true;
            }
            return false;
        }
        else
        {
            if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x, position.y + y, position.z)))
                return true;
            return false;
        }
    }

    private bool Down(Vector3 position, float y)
    {
        if (unit != null)
        {
            if
            (
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x - unit.Width, position.y + y, position.z - unit.Depth)) ||
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x + unit.Width, position.y + y, position.z - unit.Depth)) ||
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x + unit.Width, position.y + y, position.z + unit.Depth)) ||
                World.Instance.WorldBlockPositionSolid(new Vector3(position.x - unit.Width, position.y + y, position.z + unit.Depth))
            )
            {
                return true;
            }
            return false;
        }
        else
        {
            if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x, position.y + y, position.z)))
            {
                return true;
            }
            return false;
        }
    }

    private bool Front(Vector3 position, float z)
    {
        if (unit != null)
        {
            float distance = unit.Width * 2;
            float width = -unit.Width;
            float measure;
            //거리가 0이 되도 한번은 더 체크를 해야함
            while (distance > 0)
            {
                for (int height = 0; height < unit.Height; height++)
                {
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z + unit.Depth + z)))
                    {
                        return true;
                    }
                }
                measure = Mathf.Min(1, distance);
                width += measure;
                distance -= measure;
            }
            for (int height = 0; height < unit.Height; height++)
            {
                if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z + unit.Depth + z)))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x, position.y, position.z + z)))
                return true;
            return false;
        }
    }

    private bool Back(Vector3 position, float z)
    {
        if (unit != null)
        {
            float distance = unit.Width * 2;
            float width = -unit.Width;
            float measure;
            //거리가 0이 되도 한번은 더 체크를 해야함
            while (distance > 0)
            {
                for (int height = 0; height < unit.Height; height++)
                {
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z - unit.Depth + z)))
                    {
                        return true;
                    }
                }
                measure = Mathf.Min(1, distance);
                width += measure;
                distance -= measure;
            }
            for (int height = 0; height < unit.Height; height++)
            {
                if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + width, position.y + height, position.z - unit.Depth + z)))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {
            if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x, position.y, position.z + z)))
                return true;
            return false;
        }
    }

    private bool Right(Vector3 position, float x)
    {
        if (unit != null)
        {
            float distance = unit.Depth * 2;
            float depth = -unit.Depth;
            float measure;
            //거리가 0이 되도 한번은 더 체크를 해야함
            while (distance > 0)
            {
                for (int height = 0; height < unit.Height; height++)
                {
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + unit.Width + x, position.y + height, position.z - depth)))
                    {
                        return true;
                    }
                }
                measure = Mathf.Min(1, distance);
                depth += measure;
                distance -= measure;
            }
            for (int height = 0; height < unit.Height; height++)
            {
                if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x + unit.Width + x, position.y + height, position.z - depth)))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {

            return false;
        }
    }

    private bool Left(Vector3 position, float x)
    {
        if (unit != null)
        {
            float distance = unit.Depth * 2;
            float depth = -unit.Depth;
            float measure;
            //거리가 0이 되도 한번은 더 체크를 해야함
            while (distance > 0)
            {
                for (int height = 0; height < unit.Height; height++)
                {
                    if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x - unit.Width + x, position.y + height, position.z - depth)))
                    {
                        return true;
                    }
                }
                measure = Mathf.Min(1, distance);
                depth += measure;
                distance -= measure;
            }
            for (int height = 0; height < unit.Height; height++)
            {
                if (World.Instance.WorldBlockPositionSolid(new Vector3(position.x - unit.Width + x, position.y + height, position.z - depth)))
                {
                    return true;
                }
            }
            return false;
        }
        else
        {

            return false;
        }
    }
}