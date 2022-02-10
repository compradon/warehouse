-- Licensed to the Compradon Inc. under one or more agreements.
-- The Compradon Inc. licenses this file to you under the MIT license.

-- Version: 1.0
-- Release date: 1 Feb 2022.

DROP FUNCTION IF EXISTS warehouse.remove_object(IN uid UUID);
DROP FUNCTION IF EXISTS warehouse.update_object(IN item JSONB);
DROP FUNCTION IF EXISTS warehouse.create_object(IN item JSONB);
DROP FUNCTION IF EXISTS warehouse.set_attribute_value(IN uid UUID, IN attribute_code VARCHAR(50), IN value ANYELEMENT);
DROP FUNCTION IF EXISTS warehouse.value_is_uuid(IN value ANYELEMENT);
DROP FUNCTION IF EXISTS warehouse.stack_value_exists(IN uid UUID, IN attribute_id SMALLINT);
DROP FUNCTION IF EXISTS warehouse.get_table_value_type(IN table_name TEXT);
DROP FUNCTION IF EXISTS warehouse.get_table_by_attribute_id(IN attribute_id SMALLINT);
DROP FUNCTION IF EXISTS warehouse.update_stock_keeping_unit(IN item JSONB);
DROP FUNCTION IF EXISTS warehouse.create_stock_keeping_unit(IN item JSONB);
DROP FUNCTION IF EXISTS warehouse.find_sku_by_program_class(IN class VARCHAR(100));
DROP FUNCTION IF EXISTS warehouse.get_stock_keeping_unit(IN sku VARCHAR(50));

DROP TABLE IF EXISTS warehouse.stack_timestamp;
DROP TABLE IF EXISTS warehouse.stack_text;
DROP TABLE IF EXISTS warehouse.stack_string;
DROP TABLE IF EXISTS warehouse.stack_reference;
DROP TABLE IF EXISTS warehouse.stack_pointer;
DROP TABLE IF EXISTS warehouse.stack_json;
DROP TABLE IF EXISTS warehouse.stack_integer;
DROP TABLE IF EXISTS warehouse.stack_decimal;
DROP TABLE IF EXISTS warehouse.stack_boolean;

DROP TABLE IF EXISTS warehouse.heap;
DROP TABLE IF EXISTS warehouse.attributes;
DROP TABLE IF EXISTS warehouse.stock_keeping_units;
DROP TABLE IF EXISTS warehouse.registry;
DROP TABLE IF EXISTS warehouse.areas;

DROP SCHEMA IF EXISTS warehouse;