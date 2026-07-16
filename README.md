📋 Videojuego POO - Estado del Proyecto (Semana 1)

🟢 Fase 1: Configuración del Entorno y Control de Versiones (Completado)
[x] Inicializar repositorio en GitHub y clonar localmente.

[x] Resolver conflictos de sincronización y caché (Eliminación de OneDrive).

[x] Configurar variables globales de identidad en Git (user.name / user.email).

[x] Habilitar la lectura de Inputs Clásicos (Both) en los Player Settings de Unity.

🟡 Fase 2: Construcción de la "Caja Blanca" (En Progreso)
Arquitectura y Lógica (Luis)

[x] Crear el contrato base de interacciones (IInteractable).

[x] Programar la lógica vectorial de movimiento isométrico (PlayerMovement).

[x] Encapsular al jugador y sus físicas en un Prefab aislado.

[x] Programar script PlayerInteractor: Lógica con OnTriggerEnter o Raycast para detectar cuándo el jugador está frente a un objeto interactuable y presiona una tecla (ej. 'E').

[x] Programar script TuberiaBase: Una clase que herede de MonoBehaviour e implemente IInteractable para probar que el jugador puede "reparar" un cubo.

[x] Implementar sistema de herencia para herramientas (HerramientaBase, LlaveInglesa, CintaAdhesiva).

[x] Refactorizar TuberiaBase para validación de averías (Polimorfismo / Máquina de estados).

[x] Programar la lógica de entorno: Dispensadores infinitos (Instantiate) y Mesas de gestión de inventario temporal.

Level Design y Entorno (Tena)

[x] Crear la escena principal (MainScene) y los elementos primarios (Piso, Paredes).

[ ] Alinear los vértices de los muros usando Vertex Snapping (V) para cerrar la geometría.

[ ] Configurar el posicionamiento de la Main Camera (Orthographic, Rotación X:30/45, Y:45).

[ ] Auditar e implementar BoxCollider en todos los objetos estáticos (Pisos, Muros, Mesas) para evitar caídas al vacío.

Props y Modelado Base (Axel)

[x] Crear Prefab aislado: Tuberia_Rota (Usar cilindros primitivos).

[x] Crear Prefab aislado: Mesa_Herramientas (Usar cubos primitivos).

[x] Crear Prefabs aislados: Herramientas básicas (Cinta, Llave inglesa).

⚪ Fase 3: Integración Visual y UI (Semana 2)
[x] Axel: Diseñar el Canvas UI (Temporizador global y barra de "Nivel de Inundación").

[ ] Luis: Conectar las variables de los scripts (C#) con los elementos de texto y barras del Canvas.

[ ] Tena: Sustituir la geometría primitiva por los modelos 3D finales (si aplican) y configurar luces iniciales.

⚪ Fase 4: Refinamiento, Feedback y Pruebas (Semanas 3 y 4)
[ ] Equipo: Agregar partículas de agua y feedback de sonido al interactuar.

[ ] Luis: Validar la escalabilidad de la herencia y polimorfismo en los interactuables.

[ ] Equipo: Feature Freeze (Congelamiento de nuevas mecánicas). Destruir el juego buscando bugs, colisiones rotas y NullReferenceExceptions.

⚪ Fase 5: Compilación (Semana 5)
[ ] Equipo: Generar Build final (.exe) y testear framerate en el equipo de presentación.

## 🚀 Estado Actual: Fase 2 (Caja Blanca Lógica) - Completada (05 de julio de 2026)

La arquitectura base de interacciones y físicas ha sido implementada exitosamente utilizando principios de Programación Orientada a Objetos (POO).

### Sistemas Implementados:
* **Movimiento Isométrico:** El `PlayerMovement` ahora calcula la traslación y rotación (`Quaternion`) de forma relativa a la cámara principal.
* **Sistema de Herramientas (Herencia):** Se estructuró la clase abstracta/padre `HerramientaBase` y sus clases hijas (`LlaveInglesa`, `CintaAdhesiva`).
* **Interacción Espacial (Polimorfismo):** Las tuberías (`TuberiaBase`) ahora exigen validación estricta de tipos de avería cruzando datos con la herramienta en mano.
* **Gestión de Entorno:** Implementación de `DispensadorHerramientas` (clonación infinita de prefabs) y `MesaMostrador` (gestión de almacenamiento temporal).

### ⚠️ Lineamientos para Integración de Modelos 3D (Arte)
Para que los modelos definitivos de las herramientas y mesas sean reconocidos por el sistema lógico, el equipo de diseño debe asegurar que:
1. Cada objeto cuente con su respectivo componente de colisión (`BoxCollider`, `SphereCollider`, etc.) activo.
2. El modelo visual esté asignado estrictamente a la Layer `Interactable` en el Inspector de Unity.
