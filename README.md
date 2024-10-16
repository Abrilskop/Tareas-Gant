Primer nivel : Padres
Segundo nivel : Hijos

Padres - hijos:
Todos los elementos que no tengan ningún padre, serán padres por default.

Anidación:
Si en elemento 4(id: 4), su campo padre es 3(id:3), el elemento 4 se anida al elemento 3  

Fechas:
Las fechas siempre van a ir en ascendente
"fechaInicio": En el elemento "fechaInicio" va la menor fecha de todos los hijos.
"fechaFin": En el elemento "fechaFin" va la mayor fecha de todos los hijos.

El ordenamiento de los elementos siempre se da de manera ascendente, se toma en cuenta las fechasdeinicio

Niveles:
Siempre empezar del ultimo nivel.
escoje la fecha menor y fecha mayor luego las comparar con el 2do nivel

Solution:

comparacion de fechas a[i] > b[j]

1. recorrer toda la lista e identificar cuantos niveles hay
