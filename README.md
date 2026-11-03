MiradorWorks Pooling

🇪🇸 Español · 🇬🇧 English

Object pooler para Unity, orientado a cero allocations por spawn, con ciclo de vida configurable por pool y sin singletons.

⚠️ En desarrollo. Todavía no hay versión publicada. La API puede cambiar hasta v0.1.0.

Características
Cero allocations por spawn tras el prewarm.
Pools tipados por componente: ComponentPool<Ball> devuelve Ball, sin GetComponent.
Posicionamiento antes de activar: el objeto nunca aparece un frame en su posición anterior.
Políticas de desbordamiento: crecer, rechazar o reciclar el más antiguo. Se pueden añadir políticas propias.
Ciclo de vida por pool: ligado a la escena o persistente entre escenas.
Sin singletons ni service locator: cada proyecto decide quién es el dueño de los pools.
Extras opcionales: reseteo de Rigidbody/Rigidbody2D, limpieza de TrailRenderer y devolución temporizada.
Requisitos
Unity 6000.0 o superior.
Instalación

En Unity: Window → Package Manager → + → Install package from git URL… y pega:

<https://github.com/><owner>/mw-objpooling.git#v0.1.0

También puedes añadirlo directamente en Packages/manifest.json:

json
"com.miradorworks.pooling": "<https://github.com/><owner>/mw-objpooling.git#v0.1.0"

El sufijo #v0.1.0 fija la versión. Sin él, Unity instala lo que haya en la rama por defecto.

Uso rápido
csharp
using MiradorWorks.Pooling;
using UnityEngine;

public sealed class BallSpawner : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private PoolSettings poolSettings;

    private ComponentPool<Ball> _pool;

    private void Awake() => _pool = new ComponentPool<Ball>(ballPrefab, poolSettings);

    public void Spawn(Vector3 position)
    {
        if (_pool.TryGet(position, Quaternion.identity, out Ball ball))
        {
            // Use the ball. Return it later with GetComponent<PooledObject>().ReturnToPool().
        }
    }

    private void OnDestroy() => _pool?.Dispose();
}
Reglas de uso
Devuelve los objetos con ReturnToPool(). Nunca uses Destroy.
Reinicia el estado en IPoolable.OnSpawned, no en Awake, Start ni OnEnable.
Aplica fuerzas o velocidades iniciales después de Get.
Con pools persistentes, llama a ReleaseAll() antes de cambiar de escena.
No guardes referencias a un objeto después de devolverlo.
Documentación
Documento de diseño: decisiones, arquitectura, especificación y hoja de ruta.
Changelog
Licencia

Ver LICENSE.md.

<a id="english"></a>

MiradorWorks Pooling (English)

Object pooler for Unity, focused on zero allocations per spawn, with a configurable lifetime per pool and no singletons.

⚠️ Work in progress. No version has been released yet. The API may change until v0.1.0.

Features
Zero allocations per spawn after prewarm.
Component-typed pools: ComponentPool<Ball> returns Ball, no GetComponent needed.
Positioned before activation: objects never show up for a frame at their previous position.
Overflow policies: grow, reject, or recycle the oldest. Custom policies can be added.
Per-pool lifetime: bound to the scene or persistent across scenes.
No singletons or service locator: each project decides who owns the pools.
Optional extras: Rigidbody/Rigidbody2D reset, TrailRenderer clearing, and timed return.
Requirements
Unity 6000.0 or newer.
Installation

In Unity: Window → Package Manager → + → Install package from git URL… and paste:

https://github.com/goblanch/mw-objpooling.git#v0.1.0

Or add it directly to Packages/manifest.json:

json
"com.miradorworks.pooling": "https://github.com/goblanch/mw-objpooling.git#v0.1.0"

The #v0.1.0 suffix pins the version. Without it, Unity installs whatever is on the default branch.

Quick start

See the Spanish section for a code example. The API is the same.

Usage rules
Return objects with ReturnToPool(). Never use Destroy.
Reset state in IPoolable.OnSpawned, not in Awake, Start, or OnEnable.
Apply initial forces or velocities after Get.
With persistent pools, call ReleaseAll() before changing scenes.
Don't keep references to an object after returning it.
Documentation
Design document (Spanish): decisions, architecture, specification, and roadmap.
Changelog
License

See LICENSE.md.
