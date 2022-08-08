using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PaddleController : MonoBehaviour
{
    public bool Active
    {
        get;
        set;
    }
    private float _maxSpeed;
    [SerializeField] private Transform _base;
    [SerializeField] private Vector2 _positionConstraints;

    public Vector2 Velocity
    {
        get;
        private set;
    }
    public float Height
    {
        get => _base.transform.localScale.y;
        private set {
            var scale = _base.transform.localScale;
            scale.y = value;
            _base.transform.localScale = scale;
        }
    }

    public void Initialize(float maxSpeed, float height)
    {
        _maxSpeed = maxSpeed;
        Height = height;

        _positionConstraints = GetMinMaxPosition();
        Active = true;
    }

    public void MoveUp(float percentage) => Move(percentage);
    public void MoveDown(float percentage) => Move(-percentage);

    public void Move(float percentage)
    {
        if (this.Active)
        {
            percentage = Mathf.Clamp(percentage, -1, 1);
            Velocity = Vector3.up * percentage * _maxSpeed;
            var vel = Velocity * Time.fixedDeltaTime;
            this.transform.position += new Vector3(vel.x, vel.y, 0);
        }
    }

    private Vector2 GetMinMaxPosition()
    {
        int layer = LayerMask.GetMask("Wall");
        float baseline = this.transform.position.y;
        float min = baseline;
        float max = baseline;


        var hit = Physics2D.Raycast(this.transform.position, this.transform.up, float.MaxValue, layer, 0f);
        if (hit.collider != null)
        {
            max += hit.distance;
        }

        hit = Physics2D.Raycast(this.transform.position, -this.transform.up, float.MaxValue, layer, 0f);
        if (hit.collider != null)
        {
            min -= hit.distance;
        }

        return new Vector2(min, max);
    }

    private void ConstrainPaddlePosition()
    {
        float currY = this.transform.position.y;
        float minY = _positionConstraints.x + Height / 2;
        float maxY = _positionConstraints.y - Height / 2;
        currY = Mathf.Clamp(currY, minY, maxY);
        var pos = this.transform.position;
        pos.y = currY;
        this.transform.position = pos;
    }

    private void LateUpdate()
    {
        ConstrainPaddlePosition();
    }


}
