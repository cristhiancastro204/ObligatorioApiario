# Integrantes y Roles del Proyecto

En esta sección se detallan los integrantes del equipo de proyecto, la estructura de roles asignada y la matriz de responsabilidades para asegurar el correcto cumplimiento de los objetivos definidos para cada hito del desarrollo.

---

## 1. Identificación del Equipo y Cliente

*   **Cliente del Proyecto**: **Matías Verges** (Destinatario del sistema web de gestión apícola).

| Integrante | Rol(es) Principal(es) | Contacto / Correo |
| :--- | :--- | :--- |
| **Cristhian Castro** | Líder Técnico Back-end, Desarrollador Back-end Principal, Revisor de Código (Back-end), Responsable de SCM | [Correo de Cristhian] |
| **Paula [Apellido]** | Responsable de Documentación y Teóricos, QA / Tester Front-end, Desarrolladora de Apoyo | [Correo de Paula] |

---

## 2. Descripción de Roles y Responsabilidades

Para optimizar las tareas y cumplir con el ciclo de vida del software, el equipo se ha organizado en base a los siguientes roles formales:

### A. Líder Técnico y Desarrollador Back-end Principal
*   **Responsable**: Cristhian Castro
*   **Descripción**: Define la estructura de datos, patrones de negocio en el backend (ASP.NET Core MVC), y actúa como autoridad técnica de calidad de código del lado del servidor.
*   **Responsabilidades clave**:
    *   Diseñar y codificar controladores, modelos, bases de datos (SQLite) y migraciones.
    *   **Revisión y Aprobación de Código (Code Review)**: Evaluar y dar el veredicto final sobre cualquier implementación o cambio en el Back-end realizado por otros integrantes del equipo, decidiendo si se integra o si requiere refactorizaciones.
    *   Gestionar el control de versiones (Git/GitHub), la resolución de conflictos y la estructura de ramas del repositorio (SCM).

### B. Responsable de Documentación e Ingeniería de Requerimientos
*   **Responsable**: Paula [Apellido]
*   **Descripción**: Lidera la redacción teórica del proyecto, la especificación de requerimientos (Hito 2), los manuales de usuario y el aseguramiento del formato requerido por el Instituto.
*   **Responsabilidades clave**:
    *   Redactar las secciones teóricas del proyecto (Casos de uso, historias de usuario, glosarios y justificaciones).
    *   Asegurar la consistencia formal y ortográfica en todas las entregas escritas.

### C. Responsable de Aseguramiento de Calidad (QA) y Aprobación Front-end
*   **Responsable**: Paula [Apellido]
*   **Descripción**: Valida que la interfaz gráfica (maquetada inicialmente en Stitch y luego codificada en Razor/HTML/CSS) cumpla con los requisitos ergonómicos y sea funcional para el usuario final.
*   **Responsabilidades clave**:
    *   Ejecutar las pruebas funcionales de la UI (User Interface) para comprobar flujos (crear, editar, eliminar y listar tareas).
    *   Dar el veredicto de aprobación visual sobre el diseño front-end implementado.

### D. Desarrollador Back-end de Apoyo
*   **Responsable**: Paula [Apellido] (bajo supervisión de Cristhian)
*   **Descripción**: Colabora en la implementación de lógicas secundarias en C# del backend del sistema web.
*   **Responsabilidades clave**:
    *   Implementar métodos o controladores secundarios bajo las directrices de la arquitectura.
    *   Someter sus aportes en el backend a la revisión de Cristhian para obtener la aprobación y posterior integración en la rama principal.

---

## 3. Matriz de Asignación de Responsabilidades (RACI)

La matriz RACI define el nivel de involucramiento de cada integrante en las actividades clave del proyecto:
*   **R (Responsible)**: Persona que ejecuta la tarea.
*   **A (Accountable)**: Persona con la responsabilidad final y derecho de veredicto sobre la tarea (solo una por actividad).
*   **C (Consulted)**: Persona que da feedback o asesora.
*   **I (Informed)**: Persona a la que se le informan los resultados.

| Actividad del Proyecto | Cristhian Castro | Paula [Apellido] |
| :--- | :---: | :---: |
| Planificación de Hitos y Entregas | **A** / **R** | **R** |
| Redacción de Secciones Teóricas | **C** | **A** / **R** |
| Diseño de UI/UX (Stitch) | **C** | **A** / **R** |
| Implementación Back-end (C#) | **A** / **R** | **R** *(de apoyo)* |
| Revisión y Aprobación Back-end (Code Review) | **A** / **R** | **I** |
| Implementación Front-end (Razor/CSS) | **C** | **R** |
| Pruebas de Calidad del Front-end (QA / UI) | **I** | **A** / **R** |
| Administración del Repositorio (SCM) | **A** / **R** | **I** |
| Configuración de Base de Datos (SQLite) | **A** / **R** | **I** |
