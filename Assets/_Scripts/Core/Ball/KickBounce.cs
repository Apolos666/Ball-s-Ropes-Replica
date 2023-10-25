using System;
using System.Collections;
using UnityEngine;

public class KickBounce : MonoBehaviour
{
    [SerializeField] private float _kickStrength = 1.0f;
    
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

            // var impluseApplyX = Mathf.Clamp(collision.impulse.x, -3, 3);
            // var impluseApplyY = Mathf.Clamp(collision.impulse.y, -10, 10);
            // var impluseApply = new Vector3(impluseApplyX, impluseApplyY, 0);
            // //
            // // print(impluseApply);
            // //
            // // if (impluseApply.y == 0)
            // // {
            // //     // _rigidbody.AddForceAtPosition(Vector3.up * _kickUpAtZero, contact.point, ForceMode.Force);
            // //     // print("Super power force");
            // // }
            // // else 
            // // if (Mathf.Abs(impluseApply.y) <= 0.5f)
            // // {
            // //     _rigidbody.AddForceAtPosition(Vector3.up * _kickUp, contact.point, ForceMode.Impulse);
            // //     print("Better force");
            // // }
            // // else
            // // {
            // //     _rigidbody.AddForceAtPosition(-impluseApply, contact.point, ForceMode.Impulse);
            // //     print("Normal Force");
            // // }
            //
            _rigidbody.AddForceAtPosition(contact.normal * _kickStrength, contact.point, ForceMode.Impulse);
        }
        
        StartCoroutine(ResetTime());
    }

    private IEnumerator ResetTime()
    {
        yield return new WaitForEndOfFrame();
        this._isCollding = false;
    }
}
