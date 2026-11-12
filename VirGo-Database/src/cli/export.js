import path from 'node:path';
import { parseArgs } from 'node:util';
import {
  buildApiSchemas,
  checkApiSchemas,
  validateApiSchemas,
  writeApiSchemas
} from '../export/schema.js';

const usage = `Uso: node src/cli/export.js [opciones]

Opciones:
  --out <directorio>   Destino de los esquemas. Default: ../VirGo-API/db/schema.
  --check              Verifica sin escribir y falla si hay diferencias.
  --help               Muestra esta ayuda.
`;

let args;

try {
  ({ values: args } = parseArgs({
    args: process.argv.slice(2),
    allowPositionals: false,
    options: {
      out: { type: 'string' },
      check: { type: 'boolean', default: false },
      help: { type: 'boolean', short: 'h', default: false }
    }
  }));
} catch (error) {
  process.stderr.write(`Error: ${error.message}\n`);
  process.exit(2);
}

if (args.help) {
  process.stdout.write(`${usage}\n`);
  process.exit(0);
}

const outDir = path.resolve(process.cwd(), args.out ?? '../VirGo-API/db/schema');
const schemas = buildApiSchemas();
const errors = validateApiSchemas(schemas);

if (errors.length > 0) {
  for (const error of errors) {
    process.stderr.write(`${error}\n`);
  }
  process.stderr.write(`Esquemas invalidos: ${errors.length} errores.\n`);
  process.exit(1);
}

if (args.check) {
  const differences = checkApiSchemas(outDir, schemas);

  if (differences.length > 0) {
    for (const difference of differences) {
      process.stderr.write(`${difference.collection}: ${difference.reason}\n`);
    }
    process.stderr.write(`Esquemas desactualizados: ${differences.length} diferencias.\n`);
    process.exit(1);
  }

  process.stdout.write(`Esquemas actualizados: ${schemas.length} archivos.\n`);
  process.exit(0);
}

const files = writeApiSchemas(outDir, schemas);
process.stdout.write(`Esquemas generados: ${files.length} archivos en ${outDir}\n`);
