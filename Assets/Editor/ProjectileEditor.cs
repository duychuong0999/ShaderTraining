using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Projectile))]
public class ProjectileEditor : Editor
{

    void OnSceneGUI()
    {
        var projectile = target as Projectile;
        var transform = projectile.transform;
        projectile.damageRadius = Handles.RadiusHandle(
            transform.rotation,
            transform.position,
            projectile.damageRadius);
    }
}
