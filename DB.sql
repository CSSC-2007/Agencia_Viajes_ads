-- LMCC AyDS 2026
-- Proyecto: Sistema Agencia de Viajes
-- Estudiante: Carlos Alvarenga

CREATE DATABASE AgenciaDeViajes;
GO
USE AgenciaDeViajes;
GO

-- =============================================
-- 1. TABLAS DE SEGURIDAD Y ACCESO (RBAC)
-- =============================================

CREATE TABLE ROL (
    id_rol INT PRIMARY KEY IDENTITY(1,1),
    nombre_rol VARCHAR(30) NOT NULL -- Gerente, Atencion, Turismo
);

CREATE TABLE USUARIO (
    id_usuario INT PRIMARY KEY IDENTITY(1,1),
    username VARCHAR(30) NOT NULL UNIQUE,
    password_hash VARCHAR(255) NOT NULL, 
    id_rol INT NOT NULL,
    activo BIT DEFAULT 1,
    FOREIGN KEY (id_rol) REFERENCES ROL(id_rol)
);

-- =============================================
-- 2. TABLAS MAESTRAS (CATÁLOGOS)
-- =============================================

CREATE TABLE CLIENTE (
    id_cliente INT PRIMARY KEY, -- Código del cliente
    nombre VARCHAR(100) NOT NULL,
    direccion VARCHAR(200),
    telefono VARCHAR(15),
    ocupacion VARCHAR(50),
    estado_cliente BIT DEFAULT 1 -- Para bajas lógicas
);

CREATE TABLE ESCALA (
    id_escala INT PRIMARY KEY IDENTITY(1,1),
    lugar_escala VARCHAR(100) NOT NULL,
    orden INT NOT NULL
);

CREATE TABLE TOUR (
    id_tour INT PRIMARY KEY,
    id_escala INT NOT NULL,
    nombre_tour Varchar(100) NOT NULL,
    descripcion_tour VARCHAR(255),
    fecha_salida DATETIME NOT NULL,
    fecha_llegada DATETIME NOT NULL,
    cantidad_plazas INT NOT NULL,
    plazas_ocupadas INT DEFAULT 0,
    CHECK (fecha_llegada > fecha_salida),
    CHECK (plazas_ocupadas <= cantidad_plazas),
    FOREIGN KEY (id_escala) REFERENCES ESCALA(id_escala)
);

-- =============================================
-- 3. TABLAS DE DETALLE Y TRANSACCIONES
-- =============================================



-- Tabla de Pagos Unificada: Resuelve el requerimiento de "Cuotas" del Gerente
CREATE TABLE PAGO (
    id_pago INT PRIMARY KEY IDENTITY(1,1),
    monto_total DECIMAL(12,2) NOT NULL,
    metodo_pago VARCHAR(20) NOT NULL, -- Efectivo, Tarjeta, Cuotas
    cantidad_cuotas INT DEFAULT 1,    -- Crucial para el reporte del Gerente
    fecha_pago DATETIME DEFAULT GETDATE(),
    factura VARCHAR(50) NOT NULL
);

CREATE TABLE INSCRIPCION (
    id_inscripcion INT PRIMARY KEY IDENTITY(1,1),
    id_cliente INT NOT NULL,
    id_tour INT NOT NULL,
    id_pago INT NOT NULL,
    fecha_inscripcion DATETIME DEFAULT GETDATE(),
    estado VARCHAR(20) DEFAULT 'Activa', -- Activa, Cancelada
    FOREIGN KEY (id_cliente) REFERENCES CLIENTE(id_cliente),
    FOREIGN KEY (id_tour) REFERENCES TOUR(id_tour),
    FOREIGN KEY (id_pago) REFERENCES PAGO(id_pago)
);

-- =============================================
-- 4. VISTAS SUGERIDAS PARA REPORTES (GERENCIA)
-- =============================================

-- Reporte de pagos en cuotas
GO
CREATE VIEW VistaPagosEnCuotas AS
SELECT C.nombre, T.id_tour, P.monto_total, P.cantidad_cuotas
FROM INSCRIPCION I
JOIN CLIENTE C ON I.id_cliente = C.id_cliente
JOIN PAGO P ON I.id_pago = P.id_pago
JOIN TOUR T ON I.id_tour = T.id_tour
WHERE P.cantidad_cuotas > 1;
