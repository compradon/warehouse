-- Licensed to the Compradon Inc. under one or more agreements.
-- The Compradon Inc. licenses this file to you under the MIT license.

CREATE SCHEMA warehouse;

-- TABLES

CREATE TABLE warehouse.system
(
    key VARCHAR(50) CONSTRAINT pk_system PRIMARY KEY,
    value VARCHAR(250) NOT NULL
);

CREATE TABLE warehouse.dictionary
(
    item_id SERIAL CONSTRAINT pk_item PRIMARY KEY,
    parent_item_id INT NULL CONSTRAINT fk_item_parent REFERENCES warehouse.dictionary (item_id) ON DELETE CASCADE,
    alias VARCHAR(50) NULL CONSTRAINT uq_dictionary_alias UNIQUE,
    value INTEGER NULL,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    enum VARCHAR(100) NULL,
    is_removed BOOLEAN NOT NULL CONSTRAINT df_dictionary_removed DEFAULT (false),

    CONSTRAINT uq_dictionary_value UNIQUE (parent_item_id, value)
);

CREATE TABLE warehouse.dictionary_match
(
    item_id INT NOT NULL CONSTRAINT fk_dictionary REFERENCES warehouse.dictionary (item_id) ON DELETE CASCADE,
    subject VARCHAR(50) NOT NULL,
    value VARCHAR(250) NOT NULL,

    CONSTRAINT pk_dictionary_match PRIMARY KEY (item_id, subject)
);

CREATE TABLE warehouse.entity_type
(
    entity_type_id SMALLSERIAL PRIMARY KEY,
    alias VARCHAR(50) NOT NULL UNIQUE,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    class VARCHAR(100) NULL UNIQUE,
    is_private BOOLEAN NOT NULL DEFAULT (false),
    is_removed BOOLEAN NOT NULL DEFAULT (false),

    CHECK (length(alias) > 1),
    CHECK (length(name) > 1)
);

CREATE TABLE warehouse.attribute_type
(
    attribute_type_id SMALLSERIAL CONSTRAINT pk_attribute_type PRIMARY KEY,
    alias VARCHAR(15) NOT NULL CONSTRAINT uq_attribute_type_alias UNIQUE
);

INSERT INTO warehouse.attribute_type (attribute_type_id, alias)
VALUES (1, 'BOOLEAN'), (2, 'INTEGER'), (3, 'DECIMAL'), (4, 'MONEY'), (5, 'STRING'), (6, 'TEXT'), (7, 'DATETIME'), (8, 'JSON'), (9, 'DICTIONARY'), (10, 'DICTIONARY_SET'), (11, 'ENTITY'), (12, 'ENTITY_SET');

CREATE TABLE warehouse.entity_attribute
(
    attribute_id SMALLSERIAL CONSTRAINT pk_attribute PRIMARY KEY,
    entity_type_id SMALLINT NOT NULL CONSTRAINT fk_attribute_entity_type REFERENCES warehouse.entity_type (entity_type_id) ON DELETE CASCADE,
    attribute_type_id SMALLINT NOT NULL CONSTRAINT fk_attribute_type REFERENCES warehouse.attribute_type (attribute_type_id) ON DELETE CASCADE,
    dictionary_id INT NULL CONSTRAINT ck_attribute_dictionary CHECK (dictionary_id IS NULL OR (attribute_type_id = 9 AND dictionary_id IS NOT NULL)),
    alias VARCHAR(50) NOT NULL,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    default_value VARCHAR(250) NULL,
    is_required BOOLEAN NOT NULL CONSTRAINT df_attribute_is_required DEFAULT (false),
    is_unique BOOLEAN NOT NULL CONSTRAINT df_attribute_is_unique DEFAULT (false),

    CONSTRAINT uq_attribute_alias UNIQUE (entity_type_id, alias)
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
    value INT NOT NULL CONSTRAINT fk_value_item REFERENCES warehouse.dictionary (item_id) ON DELETE CASCADE,

    CONSTRAINT pk_value_dictionary PRIMARY KEY (entity_id, attribute_id)
);

CREATE TABLE warehouse.value_entity
(
    entity_id UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,
    attribute_id SMALLINT NOT NULL REFERENCES warehouse.entity_attribute (attribute_id) ON DELETE CASCADE,
    value UUID NOT NULL REFERENCES warehouse.entity (entity_id) ON DELETE CASCADE,

    CONSTRAINT pk_value_entity PRIMARY KEY (entity_id, attribute_id)
);

-- FUNCTIONS

CREATE FUNCTION warehouse.get_entity_type(id SMALLINT)
RETURNS JSONB
AS $$
BEGIN
    RETURN jsonb_build_object(
        'entity_type_id', et.entity_type_id,
        'alias', et.alias,
        'name', et.name,
        'summary', et.summary,
        'class', et.class,
        'is_private', et.is_private,
        'is_removed', et.is_removed,
        'attributes', ( SELECT jsonb_agg(ea)
                          FROM warehouse.entity_attribute ea
                         WHERE ea.entity_type_id = id )
    )
      FROM warehouse.entity_type et
     WHERE et.entity_type_id = id
       AND et.is_removed = false;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.find_entity_type_by_id(id SMALLINT)
RETURNS JSONB
AS $$
BEGIN
    RETURN warehouse.get_entity_type(id);
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.find_entity_type_by_alias(alias VARCHAR(50))
RETURNS JSONB
AS $$
DECLARE
    id SMALLINT;
BEGIN
    id := et.entity_type_id FROM warehouse.entity_type et WHERE et.alias = $1;
    RETURN warehouse.get_entity_type(id);
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.find_entity_type_by_class(class VARCHAR(100))
RETURNS JSONB
AS $$
DECLARE
    id SMALLINT;
BEGIN
    id := et.entity_type_id FROM warehouse.entity_type et WHERE et.class = $1;
    RETURN warehouse.get_entity_type(entity_type_id);
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.create_or_update_entity_type(IN item JSONB)
RETURNS JSONB
AS $$
DECLARE
    id SMALLINT;
BEGIN

    id := coalesce((item ->> 'entity_type_id')::SMALLINT, 0);

    IF id = 0 THEN

        INSERT
          INTO warehouse.entity_type (alias, name, summary, class, is_private, is_removed)
        VALUES (item ->> 'alias', item ->> 'name', item ->> 'summary', item ->> 'class', coalesce((item ->> 'is_private')::BOOLEAN, FALSE), coalesce((item ->> 'is_removed')::BOOLEAN, FALSE))
     RETURNING entity_type_id
          INTO id;

    ELSE

        UPDATE warehouse.entity_type SET
            alias = item ->> 'alias',
            name = item ->> 'name',
            summary = item ->> 'summary',
            class = item ->> 'class',
            is_private = (item ->> 'is_private')::BOOLEAN,
            is_removed = (item ->> 'is_removed')::BOOLEAN
         WHERE entity_type_id = id;
    
    END IF;

    DELETE FROM warehouse.entity_attribute
     WHERE entity_type_id = id
       AND attribute_id NOT IN (
           SELECT attribute.attribute_id FROM jsonb_to_recordset(item -> 'attributes') AS attribute (attribute_id SMALLINT)
       );

    UPDATE warehouse.entity_attribute ea SET
        attribute_type_id = attribute.attribute_type_id,
        dictionary_id = attribute.dictionary_id,
        alias = attribute.alias,
        name = attribute.name,
        summary = attribute.summary,
        default_value = attribute.default_value,
        is_required = attribute.is_required,
        is_unique = attribute.is_unique
      FROM jsonb_to_recordset(item -> 'attributes') AS attribute (
            attribute_id SMALLINT,
            attribute_type_id SMALLINT,
            dictionary_id INT,
            alias VARCHAR(50),
            name VARCHAR(100),
            summary VARCHAR(500),
            default_value VARCHAR(250),
            is_required BOOLEAN,
            is_unique BOOLEAN,
            is_removed BOOLEAN)
     WHERE ea.entity_type_id = id
       AND attribute.attribute_id = ea.attribute_id;

    INSERT
      INTO warehouse.entity_attribute (entity_type_id, attribute_type_id, dictionary_id, alias, name, summary, default_value, is_required, is_unique)
    SELECT id,
           attribute.attribute_type_id,
           attribute.dictionary_id,
           attribute.alias,
           attribute.name,
           attribute.summary,
           attribute.default_value,
           coalesce(attribute.is_required, FALSE),
           coalesce(attribute.is_unique, FALSE)
      FROM jsonb_to_recordset(item -> 'attributes') AS attribute (
          attribute_id SMALLINT, attribute_type_id SMALLINT, dictionary_id INT, alias VARCHAR(50), name VARCHAR(100), summary VARCHAR(500), default_value VARCHAR(250), is_required BOOLEAN, is_unique BOOLEAN)
     WHERE coalesce(attribute.attribute_id, 0) = 0;

    RETURN warehouse.get_entity_type(id);

END;
$$ LANGUAGE plpgsql;
