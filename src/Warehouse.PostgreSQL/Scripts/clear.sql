-- Licensed to the Compradon Inc. under one or more agreements.
-- The Compradon Inc. licenses this file to you under the MIT license.

DROP FUNCTION IF EXISTS warehouse.get_entity_type(entity_type_id SMALLINT);

DROP TABLE IF EXISTS warehouse.value_entity;
DROP TABLE IF EXISTS warehouse.value_dictionary;
DROP TABLE IF EXISTS warehouse.value_json;
DROP TABLE IF EXISTS warehouse.value_datetime;
DROP TABLE IF EXISTS warehouse.value_text;
DROP TABLE IF EXISTS warehouse.value_string;
DROP TABLE IF EXISTS warehouse.value_money;
DROP TABLE IF EXISTS warehouse.value_decimal;
DROP TABLE IF EXISTS warehouse.value_integer;
DROP TABLE IF EXISTS warehouse.value_boolean;
DROP TABLE IF EXISTS warehouse.entity;
DROP TABLE IF EXISTS warehouse.entity_attribute;
DROP TABLE IF EXISTS warehouse.attribute_type;
DROP TABLE IF EXISTS warehouse.entity_type;
DROP TABLE IF EXISTS warehouse.dictionary_match;
DROP TABLE IF EXISTS warehouse.dictionary;
DROP TABLE IF EXISTS warehouse.system;

DROP TYPE IF EXISTS warehouse.value_types;
DROP SCHEMA IF EXISTS warehouse;
