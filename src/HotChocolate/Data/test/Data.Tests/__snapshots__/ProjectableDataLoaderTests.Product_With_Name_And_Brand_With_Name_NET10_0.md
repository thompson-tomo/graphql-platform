# Product_With_Name_And_Brand_With_Name

## SQL

```text
-- @keys={ '1' } (DbType = Object)
SELECT p."Name", FALSE, b."Name", p."Id"
FROM "Products" AS p
INNER JOIN "Brands" AS b ON p."BrandId" = b."Id"
WHERE p."Id" = ANY (@keys)
```

## Result

```json
{
  "data": {
    "productById": {
      "name": "Product 0-0",
      "brand": {
        "name": "Brand0"
      }
    }
  }
}
```

