export const UUID_PATTERN =
  '^[0-9a-fA-F]{8}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{4}-[0-9a-fA-F]{12}$';

export const DATE_PATTERN = '^\\d{4}-\\d{2}-\\d{2}$';

export const MONTH_PATTERN = '^\\d{4}-\\d{2}$';

export const FOLIO_PATTERN = '^VG-\\d{4}-\\d{4,}$';

export const COUNTER_ID_PATTERN = '^folio-\\d{4}$';

export const EMAIL_PATTERN = '^[^\\s@]+@[^\\s@]+\\.[^\\s@]+$';

export const uuid = () => ({
  bsonType: 'string',
  pattern: UUID_PATTERN
});

export const string = () => ({
  bsonType: 'string'
});

export const dateString = () => ({
  bsonType: 'string',
  pattern: DATE_PATTERN
});

export const monthString = () => ({
  bsonType: 'string',
  pattern: MONTH_PATTERN
});

export const folioString = () => ({
  bsonType: 'string',
  pattern: FOLIO_PATTERN
});

export const email = () => ({
  bsonType: 'string',
  pattern: EMAIL_PATTERN
});

export const bool = () => ({
  bsonType: 'bool'
});

export const date = () => ({
  bsonType: 'date'
});

export const money = () => ({
  bsonType: 'decimal'
});

export const number = (minimum, maximum) => {
  const schema = { bsonType: 'number' };
  if (minimum !== undefined) schema.minimum = minimum;
  if (maximum !== undefined) schema.maximum = maximum;
  return schema;
};

export const int = (minimum, maximum) => {
  const schema = { bsonType: 'int' };
  if (minimum !== undefined) schema.minimum = minimum;
  if (maximum !== undefined) schema.maximum = maximum;
  return schema;
};

export const enumOf = (values) => ({
  bsonType: 'string',
  enum: values
});

export const object = (properties, required = []) => {
  const schema = {
    bsonType: 'object',
    properties
  };
  if (required.length > 0) schema.required = required;
  return schema;
};

export const arrayOf = (items) => ({
  bsonType: 'array',
  items
});

export const jsonSchema = (title, properties, required, options = {}) => {
  const schema = {
    bsonType: 'object',
    title,
    required,
    properties
  };
  if (options.additionalProperties !== undefined) {
    schema.additionalProperties = options.additionalProperties;
  }
  return { $jsonSchema: schema };
};
