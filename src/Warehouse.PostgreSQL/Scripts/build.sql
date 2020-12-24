CREATE SCHEMA warehouse;

CREATE TABLE warehouse.system
(
    key VARCHAR(32) PRIMARY KEY,
    value VARCHAR(128) NOT NULL
);

CREATE TABLE warehouse.dictionary
(
    key SERIAL CONSTRAINT pk_dictionary_key PRIMARY KEY,
    parent_key INTEGER NULL CONSTRAINT fk_dictionary_dictionary REFERENCES warehouse.dictionary (key) ON DELETE CASCADE,
    alias VARCHAR(128) NULL CONSTRAINT uq_dictionary_alias UNIQUE,
    display VARCHAR(128) NOT NULL,
    enum VARCHAR(64) NULL,
    value INTEGER NULL,
    summary VARCHAR(256) NULL,
    protected BOOLEAN NOT NULL CONSTRAINT df_dictionary_protected DEFAULT (false)
);

CREATE TABLE warehouse.type
(
    key SMALLSERIAL CONSTRAINT pk_type_id PRIMARY KEY,
    alias VARCHAR(32) CONSTRAINT uq_type_alias UNIQUE,
    display VARCHAR(64) NOT NULL,
    class VARCHAR(128) NULL,
    summary VARCHAR(256) NULL
);

CREATE TYPE warehouse.value_type AS ENUM
(
    'reference',
    'bool',
    'int',
    'deciamal',
    'string',
    'text',
    'datetime',
    'json'
);

CREATE TABLE warehouse.attribute
(
    key SMALLSERIAL CONSTRAINT pk_attribute_key PRIMARY KEY,
    type warehouse.value_type NOT NULL,
    type_key SMALLINT NOT NULL CONSTRAINT fk_attribute_type REFERENCES warehouse.type (key) ON DELETE CASCADE,
    alias VARCHAR(32) NOT NULL,
    display VARCHAR(64) NOT NULL,
    summary VARCHAR(256) NULL,
    required BOOLEAN,
    CONSTRAINT uq_attribute_alias UNIQUE (alias, type_key)
);

CREATE TABLE warehouse.entity
(
    key UUID CONSTRAINT pk_entity_id PRIMARY KEY,
    type_key SMALLINT NOT NULL REFERENCES warehouse.type (key) ON DELETE CASCADE,
    creation_date TIMESTAMP NOT NULL CONSTRAINT df_entity_creation_date DEFAULT (now() at time zone 'utc'),
    deletion_date TIMESTAMP NULL,
    read_only BOOLEAN NOT NULL CONSTRAINT df_entity_read_only DEFAULT (false)
    removed BOOLEAN NOT NULL CONSTRAINT df_entity_removed DEFAULT (false)
);

CREATE TABLE warehouse.value_reference
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value UUID NULL REFERENCES warehouse.entity (key) ON DELETE SET NULL,

    CONSTRAINT uq_value_reference UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_bool
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value INTEGER NULL,

    CONSTRAINT uq_value_bool UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_int
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value INTEGER NULL,

    CONSTRAINT uq_value_int UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_decimal
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value DECIMAL NULL,

    CONSTRAINT uq_value_decimal UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_string
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value VARCHAR(2048) NULL,

    CONSTRAINT uq_value_string UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_text
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value TEXT NULL,

    CONSTRAINT uq_value_text UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_datetime
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value TIMESTAMP NULL,

    CONSTRAINT uq_value_datetime UNIQUE (entity_key, attribute_key)
);

CREATE TABLE warehouse.value_json
(
    entity_key UUID NOT NULL REFERENCES warehouse.entity (key) ON DELETE CASCADE,
    attribute_key SMALLINT NOT NULL REFERENCES warehouse.attribute (key) ON DELETE CASCADE,
    value JSON NULL,

    CONSTRAINT uq_value_json UNIQUE (entity_key, attribute_key)
);
