# Filter_With_Expression

## SQL

```text
.param set @keys '[1]'

SELECT "b"."Id", "b"."DisplayName", "b"."Name", "b"."Details_Country_Name"
FROM "Brands" AS "b"
WHERE "b"."Id" IN (
    SELECT "k"."value"
    FROM json_each(@keys) AS "k"
) AND "b"."Name" LIKE 'Brand%'
```

## Result

```json
{
  "data": {
    "filterExpression": {
      "name": "Brand0"
    }
  }
}
```

