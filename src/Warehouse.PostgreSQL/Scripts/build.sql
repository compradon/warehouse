CREATE SCHEMA warehouse;

CREATE TABLE warehouse.system
(
    key VARCHAR(50) CONSTRAINT pk_system PRIMARY KEY,
    value VARCHAR(250) NOT NULL
);

CREATE TABLE warehouse.dictionary
(
    dictionary_id SERIAL CONSTRAINT pk_dictionary PRIMARY KEY,
    alias VARCHAR(50) NOT NULL CONSTRAINT uq_dictionary_alias UNIQUE,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    is_removed BOOLEAN NOT NULL CONSTRAINT df_dictionary_removed DEFAULT (false)
);

CREATE TABLE warehouse.dictionary_item
(
    item_id SERIAL CONSTRAINT pk_item PRIMARY KEY,
    parent_item_id INT NULL CONSTRAINT fk_item_parent REFERENCES warehouse.dictionary_item (item_id) ON DELETE CASCADE,
    value INT NULL,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    is_removed BOOLEAN NOT NULL CONSTRAINT df_item_removed DEFAULT (false)
);

CREATE TABLE warehouse.dictionary_collations
(
    item_id SMALLINT NOT NULL CONSTRAINT fk_dictionary_item REFERENCES warehouse.dictionary_item (item_id) ON DELETE CASCADE,
    subject VARCHAR(50) NOT NULL,
    value VARCHAR(250) NOT NULL,
    is_removed BOOLEAN NOT NULL CONSTRAINT df_dictionary_collations_removed DEFAULT (false),

    CONSTRAINT pk_dictionary_collation PRIMARY KEY (item_id, subject)
);

CREATE TABLE warehouse.entity_type
(
    entity_type_id SMALLSERIAL CONSTRAINT pk_type PRIMARY KEY,
    alias VARCHAR(50) NOT NULL CONSTRAINT uq_type_alias UNIQUE,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    is_private BOOLEAN NOT NULL CONSTRAINT df_type_is_private DEFAULT (false),
    is_removed BOOLEAN NOT NULL CONSTRAINT df_type_removed DEFAULT (false)
);

CREATE TABLE warehouse.attribute_type
(
    alias VARCHAR(15) CONSTRAINT pk_attribute_alias PRIMARY KEY
);

INSERT INTO warehouse.attribute_type (alias)
VALUES ('BOOLEAN'), ('INTEGER'), ('DECIMAL'), ('MONEY'), ('STRING'), ('TEXT'), ('DATETIME'), ('JSON'), ('DICTIONARY'), ('ENTITY');

CREATE TABLE warehouse.entity_attribute
(
    attribute_id SMALLSERIAL CONSTRAINT pk_attribute PRIMARY KEY,
    entity_type_id SMALLINT NOT NULL CONSTRAINT fk_attribute_entity_type REFERENCES warehouse.entity_type (entity_type_id) ON DELETE CASCADE,
    alias VARCHAR(50) NOT NULL CONSTRAINT uq_attribute_alias UNIQUE,
    attribute_type VARCHAR(15) NOT NULL CONSTRAINT fk_attribute_type REFERENCES warehouse.attribute_type (alias) ON DELETE CASCADE,
    dictionary_id INT NULL CONSTRAINT ck_attribute_dictionary CHECK (dictionary_id IS NULL OR (attribute_type = 'DICTIONARY' AND dictionary_id IS NOT NULL)),
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    default_value VARCHAR(250) NULL,
    is_required BOOLEAN NOT NULL CONSTRAINT df_attribute_is_required DEFAULT (false),
    is_unique BOOLEAN NOT NULL CONSTRAINT df_attribute_is_unique DEFAULT (false),
    is_removed BOOLEAN NOT NULL CONSTRAINT df_attribute_removed DEFAULT (false)
);

CREATE TABLE warehouse.entity
(
    entity_id UUID CONSTRAINT pk_entity PRIMARY KEY,
    entity_type_id SMALLINT NOT NULL CONSTRAINT fk_entity_type REFERENCES warehouse.entity_type (entity_type_id) ON DELETE CASCADE,
    security_stamp UUID NULL,
    created_at TIMESTAMP NOT NULL CONSTRAINT fk_entity_information_created_at DEFAULT (now() at time zone 'utc'),
    updated_at TIMESTAMP NULL,
    removed_at TIMESTAMP NULL,
    is_read_only BOOLEAN NOT NULL CONSTRAINT df_entity_is_read_only DEFAULT (false),
    is_removed BOOLEAN NOT NULL CONSTRAINT df_entity_removed DEFAULT (false)
);

CREATE TABLE warehouse.value_boolean
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value BOOLEAN NOT NULL,

    CONSTRAINT pk_value_boolean PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_integer
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value INT NOT NULL,

    CONSTRAINT pk_value_integer PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_decimal
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value DECIMAL NOT NULL,

    CONSTRAINT pk_value_decimal PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_money
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value MONEY NOT NULL,

    CONSTRAINT pk_value_money PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_string
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value VARCHAR(250) NOT NULL,

    CONSTRAINT pk_value_string PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_text
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value TEXT NOT NULL,

    CONSTRAINT pk_value_text PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_datetime
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value TIMESTAMP NOT NULL,

    CONSTRAINT pk_value_datetime PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_json
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value JSONB NOT NULL,

    CONSTRAINT pk_value_json PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_dictionary
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value INT NOT NULL CONSTRAINT fk_value_item REFERENCES warehouse.dictionary_item (item_id) ON DELETE CASCADE,

    CONSTRAINT pk_value_dictionary PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_entity
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,

    CONSTRAINT pk_value_entity PRIMARY KEY (entity_id, attribute_id)
);
