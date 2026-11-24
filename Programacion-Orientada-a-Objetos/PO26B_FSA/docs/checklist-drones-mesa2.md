# Checklist: drones de Mesa 2 a la par de la referencia

Referencia: `docs/Referencia-código/` (`Cdron`, `CdronVigilante`).
Meta: las mismas features visibles, adaptadas a nuestras convenciones y
sin copiar los fallos de la referencia.

Proceso por tarea: propuesta → Alfredo escribe o dice "aplícalo" →
compilar → prueba de aceptación → siguiente tarea.

## Decisiones

- Parámetro de `Encender`: define la espera de arranque (la referencia lo ignora).
  Si se quiere que cada dron arranque a un tiempo distinto, el azar se pone
  en quien llama (el botón), no dentro de `Encender`.
- Colores (paleta Sonnet): gris `122; 119; 110` = quieto (apagado o pausado,
  ambos tipos); terracota `201; 100; 66` = dron normal moviéndose;
  `43; 42; 39` = vigilante moviéndose.
- Contadores: cuentan drones **creados** (no bajan al destruir).

## Tareas

- [ ] **T1 – Hilos seguros.** Hilos en segundo plano, bandera `destruido`,
  `Dispose` sobrescrito. *(Código aplicado y compila; falta la prueba manual.)*
  Prueba: crear 5 drones, cerrar Mesa 2, cerrar la app → sin errores y el
  proceso desaparece del Administrador de tareas.
- [ ] **T2 – Arranque y parpadeo.** Espera `tiempoArranque` ms, parpadea 4
  veces, luego se mueve.
  Prueba: el parpadeo se ve antes de moverse.
- [ ] **T3 – Pausar / reanudar con clic.** Como máximo un hilo de movimiento
  por dron.
  Prueba: 10 clics rápidos → al final se mueve a su velocidad normal, no al doble.
- [ ] **T4 – Shift+clic destruye.** Parpadea 3 veces y se quita; el manejador
  termina ahí (no cae en la lógica de pausar/reanudar).
  Prueba: destruir uno en movimiento y uno pausado → ninguno revive.
- [ ] **T5 – `Desplazar` pasa a `virtual`.**
  Prueba: compila y nada visible cambia.
- [ ] **T6 – `CDronVigilante`.** Clase nueva en su `.cs`, agregada a mano al
  `POO26B_FSA.csproj`; recorre el perímetro.
  Prueba: baja hasta abajo y recorre este → norte → oeste → sur sin salirse,
  incluidas las 4 esquinas.
- [ ] **T7 – Botón y contadores.** Segundo botón y dos contadores (controles
  desde el diseñador).
  Prueba: cada botón crea su tipo y su contador sube en 1.

## Fallos de la referencia que no se copian

1. Reanudar crea un hilo nuevo sin comprobar el anterior → dos hilos, doble velocidad.
2. Tras Shift+clic el código sigue y puede reanudar el dron.
3. Dos manejadores de `Click`; uno (`accion`) se pisa siempre.
4. El parámetro `Timerun` de `Activar` no se usa.
