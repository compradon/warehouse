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
    is_removed BOOLEAN NOT NULL CONSTRAINT df_dictionary_removed DEFAULT (false)
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
    entity_type_id SMALLSERIAL CONSTRAINT pk_entity_type PRIMARY KEY,
    alias VARCHAR(50) NOT NULL CONSTRAINT uq_entity_type_alias UNIQUE,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    class VARCHAR(100) NULL CONSTRAINT uq_entity_type_class UNIQUE,
    is_private BOOLEAN NOT NULL CONSTRAINT df_entity_type_is_private DEFAULT (false),
    is_removed BOOLEAN NOT NULL CONSTRAINT df_entity_type_removed DEFAULT (false)
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
    alias VARCHAR(50) NOT NULL CONSTRAINT uq_attribute_alias UNIQUE,
    name VARCHAR(100) NOT NULL,
    summary VARCHAR(500) NULL,
    default_value VARCHAR(250) NULL,
    is_required BOOLEAN NOT NULL CONSTRAINT df_attribute_is_required DEFAULT (false),
    is_unique BOOLEAN NOT NULL CONSTRAINT df_attribute_is_unique DEFAULT (false)
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

CREATE FUNCTION warehouse.create_entity_type(input JSONB)
RETURNS JSONB
AS $$
DECLARE
    inserted_id SMALLINT;
BEGIN

    INSERT
      INTO warehouse.entity_type (alias, name, summary, class, is_private, is_removed)
    VALUES ((input::JSONB ->> 'alias')::VARCHAR(50),
            (input::JSONB ->> 'name')::VARCHAR(100),
            (input::JSONB ->> 'summary')::VARCHAR(500),
            (input::JSONB ->> 'class')::VARCHAR(100),
            (input::JSONB ->> 'is_private')::BOOLEAN,
            (input::JSONB ->> 'is_removed')::BOOLEAN)
 RETURNING entity_type_id INTO inserted_id;

    INSERT
      INTO warehouse.entity_attribute (entity_type_id, attribute_type_id, dictionary_id, alias, name, summary, default_value, is_required, is_unique)
    SELECT inserted_id, items.*
      FROM jsonb_to_recordset(input::JSONB -> 'attributes') AS items (
          attribute_type_id SMALLINT,
          dictionary_id INT,
          alias VARCHAR(50),
          name VARCHAR(100),
          summary VARCHAR(500),
          default_value VARCHAR(250),
          is_required BOOLEAN,
          is_unique BOOLEAN);

    RETURN warehouse.get_entity_type(inserted_id);

END;
$$ LANGUAGE plpgsql;






CREATE OR REPLACE FUNCTION warehouse.update_entity_type(input JSONB)
RETURNS JSONB
AS $$
DECLARE
    id SMALLINT;
BEGIN

    id := (input::JSONB ->> 'entity_type_id')::SMALLINT;

    UPDATE warehouse.entity_type SET
        alias = (input::JSONB ->> 'alias')::VARCHAR(50),
        name = (input::JSONB ->> 'name')::VARCHAR(100),
        summary = (input::JSONB ->> 'summary')::VARCHAR(500),
        class = (input::JSONB ->> 'class')::VARCHAR(100),
        is_private = (input::JSONB ->> 'is_private')::BOOLEAN,
        is_removed = (input::JSONB ->> 'is_removed')::BOOLEAN
     WHERE entity_type_id = id;

    DELETE FROM warehouse.entity_attribute AS ea
     WHERE ea.entity_type_id = id
       AND ea.attribute_id NOT IN (
           SELECT items.attribute_id FROM jsonb_to_recordset(input::JSONB -> 'attributes') AS items (attribute_id SMALLINT)
       );

    UPDATE ea SET
        ea.attribute_type_id = items.attribute_type_id,
        ea.dictionary_id = items.dictionary_id,
        ea.alias = items.alias,
        ea.name = items.name,
        ea.summary = items.summary,
        ea.default_value = items.default_value,
        ea.is_required = items.is_required,
        ea.is_unique = items.is_unique
      FROM warehouse.entity_attribute AS ea
     INNER JOIN jsonb_to_recordset(input::JSONB -> 'attributes') AS items (
          attribute_type_id SMALLINT,
          dictionary_id INT,
          alias VARCHAR(50),
          name VARCHAR(100),
          summary VARCHAR(500),
          default_value VARCHAR(250),
          is_required BOOLEAN,
          is_unique BOOLEAN,
          is_removed BOOLEAN
     ) ON items.attribute_id = ea.attribute_id

     WHERE ea.entity_type_id = id;

    INSERT
      INTO warehouse.entity_attribute (entity_type_id, attribute_type_id, dictionary_id, alias, name, summary, default_value, is_required, is_unique)
    SELECT inserted_id, items.*
      FROM jsonb_to_recordset(input::JSONB -> 'attributes') AS items (
          attribute_type_id SMALLINT,
          dictionary_id INT,
          alias VARCHAR(50),
          name VARCHAR(100),
          summary VARCHAR(500),
          default_value VARCHAR(250),
          is_required BOOLEAN,
          is_unique BOOLEAN)
     WHERE items.attribute_id = 0;

    RETURN warehouse.get_entity_type(inserted_id);
END;
$$ LANGUAGE plpgsql;







































DROP FUNCTION IF EXISTS warehouse.get_entity_type(entity_type_id SMALLINT);
CREATE FUNCTION warehouse.get_entity_type(entity_type_id SMALLINT)
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
                         WHERE ea.entity_type_id = $1
                           AND ea.is_removed = false )
    ) 
      FROM warehouse.entity_type et
     WHERE et.entity_type_id = $1
       AND et.is_removed = false;
END
$$ LANGUAGE plpgsql;









CREATE FUNCTION warehouse.find_entity_type_by_alias(alias NVARCHAR(50))
RETURNS SMALLINT
AS $$
    SELECT row_to_json(entity_type)
      FROM warehouse.entity_type
     WHERE entity_type_id = :alias
       AND is_removed = false
$$ LANGUAGE SQL;

CREATE FUNCTION warehouse.find_entity_type_by_class(class NVARCHAR(100))
RETURNS SMALLINT
AS $$
    SELECT row_to_json(entity_type)
      FROM warehouse.entity_type
     WHERE entity_type_id = :class
       AND is_removed = false
$$ LANGUAGE SQL;

CREATE FUNCTION warehouse.find_entity_type_by_id(entity_type_id SMALLINT)
RETURNS SMALLINT
AS $$
    SELECT row_to_json(entity_type)
      FROM warehouse.entity_type
     WHERE entity_type_id = :entity_type_id
       AND is_removed = false
$$ LANGUAGE SQL;

CREATE FUNCTION warehouse.get_all_entity_types()
RETURNS JSONB
AS $$
    SELECT jsonb_agg(entity_type)
      FROM warehouse.entity_type
     WHERE is_removed = false
$$ LANGUAGE SQL;

CREATE FUNCTION warehouse.create_entity_type(input JSONB)
RETURNS SMALLINT
AS $$

$$ LANGUAGE SQL;

CREATE FUNCTION warehouse.update_entity_type(input JSONB)
AS $$

$$ LANGUAGE SQL;

CREATE FUNCTION warehouse.delete_entity_type(entity_type_id SMALLINT)
RETURNS SMALLINT
AS $$

$$ LANGUAGE SQL;




CREATE OR REPLACE FUNCTION warehouse.create_entity_type(input JSONB)
RETURNS SMALLINT
AS $$
DECLARE
    inserted_entity_type_id SMALLINT;
BEGIN

    INSERT
      INTO warehouse.entity_type (alias, name, summary, class, is_private, is_removed)
    VALUES ((input::JSONB ->> 'alias')::VARCHAR(50),
            (input::JSONB ->> 'name')::VARCHAR(100),
            (input::JSONB ->> 'summary')::VARCHAR(500),
            (input::JSONB ->> 'class')::VARCHAR(100),
            (input::JSONB ->> 'is_private')::BOOLEAN,
            (input::JSONB ->> 'is_removed')::BOOLEAN)
 RETURNING entity_type_id INTO inserted_entity_type_id;

    INSERT
      INTO warehouse.entity_attribute (entity_type_id, attribute_type_id, dictionary_id, alias, name, summary, default_value, is_required, is_unique, is_removed)
    SELECT inserted_entity_type_id,
           items.attribute_type_id,
           items.dictionary_id,
           items.alias,
           items.name,
           items.summary,
           items.default_value,
           items.is_required,
           items.is_unique,
           items.is_removed

      FROM jsonb_to_recordset(input::JSONB -> 'attributes') AS items (
          alias VARCHAR(50),
          attribute_type_id SMALLINT,
          dictionary_id INT,
          name VARCHAR(100),
          summary VARCHAR(500),
          default_value VARCHAR(250),
          is_required BOOLEAN,
          is_unique BOOLEAN,
          is_removed BOOLEAN);

    RETURN inserted_entity_type_id;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION warehouse.update_entity_type(input TEXT)
RETURNS SMALLINT
AS $$
DECLARE
    inserted_entity_type_id SMALLINT;
BEGIN

    UPDATE warehouse.entity_type SET
        alias = (input::JSONB ->> 'alias')::VARCHAR(50),
        name = (input::JSONB ->> 'name')::VARCHAR(100),
        summary = (input::JSONB ->> 'summary')::VARCHAR(500),
        clas = (input::JSONB ->> 'class')::VARCHAR(100),
        is_private = (input::JSONB ->> 'is_private')::BOOLEAN,
        is_removed = (input::JSONB ->> 'is_removed')::BOOLEAN
     WHERE entity_type_id = (input::JSONB ->> 'entity_type_id')::INT

    -- TODO: Remove if not exists in JSON
    -- TODO: Add if not exists in TABLE
    -- TODO: Update if exists in TABLE

    INSERT
      INTO warehouse.entity_attribute
    SELECT default,
           inserted_entity_type_id,
           items.attribute_type_id,
           items.dictionary_id,
           items.alias,
           items.name,
           items.summary,
           items.default_value,
           items.is_required,
           items.is_unique,
           items.is_removed

      FROM jsonb_to_recordset(input::JSONB -> 'attributes') AS items (
          alias VARCHAR(50),
          attribute_type_id SMALLINT,
          dictionary_id INT,
          name VARCHAR(100),
          summary VARCHAR(500),
          default_value VARCHAR(250),
          is_required BOOLEAN,
          is_unique BOOLEAN,
          is_removed BOOLEAN);

    RETURN inserted_entity_type_id;
END;
$$ LANGUAGE plpgsql;


CREATE OR REPLACE FUNCTION warehouse.find_entity_type_by_id(id SMALLINT)
RETURNS JSONB
AS $$
    SELECT row_to_json(t)::JSONB
      FROM warehouse.entity_type t
     INNER JOIN warehouse.entity_attribute ta
        ON ta.entity_type_id = t.entity_type_id
     WHERE t.entity_type_id = id
       AND t.is_removed = false;
$$ LANGUAGE SQL;
