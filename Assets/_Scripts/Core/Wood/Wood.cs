using UnityEngine;

public class Wood : MonoBehaviour
{
    [SerializeField] private PhysicsMaterial2D _exitCollision;

    private void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            other.gameObject.layer = LayerMask.NameToLayer("Ball");
            if (other.gameObject.TryGetComponent<Rigidbody2D>(out var ballRB))
            {
                ballRB.sharedMaterial = _exitCollision;
            }
        }
    }
}
