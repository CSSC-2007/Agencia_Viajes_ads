# ANÁLISIS Y DISEÑO DE SISTEMAS – ADS104

## PARTE 1 – Implementación de la base de datos (30%)

Esta base de datos deberá contener los siguientes puntos:

1. **Normalización de tablas.**
2. **Relaciones** entre tablas implementadas correctamente.
3. **Inserción de al menos 20 datos por tabla creada** (previo a la exposición). Estos deben ser consistentes con las relaciones establecidas en el punto anterior.

---

## PARTE 2 – Implementación de una interfaz (30%)

> **Nota importante:** La interfaz debe estar conectada a la base de datos para su funcionamiento.

Formatos elegibles para la entrega de la interfaz (elegir uno):

* Formularios en HTML, CSS y JavaScript.
* Interfaz gráfica creada en C#, Java u otro lenguaje que el grupo pueda utilizar.
* Página web creada con C#, PHP, Java u otro lenguaje que el grupo pueda utilizar.

### Requerimientos de Visualización (Vistas/Roles)

1. Mostrar y explicar interfaz del **usuario de atención al público**.
2. Mostrar y explicar interfaz del **usuario de Turismo**.
3. Mostrar y explicar interfaz del **usuario Gerente**.

### Requerimientos de Funcionamiento por Módulo

#### 4. Funcionamiento del Módulo de Atención al Público

* **4.1** Registrar la información de un cliente, mostrar listado de clientes, modificar cliente y darle de baja (sin inscripción a un tour).
* **4.2** Registrar información de un cliente que se anote en distintos tours para cada tipo de pago (contado, tarjeta, en cuotas, etc.).
* **4.3** Mostrar que al inscribir un cliente se verifique si está lleno el cupo o cuántas plazas hay disponibles, y actualizar la cantidad de plazas ocupadas para el tour.
* **4.4** Dar de baja a un cliente que está anotado en un tour y modificar su forma de pago.

#### 5. Funcionamiento del Módulo de Turismo

* **5.1** Registrar datos de 3 tours, con:
* Fecha y hora de salida.
* Escalas (pueden ser varias).
* Fecha y hora de llegada.
* Cantidad de plazas.



---

## PARTE 3 – Creación de consultas e informes (20% en conjunto con Vistas/Formularios)

### Funcionamiento del Módulo de Gerente

1. Mostrar informe de tours con toda la información que se registra **más la cantidad de plazas ocupadas**.
2. Mostrar informe de todos los clientes que **pagaron los tours en cuotas**.
3. Consultar los clientes que hicieron **más de una determinada cantidad de viajes** por la empresa.
4. Consultar las **plazas disponibles** de cualquier tour.

---

## RÚBRICA DE EVALUACIÓN (Resumen de Criterios)

| Actividad a evaluar | Criterio a evaluar | Porcentaje |
| --- | --- | --- |
| **Implementación de la base de datos** | Se crea base de datos, sus objetos y la inserción de información ordenado y funcional. | 15% |
|  | Se crea las relaciones correctamente, teniendo en cuenta los criterios de normalización. | 15% |
| **Implementación de una interfaz** | En el dashboard se habilitan y/o deshabilitan opciones de menú de acuerdo con el rol del usuario. | 10% |
|  | Un usuario puede interactuar con la interfaz y el funcionamiento del sistema de forma amigable. | 10% |
|  | La interfaz presentada es funcional sin errores y tiene un diseño atractivo. | 10% |
| **Creación de consultas e informes** | Se crean consultas SQL para mostrar los datos. | 10% |
|  | Se crea vista(s) y formulario(s) para consultar la información en el sistema. | 10% |
| **Documento de análisis funcional** | El documento es presentado con puntualidad en el espacio indicado por el docente. | 4% |
|  | Entrega de documento ordenado y con el contenido respectivamente indicado. | 8% |
|  | Documento no tiene errores de redacción u ortográficos. | 8% |
| **TOTAL** |  | **100%** |
