# Instituto Tecnológico CTC Colonia

## Bases de Datos II
### Obligatorio

**Docente:** Andrés Klett
**Estudiantes:**
- Paula Camacho
- Cristhian Castro

**Año:** 2026

---

## Declaración de Autoría

Nosotros, Cristhian Castro y Paula Camacho, declaramos que el trabajo que se presenta es de nuestra propia mano. Puedo asegurar que:

- La obra fue producida mientras cursamos el tercer semestre de analista programador en la materia Bases de datos II con el docente Andrés Klett;
- Hemos tenido en cuenta las clases dictadas por el docente Andrés Klett, quien además proporcionó el material;
- Cuando hemos consultado el trabajo publicado por otros, lo hemos atribuido con claridad;
- Cuando hemos citado obras de otros, hemos indicado las fuentes. Con excepción de estas citas, la obra es enteramente nuestra;
- Cuando la obra se basa en trabajo realizado conjuntamente con otros, hemos explicado claramente que fue construido por otros, y qué fue contribuido por nosotros;
- Ninguna parte de este trabajo ha sido publicada previamente a su entrega, excepto donde se han realizado las aclaraciones correspondientes.

---

## MER (Modelo Entidad Relación)

*(Nota: Inserta aquí la imagen del diagrama MER que tienen en el PDF original)*

---

## Justificación del Diseño

Las decisiones de diseño tomadas para la arquitectura de esta base de datos están enfocadas en dos pilares fundamentales: la escalabilidad del sistema y la capacidad de llevar un historial detallado de las operaciones de Zánganos S.A. Como el software debe acompañar el crecimiento de la empresa, la estructura se pensó para que pueda adaptarse sin problemas a la futura incorporación de empleados y al aumento en el volumen de la producción de miel. También, se prioriza el registro cronológico de cada evento en el campo, transformando los datos diarios en un archivo histórico valioso.

La entidad **Apicultor** lleva el atributo *Cargo* porque, aunque hoy Matías hace todo solo, el día de mañana puede contratar empleados; de esta forma, el software permite que solo Matías (como Administrador) pueda manejar la venta de Barriles, separando su rol del de los futuros peones de campo. Por eso también su relación de 1 a N.

Por otro lado, para que no se pierda el rastro de los movimientos en el monte de Rivera, se armó una relación de muchos a muchos entre **Apiarios** y **Colmenas**. Esto se hizo así para que funcione como un historial: cuando una colmena se mueve de un apiario a otro por temas de floración o clima, el sistema no borra el pasado, sino que guarda todo el camino que hizo esa colmena. Así, Matías puede mirar hacia atrás y entender por qué rindió más o menos según el lugar donde estuvo instalada y detectar enfermedades o pesticidas. La relación que hay entre estas entidades, **Tiene**, contiene el atributo de la *Fecha_Instalacion* junto con la ubicación (*Fila, Columna y Sector*), los cuales ayudan a trazar el historial de forma correcta.

El diseño incluye restricciones específicas que aseguran la buena inserción de datos y que sean intuitivos para Matías. En la tabla **Colmenas**, el campo *Fortaleza_Abejas* está limitado únicamente a los valores 'Fuerte', 'Media' o 'Débil', haciendo un estándar para el criterio de evaluación de las abejas.

También, en la tabla **Planifica** el sistema exige ingresar de forma obligatoria la cantidad de herramientas y bastidores para cada visita; esto fuerza una preparación previa y soluciona el problema de llegar al apiario sin el equipamiento necesario.

Siguiendo la misma lógica de control histórico, la relación entre **Colmenas** y **Reinas** también se diseñó como una relación de muchos a muchos N a N a través de la tabla **Asigna**, incorporando el atributo *Fecha_Asignacion*. En la práctica de la apicultura, las reinas se cambian cuando envejecen, bajan su producción o por una enjambrazón. Al registrar el día y la hora exacta en que una reina entra a una colmena, el sistema no borra el pasado cuando Matías introduce una reina nueva, sino que guarda la línea de tiempo completa. Esto resuelve el problema de la falta de registros históricos, permitiendo a Matías analizar el rendimiento del panal hacia atrás y descubrir qué reinas o qué genéticas dieron los mejores resultados en cada cosecha.

Por último, al requerir la fecha exacta en la tabla **Aplica**, la base de datos registra el momento exacto en que se inicia un tratamiento sanitario, permitiendo controlar con precisión el vencimiento y la aplicación de las segundas dosis para proteger la salud de las colmenas.
