using System;
using UnityEngine;

public class RopeRefContainer : MonoBehaviour
{
    [SerializeField] private RopeController2D _ropeController2D;
    public RopeController2D RopeController2D => _ropeController2D;
}
