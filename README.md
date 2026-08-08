📋 Videojuego POO - Estado del Proyecto (Semana 1 a 4)

🟢 Fase 1: Configuración del Entorno y Control de Versiones (Completado)
[x] Inicializar repositorio en GitHub y clonar localmente.
[x] Resolver conflictos de sincronización y caché (Eliminación de OneDrive).
[x] Configurar variables globales de identidad en Git (user.name / user.email).
[x] Habilitar la lectura de Inputs Clásicos (Both) en los Player Settings de Unity.

🟢 Fase 2: Construcción de la "Caja Blanca" (Completado)
[x] Crear el contrato base de interacciones (IInteractable).
[x] Programar la lógica vectorial de movimiento isométrico (PlayerMovement).
[x] Encapsular al jugador y sus físicas en un Prefab aislado.
[x] Programar script PlayerInteractor: Lógica con SphereCast/LayerMask para detección precisa.
[x] Programar script TuberiaBase: Clase con herencia e implementación de IInteractable.
[x] Implementar sistema de herencia para herramientas (HerramientaBase, LlaveInglesa, CintaAdhesiva).
[x] Refactorizar TuberiaBase para validación de averías (Polimorfismo / Máquina de estados).
[x] Programar la lógica de entorno: Dispensadores y Mesas de gestión temporal.

🟡 Fase 3: Integración Visual y UI (En Progreso)
[x] Axel: Diseñar el Canvas UI (Temporizador global, barra de "Nivel de Inundación", menú de pausa).
[x] Luis: Conectar las variables de los scripts (C#) con los elementos de texto y barras del Canvas.
[x] Luis: Crear e inyectar Prefab anidado "UI_BotonInteraccion" en World Space para todos los interactuables.
[ ] Axel / Tena: Inyectar los modelos 3D finales dentro de los Prefabs lógicos de Luis y ajustar sus BoxColliders.
[ ] Tena: Ensamblar la MainScene usando ESTRICTAMENTE los Prefabs maestros de la carpeta lógica.

🟡 Fase 4: Refinamiento, Feedback y Pruebas (En Progreso)
[x] Luis: Validar la escalabilidad de la herencia y polimorfismo en los interactuables.
[x] Luis: Auditar la prioridad de interacción (SphereCast y capas físicas) para evitar selección errónea de objetos.
[ ] Axel: Integrar el script GameManager mediante el patrón Observer (Action) para escuchar el evento OnTuberiaReparada.
[ ] Equipo: Agregar partículas de agua y feedback de sonido al interactuar (Audios ya preparados).
[ ] Axel: Diseñar e integrar el menú de "Fin del Juego" (Game Over/Victoria).
[ ] Equipo: Feature Freeze. Testeo exhaustivo para destruir bugs, colisiones rotas y NullReferenceExceptions.

⚪ Fase 5: Compilación (Semana 5)
[ ] Equipo: Generar Build final (.exe) y testear framerate en el equipo de presentación.

---

## 🚀 Estado Actual: Fase 4 (Auditoría Lógica e Integración) - (08 de agosto de 2026) LUIS

El sistema de interacción espacial está 100% desacoplado y funcional. Se resolvieron los cuellos de botella de renderizado y escalabilidad de los Canvas en World Space.

### Sistemas Implementados y Optimizados:
* **Escáner Espacial Avanzado:** El `PlayerInteractor` abandonó la dependencia de `OnTriggerEnter` a favor de un `SphereCast` continuo, utilizando `LayerMasks` para ignorar geometría inerte.
* **Component-Based UI:** La retroalimentación visual (Brillo de emisión y botón "Presiona E") se abstrajo en componentes independientes (`FeedbackVisual`), permitiendo a cualquier objeto del juego reaccionar a la vista del jugador sin ensuciar la lógica de negocio.
* **Patrón Observer (Preparación):** `TuberiaBase` cuenta con un evento estático `Action` listo para notificar reparaciones sin acoplarse directamente a otros scripts.

### ⚠️ Directriz Arquitectónica Estricta para Level Design (Tena & Axel)
**PROHIBIDO** instanciar objetos grises o modelos 3D puros en la escena principal. Todo el ecosistema de la `MainScene` debe ensamblarse arrastrando los Prefabs de la carpeta lógica maestra, ya que estos contienen la inyección de dependencias necesaria para que el juego funcione.
