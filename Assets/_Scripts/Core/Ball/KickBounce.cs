using System.Collections;
using UnityEngine;

public class KickBounce : MonoBehaviour
{
    public float kickStrength = 1.0f;
    
    private Rigidbody _rigidbody;
    
    private bool _isCollding = false;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    void OnCollisionEnter(Collision collision)
    {
        if (this._isCollding) return;
        this._isCollding = true;
        
        if (collision.gameObject.CompareTag("Rope"))
        {
            ContactPoint contact = collision.contacts[0];
            Rigidbody otherRigidbody = collision.rigidbody;

            _rigidbody.AddForce(contact.normal  * kickStrength, ForceMode.Impulse);
        }
        
        StartCoroutine(ResetTime());
    }
    
    private IEnumerator ResetTime()
    {
        yield return new WaitForEndOfFrame();
        this._isCollding = false;
    }
}
