# Brand_Details_Country_Name_With_Details_As_Custom_Resolver

## SQL

```text
-- @keys={ '1' } (DbType = Object)
SELECT b."Name", b."Id"
FROM "Brands" AS b
WHERE b."Id" = ANY (@keys)
```

## Result

```json
{
  "data": {
    "brandById": {
      "name": "Brand0",
      "details": {
        "country": {
          "name": "Germany"
        }
      }
    }
  }
}
```

