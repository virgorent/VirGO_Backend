const defaultIndex = () => [{ name: '_id_', key: { _id: 1 } }];

export const createFakeDb = ({
  databaseName = 'virgo_test',
  collections = {}
} = {}) => {
  const state = new Map(
    Object.entries(collections).map(([name, value]) => [
      name,
      {
        exists: value.exists ?? true,
        validator: value.validator,
        validationLevel: value.validationLevel ?? 'strict',
        validationAction: value.validationAction ?? 'error',
        indexes: value.indexes ?? defaultIndex(),
        documentCount: value.documentCount ?? 0
      }
    ])
  );

  return {
    databaseName,
    state,
    listCollections(filter = {}) {
      return {
        async toArray() {
          const entry = state.get(filter.name);
          if (!entry?.exists) return [];
          return [{
            name: filter.name,
            options: {
              validator: entry.validator,
              validationLevel: entry.validationLevel,
              validationAction: entry.validationAction
            }
          }];
        }
      };
    },
    collection(name) {
      const entry = state.get(name);
      return {
        listIndexes() {
          return {
            async toArray() {
              return entry?.indexes ?? [];
            }
          };
        },
        async estimatedDocumentCount() {
          return entry?.documentCount ?? 0;
        }
      };
    }
  };
};
