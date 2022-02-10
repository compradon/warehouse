-- Licensed to the Compradon Inc. under one or more agreements.
-- The Compradon Inc. licenses this file to you under the MIT license.

-- Version: 1.0
-- Release date: 1 Feb 2022.

CREATE SCHEMA warehouse;

-- Tables

CREATE TABLE warehouse.areas
(
    PRIMARY KEY (area_code),

    area_code           VARCHAR(50) NOT NULL,
                        CONSTRAINT area_code_not_empty
                             CHECK (LENGTH(area_code) > 0),
    area_name           VARCHAR(100) NOT NULL,
                        CONSTRAINT area_name_not_empty
                             CHECK (LENGTH(area_name) > 0),
    area_summary        VARCHAR(500) NULL,
                        CONSTRAINT area_summary_min_length
                             CHECK (LENGTH(area_summary) > 2),
    area_version        VARCHAR(50) NULL
);

INSERT INTO warehouse.areas
VALUES ('SYSTEM', 'System', 'Area of system', '1.0'),
       ('BUSINESS', 'Business', 'Area of business', '1.0');

CREATE TABLE warehouse.registry
(
    PRIMARY KEY (registry_key),

    area_code           VARCHAR(50) NOT NULL,
                        FOREIGN KEY (area_code)
                        REFERENCES warehouse.areas (area_code)
                         ON DELETE SET NULL
                         ON UPDATE CASCADE,
    registry_key        VARCHAR(100) NOT NULL,
                        CONSTRAINT registry_key_not_empty
                             CHECK (LENGTH(registry_key) > 0),
    parent_key          VARCHAR(100) NULL,
                        FOREIGN KEY (parent_key)
                        REFERENCES warehouse.registry (registry_key)
                         ON DELETE CASCADE
                         ON UPDATE CASCADE,
    registry_name       VARCHAR(250) NOT NULL,
                        CONSTRAINT registry_name_not_empty
                             CHECK (LENGTH(registry_name) > 0),
    registry_value      VARCHAR(250) NULL,
                        CONSTRAINT registry_value_not_empty
                             CHECK (LENGTH(registry_value) > 0),
    value_type          VARCHAR(10) NULL
                        DEFAULT 'string',
                        CONSTRAINT value_type_available_values
                             CHECK (
                                value_type IN (
                                    'boolean', 'decimal', 'integer', 'string', 'timestamp'
                                )),
    value_summary       VARCHAR(500) NULL,
                        CONSTRAINT summary_min_length
                             CHECK (LENGTH(value_summary) > 2),
    is_removed          BOOLEAN NOT NULL
                        DEFAULT (false)
);

INSERT INTO warehouse.registry (area_code, registry_key, parent_key, registry_name, registry_value, value_type, value_summary)
VALUES ('SYSTEM', 'SYSTEM_VARIABLES', NULL, 'System variables', NULL, NULL, 'System variables are used to configure the system.'),
       ('SYSTEM', 'SYSTEM_VARIABLES__VERSION', 'SYSTEM_VARIABLES', 'Version', '1.0', 'string', 'Version of the database structure.'),
       ('SYSTEM', 'SYSTEM_VARIABLES__RELEASE_DATE', 'SYSTEM_VARIABLES', 'Release date', '2022-02-20', 'timestamp', 'Date of release of the database structure.');

CREATE TABLE warehouse.stock_keeping_units
(
    PRIMARY KEY (sku),

    area_code           VARCHAR(50) NOT NULL,
                        FOREIGN KEY (area_code)
                        REFERENCES warehouse.areas (area_code)
                         ON DELETE SET NULL
                         ON UPDATE CASCADE,
    sku                 VARCHAR(50) NOT NULL, 
                        CONSTRAINT sku_not_empty
                             CHECK (LENGTH(sku) > 0),
    program_class       VARCHAR(100) NULL,
                        CONSTRAINT program_class_unique
                            UNIQUE (program_class),
    unit_name           VARCHAR(100) NOT NULL,
                        CONSTRAINT unit_name_not_empty
                             CHECK (LENGTH(unit_name) > 0),
    unit_summary        VARCHAR(500) NULL,
                        CONSTRAINT unit_summary_min_length
                             CHECK (LENGTH(unit_summary) > 2),
    is_private          BOOLEAN NOT NULL
                        DEFAULT (false),
    is_removed          BOOLEAN NOT NULL
                        DEFAULT (false)
);

CREATE TABLE warehouse.attributes
(
    PRIMARY KEY (sku, attribute_code),

    attribute_id        SMALLSERIAL,
                        CONSTRAINT attribute_id_unique
                            UNIQUE (attribute_id),
    sku                 VARCHAR(50) NOT NULL,
                        FOREIGN KEY (sku)
                        REFERENCES warehouse.stock_keeping_units (sku)
                        ON DELETE CASCADE,
    attribute_code      VARCHAR(50) NOT NULL,
                        CONSTRAINT attribute_code_not_empty
                             CHECK (LENGTH(attribute_code) > 0),
    attribute_type      VARCHAR(10) NOT NULL,
                        CONSTRAINT attribute_type_available_values
                             CHECK (
                                attribute_type IN (
                                    'boolean', 'integer', 'decimal',
                                    'json', 'pointer', 'reference',
                                    'string', 'text', 'timestamp'
                                )),
    registry_key        VARCHAR(100) NULL,
                        FOREIGN KEY (registry_key)
                        REFERENCES warehouse.registry (registry_key)
                         ON DELETE CASCADE
                         ON UPDATE CASCADE,
                        CONSTRAINT registry_key_used_to_attribute_registry_type
                             CHECK (registry_key IS NULL OR (attribute_type = 'pointer' AND registry_key IS NOT NULL)),
    reference_sku       VARCHAR(50) NULL,
                        FOREIGN KEY (reference_sku)
                        REFERENCES warehouse.stock_keeping_units (sku)
                         ON DELETE CASCADE
                         ON UPDATE CASCADE,
                        CONSTRAINT reference_sku_used_to_attribute_reference_type
                             CHECK (reference_sku IS NULL OR (attribute_type = 'reference' AND reference_sku IS NOT NULL)),
    attribute_name      VARCHAR(100) NOT NULL,
                        CONSTRAINT attribute_name_not_empty
                             CHECK (LENGTH(attribute_name) > 0),
    attribute_summary   VARCHAR(500) NULL,
                        CONSTRAINT attribute_summary_min_length
                             CHECK (LENGTH(attribute_summary) > 2),
    default_value       VARCHAR(250) NULL,
                        CONSTRAINT default_value_not_empty
                             CHECK (LENGTH(default_value) > 0),
    is_required         BOOLEAN NOT NULL
                        DEFAULT (false),
    is_unique           BOOLEAN NOT NULL
                        DEFAULT (false)
);

CREATE TABLE warehouse.heap
(
    PRIMARY KEY (uid),

    uid                 UUID
                        DEFAULT (gen_random_uuid()),
    sku                 VARCHAR(50) NOT NULL,
                        FOREIGN KEY (sku)
                        REFERENCES warehouse.stock_keeping_units (sku)
                         ON DELETE CASCADE
                         ON UPDATE CASCADE,
    security_stamp      VARCHAR(100) NULL,
    creation_date       TIMESTAMP NOT NULL
                        DEFAULT (now() at time zone 'utc'),
    is_locked           BOOLEAN NOT NULL
                        DEFAULT (false),
    is_removed          BOOLEAN NOT NULL
                        DEFAULT (false)
);

CREATE TABLE warehouse.stack_boolean
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               BOOLEAN NULL
);

CREATE TABLE warehouse.stack_decimal
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               DECIMAL NULL
);

CREATE TABLE warehouse.stack_integer
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               INT NULL
);

CREATE TABLE warehouse.stack_json
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               JSONB NULL
);

CREATE TABLE warehouse.stack_pointer
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               VARCHAR(100),
                        FOREIGN KEY (value)
                        REFERENCES warehouse.registry (registry_key)
                         ON DELETE SET NULL
);

CREATE TABLE warehouse.stack_reference
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               UUID NULL,
                        FOREIGN KEY (value)
                        REFERENCES warehouse.heap (uid)
                         ON DELETE CASCADE
);

CREATE TABLE warehouse.stack_string
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               VARCHAR(250) NULL
);

CREATE TABLE warehouse.stack_text
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               TEXT NULL
);

CREATE TABLE warehouse.stack_timestamp
(
    PRIMARY KEY (uid, attribute_id),

    uid                 UUID NOT NULL,
                        FOREIGN KEY (uid)
                        REFERENCES warehouse.heap (uid)
                        ON DELETE CASCADE,
    attribute_id        SMALLINT NOT NULL,
                        FOREIGN KEY (attribute_id)
                        REFERENCES warehouse.attributes (attribute_id)
                         ON DELETE CASCADE,
    value               TIMESTAMP NULL
);

-- Functions

CREATE FUNCTION warehouse.get_stock_keeping_unit(IN sku VARCHAR(50))
RETURNS JSONB
AS $$
BEGIN
    RETURN jsonb_build_object(
        'area_code', u.area_code, 
        'sku', u.sku,
        'program_class', u.program_class,
        'unit_name', u.unit_name,
        'unit_summary', u.unit_summary,
        'is_private', u.is_private,
        'is_removed', u.is_removed,
        'attributes', ( SELECT jsonb_agg(a)
                          FROM warehouse.attributes a
                         WHERE a.sku = $1 )
    )
      FROM warehouse.stock_keeping_units u
     WHERE u.sku = $1;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.find_sku_by_program_class(IN class VARCHAR(100))
RETURNS VARCHAR(50)
AS $$
BEGIN
    RETURN u.sku FROM warehouse.stock_keeping_units u WHERE u.program_class = $1;
END
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.create_stock_keeping_unit(IN item JSONB)
RETURNS void
AS $$
BEGIN

    INSERT
      INTO warehouse.stock_keeping_units (area_code, sku, program_class, unit_name, unit_summary, is_private, is_removed)
    VALUES (item ->> 'area_code',
            item ->> 'sku',
            item ->> 'program_class',
            item ->> 'unit_name',
            item ->> 'unit_summary',
            coalesce((item ->> 'is_private')::BOOLEAN, false),
            coalesce((item ->> 'is_removed')::BOOLEAN, false));

    INSERT
      INTO warehouse.attributes (sku, attribute_code, attribute_type, registry_key, attribute_name,
                                 attribute_summary, default_value, is_required, is_unique)
    SELECT item ->> 'sku',
           a.attribute_code,
           a.attribute_type,
           a.registry_key,
           a.attribute_name,
           a.attribute_summary,
           a.default_value,
           coalesce(a.is_required, FALSE),
           coalesce(a.is_unique, FALSE)
      FROM jsonb_to_recordset(item -> 'attributes') AS a (attribute_code VARCHAR(50),
                                                          attribute_type VARCHAR(10),
                                                          registry_key VARCHAR(100),
                                                          attribute_name VARCHAR(50),
                                                          attribute_summary VARCHAR(500),
                                                          default_value VARCHAR(250),
                                                          is_required BOOLEAN,
                                                          is_unique BOOLEAN);

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.update_stock_keeping_unit(IN item JSONB)
RETURNS void
AS $$
BEGIN

    UPDATE warehouse.stock_keeping_units t SET
        area_code = item ->> 'area_code',
        program_class = item ->> 'program_class',
        unit_name = item ->> 'unit_name',
        unit_summary = item ->> 'unit_summary',
        is_private = COALESCE((item ->> 'is_private')::BOOLEAN, t.is_private),
        is_removed = COALESCE((item ->> 'is_removed')::BOOLEAN, t.is_removed)
     WHERE t.sku = item ->> 'sku';

    DELETE FROM warehouse.attributes
     WHERE sku = item ->> 'sku'
       AND attribute_code NOT IN (
           SELECT a.attribute_code FROM jsonb_to_recordset(item -> 'attributes') AS a (attribute_code VARCHAR(50))
       );

    UPDATE warehouse.attributes t SET
        attribute_type = a.attribute_type,
        registry_key = a.registry_key,
        attribute_name = a.attribute_name,
        attribute_summary = a.attribute_summary,
        default_value = a.default_value,
        is_required = COALESCE(a.is_required, t.is_required),
        is_unique = COALESCE(a.is_unique, t.is_unique)
      FROM jsonb_to_recordset(item -> 'attributes') AS a (attribute_code VARCHAR(50),
                                                          attribute_type VARCHAR(10),
                                                          registry_key VARCHAR(100),
                                                          attribute_name VARCHAR(50),
                                                          attribute_summary VARCHAR(500),
                                                          default_value VARCHAR(250),
                                                          is_required BOOLEAN,
                                                          is_unique BOOLEAN)
     WHERE t.sku = item ->> 'sku'
       AND t.attribute_code = a.attribute_code;

    INSERT
      INTO warehouse.attributes (sku, attribute_code, attribute_type, registry_key, attribute_name,
                                 attribute_summary, default_value, is_required, is_unique)
    SELECT item ->> 'sku',
           a.attribute_code,
           a.attribute_type,
           a.registry_key,
           a.attribute_name,
           a.attribute_summary,
           a.default_value,
           coalesce(a.is_required, FALSE),
           coalesce(a.is_unique, FALSE)
      FROM jsonb_to_recordset(item -> 'attributes') AS a (attribute_code VARCHAR(50),
                                                          attribute_type VARCHAR(10),
                                                          registry_key VARCHAR(100),
                                                          attribute_name VARCHAR(50),
                                                          attribute_summary VARCHAR(500),
                                                          default_value VARCHAR(250),
                                                          is_required BOOLEAN,
                                                          is_unique BOOLEAN)
     WHERE a.attribute_code NOT IN ( SELECT attribute_code
                                       FROM warehouse.attributes
                                      WHERE sku = item ->> 'sku' );

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.get_table_by_attribute_id(IN attribute_id SMALLINT)
RETURNS TEXT
AS $$
DECLARE
    attribute_type TEXT;
BEGIN

    attribute_type := a.attribute_type
      FROM warehouse.attributes a
     WHERE a.attribute_id = $1;

    RETURN FORMAT('warehouse.stack_%s', attribute_type);

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.get_table_value_type(IN table_name TEXT)
RETURNS TEXT
AS $$
BEGIN

    CASE table_name
        WHEN 'warehouse.stack_boolean'      THEN RETURN 'BOOLEAN';
        WHEN 'warehouse.stack_decimal'      THEN RETURN 'DECIMAL';
        WHEN 'warehouse.stack_integer'      THEN RETURN 'INTEGER';
        WHEN 'warehouse.stack_json'         THEN RETURN 'JSONB';
        WHEN 'warehouse.stack_pointer'      THEN RETURN 'VARCHAR(100)';
        WHEN 'warehouse.stack_reference'    THEN RETURN 'UUID';
        WHEN 'warehouse.stack_string'       THEN RETURN 'VARCHAR(250)';
        WHEN 'warehouse.stack_text'         THEN RETURN 'TEXT';
    END CASE;

    RAISE EXCEPTION 'table "%" does not exist', table_name;

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.stack_value_exists(IN uid UUID, IN attribute_id SMALLINT)
RETURNS BOOLEAN
AS $$
DECLARE
    is_exists BOOLEAN;
BEGIN

    EXECUTE FORMAT('SELECT EXISTS (SELECT * FROM %s WHERE uid = $1 AND attribute_id = $2)',
                warehouse.get_table_by_attribute_id($2)
            )
       INTO is_exists
      USING $1, $2;

    RETURN is_exists;

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.value_is_uuid(IN value ANYELEMENT)
RETURNS BOOLEAN
AS $$
DECLARE
    is_uuid BOOLEAN;
BEGIN

    is_uuid := true;

    EXECUTE 'SELECT $1::UUID'
      USING $1;
  EXCEPTION WHEN others THEN is_uuid := false;

    RETURN is_uuid;

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.set_attribute_value(IN uid UUID, IN attribute_code VARCHAR(50), IN value ANYELEMENT)
RETURNS void
AS $$
DECLARE
    attribute_id SMALLINT;
    attribute_table TEXT;
    attribute_type TEXT;
BEGIN

    attribute_id := a.attribute_id
      FROM warehouse.heap h
     INNER JOIN warehouse.attributes a
        ON a.sku = h.sku
       AND a.attribute_code = $2
     WHERE h.uid = $1;

    IF attribute_id IS NULL
    THEN
        RAISE EXCEPTION 'attrubute_code "%" does not exist', attribute_code;
    END IF;

    attribute_table := warehouse.get_table_by_attribute_id(attribute_id);

    IF attribute_table = 'warehouse.stack_reference' AND warehouse.value_is_uuid(value) = false
    THEN
        value := warehouse.create_object(value::JSONB)::UUID;
    END IF;

    IF warehouse.stack_value_exists($1, attribute_id) THEN
        EXECUTE FORMAT('UPDATE %s SET value = $3::%s WHERE uid = $1 AND attribute_id = $2',
            attribute_table, warehouse.get_table_value_type(attribute_table))
          USING uid, attribute_id, value;
    ELSE
        EXECUTE FORMAT('INSERT INTO %s (uid, attribute_id, value) VALUES ($1, $2, $3::%s)',
            attribute_table, warehouse.get_table_value_type(attribute_table))
          USING uid, attribute_id, value;
    END IF;

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.create_object(IN item JSONB)
RETURNS UUID
AS $$
DECLARE
    inserted_uid UUID;
    attribute RECORD;
BEGIN

    INSERT
      INTO warehouse.heap (sku)
    VALUES (item ->> 'sku')
 RETURNING uid INTO inserted_uid;

    item := item || jsonb_build_object('uid', inserted_uid);

    PERFORM warehouse.update_object(item);

    RETURN inserted_uid;

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.update_object(IN item JSONB)
RETURNS VOID
AS $$
DECLARE
    object_uid UUID;
    attribute RECORD;
BEGIN

    object_uid := (item ->> 'uid')::UUID;

    FOR attribute IN SELECT key, value FROM jsonb_each_text(item -> 'attributes')
    LOOP
        PERFORM warehouse.set_attribute_value(object_uid, attribute.key, attribute.value);
    END LOOP;

END;
$$ LANGUAGE plpgsql;

CREATE FUNCTION warehouse.remove_object(IN uid UUID)
RETURNS VOID
AS $$
BEGIN

    UPDATE warehouse.heap
       SET is_removed = true
     WHERE sku = $1;

END;
$$ LANGUAGE plpgsql;
