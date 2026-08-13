Videojuego POO - Estado del Proyecto

Fase 1: Configuración del Entorno y Control de Versiones (Completado)
[x] Inicializar repositorio en GitHub y clonar localmente.
[x] Resolver conflictos de sincronización y caché (Eliminación de OneDrive).
[x] Configurar variables globales de identidad en Git (user.name / user.email).
[x] Habilitar la lectura de Inputs Clásicos (Both) en los Player Settings de Unity.

Fase 2: Construcción de la "Caja Blanca" (Completado)
[x] Crear el contrato base de interacciones (IInteractable).
[x] Programar la lógica vectorial de movimiento isométrico (PlayerMovement).
[x] Encapsular al jugador y sus físicas en un Prefab aislado.
[x] Programar script PlayerInteractor: Lógica con OverlapSphere/LayerMask para detección volumétrica estática.
[x] Programar script TuberiaBase: Clase con herencia e implementación de IInteractable.
[x] Implementar sistema de herencia para herramientas (HerramientaBase, LlaveInglesa, CintaAdhesiva).
[x] Refactorizar TuberiaBase para validación de averías (Polimorfismo / Máquina de estados).
[x] Programar la lógica de entorno: Dispensadores y Mesas de gestión temporal.

Fase 3: Integración Visual y UI (Completado)
[x] Axel: Diseñar el Canvas UI (Temporizador global, barra de "Nivel de Inundación", menú de pausa).
[x] Luis: Conectar las variables de los scripts (C#) con los elementos de texto y barras del Canvas.
[x] Luis: Crear e inyectar Prefab anidado "UI_BotonInteraccion" en World Space para todos los interactuables.
[x] Axel / Tena: Inyectar los modelos 3D finales dentro de los Prefabs lógicos y ajustar BoxColliders.
[x] Tena: Ensamblar la MainScene usando ESTRICTAMENTE los Prefabs maestros de la carpeta lógica.

Fase 4: Refinamiento, Feedback y Pruebas (Completado)
[x] Luis: Validar la escalabilidad de la herencia y polimorfismo en los interactuables.
[x] Luis: Migrar motor de detección geométrica (SphereCast -> OverlapSphere) para erradicar el defecto nativo 'Inside Collider Ignore'.
[x] Luis: Refactorizar FloodManager eliminando hardcoding; conteo dinámico de tuberías mediante inyección de dependencias (FindObjectsByType).
[x] Axel: Integrar el script GameManager mediante el patrón Observer (Action) para escuchar los eventos de UI.
[x] Equipo: Parametrización de partículas de agua y ajuste de valores de Game Feel (Velocidad, Temporizador, Inundación).
[x] Axel: Diseñar e integrar el menú de "Fin del Juego" (Game Over/Victoria).
[x] Luis: Auditoría Pre-Build en MainScene. Purga de Z-Fighting (Overrides de UI), apagado de menús y limpieza de Raycasters para optimizar VRAM.
[x] Equipo: Feature Freeze. Testeo exhaustivo completado.

Fase 5: Empaquetado y Entrega Documental (En Progreso)
[x] Equipo: Generar Build final estructurado (MainScene en índice 0) y compilar ejecutable (.exe).

---

Cierre (Equipo)
El repositorio queda bloqueado para nuevas características lógicas o alteraciones estructurales de la `MainScene`. El enfoque actual y definitivo se centra exclusivamente en la generación de la documentación automatizada (Doxygen) y el cierre de la literatura en el reporte PDF.
