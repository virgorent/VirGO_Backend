"""Check db/schema: every free-text field has maxLength and, with --live, the database matches.

Usage (inside VirGo-API/):
  python db/tools/check_schema.py                 # offline, no credentials
  python db/tools/check_schema.py --live          # needs VIRGO_MONGO_URI and pymongo
  python db/tools/check_schema.py --live --db virgo_dev
"""
import argparse
import glob
import json
import os
import sys

SCHEMA_DIR = os.path.join(os.path.dirname(os.path.abspath(__file__)), "..", "schema")


def load():
    docs = {}
    for path in sorted(glob.glob(os.path.join(SCHEMA_DIR, "*.json"))):
        with open(path, encoding="utf-8") as f:
            doc = json.load(f)
        docs[doc["collection"]] = doc
    return docs


def json_schema(validator):
    if "$jsonSchema" in validator:
        return validator["$jsonSchema"]
    return next(x["$jsonSchema"] for x in validator.get("$and", []) if "$jsonSchema" in x)


def unbounded(node, path=""):
    if not isinstance(node, dict):
        return
    if node.get("bsonType") == "string" and "enum" not in node and "pattern" not in node and "maxLength" not in node:
        yield path
    for key, child in node.get("properties", {}).items():
        yield from unbounded(child, f"{path}.{key}" if path else key)
    if isinstance(node.get("items"), dict):
        yield from unbounded(node["items"], f"{path}[]")


def offline(docs):
    errors = []
    for name, doc in docs.items():
        errors += [f"{name}.{field}: sin maxLength" for field in unbounded(json_schema(doc["validator"]))]
    return errors


def index_view(key, options):
    return json.dumps({"key": key, **options}, sort_keys=True)


def live(docs, db_name):
    from pymongo import MongoClient

    db = MongoClient(os.environ["VIRGO_MONGO_URI"], serverSelectionTimeoutMS=15000)[db_name]
    errors = []
    current = {c["name"]: c for c in db.list_collections()}
    errors += [f"{n}: falta en la base" for n in docs.keys() - current.keys()]
    errors += [f"{n}: sobra en la base" for n in current.keys() - docs.keys()]
    for name in docs.keys() & current.keys():
        expected = docs[name]
        if json.dumps(current[name]["options"].get("validator"), sort_keys=True, default=str) != json.dumps(expected["validator"], sort_keys=True):
            errors.append(f"{name}: el validador difiere")
        have = set()
        for ix in db[name].list_indexes():
            if ix["name"] != "_id_":
                options = {k: ix[k] for k in ("unique", "expireAfterSeconds", "partialFilterExpression", "sparse") if k in ix}
                have.add(index_view(dict(ix["key"]), options))
        want = {index_view(ix["key"], ix["options"]) for ix in expected["indexes"]}
        errors += [f"{name}: falta el índice {x}" for x in want - have]
        errors += [f"{name}: sobra el índice {x}" for x in have - want]
    return errors


def main():
    parser = argparse.ArgumentParser()
    parser.add_argument("--live", action="store_true")
    parser.add_argument("--db", default="virgo_dev")
    args = parser.parse_args()
    docs = load()
    errors = offline(docs)
    if args.live:
        errors += live(docs, args.db)
    for error in sorted(errors):
        print(error)
    print(f"{len(docs)} colecciones, {len(errors)} diferencias")
    return 1 if errors else 0


if __name__ == "__main__":
    sys.exit(main())
