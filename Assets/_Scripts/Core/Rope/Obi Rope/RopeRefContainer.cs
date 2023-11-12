using System;
using UnityEngine;

public class RopeRefContainer : MonoBehaviour
{
    [SerializeField] private RopeResizing _ropeResizing;
    public RopeResizing RopeResizing => _ropeResizing;
}
