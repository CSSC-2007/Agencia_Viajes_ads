CREATE TABLE public.cliente (
  id_cliente bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  nombre character varying NOT NULL,
  direccion character varying,
  telefono character varying,
  ocupacion character varying,
  estado_cliente boolean DEFAULT true,
  CONSTRAINT cliente_pkey PRIMARY KEY (id_cliente)
);
CREATE TABLE public.escala (
  id_escala bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  lugar_escala character varying NOT NULL,
  orden integer NOT NULL,
  CONSTRAINT escala_pkey PRIMARY KEY (id_escala)
);
CREATE TABLE public.inscripcion (
  id_inscripcion bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  id_cliente bigint NOT NULL,
  id_tour bigint NOT NULL,
  id_pago bigint NOT NULL,
  fecha_inscripcion timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
  estado character varying DEFAULT 'Activa'::character varying,
  CONSTRAINT inscripcion_pkey PRIMARY KEY (id_inscripcion),
  CONSTRAINT fk_inscripcion_cliente FOREIGN KEY (id_cliente) REFERENCES public.cliente(id_cliente),
  CONSTRAINT fk_inscripcion_tour FOREIGN KEY (id_tour) REFERENCES public.tour(id_tour),
  CONSTRAINT fk_inscripcion_pago FOREIGN KEY (id_pago) REFERENCES public.pago(id_pago)
);
CREATE TABLE public.pago (
  id_pago bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  monto_total numeric NOT NULL,
  metodo_pago character varying NOT NULL,
  cantidad_cuotas integer DEFAULT 1,
  fecha_pago timestamp without time zone DEFAULT CURRENT_TIMESTAMP,
  factura character varying NOT NULL,
  CONSTRAINT pago_pkey PRIMARY KEY (id_pago)
);
CREATE TABLE public.rol (
  id_rol integer GENERATED ALWAYS AS IDENTITY NOT NULL,
  nombre_rol character varying NOT NULL,
  CONSTRAINT rol_pkey PRIMARY KEY (id_rol)
);
CREATE TABLE public.tour (
  id_tour bigint GENERATED ALWAYS AS IDENTITY NOT NULL,
  id_escala bigint NOT NULL,
  nombre_tour character varying NOT NULL,
  descripcion_tour character varying,
  fecha_salida timestamp without time zone NOT NULL,
  fecha_llegada timestamp without time zone NOT NULL,
  cantidad_plazas integer NOT NULL,
  plazas_ocupadas integer DEFAULT 0,
  CONSTRAINT tour_pkey PRIMARY KEY (id_tour),
  CONSTRAINT fk_tour_escala FOREIGN KEY (id_escala) REFERENCES public.escala(id_escala)
);
CREATE TABLE public.usuario (
  id_usuario integer GENERATED ALWAYS AS IDENTITY NOT NULL,
  username character varying NOT NULL,
  password_hash character varying NOT NULL,
  id_rol integer NOT NULL,
  activo boolean NOT NULL,
  CONSTRAINT usuario_pkey PRIMARY KEY (id_usuario),
  CONSTRAINT FK_usuario_rol_id_rol FOREIGN KEY (id_rol) REFERENCES public.rol(id_rol)
);
