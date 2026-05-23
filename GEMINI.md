# Proyecto: Agencia de Viajes ADS

## Arquitectura y Tecnologías
- **Framework:** ASP.NET Core Razor Pages (Targeting .NET 10.0).
- **ORM:** Entity Framework Core con **Npgsql**.
- **Base de Datos:** PostgreSQL (Supabase).
- **Autenticación:** Cookie Authentication con Roles (Gerente, Atencion, Turismo).

## Convenciones del Repositorio
- **Relaciones de Datos:**
    - `Tour` -> `Escala` es **1:N** (Un tour tiene múltiples escalas con un orden específico).
    - `Inscripcion` -> `Pago` es **1:1** en el modelo actual (el monto del pago se actualiza para reflejar abonos).
- **Validaciones:**
    - Siempre validar la unicidad de IDs manuales en `Crear`.
    - Validar que `Monto Pago <= Precio Tour`.
- **Base de Datos:**
    - El archivo `Program.cs` contiene lógica de migración manual para asegurar que las columnas `id_tour` (en escala) y `precio` (en tour) existan sin necesidad de herramientas externas de EF.

## Roles y Permisos
- **Gerente:** Acceso total, incluyendo reportes financieros y configuración de usuarios/escalas.
- **Atencion:** Gestión de clientes y reservaciones (incluyendo bajas y actualización de pagos).
- **Turismo:** Gestión de paquetes/tours y escalas.
