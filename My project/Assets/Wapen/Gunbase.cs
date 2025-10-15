using UnityEngine;

public interface IGun
{
    void Shoot();
    int currentAmmo { get; }
    int maxAmmo { get; }
    Color CurrentPaintColor { get; }
}
