using UnityEngine;

public class BuildingDestructionSquare : BuildingDestructable
{
    protected override void HandleExplosion()
    {
        if (!hasExploded)
        {
            Vector2 screenPosition = _camera.WorldToScreenPoint(transform.position);

            if (screenPosition.x >= 0 &&
                screenPosition.x <= _camera.pixelWidth &&
                screenPosition.y >= 0 &&
                screenPosition.y <= _camera.pixelHeight)
            {
                Instantiate(explosion, transform.position, Quaternion.identity);
                hasExploded = true;
                Instantiate(fire, transform.position, Quaternion.identity);
                Instantiate(fire, transform.position + new Vector3(1f, 0f, 0f), Quaternion.identity);
                Instantiate(fire, transform.position + new Vector3(-1f, 0f, 0f), Quaternion.identity);
                Instantiate(fire, transform.position + new Vector3(0f, 1f, 0f), Quaternion.identity);
                Instantiate(fire, transform.position + new Vector3(0f, -1f, 0f), Quaternion.identity);
            }
        }
    }
}
